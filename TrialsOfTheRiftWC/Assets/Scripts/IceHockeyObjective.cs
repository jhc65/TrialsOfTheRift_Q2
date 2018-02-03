using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceHockeyObjective : Objective
{
	public GameObject go_redPuck, go_bluePuck;	// referenced puck objects
	public GameObject go_redHockeyGoal, go_blueHockeyGoal;	// referenced hockey goal objects

	private GameObject go_currentPuck, go_currentHockeyGoal;  // active objects specific to this objective instance
	private int i_score = 0;	// current progress towards i_maxScore

	override public void Instantiate() {
		// instantiate prefabs based on color
		if(e_color == Constants.Global.Color.RED) {
            go_currentPuck = Instantiate(go_redPuck, Constants.ObjectiveStats.C_RedPuckSpawn, new Quaternion(0, 0, 0, 0));
            go_currentHockeyGoal = Instantiate(go_blueHockeyGoal, Constants.ObjectiveStats.C_BlueHockeyGoalSpawn, new Quaternion(0, 0, 0, 0));
		}
        else
        {
            go_currentPuck = Instantiate(go_bluePuck, Constants.ObjectiveStats.C_BluePuckSpawn, new Quaternion(0, 0, 0, 0));
            go_currentHockeyGoal = Instantiate(go_redHockeyGoal, Constants.ObjectiveStats.C_RedHockeyGoalSpawn, new Quaternion(0, 0, 0, 0));
        }
    }

	override public void Complete() {
		// destroy prefabs
		b_complete = true;
		GameController.GetInstance().Score(e_color, 0);
		Destroy(go_currentPuck);
		Destroy(go_currentHockeyGoal);
	}

	void Update() {
		if (go_currentPuck.GetComponent<HockeyPuckController>().b_scored) {
			go_currentPuck.GetComponent<HockeyPuckController>().b_scored = false;
			i_score += 1;
			GameController.GetInstance().Score(e_color, i_score);
		}
		if(i_score >= Constants.ObjectiveStats.C_HockeyMaxScore) {
			Complete();
		}
	}

    // [Param Fix] - Used in Parameters Screen. Will be removed in main game (probably)
    public override void ParamReset(float param_in) {
        i_score = 0;
        GameController.GetInstance().Score(e_color, i_score);
        go_currentPuck.GetComponent<HockeyPuckController>().ResetHome();
    }
}
