using UnityEngine;
using System.Collections;

public class GameControllerRL : MonoBehaviour {
	public constantRL.States state;// = constantRL.States.flag1Center_flag2Center;

	private enum flagState {ground, carried, center};

//	private flagState stateFlag1, stateFlag2;
	
	private GameObject[] team1;
	private GameObject[] team2;

	public constantRL.States oldState = constantRL.States.flag1Ground_flag2Ground;

	// Use this for initialization
	void Start () {
//		team1 = GameObject.FindGameObjectsWithTag ("team1");
//		team2 = GameObject.FindGameObjectsWithTag ("team2");

	}
	public void startMatch(){
		team1 = GameObject.FindGameObjectsWithTag ("team1");
		team2 = GameObject.FindGameObjectsWithTag ("team2");

	}

	// Update is called once per frame
	void Update () {
		int newStateFlag1 = (int)checkFlag ("flagTeam1");
		int newStateFlag2 = (int)checkFlag ("flagTeam2");

		state = (constantRL.States)(newStateFlag1 * 2 + newStateFlag2);
		if(state != oldState) {
//			Debug.Log ("state: "+state);
			oldState = state;
		}
	}

	private flagState checkFlag(string flagTag){
		GameObject flag = GameObject.FindGameObjectWithTag (flagTag);
		if(flag == null){
			return flagState.carried;
		}
//		Vector3 flagPos = flag.transform.position;
		//Debug.Log (flagPos);
//		flagPos.y = 0;
//		if(Vector3.Distance(flagPos, Vector3.zero) < 10){
//			return flagState.center;
//		}

		return flagState.ground;
	}

	public Vector3 closestEnemy(){
		Vector3 target = Vector3.zero;
		float dist = int.MaxValue;
		Vector3 myPos = transform.position;

		//GameObject[] team = (tag == "team1") ? team2 : team1;
		foreach (GameObject player in team2) {
			Vector3 pos = player.transform.position;
			float d = Vector3.Distance(myPos, pos);
			if(d < dist){
				dist = d;
				target = pos;
			}
		}

		return target;
	}

	public void eventMessageScore(int team){
		//GameObject[] goodTeam;
		GameObject[] badTeam;

		if(team == 1){
//			goodTeam = team1;
			badTeam = team2;
		} else {
//			goodTeam = team2;
			badTeam = team1;
		}

		foreach(GameObject player in badTeam){
			PlayerController controller = player.GetComponent<PlayerController>();
			if(controller!=null){
				controller.updateEvent(constantRL.Events.enemyMakeScore);
			}
		}
		//CONTINUE ??
	}

	public GameObject getCatcherTeam1(){
		foreach(GameObject player in team1){
			PlayerController controller = player.GetComponent<PlayerController>();
			if(controller.hasFlag){
				return player;
			}
		}
		return null;
	}
	public GameObject getCatcherTeam2(){
		foreach(GameObject player in team2){
			PlayerController controller = player.GetComponent<PlayerController>();
			if(controller.hasFlag){
				return player;
			}
		}
		return null;
	}
}
