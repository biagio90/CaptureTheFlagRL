using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public GameObject playerShooter;
	public string enemyTag;
	public int killProbability;

	public float speed;

	public Vector3 destination;
	public bool go = false;

	public PlayerAction action;

	void FixedUpdate () {
		if (go) {
			go = false;
			Vector3 direction = (destination - transform.position).normalized;
			rigidbody.velocity = direction * speed;

			//action = playerShooter.GetComponent<PlayerAction>();
		}
	}

	
	void OnTriggerEnter(Collider other) 
	{
		//	Instantiate(explosion, transform.position, transform.rotation);
		if (other.tag == enemyTag)
		{
			int probability = Random.Range(0, 100);
			if(probability < killProbability){
				PlayerController pc = other.gameObject.GetComponent<PlayerController>();
				if (pc != null) {
					pc.killPlayer();
					if(action!=null && action.getCurrentAction() != constantRL.Actions.GET_FLAG_AND_SCORE)
						action.enemyKilled(pc.hasFlag);
				}
			}
		}

		if (other.tag != "bullet" && other.tag != "flagTeam1" && other.tag != "flagTeam2" ){
			Destroy(gameObject);
		}
	}
}
