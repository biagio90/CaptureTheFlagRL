using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public bool started = false;
	
	// MENU
	private float left2 = 350, top2 = 200;
	private float width2 = 250, hight2 = 30;
	private float offset = 40;
	
	//private int select1 = 0;
	//private int select2 = 0;
	
	//
	public int numPlayers;
	public int endScore = 10;
	private int winner = 0;
	
	private float width = 80, hight = 30;
	private float left = Screen.width-100, top = 10;

	private int scoreTeam1 = 0;
	private int scoreTeam2 = 0;

	private GameControllerRL controllerRL;

	public void startMatch() {
		GameObject team1 = GameObject.FindGameObjectWithTag ("team1");
		GameObject team2 = GameObject.FindGameObjectWithTag ("team2");
		
		for (int i=0; i<numPlayers-1; i++) {
			if(team1!=null) {
				GameObject clone = (GameObject)Instantiate(team1, team1.transform.position, team1.transform.rotation);
				clone.GetComponent<PlayerController>().id = i+1;
			}
			if(team2!=null) {
				GameObject clone = (GameObject)Instantiate(team2, team2.transform.position, team2.transform.rotation);
				clone.GetComponent<DummyController>().id = i+1;
			}
		}
		
		started = true;
		controllerRL = GetComponent<GameControllerRL> ();
		controllerRL.startMatch ();
	}
	
	// Use this for initialization
	void Start () {
		startMatch ();
	}
	
	private void endOfTheMatch(){
		GameObject[] team1 = GameObject.FindGameObjectsWithTag ("team1");
		GameObject[] team2 = GameObject.FindGameObjectsWithTag ("team2");
		
		foreach (GameObject player in team1) {
			MovePlayer mover = player.GetComponent<MovePlayer>();
			mover.reset();
			MonoBehaviour[] scripts = player.GetComponents<MonoBehaviour>();
			foreach(MonoBehaviour script in scripts){
				script.enabled = false;
			}
		}
		
		foreach (GameObject player in team2) {
			MovePlayer mover = player.GetComponent<MovePlayer>();
			mover.reset();
			MonoBehaviour[] scripts = player.GetComponents<MonoBehaviour>();
			foreach(MonoBehaviour script in scripts){
				script.enabled = false;
			}
		}
		
	}
	
	void OnGUI() {
		if (started) {
			displayPoints();
			if (winner != 0) {
				if (GUI.Button (new Rect (left2-offset*2, top2+offset*4, width2, hight2), "Restart")) {
					Application.LoadLevel(Application.loadedLevel);
				}
			}
		} else {

		}
	}

	private void displayPoints() {
		if (winner != 0) {
			GUIStyle style = new GUIStyle();
			style.fontSize = 50;
			style.normal.textColor = Color.green;
			style.alignment = TextAnchor.MiddleCenter;
			float w = 200, h = 40;
			GUI.Label(new Rect (Screen.width/2-w/2, Screen.height/2-h/2+20, w, h), 
			          "END OF THE MATCH", style);
			if(winner == 1){
				GUI.Label(new Rect (Screen.width/2-w/2, Screen.height/2-h, w, h), 
				          "TEAM 1 WON!!", style);
			} else {
				GUI.Label(new Rect (Screen.width/2-w/2, Screen.height/2-h, w, h), 
				          "TEAM 2 WON!!", style);
			}
			
			endOfTheMatch();
		}
		
		GUI.color = Color.red;
		GUI.Label(new Rect (left, top, width, hight), 
		          "Team1: "+getScore(1));
		GUI.Label(new Rect (left, top+hight, width, hight), 
		          "Team2: "+getScore(2));
	}
	
	// Update is called once per frame
	void Update () {
		if (scoreTeam1 >= endScore) {
			winner = 1;
			return;
		}
		if (scoreTeam2 >= endScore) {
			winner = 2;
			return;
		}
	}

	public GameObject getFlagCatcher (string team){
		if(team == "team1"){
			return controllerRL.getCatcherTeam1();
		} 
		return controllerRL.getCatcherTeam2();
	}

	public void increaseScore(string team) {
		if(team == "team1") {
			scoreTeam1++;
			controllerRL.eventMessageScore(1);
		}else {
			scoreTeam2++;
			controllerRL.eventMessageScore(2);
		}
	}
	
	public int getScore(string team){
		if(team == "team1") return scoreTeam1;
		else 		  return scoreTeam2;
	}

	public void increaseScore(int team) {
		if(team == 1) scoreTeam1++;
		else 		  scoreTeam2++;
	}
	
	public int getScore(int team){
		if(team == 1) return scoreTeam1;
		else 		  return scoreTeam2;
	}
}
