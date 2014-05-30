using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	// Player state
	public bool hasFlag;
	public bool dead = false;
	public GameObject playerExplosion;
	public GameObject flagPrefabs;

	private string teamTag;

	public GameController gameController;
	public GameObject myBase;

	private MovePlayer mover;
	private PlayerAction action;

	private bool start = true;

	// Use this for initialization
	void Start () {
		mover = GetComponent<MovePlayer> ();
		action = GetComponent<PlayerAction> ();
		teamTag = this.tag;
		if(teamTag == "team1"){
			
		} else {
			
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(tag=="team1"){
			if (start) {
				start = false;
				action.startAction(PlayerAction.Actions.ATTACK_ENEMY_BASE);
			}
			if (action.finish) {
				Debug.Log("Action computed!!");
			}
		}
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
	}

	public void catchTheFLag(){
		gameObject.transform.Find("flag").gameObject.SetActive(true);
		hasFlag = true;
	}

}
