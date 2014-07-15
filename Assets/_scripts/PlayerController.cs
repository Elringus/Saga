using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	public Transform Transform;
	public float MoveSpeed;
	public float PointMoveLimit;
	public bool IsAvatar = true;

	private float currentMoveSpeed;
	private Vector3 prevPosition;
	private Vector3 currentMovePoint;
	private CharacterController controller;
	private Animator animator;

	private void Awake () 
	{
		Transform = transform;
		controller = GetComponent<CharacterController>();
		animator = GetComponentInChildren<Animator>();
		currentMovePoint = Transform.position;
	}

	private void Update () 
	{
		if (IsAvatar && GUIUtility.hotControl == 0)
		{
			if (Input.GetKeyUp(KeyCode.Alpha1)) ChatGUI.chat.Attack(GetComponent<Player>().ID, 1);

			RaycastHit hit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, (1 << 8) | (1 << 9)))
			{
				if (Input.GetMouseButton(0))
				{
					if (hit.collider.gameObject.layer == 8) currentMovePoint = hit.point;
					//if (hit.collider.gameObject.layer == 9 && hit.collider.gameObject.GetComponent<Player>()) Target = hit.collider.gameObject.GetComponent<Actor>();
				}

				if (Input.GetMouseButtonDown(1))
				{
					//if (hit.collider.gameObject.layer == 9) ((IInteractiveObject)hit.collider.GetComponent(typeof(IInteractiveObject))).Activate(this);
				}
			}

			if (Vector3.Distance(Transform.position, currentMovePoint) > PointMoveLimit)
			{
				Transform.LookAt(currentMovePoint);
				Transform.eulerAngles = new Vector3(0, Transform.eulerAngles.y, 0);
				controller.Move(Transform.TransformDirection(Vector3.forward) * Time.deltaTime * MoveSpeed);
			}
		}

		if (!controller.isGrounded) controller.Move(Vector3.down * Time.deltaTime * 9f);

		currentMoveSpeed = (Transform.position - prevPosition).sqrMagnitude;
		prevPosition = Transform.position;

		Animate();
	}

	private void Animate ()
	{
		animator.SetFloat("Forward", currentMoveSpeed * MoveSpeed * 50, .1f, Time.deltaTime);
		animator.SetBool("InAir", !controller.isGrounded);
		//Animator.SetBool("Climbing", IsClimbing);
		//Animator.SetBool("Fighting", InBattle);
	}
}