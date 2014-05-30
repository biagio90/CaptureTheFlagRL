﻿using UnityEngine;
using System.Collections;

public class PickUpFlag : MonoBehaviour {
	public string teamTag;
	public Vector3 flagPos;

	void OnTriggerEnter(Collider other) 
	{
		if (other.tag == teamTag)
		{
			Destroy(gameObject);
			PlayerController player = other.GetComponent<PlayerController>();
			player.catchTheFLag();
		} else if (other.tag != "bullet"){
			other.transform.position = flagPos;
		}
	}
}
