using UnityEngine;
using System.Collections;

public class TakeMe : MonoBehaviour {
	
	private Transform taker;
	private Transform t;
	private Transform player;
	private Transform leftHand;
	private int smoothFactor = 20;
	
	// Use this for initialization
	void Awake () {
		t = this.transform;
		
		player = GameObject.FindGameObjectWithTag("Player").transform;
		leftHand = player.FindChild ("leftHand");
	}
	
	// Update is called once per frame
	void Update () {


		if(Input.GetMouseButtonDown(0) || Input.GetKeyDown("g")) {

			var distance = Vector3.Distance(t.position, player.position);
			Debug.Log("distance from " + distance);

			if (distance < 2.0){
				transform.parent = player;

				//transform.rotation = Quaternion.Slerp(transform.rotation, leftHand.rotation, smoothFactor * Time.deltaTime);
				//transform.position = Vector3.MoveTowards(transform.position, leftHand.position, smoothFactor * Time.deltaTime);
				transform.rotation = leftHand.rotation;
				transform.position = leftHand.position;

				rigidbody.useGravity = false;
				rigidbody.isKinematic = false;
				collider.isTrigger = true;
			}
			Debug.Log("left click");

			
		} else if(Input.GetMouseButtonDown(1) || Input.GetKeyDown("t")) {
			
			Debug.Log("right click");
			transform.parent = null;
			rigidbody.useGravity = true;
			rigidbody.isKinematic = false;
			collider.isTrigger = false;
			
		}
		
	} 
}