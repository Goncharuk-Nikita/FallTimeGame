using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

[RequireComponent(typeof(LineRenderer)),
RequireComponent(typeof(Rigidbody2D))]
public class Rope : MonoBehaviour
{
	public const int BaseDrawSegmentsCount = 2;
	public const float LegJointDistance = 0.1f;
	
	
	[SerializeField] private Prefab ropeSegmentPrefab;

	public float maxRopeSegmentLength = 1.0f;
	public float ropeSpeed = 4.0f;

	
	public bool isIncreasing { get; set; }
	public bool isDecreasing { get; set; }

	
	private GameObject _connectedObject;
	
	private List<RopeSegment> _ropeSegments = new List<RopeSegment>();

	private Rigidbody2D _body;
	private LineRenderer _lineRenderer;

	private SpringJoint2D _connectedSpringJoint;

	

#if UNITY_EDITOR
	private void OnValidate()
	{
		_lineRenderer = GetComponent<LineRenderer>();
		if (_lineRenderer == null)
		{
			Debug.LogError("LineRenderer component not found!", this);
		}

		_body = GetComponent<Rigidbody2D>();
		if (_body == null)
		{
			Debug.LogError("Rigidbody2D component not found!", this);
		}
	}
#endif


	private void Awake()
	{
		ropeSegmentPrefab.Install();

		this.UpdateAsObservable()
			.Where((unit, i) => _connectedObject != null)
			.Subscribe(unit => UpdateRope());
	}

	
	private void UpdateRope() 
	{
		if (isIncreasing)
		{
			IncreaseRope();
		}
		else if (isDecreasing)
		{
			DecreaseRope();
		}

		RenderLine();
	}

	
	private void IncreaseRope()
	{	
		var firstSegmentSpringJoint = _ropeSegments[0].springJoint;
		
		if (firstSegmentSpringJoint.distance >= maxRopeSegmentLength) 
		{
			CreateRopeSegment();
		} 
		else 
		{
			firstSegmentSpringJoint.distance += 
				ropeSpeed * Time.deltaTime;
		}
	}
	
	private void DecreaseRope()
	{	
		var firstSegmentSpringJoint = _ropeSegments[0].springJoint;
		
		if (firstSegmentSpringJoint.distance <= 0.005f) 
		{
			RemoveRopeSegment();
		} 
		else 
		{
			firstSegmentSpringJoint.distance -= 
				ropeSpeed * Time.deltaTime;
		}
	}
	
	private void RenderLine()
	{
		_lineRenderer.positionCount = _ropeSegments.Count + BaseDrawSegmentsCount;
			
		_lineRenderer.SetPosition(0,
			this.transform.position);
			
		for (int i = 0; i < _ropeSegments.Count; i++) 
		{
			_lineRenderer.SetPosition(i + 1,
				_ropeSegments[i].transform.position);
		}
			
		_lineRenderer.SetPosition(
			_ropeSegments.Count + 1,
			_connectedObject.transform.TransformPoint(_connectedSpringJoint.anchor)
		);
	}
	

	public void SetConnectedObject(GameObject go)
	{
		_connectedObject = go;
		
		_connectedSpringJoint = _connectedObject.GetComponent<SpringJoint2D>();
	}
	

	public void ResetLength() 
	{
		foreach (var segment in _ropeSegments)
		{
			PrefabPoolingSystem.Despawn(segment.gameObject);
		}
		
		_ropeSegments.Clear();
		
		isDecreasing = isIncreasing = false;
		InitializeRope();
	}

	private void InitializeRope()
	{
		var ropeSegment = SpawnRopeSegment();
		
		_connectedSpringJoint.connectedBody
			= ropeSegment.body;
		_connectedSpringJoint.distance = LegJointDistance;

		ropeSegment.SetSpringDistance(maxRopeSegmentLength);
		ropeSegment.SetConnectedBody(_body);
		
		AddFirst(ropeSegment);
	}

	
	private void CreateRopeSegment()
	{
		var ropeSegment = SpawnRopeSegment();
		
		PushSegment(ropeSegment);
	}

	private RopeSegment SpawnRopeSegment()
	{
		var prefabData = ropeSegmentPrefab.Spawn(
			this.transform.position);

		return (RopeSegment) prefabData.poolableComponent;
	}
	
	private void PushSegment(RopeSegment ropeSegment)
	{
		var firstSegment = _ropeSegments[0];

		firstSegment.SetConnectedBody(ropeSegment.body);
		ropeSegment.SetConnectedBody(_body);

		AddFirst(ropeSegment);
	}

	private void AddFirst(RopeSegment ropeSegment)
	{
		_ropeSegments.Insert(0, ropeSegment);
	}


	private void RemoveRopeSegment() {
		
		if (_ropeSegments.Count < 2) 
		{
			return;
		}
		
		var firstSegment = _ropeSegments[0];
		var secondSegment = _ropeSegments[1];
		
		secondSegment.SetConnectedBody(_body);
		
		DestroySegment(firstSegment);
	}

	private void DestroySegment(RopeSegment ropeSegment)
	{
		_ropeSegments.Remove(ropeSegment);
		PrefabPoolingSystem.Despawn(ropeSegment.gameObject);
	}
}
