﻿using UnityEngine;
using System.Collections;
 
public class EnemyMovement : MonoBehaviour
{
	public float playerAttraction;
	public float moveSpeed;
	public float chaseDistance;
	public float yHeight;
	public float threatDistance;
	public Animator anim; 
	public float attackSpeed;

	private Transform player;               // Reference to the player's position.
	private float mostFear = 0.0f;
	private Collider mostFeared;  
	private bool isAlive = true;

	void Awake ()
	{
		// Set up the references.
		player = GameObject.FindGameObjectWithTag ("Player").transform;
	}
	void Update ()
	{
		if (DistFromPlayer() < threatDistance)
		{
			if (isAlive)
			{
				attackPlayer();
			}
			lookAt(player);
			transform.position += transform.forward*attackSpeed*Time.deltaTime;

		}
		else if (mostFear >= playerAttraction) 
		{
			lookAt(mostFeared.transform);
			transform.position += -1 * gameObject.transform.forward*moveSpeed*Time.deltaTime;
			gameObject.transform.LookAt(player);
			mostFear = 0.0f;
		}
		else if(DistFromPlayer() <= chaseDistance)
		{
			lookAt(player);
			transform.position += transform.forward*moveSpeed*Time.deltaTime;
			//print (mostFear);
		}
		// Otherwise...
		else
		{
			lookAt(player);
//			Vector2 Temp = Random.insideUnitCircle;
//			float TempDistance = Random.Range(0, 10);
//			Temp = Temp * TempDistance;
//			Vector3 MovePos = new Vector3(Temp.x, 0, Temp.y);
//			//Vector3 MovePos = transform.position + FinalTemp;
//			transform.LookAt(MovePos);
//			transform.Translate (MovePos*moveSpeed*Time.deltaTime);
//			transform.LookAt(player);
		}
		//anchors character to Y position.
		//transform.position = new Vector3 (transform.position.x, yHeight, transform.position.z);
	}
	void OnTriggerStay (Collider other)
	{
		if (other.tag != "Lighting")
		{
			return;
		}
		GetFear (other);
	}
	void GetFear(Collider light)
	{
		float distance = Vector3.Distance (gameObject.transform.position, light.transform.position);
		if (Physics.Raycast (gameObject.collider.transform.position, light.transform.position) != true)
		{
			return;
		} 
		float fear = light.light.intensity *(light.light.range / distance);
		if(fear > mostFear)
		{
			mostFear = fear;
				mostFeared = light;
		}
		return;
	}
	float DistFromPlayer()
	{
		return Vector3.Distance (gameObject.transform.position, player.position);
	}
	void attackPlayer()
	{
		anim = GetComponent <Animator> ();
		anim.SetTrigger ("Dead");
		threatDistance = 100.0f;
		isAlive = false;
	}
	void lookAt(Transform other)
	{
		Vector3 otherPos = other.position;
		otherPos.y = yHeight;
		gameObject.transform.LookAt(otherPos);
	}
}

