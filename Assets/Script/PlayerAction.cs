using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MovePlayer))]
[RequireComponent(typeof(PlayerController))]

public class PlayerAction : MonoBehaviour {
	// ACTIONS
	public enum Actions
	{
		GET_FLAG_AND_SCORE,
		ATTACK_ENEMY_BASE,
		TAKE_CARE_ENEMY_FLAG
	};

	// PlayerController
	public bool finish = false;

	private bool enable = false;
	private Actions action = Actions.GET_FLAG_AND_SCORE;

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

	// ATTACK_ENEMY_BASE
	public string enemyTag = "team2";
	
	public float viewLength = 15.0f;
	public float viewAngle  = 50.0f;
	public int killProbability = 100;
	public GameObject bullet;
	
	public float delay = 0.8f;
	
	private float nextTime = 0.0f;

	// TAKE_CARE_ENEMY_FLAG

	void Start () {
		myBasePos = myBase.transform.position;
		enemyBasePos = enemyBase.transform.position;

		mover = GetComponent<MovePlayer> ();
		player = GetComponent<PlayerController> ();
		basePosition = myBase.transform.position;
	}
	
	void Update () {
		if (enable) {
			switch(action) {
			case Actions.GET_FLAG_AND_SCORE: controllerGetFlag();
				break;
			case Actions.ATTACK_ENEMY_BASE:	 controllerAttack();
				break;
			case Actions.TAKE_CARE_ENEMY_FLAG:
				break;

			}
		}
	}

	private void controllerAttack(){
		checkForShoot ();
		moveToAttack ();
	}

	private void moveToAttack (){
		if(movingState == 0){
			movingState=1;
			mover.moveTo(enemyBasePos);
		}
	}

	private void checkForShoot(){
		if (Time.time > nextTime) {
			nextTime = Time.time + delay;
			
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
		script.speed = 20;
		script.enemyTag = enemyTag;
		script.killProbability = killProbability;
		script.playerShooter = gameObject;
		script.go = true;
		
	}

	public void enemyKilled(){
		
	}

	private void controllerGetFlag(){
		switch (movingState) {
		case 0: if (!player.hasFlag){
				movingState = 1;
				Vector3 flagPosition = GameObject.FindGameObjectWithTag(myFlagTag).transform.position;
				mover.moveTo(flagPosition);
			}
			break;
		case 1: if(player.hasFlag){
				movingState = 2;
				mover.moveTo(basePosition);
			}
			break;
		case 2: if(Vector3.Distance(transform.position, myBasePos) < 1){
				player.gameController.increaseScore(tag);
				gameObject.transform.Find("flag").gameObject.SetActive(false);
				player.hasFlag=false;

				movingState = 0;
				finish = true;
				enable = false;

			}
			break;
		}
	}

	public void startAction(Actions action) {
		this.action = action;
		movingState = 0;
		enable = true;
	}



}
