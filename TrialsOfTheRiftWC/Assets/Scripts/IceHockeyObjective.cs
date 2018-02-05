/*  Ice Hockey Objective - Dana Thompson
 * 
 *  Desc:   Facilitates Ice Hockey Objective
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceHockeyObjective : Objective {

    public HockeyPuckController hpc_activePuck;
	private int i_score = 0;    // current progress towards i_maxScore

    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    override protected void SetUI() {
        // @Sam - Turn on Hockey UI
        if (e_color == Constants.Global.Color.RED) {
            GameController.GetInstance().txt_redScoreText.transform.parent.gameObject.SetActive(true);
            GameController.GetInstance().txt_redObjvTitle.text = "Ice Hockey";
            GameController.GetInstance().txt_redObjvDescription.text = "Shoot and parry your puck into the enemy's goal! Careful, you can't score from behind! " + Constants.ObjectiveStats.C_HockeyMaxScore + " Goals wins!";
        } else {
            GameController.GetInstance().txt_blueScoreText.transform.parent.gameObject.SetActive(true);
            GameController.GetInstance().txt_blueObjvTitle.text = "Ice Hockey";
            GameController.GetInstance().txt_blueObjvDescription.text = "Shoot and parry your puck into the enemy's goal! Careful, you can't score from behind! " + Constants.ObjectiveStats.C_HockeyMaxScore + " Goals wins!";
        }
        GameController.GetInstance().Score(e_color, 0);
        GameController.GetInstance().PopupFadeIn(e_color);
    }

    override protected void ResetUI() {
        // @Sam - Turn off Hockey UI
        if (e_color == Constants.Global.Color.RED) {
            GameController.GetInstance().txt_redScoreText.transform.parent.gameObject.SetActive(false);
        } else {
            GameController.GetInstance().txt_blueScoreText.transform.parent.gameObject.SetActive(false);
        }
    }

    public void UpdatePuckScore() {
        i_score++;
        GameController.GetInstance().Score(e_color, i_score);
        if (i_score >= Constants.ObjectiveStats.C_HockeyMaxScore) {
            b_isComplete = true;
        }
    }

    // [Param Fix] - Used in Parameters Screen. Will be removed in main game (probably)
    public override void ParamReset(float param_in) {
        i_score = 0;
        GameController.GetInstance().Score(e_color, i_score);
        hpc_activePuck.ResetPuckPosition();
    }
}
