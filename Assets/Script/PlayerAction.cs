﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MovePlayer))]
[RequireComponent(typeof(PlayerController))]

public class PlayerAction : MonoBehaviour {

	// PlayerController
	public bool finish = true;

	private bool enable = false;
	private constantRL.Actions action = constantRL.Actions.GET_FLAG_AND_SCORE;

	public GameObject gameController;
	private GameController controller;
	private GameControllerRL controllerRL;

	private PlayerController player;
	private MovePlayer mover;

	// GET_FLAG_AND_SCORE
	public string myFlagTag;
	public GameObject myBase;
	private Vector3 myBasePos;
	public GameObject enemyBase;
	private Vector3 enemyBasePos;

	private int movingState = 0;
	private Vector3 basePosition;

	public GameObject flagPrefabs;
	private Vector3 flagPos;

	// ATTACK_ENEMY_BASE
	public string enemyTag = "team2";
	
	public float viewLength = 15.0f;
	public float viewAngle  = 50.0f;
	public int killProbability = 100;
	public GameObject bullet;
	public float bulletSpeed = 40.0f;

	// TAKE_CARE_ENEMY_FLAG
	public string enemyFlagTag;

	// TIMER
	public float delayNewAttack = 0.0f;
	private float nextNewAttack = 0.0f;

	public float delayShoot = 0.8f;
	private float nextShootTime = 0.0f;


	void Start () {
		myBasePos = myBase.transform.position;
		enemyBasePos = enemyBase.transform.position;

		flagPos = GameObject.FindGameObjectWithTag (myFlagTag).transform.position;
		mover = GetComponent<MovePlayer> ();
		player = GetComponent<PlayerController> ();
		basePosition = myBase.transform.position;

		controller = gameController.GetComponent<GameController>();
		controllerRL = gameController.GetComponent<GameControllerRL>();

	}
	
	public void startAction(constantRL.Actions action) {
		this.action = action;
		movingState = 0;
		finish = false;
		enable = true;
	}

	private void resetAction(){
		movingState = 0;
		finish = true;
		enable = false;
		
	}

	void Update () {
		checkForShoot ();

		if (enable) {
			switch(action) {
			case constantRL.Actions.GET_FLAG_AND_SCORE: controllerGetFlag();
				break;
			case constantRL.Actions.ATTACK_ENEMY_BASE:	 controllerAttack();
				break;
			case constantRL.Actions.TAKE_CARE_ENEMY_FLAG: controllerTakeEnemyFlag();
				break;

			}

		}
	}

	// TAKE_CARE_ENEMY_FLAG
	private void controllerTakeEnemyFlag(){
//		checkForShoot ();
//
		if(mover.arrived){
			transform.Find("WheelBall").gameObject.SetActive(true);
			GameObject enemyFlag = GameObject.FindGameObjectWithTag (enemyFlagTag);
			if(enemyFlag != null) {
				if(!closeTheCenter(enemyFlag.transform.position)){
					mover.moveTo(enemyFlag.transform.position);
					return;
				}
				//mover.moveTo(getRandomPosition());
				attackClosest();
			} else {
				//string enemyTag = (tag=="team1") ? "team2" : "team1";
				GameObject enemy = controller.getFlagCatcher(enemyTag);

				if(enemy!=null) mover.moveTo(enemy.transform.position);
			}
		}
	}
	public void enemyFlagTouched(){
		resetAction();
		player.updateEvent (constantRL.Events.enemyFlagCatched);
		transform.Find("WheelBall").gameObject.SetActive(false);
	}
	private bool closeTheCenter(Vector3 pos){
		pos.y = 0;
		return Vector3.Distance (pos, Vector3.zero) < 5;
	}

