using UnityEngine;
using System.Collections;
using Pathfinding;

public class AstarAIrandom : MonoBehaviour {
	private Vector3 target;
	
	//The calculated path
	public Path path;
	public float speed = 5;
	public float turnSpeed = 5f;
	//The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = 1;
	//The waypoint we are currently moving towards
	private int currentWaypoint = 0;
	
	public void Start () {
		Vector3 destination = getRandomPosition ();
		destination.y = 0;
		Vector3 pos = transform.position;
		pos.y = 0;
		Path p = ABPath.Construct (pos, destination, OnPathComplete);
		AstarPath.StartPath (p);
		AstarPath.WaitForPath (p);

	}

	public void OnPathComplete (Path p) {
//		Debug.Log ("Path lenght: "+p.path.Count);
		if (!p.error) {
			/*for(int i=0; i<p.path.Count; i++){
				Debug.Log("point: "+(Vector3)p.vectorPath[i]);
			}*/
			path = p;
			//Reset the waypoint counter
			currentWaypoint = 0;
		}
	}
	public void FixedUpdate () {
		if (path == null) {
			//We have no path to move after yet
			return;
		}

		if (currentWaypoint >= path.vectorPath.Count) {
			//Debug.Log ("End Of Path Reached");
			rigidbody.velocity = Vector3.zero;
			rigidbody.angularVelocity = Vector3.zero;
			path = null;

			Vector3 destination = getRandomPosition ();
			destination.y = 0;
			Vector3 pos = transform.position;
			pos.y = 0;
			Path p = ABPath.Construct (pos, destination, OnPathComplete);
			AstarPath.StartPath (p);
			AstarPath.WaitForPath(p);

			return;
		}
		Vector3 point = path.vectorPath [currentWaypoint];
		point.y = transform.position.y;
		makeMove (point);
		if (Vector3.Distance (transform.position,point) < nextWaypointDistance) {
			currentWaypoint++;
			//			Debug.Log("Current point: "+ currentWaypoint);
			return;
		}
	}
	
	private void makeMove(Vector3 destination) {
		destination.y = transform.position.y;
		Vector3 direction = (destination - transform.position).normalized;
//		float sight = (destination - transform.position).magnitude;
//		bool collision = false;
		//direction = avoidCollision (direction, sight, ref collision);
		
		Quaternion _lookRotation = Quaternion.LookRotation (direction);
		transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, turnSpeed * Time.deltaTime);
		
		rigidbody.velocity = direction * speed;
	}

	private Vector3 getRandomPosition(){
		bool free = false;
		Vector3 ret = new Vector3 ();
		ret.y = 1;
		while(!free) {
			ret.x = Random.Range(-27, 27);
			ret.z = Random.Range(-20, 20);
			free = isFree(ret);
		}
		return ret;
	}

	private bool isFree (Vector3 p) {
		Collider[] hitColliders = Physics.OverlapSphere(p, 0.5f);
		int i = 0;
		bool collision = false;
		while (i < hitColliders.Length) {
			if (hitColliders[i] != this.collider){ 
				collision = true;
			}
			i++;
		}
		
		return !collision;
	}
}
