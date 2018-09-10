using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class FollowCamera2D : MonoBehaviour
{
	private CinemachineVirtualCamera _virtualCamera;
	
#if UNITY_EDITOR
	private void OnValidate()
	{
		_virtualCamera = GetComponent<CinemachineVirtualCamera>();
		if (_virtualCamera == null)
		{
			Debug.LogError("CinemachineVirtualCamera component not found", this);
		}
	}
#endif


	public void ChangeFollowTarget(Transform target)
	{
		_virtualCamera.Follow = target;
		_virtualCamera.LookAt = target;
	}
}
