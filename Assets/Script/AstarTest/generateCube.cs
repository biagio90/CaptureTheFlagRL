using UnityEngine;
using System.Collections;

public class generateCube : MonoBehaviour {
	public int num_players;
	public GameObject player;

	// Use this for initialization
	void Start () {
		for (int i=0; i<num_players-1; i++) {
			Instantiate(player, getRandomPosition(), Quaternion.identity);
		}
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
