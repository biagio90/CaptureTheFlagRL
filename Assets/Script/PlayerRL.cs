using UnityEngine;
using System.Collections;

public class PlayerRL {

	private int state = 0;
	private int action = 0;

	// Qtable needs to chose the next action
	// dim [state, action]
	private float[,] Qtable;

	public PlayerRL(){
		Qtable = new float[constantRL.num_actions, constantRL.num_events];
		// Qtable start empty
	}

	public int nextAction(int eventHappened, int newState){
		//int newState = constantRL.selectNextState [state, eventHappened];

		int r = calculateReward (action, eventHappened);
		updateQ (action, state, newState, eventHappened, r);
		
		int prob = Random.Range (0, 100);
		if (prob < constantRL.epsilon) {
			action = Random.Range(0, constantRL.num_actions);
		} else {
			action = getArgmaxAction(newState);
		}

		state = newState;
		return action;
	}
	
	private int calculateReward(int a, int e) {
		return constantRL.rewards [a, e];
	}

	private void updateQ(int a, int p, int s, int e, int r) {
		float delta = r + constantRL.gamma * getMaxAction (s) - Qtable [p, a];
		Qtable [p, a] = Qtable [p, a] + constantRL.alpha * delta;
	}
	
	private float getMaxAction (int s) {
		float max = 0;
		for (int i=0; i<constantRL.num_actions; i++) {
			if(Qtable[s, i] > max) {
				max = Qtable[s, i];
			}
		}
		return max;
	}
	
	private int getArgmaxAction (int s) {
		float max = 0;
		int index = 0;
		for (int i=0; i<constantRL.num_actions; i++) {
			if(Qtable[s, i] > max) {
				max = Qtable[s, i];
				index = i;
			}
		}
		return index;
	}

}
