using UnityEngine;
using System.Collections;
using Pathfinding;

public class modifyWeight : MonoBehaviour {
	public float delay = 0.8f;
	private float nextTime = 0.0f;

	// Use this for initialization
	void Start () {
		GraphUpdateObject graphUpdate = new GraphUpdateObject (collider.bounds);
		graphUpdate.addPenalty = 20000;
		AstarPath.active.UpdateGraphs (graphUpdate);
	}
	
	// Update is called once per frame
	void Update () {
//		if (Time.time > nextTime) {
//			nextTime = Time.time + delay;
//		
//			GraphUpdateObject graphUpdate = new GraphUpdateObject (collider.bounds);
//			graphUpdate.addPenalty = 10000;
//			AstarPath.active.UpdateGraphs (graphUpdate);
//		}
	}
}
