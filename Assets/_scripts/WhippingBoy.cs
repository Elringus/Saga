using UnityEngine;
using System.Collections;

public class WhippingBoy : MonoBehaviour
{
	public Animator Animator;

	private Transform player;

	private void Awake () 
	{
		Animator = GetComponentInChildren<Animator>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	private void Update () 
	{
		transform.LookAt(player);
		transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
	}
}