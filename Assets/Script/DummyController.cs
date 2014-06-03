﻿using UnityEngine;
using System.Collections;

public class DummyController : MonoBehaviour {
	public bool circle = false;

	public int id = 0;

	private MovePlayer mover;
	private PlayerController player;
	public string flagTag = "flagTeam2";
	public GameController controller;
	public GameObject flagPrefabs;
	private Vector3 flagPos;

	private string enemyTag;
	private bool isFollowing = false;

	//for respawn
	public float timeToRespoun = 5;
	//public bool dead = false;
	private float timer = 0.0f;
	
	// Use this for initialization
	void Start () {
		player = GetComponent<PlayerController> ();
		flagPos = GameObject.FindGameObjectWithTag (flagTag).transform.position;
		mover = GetComponent<MovePlayer> ();
		enemyTag = (tag == "team1") ? "team2" : "team1";
	}

	private bool follow () {
		Vector3 direction = transform.TransformDirection(Vector3.forward).normalized;
		RaycastHit hit = new RaycastHit ();
		Ray ray = new Ray(transform.position, direction);
		
		for (float angle = -40; angle < 40; angle += 1.0f) {
			ray.direction = Quaternion.Euler(0, angle, 0) * direction;
			if (Physics.Raycast(ray, out hit, 10)) {
				if (hit.collider.tag == enemyTag ) {
					mover.moveTo(hit.transform.gameObject.transform.position);
					isFollowing = true;
					return true;
				}
			}
		}

		return false;
	}
	
	// Update is called once per frame
	void Update () {
		if(!player.dead){
			checkForScore ();

			if(!isFollowing && !player.hasFlag && id != 0){
				if(follow()) return;
			} else {
				if(mover.arrived){
					isFollowing = false;
				}
			}

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

				Vector3 target = circle ? getRandomCenter(10) : getRandomPosition();
				mover.moveTo(target);
				
			}
		} else {
			timer += Time.deltaTime;
			if (timer > timeToRespoun){
				timer = 0;
				player.dead = false;
			}
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
	
	private Vector3 getRandomCenter(float radius){
		bool free = false;
		Vector3 ret = new Vector3 ();
		ret.y = 1;
		while(!free) {
			ret.x = Random.Range(-radius, radius);
			ret.z = Random.Range(-radius, radius);
			free = isFree(ret);
		}
		return ret;
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
