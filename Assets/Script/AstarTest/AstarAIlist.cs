using UnityEngine;
using System.Collections;
using Pathfinding;

public class AstarAIlist : MonoBehaviour {
	private Vector3[] target;
	
	//The calculated path
	public Path path;
	public float speed = 5;
	public float turnSpeed = 5f;
	//The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = 1;
	//The waypoint we are currently moving towards
	private int currentWaypoint = 0;

	private int index_target = 0;

	// Use this for initialization
	void Start () {
		target = new Vector3[3] {
			new Vector3(-20, 1, -20),
			new Vector3(20, 1, 20),
			new Vector3(0, 1, 0)
		};

		Path p = ABPath.Construct (transform.position, target[0], OnPathComplete);
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

			index_target++;
			if (index_target < target.Length){
				Path p = ABPath.Construct (transform.position, target[index_target],
				                           OnPathComplete);
				AstarPath.StartPath (p);
				AstarPath.WaitForPath(p);
			}
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
		Vector3 direction = (destination - transform.position).normalized;
//		float sight = (destination - transform.position).magnitude;
//		bool collision = false;
		//direction = avoidCollision (direction, sight, ref collision);
		
		Quaternion _lookRotation = Quaternion.LookRotation (direction);
		transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, turnSpeed * Time.deltaTime);
		
		rigidbody.velocity = direction * speed;
	}

}
