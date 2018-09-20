﻿using System;
using System.Diagnostics.CodeAnalysis;
using UniRx.InternalUtil;
#if (NET_4_6 || NET_STANDARD_2_0)
using System.Runtime.CompilerServices;
using System.Threading;
#endif

namespace UniRx
{
    public sealed class AsyncSubject<T> : ISubject<T>, IOptimizedObservable<T>, IDisposable
#if (NET_4_6 || NET_STANDARD_2_0)
        , INotifyCompletion
#endif
    {
        object observerLock = new object();

        T lastValue;
        bool hasValue;
        bool isStopped;
        bool isDisposed;
        Exception lastError;
        IObserver<T> outObserver = EmptyObserver<T>.Instance;

        public T Value
        {
            get
            {
                ThrowIfDisposed();
                if (!isStopped) throw new InvalidOperationException("AsyncSubject is not completed yet");
                if (lastError != null) lastError.Throw();
                return lastValue;
            }
        }

        public bool HasObservers
        {
            get
            {
                return !(outObserver is EmptyObserver<T>) && !isStopped && !isDisposed;
            }
        }

        public bool IsCompleted { get { return isStopped; } }

        public void OnCompleted()
        {
            IObserver<T> old;
            T v;
            bool hv;
            lock (observerLock)
            {
                ThrowIfDisposed();
                if (isStopped) return;

                old = outObserver;
                outObserver = EmptyObserver<T>.Instance;
                isStopped = true;
                v = lastValue;
                hv = hasValue;
            }

            if (hv)
            {
                old.OnNext(v);
                old.OnCompleted();
            }
            else
            {
                old.OnCompleted();
            }
        }

        public void OnError(Exception error)
        {
            if (error == null) throw new ArgumentNullException("error");

            IObserver<T> old;
            lock (observerLock)
            {
                ThrowIfDisposed();
                if (isStopped) return;

                old = outObserver;
                outObserver = EmptyObserver<T>.Instance;
                isStopped = true;
                lastError = error;
            }

            old.OnError(error);
        }

        public void OnNext(T value)
        {
            lock (observerLock)
            {
                ThrowIfDisposed();
                if (isStopped) return;

                hasValue = true;
                lastValue = value;
            }
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer == null) throw new ArgumentNullException("observer");

            Exception ex;
            T v;
            bool hv;

            lock (observerLock)
            {
                ThrowIfDisposed();
                if (!isStopped)
                {
                    var listObserver = outObserver as ListObserver<T>;
                    if (listObserver != null)
                    {
                        outObserver = listObserver.Add(observer);
                    }
                    else
                    {
                        var current = outObserver;
                        if (current is EmptyObserver<T>)
                        {
                            outObserver = observer;
                        }
                        else
                        {
                            outObserver = new ListObserver<T>(new ImmutableList<IObserver<T>>(new[] { current, observer }));
                        }
                    }

                    return new Subscription(this, observer);
                }

                ex = lastError;
                v = lastValue;
                hv = hasValue;
            }

            if (ex != null)
            {
                observer.OnError(ex);
            }
            else if (hv)
            {
                observer.OnNext(v);
                observer.OnCompleted();
            }
            else
            {
                observer.OnCompleted();
            }

            return Disposable.Empty;
        }

        public void Dispose()
        {
            lock (observerLock)
            {
                isDisposed = true;
                outObserver = DisposedObserver<T>.Instance;
                lastError = null;
                lastValue = default(T);
            }
        }

        void ThrowIfDisposed()
        {
            if (isDisposed) throw new ObjectDisposedException("");
        }

        public bool IsRequiredSubscribeOnCurrentThread()
        {
            return false;
        }

        private class Subscription : IDisposable
        {
            readonly object _gate = new object();
            AsyncSubject<T> _parent;
            IObserver<T> _unsubscribeTarget;

            public Subscription(AsyncSubject<T> parent, IObserver<T> unsubscribeTarget)
            {
                this._parent = parent;
                this._unsubscribeTarget = unsubscribeTarget;
            }

            public void Dispose()
            {
                lock (_gate)
                {
                    if (_parent != null)
                    {
                        lock (_parent.observerLock)
                        {
                            var listObserver = _parent.outObserver as ListObserver<T>;
                            if (listObserver != null)
                            {
                                _parent.outObserver = listObserver.Remove(_unsubscribeTarget);
                            }
                            else
                            {
                                _parent.outObserver = EmptyObserver<T>.Instance;
                            }

                            _unsubscribeTarget = null;
                            _parent = null;
                        }
                    }
                }
            }
        }


#if (NET_4_6 || NET_STANDARD_2_0)

        /// <summary>
        /// Gets an awaitable object for the current AsyncSubject.
        /// </summary>
        /// <returns>Object that can be awaited.</returns>
        public AsyncSubject<T> GetAwaiter()
        {
            return this;
        }

        /// <summary>
        /// Specifies a callback action that will be invoked when the subject completes.
        /// </summary>
        /// <param name="continuation">Callback action that will be invoked when the subject completes.</param>
        /// <exception cref="ArgumentNullException"><paramref name="continuation"/> is null.</exception>
        public void OnCompleted(Action continuation)
        {
            if (continuation == null)
                throw new ArgumentNullException("continuation");

            OnCompleted(continuation, true);
        }

         void OnCompleted(Action continuation, bool originalContext)
        {
            //
            // [OK] Use of unsafe Subscribe: this type's Subscribe implementation is safe.
            //
            Subscribe/*Unsafe*/(new AwaitObserver(continuation, originalContext));
        }

        class AwaitObserver : IObserver<T>
        {
            private readonly SynchronizationContext _context;
            private readonly Action _callback;

            public AwaitObserver(Action callback, bool originalContext)
            {
                if (originalContext)
                    _context = SynchronizationContext.Current;

                _callback = callback;
            }

            public void OnCompleted()
            {
                InvokeOnOriginalContext();
            }

            public void OnError(Exception error)
            {
                InvokeOnOriginalContext();
            }

            public void OnNext(T value)
            {
            }

            private void InvokeOnOriginalContext()
            {
                if (_context != null)
                {
                    //
                    // No need for OperationStarted and OperationCompleted calls here;
                    // this code is invoked through await support and will have a way
                    // to observe its start/complete behavior, either through returned
                    // Task objects or the async method builder's interaction with the
                    // SynchronizationContext object.
                    //
                    _context.Post(c => ((Action)c)(), _callback);
                }
                else
                {
                    _callback();
                }
            }
        }

        /// <summary>
        /// Gets the last element of the subject, potentially blocking until the subject completes successfully or exceptionally.
        /// </summary>
        /// <returns>The last element of the subject. Throws an InvalidOperationException if no element was received.</returns>
        /// <exception cref="InvalidOperationException">The source sequence is empty.</exception>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Await pattern for C# and VB compilers.")]
        public T GetResult()
        {
            if (!isStopped)
            {
                var e = new ManualResetEvent(false);
                OnCompleted(() => e.Set(), false);
                e.WaitOne();
            }

            if (lastError != null)
            {
                lastError.Throw();
            }

            if (!hasValue)
                throw new InvalidOperationException("NO_ELEMENTS");

            return lastValue;
        }
#endif
    }
}
