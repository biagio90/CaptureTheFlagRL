using UnityEngine;
using System.Collections;
using Pathfinding;

[RequireComponent(typeof(Seeker))]
public class AstarAI : MonoBehaviour {
	public Transform target;
//	private Seeker seeker;

	//The calculated path
	public Path path;
	public float speed = 100;
	public float turnSpeed = 2f;
	//The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = 1;
	//The waypoint we are currently moving towards
	private int currentWaypoint = 0;

	public void Start () {
//		seeker = GetComponent<Seeker>();
		//Start a new path to the targetPosition, return the result to the OnPathComplete function
		//seeker.StartPath (transform.position,target.position, OnPathComplete);
		Vector3 destination = target.position;
		destination.y = transform.position.y;
		Path p = ABPath.Construct (transform.position, destination, OnPathComplete);
		AstarPath.StartPath (p);
	}
	public void OnPathComplete (Path p) {
		Debug.Log ("Path lenght: "+p.path.Count);
		if (!p.error) {
			for(int i=0; i<p.path.Count; i++){
				Debug.Log("point: "+(Vector3)p.vectorPath[i]);
			}
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

			return;
		}
		//Direction to the next waypoint
		Vector3 point = path.vectorPath [currentWaypoint];
		point.y = transform.position.y;
		makeMove (point);
//		Vector3 dir = (point-transform.position).normalized;
//		dir *= speed * Time.deltaTime;
//		dir.y = 0;
//		transform.position += dir;
		//Check if we are close enough to the next waypoint
		//If we are, proceed to follow the next waypoint
		if (Vector3.Distance (transform.position,point) < nextWaypointDistance) {
			currentWaypoint++;
//			Debug.Log("Current point: "+ currentWaypoint);
			return;
		}
	}

	private void makeMove(Vector3 destination) {
		Vector3 direction = (destination - transform.position).normalized;
//		float sight = (destination - transform.position).magnitude;
//		bool collision = false;
		//direction = avoidCollision (direction, sight, ref collision);
		
		Quaternion _lookRotation = Quaternion.LookRotation (direction);
		transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, turnSpeed * Time.deltaTime);
		
		rigidbody.velocity = direction * speed;
	}
}