	// GET_FLAG_AND_SCORE
	private void controllerGetFlag(){
		switch (movingState) {
		case 0: // go to the flag
			if (!player.hasFlag){
				transform.Find("EyeBall").gameObject.SetActive(true);
				movingState = 1;
				GameObject flag = GameObject.FindGameObjectWithTag(myFlagTag);
				if(flag!=null) mover.moveTo(flag.transform.position);
			}
			break;
		case 1: // you have the flag, go to the base
			if(mover.arrived){
				if(player.hasFlag){
					movingState = 2;
					mover.moveTo(basePosition);
				} else {
					resetAction();
					transform.Find("EyeBall").gameObject.SetActive(false);
					player.updateEvent (constantRL.Events.teammateCatchFlag);
				}
			}
			break;
		case 2: // reach the base, make score, leave the flag, end action
			if(Vector3.Distance(transform.position, myBasePos) < 1
			   && player.hasFlag){
				controller.increaseScore(tag);
				gameObject.transform.Find("flag").gameObject.SetActive(false);
				player.hasFlag=false;
				
				Instantiate(flagPrefabs, flagPos, Quaternion.identity);

				resetAction();
				player.updateEvent (constantRL.Events.makeScore);
				transform.Find("EyeBall").gameObject.SetActive(false);
			}
			break;
		}
	}
	
	// ATTACK_ENEMY_BASE
	private void controllerAttack(){
		//checkForShoot ();
		//moveToAttack ();
		attackClosest ();
	}

	private void attackClosest (){
		if (mover.arrived) {
			Vector3 target = controllerRL.closestEnemy();
			mover.moveTo(target);
		}
	}

	private void moveToAttack (){
		if(movingState == 0){
			movingState=1;
			Vector3 point = getRandomPosition();
			point = (point - enemyBasePos).normalized * Random.Range(3.0f, 20.0f);
			mover.moveTo(enemyBasePos+point);
		}
		if(movingState==1 && mover.arrived){
			mover.rotateTo(enemyBasePos);
			nextNewAttack = Time.time + delayNewAttack;
			movingState = 2;
		}
		else if (movingState == 2) {
			if (Time.time > nextNewAttack) {
				movingState = 0;
				mover.reset();
			}
		}
	}
	
	private void checkForShoot(){
		if (Time.time > nextShootTime) {
			nextShootTime = Time.time + delayShoot;
			
			Vector3 direction = transform.TransformDirection(Vector3.forward);
			RaycastHit hit = new RaycastHit ();
			Ray ray = new Ray(transform.position, direction);
			
			for (float angle = -viewAngle; angle < viewAngle; angle += 1.0f) {
				ray.direction = Quaternion.Euler(0, angle, 0) * direction;
				if (Physics.Raycast(ray, out hit, viewLength)) {
					if (hit.collider.tag == enemyTag ) {
						shoot(hit.transform.position);
						return;
					}
				}
			}
		}
	}

	private void shoot (Vector3 enemyPos) {
		Vector3 t = transform.position;
		Vector3 direction = (enemyPos - t).normalized;
		t = t + direction * 1;
		t.y = transform.position.y;
		GameObject shoot = (GameObject) Instantiate(bullet, t, Quaternion.identity);
		Bullet script = shoot.GetComponent<Bullet> ();
		script.destination = enemyPos;
		script.speed = bulletSpeed;
		script.enemyTag = enemyTag;
		script.killProbability = killProbability;
		script.playerShooter = gameObject;
		script.action = GetComponent<PlayerAction>();
		script.go = true;
		
	}

	public void enemyKilled(bool hasFlag){
		resetAction();
		if(!hasFlag)	player.updateEvent (constantRL.Events.enemyKilled);
		else 			player.updateEvent (constantRL.Events.enemyCatcherKilled);
		transform.Find("WheelBall").gameObject.SetActive(false);
	}

	public void getKilled(){
		resetAction();
		player.updateEvent (constantRL.Events.killed);
	}

	/*
	private Vector3 getRandomFreePosition(){
		bool free = false;
		Vector3 ret = new Vector3 ();
		ret.y = 1;
		while(!free) {
			ret.x = Random.Range(-27, 27);
			ret.z = Random.Range(-20, 20);
			free = isFree(ret);
		}
		return ret;
	}*/

	private Vector3 getRandomPosition(){
		Vector3 ret = new Vector3 ();
		ret.y = 1;
		ret.x = Random.Range(-27, 27);
		ret.z = Random.Range(-20, 20);
		return ret;
	}

	public constantRL.Actions getCurrentAction(){
		return action;
	}

}
