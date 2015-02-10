using UnityEngine;
using System.Collections;

using System;
using System.Threading;
 
public class EnemyMovement : MonoBehaviour
{
	public float playerAttraction;
	public float moveSpeed;
	public float chaseDistance;
	public float killDistance;
	public float yHeight;
	public float threatDistance; 
	public float attackSpeed;
	public int walkMultiplier;
	public float rotationSpeed;
	public float fearMultiplier;
	public AudioClip killSound;
	public bool gameOver = false;

	private Animator anim;
	private Transform player;               // Reference to the player's position.
	private Collider mostFeared;  
	private float mostFear = 0.0f;
	private Vector3 walkDirection;
	private int walkTimer;
	private bool isStanding = true;
//	private int waitTimer;
	//private int fleeing;
	private bool isAlive = true;
	private bool stopPursue;
	private bool killOwner;
	private Transform killCam;

//	private RaycastHit testHit;

	void Awake ()
	{
		// Set up the references.
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		anim = GetComponent <Animator> ();
		anim.Play ("Idle");
		killCam = GameObject.Find("KillCam").camera.transform;
	}
	void Update ()
	{

		if (stopPursue == true) 
		{
			return;
		}

		if(DistFromPlayer() < killDistance)
		{
			if (gameOver == false){
				isAlive = false;
				gameOver = true;
				Thread.Sleep(200);
				BroadcastMessage("CharacterDeath", this.transform);
			}
		}

		else if (DistFromPlayer() < threatDistance)
		{
			if (RaycastToOther(player.position, "Player") == true)
			{
				anim.Play ("Move");
				if (isAlive)
				{
					isAlive = false;
					attackPlayer();

					killOwner = true;
					audio.Play();
					BroadcastMessage("CharacterKill", this.transform);
				}			
				playerLookAt(killCam);
				gameObject.transform.LookAt(player);
				transform.position += transform.forward*attackSpeed*Time.deltaTime;
			}
			//Debug.DrawLine(transform.position, testHit.point);
		}

		else if (mostFear >= playerAttraction) //|| fleeing > 0) 
		{
			lookAt(mostFeared.transform.position);
			transform.position += -1 * gameObject.transform.forward*moveSpeed/2*Time.deltaTime;
			transform.position = new Vector3(transform.position.x, yHeight, transform.position.z);
			mostFear = 0.0f;
			isStanding = true;
//			if  (fleeing <= 0)
//			{
//				fleeing = UnityEngine.Random.Range(1, 100);
//			}
//			fleeing--;
		}

		else if(DistFromPlayer() <= chaseDistance)
		{
			lookAt(player.position);
			transform.position += transform.forward*moveSpeed*Time.deltaTime;
			transform.position = new Vector3(transform.position.x, yHeight, transform.position.z);
			isStanding = true;
		}
		else
		{
			//Idle state
			if (isStanding)
			{
				randomWalk();
			}
			else 
			{
				lookAt(walkDirection);
				transform.position += transform.forward*moveSpeed*Time.deltaTime;
				transform.position = new Vector3(transform.position.x, yHeight, transform.position.z);
				walkTimer--;
				if (walkTimer <= 0)
				{
					isStanding = true;				
				}
			}
		}
	}
//	void OnDrawGizmosSelected() {
//		Gizmos.color = Color.yellow;
//		Gizmos.DrawSphere(walkDirection, 1);
//	}

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
		if (RaycastToOther (light.transform.position, "Lighting") == true)
		{
			float fear = light.light.intensity * (light.light.range / distance) * fearMultiplier;
			if (fear > mostFear) 
			{
				mostFear = fear;
				mostFeared = light;
			}
		}
		return;
	
	}

	float DistFromPlayer()
	{
		return Vector3.Distance (gameObject.transform.position, player.position);
	}

	void lookAt(Vector3 other)
	//smooothly rotates gameObject
	{
		Vector3 otherPos = other - transform.position;
		otherPos.y = 0.0f;
		Quaternion lookRotation = Quaternion.LookRotation(otherPos);
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
//		Debug.DrawLine(gameObject.collider.transform.position, other);
	}

	void randomWalk()
	//sets a new Destination in vicinty of GameObject
	{
		Vector3 Temp = UnityEngine.Random.insideUnitSphere;
		float TempDistance = UnityEngine.Random.Range(0, 10);
		walkDirection = transform.position - (Temp * TempDistance);
		walkDirection.y = yHeight;
		isStanding = false;
		walkTimer = Mathf.RoundToInt(TempDistance)*walkMultiplier;
	}
	
	bool RaycastToOther(Vector3 otherPos, string tag)
	//returns to if Raycast hits target and target is the right tag
	{
		float distance = Vector3.Distance (transform.position, otherPos);
		Vector3 direction = otherPos - gameObject.collider.transform.position;
		bool result = false;
		RaycastHit testHit;

		// Does the ray intersect the target?
		if (Physics.Raycast (gameObject.collider.transform.position, direction, out testHit, distance)) 
		{
//			print (testHit.collider.tag);
			if(testHit.collider.tag == tag)
			{
				result = true;
			}
//			Debug.DrawLine(gameObject.collider.transform.position, testHit.point);
		}
//		print (result);
		return result;
	}

	void attackPlayer()
	{
		anim.SetTrigger ("Dead");
		threatDistance = threatDistance * 100.0f;
	}
	void CharacterKill()
	{
		if (killOwner == false) 
		{
			stopPursue = true;
		}
	}
	void playerLookAt(Transform killCam)
	{
		Vector3 killerPos = this.transform.position;
		killerPos.y = yHeight + 1;
		killCam.transform.LookAt(killerPos);
	}
}

