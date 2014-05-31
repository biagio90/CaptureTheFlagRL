using UnityEngine;
using System.Collections;

public class DummyController : MonoBehaviour {
	public int id = 0;

	private MovePlayer mover;
	private PlayerController player;
	public string flagTag = "flagTeam2";
	public GameController controller;
	public GameObject flagPrefabs;
	private Vector3 flagPos;

	// Use this for initialization
	void Start () {
		player = GetComponent<PlayerController> ();
		flagPos = GameObject.FindGameObjectWithTag (flagTag).transform.position;
		mover = GetComponent<MovePlayer> ();

	}
	
	// Update is called once per frame
	void Update () {
		checkForScore ();

		if (mover.arrived){
			if(player.hasFlag){
				mover.moveTo(player.myBase.transform.position);
				return;
			}

			if(id == 0){
				// only the original go to catch the flag
				GameObject flag = GameObject.FindGameObjectWithTag (flagTag);
				if(flag != null) {
					mover.moveTo(flag.transform.position);
				}
				return;
			}

			Vector3 target = getRandomPosition();
			mover.moveTo(target);
			
		}
	}

	private void checkForScore(){
		if(player.hasFlag &&
		   Vector3.Distance(transform.position, 
		                    player.myBase.transform.position) < 1){
			controller.increaseScore(tag);
			gameObject.transform.Find("flag").gameObject.SetActive(false);
			player.hasFlag=false;
			
			Instantiate(flagPrefabs, flagPos, Quaternion.identity);
			player.updateEvent (constantRL.Events.makeScore);
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
