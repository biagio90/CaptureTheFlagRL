using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	// Player state
	public bool hasFlag;
	public GameObject playerExplosion;
	public GameObject flagPrefabs;

	// for Reinforcement Learning
	constantRL.Actions nextAction = constantRL.Actions.GET_FLAG_AND_SCORE;
	private constantRL.Events eventLast;

	// Game info
	private string teamTag;
	public GameObject gameController;
	private GameController controller;
	private GameControllerRL controllerRL;
	public GameObject myBase;

	// Player info
	private MovePlayer mover;
	private PlayerAction action;
	private PlayerRL playerRL;
	private bool start = true;
	
	//for respawn
	public float timeToRespoun = 5;
	public bool dead = false;
	private float timer = 0.0f;

	// Use this for initialization
	void Start () {
		controller = gameController.GetComponent<GameController>();
		controllerRL = gameController.GetComponent<GameControllerRL>();

		mover = GetComponent<MovePlayer> ();
		action = GetComponent<PlayerAction> ();
//		playerRL = GetComponent<PlayerRL> ();
		playerRL = new PlayerRL ();

		teamTag = this.tag;
		if(teamTag == "team1"){
			
		} else {
			
		}

	}

	void Update () {
		if(tag=="team1"){
			if(!dead){
				if(action.finish){
					//constantRL.Actions nextAction = (constantRL.Actions)playerRL.nextAction((int)eventLast);
					action.startAction(nextAction);
				}
			} else {
				timer += Time.deltaTime;
				if (timer > timeToRespoun){
					timer = 0;
					dead = false;
				}
			}
		}
	}

	public void updateEvent(constantRL.Events eventHappened){
		//eventLast = eventHappened;
		nextAction = (constantRL.Actions)playerRL.nextAction(
						(int)eventHappened, (int)controllerRL.state);
		Debug.Log ("event: " + eventHappened + " action: " + nextAction);
	}

	public void killPlayer(){
		Instantiate(playerExplosion, transform.position, transform.rotation);

		if (hasFlag) {
			Instantiate(flagPrefabs, transform.position, Quaternion.identity);
			transform.Find("flag").gameObject.SetActive(false);
		}
		hasFlag = false;
		transform.position = myBase.transform.position;
		mover.reset ();

		dead = true;

		action.getKilled ();
	}

	public void catchTheFLag(){
		gameObject.transform.Find("flag").gameObject.SetActive(true);
		hasFlag = true;
	}

}
