using UnityEngine;
using System.Collections;
using Pathfinding;

public class MovePlayer : MonoBehaviour {

	private Vector3 target;
	
	//The calculated path
	public Path path;
	public float speed = 5;
	public float turnSpeed = 5f;
	//The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = 1;
	//The waypoint we are currently moving towards
	private int currentWaypoint = 0;

	// Use this for initialization
	void Start () {
	
	}

	public void moveTo(Vector3 destination){
		reset ();
		target = destination;
		Path p = ABPath.Construct (transform.position, target, OnPathComplete);
		AstarPath.StartPath (p);
		AstarPath.WaitForPath (p);
	}
	
	public void OnPathComplete (Path p) {
		if (!p.error) {
			path = p;
			currentWaypoint = 0;
		}
	}

	// Update is called once per frame
	void Update () {
		if (path == null) {
			//We have no path to move after yet
			return;
		}

		Vector3 point = target;
		if (currentWaypoint < path.vectorPath.Count) {
			point = path.vectorPath [currentWaypoint];
		} else if(currentWaypoint > path.vectorPath.Count){
			reset();
			return;
		}

		point.y = transform.position.y;
		makeMove (point);
		if (Vector3.Distance (transform.position,point) < nextWaypointDistance) {
			currentWaypoint++;
			return;
		}	
	}

	private void makeMove(Vector3 destination) {
		Vector3 direction = (destination - transform.position).normalized;

		Quaternion _lookRotation = Quaternion.LookRotation (direction);
		transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, turnSpeed * Time.deltaTime);
		
		rigidbody.velocity = direction * speed;
	}

	public void reset(){
		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;
		path = null;

	}
}
