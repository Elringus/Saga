using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	public Transform Transform;
	public float MoveSpeed;
	public float ClimbSpeed;
	public float PointMoveLimit;
	public bool IsAvatar = true;

	private bool isClimbing;
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

			if (isClimbing)
			{
				if (Vector3.Distance(Transform.position, currentMovePoint) > PointMoveLimit)
				{
					controller.Move((currentMovePoint - Transform.position).normalized * Time.deltaTime * ClimbSpeed);
				}

				RaycastHit? vertHit = CanClimb();
				if (!vertHit.HasValue)
				{
					isClimbing = false;
				}
				else
				{
					Transform.LookAt(vertHit.Value.point, vertHit.Value.normal);
					Transform.eulerAngles = new Vector3(0, Transform.eulerAngles.y, 0);

					if (Vector3.Distance(Transform.position, vertHit.Value.point) > .5f) 
						controller.Move(Transform.TransformDirection(Vector3.forward) * Time.deltaTime * 5f);
				}
			}
			else
			{
				if (Vector3.Distance(Transform.position, currentMovePoint) > PointMoveLimit)
				{
					Transform.LookAt(currentMovePoint);
					Transform.eulerAngles = new Vector3(0, Transform.eulerAngles.y, 0);
					controller.Move(Transform.TransformDirection(Vector3.forward) * Time.deltaTime * MoveSpeed);
				}
				if (!controller.isGrounded) controller.Move(Vector3.down * Time.deltaTime * 9f);
			}

			if (Input.GetKeyDown(KeyCode.LeftShift))
			{
				if (isClimbing)
				{
					isClimbing = false;
					return;
				}

				RaycastHit? vertHit = CanClimb();
				if (vertHit.HasValue)
				{
					isClimbing = true;
					Transform.position = vertHit.Value.point + (Transform.position - vertHit.Value.point).normalized;
					Transform.LookAt(vertHit.Value.point, vertHit.Value.normal);
					Transform.eulerAngles = new Vector3(0, Transform.eulerAngles.y, 0);
				}
			}
		}

		currentMoveSpeed = (Transform.position - prevPosition).sqrMagnitude;
		prevPosition = Transform.position;

		Animate();
	}

	private RaycastHit? CanClimb ()
	{
		RaycastHit hit;
		if (Physics.Raycast(Transform.position, Transform.TransformDirection(Vector3.forward), out hit, 1.5f, 1 << 8)) return hit;
		else return null;
	}

	private void Animate ()
	{
		animator.SetFloat("Forward", currentMoveSpeed * MoveSpeed * 50, .1f, Time.deltaTime);
		animator.SetBool("InAir", !controller.isGrounded);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            
            animator.SetBool("Fighting", true);
            if (Random.Range(0, 2) == 0)
            {
                animator.SetBool("Attack1", true);
                Debug.Log("Attack1");
            }
            else
            {
                animator.SetBool("Attack2", true);
                Debug.Log("Attack2");
            }
        }
		animator.SetBool("Climbing", isClimbing);
		//animator.SetBool("Fighting", InBattle);
	}
}