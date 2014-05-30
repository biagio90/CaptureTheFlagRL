using UnityEngine;
using System.Collections;

public class GameControllerRL : MonoBehaviour {
	public constantRL.States state;

	private enum flagState {ground, carried, center};

	private flagState stateFlag1, stateFlag2;
	
	private GameObject[] team1;
	private GameObject[] team2;

	// Use this for initialization
	void Start () {
		team1 = GameObject.FindGameObjectsWithTag ("team1");
		team2 = GameObject.FindGameObjectsWithTag ("team2");

		int newStateFlag1 = (int)flagState.ground;
		int newStateFlag2 = (int)flagState.carried;
		state = (constantRL.States)(newStateFlag1 * constantRL.num_states + newStateFlag2);
		Debug.Log ("state1: "+state+" == "+constantRL.States.flag1Ground_flag2Carried);

		newStateFlag1 = (int)flagState.carried;
		newStateFlag2 = (int) flagState.carried;
		state = (constantRL.States)(newStateFlag1 * constantRL.num_states + newStateFlag2);
		Debug.Log ("state2: "+state+" == "+constantRL.States.flag1Carried_flag2Carried);

	}
	
	// Update is called once per frame
	void Update () {
		int newStateFlag1 = (int)checkFlag ("flagTeam1");
		int newStateFlag2 = (int)checkFlag ("flagTeam2");

		state = (constantRL.States)(newStateFlag1 * constantRL.num_states + newStateFlag2);
	}

	private flagState checkFlag(string flagTag){
		GameObject flag = GameObject.FindGameObjectWithTag (flagTag);
		if(flag == null){
			return flagState.carried;
		}
		Vector3 flagPos = flag.transform.position;
		flagPos.y = 0;
//		if(Vector3.Distance(flagPos, Vector3.zero) < 5){
//			return flagState.center;
//		}

		return flagState.ground;
	}
	
	public void eventMessageScore(int team){
		GameObject[] goodTeam, badTeam;
		if(team == 1){
			goodTeam = team1;
			badTeam = team2;
		} else {
			goodTeam = team2;
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
}
