using UnityEngine;
using System.Collections;

public class constantRL {

	public const float alpha = 0.1f;
	public const float gamma = 0.9995f;
	static public int epsilon = 0; // probability to get a random action

	// STATES
	public enum States
	{
		flag1Ground_flag2Ground,
		flag1Ground_flag2Carried,
		flag1Carried_flag2Ground,
		flag1Carried_flag2Carried
	};
	public const int num_states = 4;

	// EVENTS
	public enum Events
	{
		makeScore,
		enemyMakeScore,
		enemyKilled,
		killed,
		teammateCatchFlag,
		enemyFlagCatched,
		enemyCatcherKilled
	};

	public const int num_events = 7;

	// dim [action, event]
	static public int[,] rewards = new int[3, 7]{
		{200, -40, 0, -40, -20, 0, 0},
		{0, -60, 80, -20, 0, 20, 100},
		{0, -100, 50, -20, 0, 80, 150}
	};

//	// ACTIONS
	public enum Actions
		{
		GET_FLAG_AND_SCORE,
		ATTACK_ENEMY_BASE,
		TAKE_CARE_ENEMY_FLAG
	};

	public const int num_actions = 3;

	// Reinforcement Learning Matrix
	// dim [state, event]
//	static public int[,] selectNextState = new int[4, 4]{
//		{0, 0, 0, 0},
//		{0, 0, 0, 0},
//		{0, 0, 0, 0},
//		{0, 0, 0, 0}
//	};
	
//
//	static public float[,] Q = new float[8, 8]{
//		{0, 0, 0, 0, 0, 0, 0, 0},
//		{0, 0, 0, 0, 0, 0, 0, 0},
//		{0, 0, 0, 0, 0, 0, 0, 0},
//		{0, 0, 0, 0, 0, 0, 0, 0},
//		{0, 0, 0, 0, 0, 0, 0, 0},
//		{0, 0, 0, 0, 0, 0, 0, 0},
//		{0, 0, 0, 0, 0, 0, 0, 0},
//		{0, 0, 0, 0, 0, 0, 0, 0}	
//	};
}




/* OLD REWARDS
static public int[,] rewards = new int[3, 7]{
		{200, -50, 0, -40, -20, 0, 0},
		{0, -100, 80, -20, 0, 20, 100},
		{0, -100, 50, -20, 0, 100, 150}
	};
*/