using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour
{
	public Transform Target;
	public float XSpeed;
	public float YSpeed;
	public float ZoomSpeed;

	public float XMinLimit;
	public float XMaxLimit;

	public float MinDistance;
	public float MaxDistance;

	public float DefaultXAngle;

	[HideInInspector]
	public Transform Transform;
	private float currentDistance;
	private float targetDistance;
	private float y;
	private float x;

	private void Awake ()
	{
		Application.targetFrameRate = 60;

		Transform = transform;
		targetDistance = MaxDistance / 3;
	}

	private void Start ()
	{
		x = DefaultXAngle;
		y = 180 + Target.eulerAngles.y;
	}

	private void LateUpdate ()
	{
		if (Target)
		{
			if (Input.GetMouseButton(1))
			{
				Screen.lockCursor = true;

				y += Input.GetAxis("Mouse X") * XSpeed;
				x -= Input.GetAxis("Mouse Y") * YSpeed;
			}
			else
			{
				Screen.lockCursor = false;
			}

			targetDistance = Mathf.Clamp(targetDistance - Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed, MinDistance, MaxDistance);
			currentDistance = Mathf.Lerp(currentDistance, targetDistance, Time.deltaTime * 5);

			x = ClampAngle(x, XMinLimit, XMaxLimit);

			RaycastHit hit;
			if (Physics.SphereCast(Target.transform.position, .15f, (Transform.position - (Target.transform.position)).normalized, out hit, currentDistance, 1 << 8)) currentDistance = hit.distance;

			Transform.position = new Vector3(
				Target.position.x + currentDistance * Mathf.Sin(y * Mathf.Deg2Rad) * Mathf.Cos(x * Mathf.Deg2Rad),
				Target.position.y + currentDistance * Mathf.Sin(x * Mathf.Deg2Rad),
				Target.position.z + currentDistance * Mathf.Cos(y * Mathf.Deg2Rad) * Mathf.Cos(x * Mathf.Deg2Rad)
			);

			Transform.LookAt(Target.position);
		}
	}

	private float ClampAngle (float angle, float min, float max)
	{
		if (angle < -360F) angle += 360F;
		if (angle > 360F) angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}
}