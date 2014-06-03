using UnityEngine;
using System.Collections;
using Pathfinding;

public class PlayerController : MonoBehaviour {
	public int id;

	// Player state
	public bool hasFlag = false;
	public GameObject playerExplosion;
	public GameObject flagPrefabs;

	// for Reinforcement Learning
	constantRL.Actions nextAction = constantRL.Actions.GET_FLAG_AND_SCORE;
	private constantRL.Events eventLast;

	// Game info
	private string teamTag;
	public GameObject gameController;
	//private GameController controller;
	private GameControllerRL controllerRL;
	public GameObject myBase;

	// Player info
	private MovePlayer mover;
	private PlayerAction action;
	private PlayerRL playerRL;
	private bool start = true;
	public int max_life = 3;
	private int life;

	//for respawn
	public float timeToRespoun = 5;
	public bool dead = false;
	private float timer = 0.0f;

	// initial delay
	private float delayBegin;
	private float timerBegin = 0.0f;

	// write file log
	private string nameFile;

	// Use this for initialization
	void Start () {
		nameFile = composeName();

//		controller = gameController.GetComponent<GameController>();
		controllerRL = gameController.GetComponent<GameControllerRL>();

		mover = GetComponent<MovePlayer> ();
		action = GetComponent<PlayerAction> ();
//		playerRL = GetComponent<PlayerRL> ();
//		playerRL = new PlayerRL ();

		teamTag = this.tag;
		if(teamTag == "team1"){
			
		} else {
			
		}

		delayBegin = id;
		life = max_life;
	}

	void Update () {
		if(start){
			timerBegin = Time.time + delayBegin;
			start = false;
//
//			float[,] Qinitial = new float[constantRL.num_states, constantRL.num_actions];
//			for (int i=0; i<constantRL.num_states;i++){
//				for (int j=0; j<constantRL.num_actions;j++){
//					Qinitial[i,j] = constantRL.Qtables[id, i, j];
//				}
//			}
//			playerRL = new PlayerRL (Qinitial);
//			nextAction = (constantRL.Actions)playerRL.nextAction(
//				(int)constantRL.Event_start, (int)controllerRL.state);

			playerRL = new PlayerRL ();
			return;
		}

		if(tag=="team1" && Time.time > timerBegin){
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
		if(tag=="team1"){
			constantRL.Actions lastAction = nextAction;
			//Debug.Log ("state: "+ (int)controllerRL.state+"  REWARD: "+constantRL.rewards [(int)nextAction, (int)eventHappened]);
			nextAction = (constantRL.Actions)playerRL.nextAction(
							(int)eventHappened, (int)controllerRL.state);
			//Debug.Log ( "nextAction: " + nextAction + "  event: " + eventHappened);
			int r = constantRL.rewards [(int)nextAction, (int)eventHappened];
			saveOnFile(lastAction, nextAction, eventHappened, controllerRL.oldState, controllerRL.state, r);
		}
	}
	
	public void killPlayer(){
		life -= 1;
		if ( life == 0){
			killPlayerSeriously ();
		}
	}

	private void killPlayerSeriously(){
		life = max_life;
		Instantiate(playerExplosion, transform.position, transform.rotation);
		transform.Find("EyeBall").gameObject.SetActive(false);
		transform.Find("WheelBall").gameObject.SetActive(false);
		
		if (hasFlag) {
			Instantiate(flagPrefabs, transform.position, Quaternion.identity);
			transform.Find("flag").gameObject.SetActive(false);
		}
		hasFlag = false;
		if(tag == "team1") updateGraphWeights (100);
		
		transform.position = myBase.transform.position;
		
		if(mover!=null) mover.reset ();
		
		dead = true;
		
		if(action!=null) action.getKilled ();

	}

	private void updateGraphWeights(int penalty){
		Vector3 center = transform.position;
		center.y = 0;
		Vector3 size = new Vector3 (2, 2, 2);

		GraphUpdateObject graphUpdate = new GraphUpdateObject ();
		graphUpdate.addPenalty = penalty;

		for (int i=1; i<7; i++){
			Bounds b = new Bounds (center, size*i);
			graphUpdate.bounds = b;
			AstarPath.active.UpdateGraphs (graphUpdate);
		}

	}

	public void catchTheFLag(){
		gameObject.transform.Find("flag").gameObject.SetActive(true);
		hasFlag = true;
	}

	private void saveOnFile(constantRL.Actions action, constantRL.Actions nextAction, constantRL.Events eventHappened, constantRL.States state, constantRL.States nextState, int reward){
		using (System.IO.StreamWriter file = new System.IO.StreamWriter(nameFile, true))
		{
			file.WriteLine("State: "+state+" Action: "+action);
			file.WriteLine("event: "+eventHappened+" reward: "+reward);
			file.WriteLine("next state: "+nextState+" next action: "+nextAction);
			file.WriteLine("Q table: ");
			for (int i=0; i<constantRL.num_states;i++){
				string line = "";
				for (int j=0; j<constantRL.num_actions;j++){
					line += playerRL.Qtable[i, j]+" ";
				}
				file.WriteLine(line);
			}
			file.WriteLine("\n\n");
		}
	}

	private string composeName(){
		string name = "player" + id;
		string dir = @"C:\Users\Biagio\Documents\GitHub\CaptureTheFlagRL\LogFile\"+name+@"\";
		int count = 0;

		string path = dir + name + "_" + count+".txt";
		while(System.IO.File.Exists(path)){
			count++;
			path = dir + name + "_" + count+".txt";
		}
		return path;
	}
}
