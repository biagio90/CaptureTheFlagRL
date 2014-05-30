﻿using UnityEngine;
using System.Collections;

public class constantRL {

	public const float alpha = 0.1f;
	public const float gamma = 0.9995f;
	static public int epsilon = 50;

	// STATES
	public const int num_states = 8;
	public const int flag1Ground_flag2Ground 	= 0;
	public const int flag1Ground_flag2Carried  	= 1;
	public const int flag1Carried_flag2Ground  	= 2;
	public const int flag1Carried_flag2Carried  = 3;
	public const int team1Score_flag2Carried 	= 4;
	public const int team1Score_flag2Ground 	= 5;
	public const int flag1Ground_team2Score   	= 6;
	public const int flag1Carried_team2Score   	= 7;
	
	// EVENTS
	public const int num_events = 10;
	public const int tookEnemysFlag 				= 0;
	public const int tookFlag 						= 1;
	public const int teammateTookFlag 				= 2;
	public const int teammateTookEnemysFlag			= 3;
	public const int killEnemyCarringFlag			= 4;
	public const int teammatekillEnemyCarringFlag	= 5;
	public const int enemyKillTeammateCarringFlag 	= 6;
	public const int makeScore					 	= 7;
	public const int teammateMakeScore			 	= 8;
	public const int enemyMakeScore			 		= 9;
	//public const int enemysTooksEnemysFlag 		= 4;
	//public const int enemysTooksOurFlag 			= 5;

//	// ACTIONS
//	public enum Actions
//		{
//		GET_FLAG_AND_SCORE,
//		ATTACK_ENEMY_BASE,
//		TAKE_CARE_ENEMY_FLAG
//	};

	public const int num_actions = 8;
	public const int GET_FLAG_AND_SCORE			= 0;
	public const int ATTACK_ENEMY_BASE	 		= 1;
	public const int TAKE_CARE_ENEMY_FLAG	 	= 2;

	// Reinforcement Learning Matrix
	static public int[,] selectNextState = new int[8, 10]{
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
	};

	static public int[,] rewards = new int[8, 10]{
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0}	
	};

	static public float[,] Q = new float[8, 8]{
		{0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0}	
	};
}
