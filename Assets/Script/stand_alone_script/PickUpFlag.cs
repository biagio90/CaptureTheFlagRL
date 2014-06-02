﻿using UnityEngine;
using System.Collections;

public class PickUpFlag : MonoBehaviour {
	public string teamTag;
	public Vector3 flagPos;

	void OnTriggerEnter(Collider other) 
	{
		if (other.tag == teamTag)
		{
			PlayerAction action = other.GetComponent<PlayerAction>();
			if(action == null || action.getCurrentAction() == constantRL.Actions.GET_FLAG_AND_SCORE){
				Destroy(gameObject);
				PlayerController player = other.GetComponent<PlayerController>();
				player.catchTheFLag();
			} 
		} else if (other.tag != "bullet"){
			if(Vector3.Distance(transform.position, flagPos) > 2) {
				transform.position = flagPos;
				PlayerAction player = other.GetComponent<PlayerAction>();
				if(player != null) player.enemyFlagTouched();
			}
		}
	}
}
