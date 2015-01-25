using UnityEngine;
using System.Collections;
 
public class EnemyMovement : MonoBehaviour
{
	public float playerAttraction;
	public float moveSpeed;
	public float chaseDistance;
	public float yHeight;
	public float threatDistance; 
	public float attackSpeed;
	public int walkMultiplier;

	private Transform player;               // Reference to the player's position.
	private float mostFear = 0.0f;
	private Collider mostFeared;  
	private bool isAlive = true;
	private bool isStanding = true;
	private int walkTimer;
	private Vector3 walkDirection;


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
			isStanding = true;
		}
		else if(DistFromPlayer() <= chaseDistance)
		{
			lookAt(player);
			transform.position += transform.forward*moveSpeed*Time.deltaTime;
			isStanding = true;
		}
		// Otherwise...
		else
		{
			if (isStanding)
			{
				randomWalk();
			}
			else 
			{
				gameObject.transform.LookAt(walkDirection);
			}
			transform.position += transform.forward*moveSpeed*Time.deltaTime;
			walkTimer--;
			if (walkTimer <= 0)
			{
				isStanding = true;
			}
			lookAt(player);
		}
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
		Animator anim = GetComponent <Animator> ();
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
	void randomWalk()
	{
		Vector3 Temp = Random.insideUnitSphere;
		float TempDistance = Random.Range(0, 10);
		walkDirection = transform.position - (Temp * TempDistance);
		walkDirection.y = yHeight;
		gameObject.transform.LookAt(walkDirection);
		isStanding = false;
		walkTimer = Mathf.RoundToInt(TempDistance)*walkMultiplier;
	}
}

