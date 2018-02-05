/*  Capture The Flag Objective - Zak Olyarnik
 * 
 *  Desc:   Facilitates Capture The Flag Objective
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureTheFlagObjective : Objective {

    public FlagController fc_activeFlag; // TODO: see note on ParamReset
	private int i_score = 0;     // current progress towards i_maxScore

    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    override protected void SetUI() {
        // @Sam - Turn on CTF UI
        if (e_color == Constants.Global.Color.RED) {
            GameController.GetInstance().txt_redScoreText.transform.parent.gameObject.SetActive(true);
            GameController.GetInstance().txt_redObjvTitle.text = "Capture The Flag";
            GameController.GetInstance().txt_redObjvDescription.text = "Pick up the opponent's flag with [Interact] and drag it back to your goal! " + Constants.ObjectiveStats.C_CTFMaxScore + " Goals wins!";
        } else {
            GameController.GetInstance().txt_blueScoreText.transform.parent.gameObject.SetActive(true);
            GameController.GetInstance().txt_blueObjvTitle.text = "Capture The Flag";
            GameController.GetInstance().txt_blueObjvDescription.text = "Pick up the opponent's flag with [Interact] and drag it back to your goal! " + Constants.ObjectiveStats.C_CTFMaxScore + " Goals wins!";
        }
        GameController.GetInstance().Score(e_color, 0);
        
        GameController.GetInstance().PopupFadeIn(e_color);
    }

    override protected void ResetUI() {
        // @Sam - Turn off CTF UI
        if (e_color == Constants.Global.Color.RED) {
            GameController.GetInstance().txt_redScoreText.transform.parent.gameObject.SetActive(false);
        } else {
            GameController.GetInstance().txt_blueScoreText.transform.parent.gameObject.SetActive(false);
        }
    }

    public void UpdateFlagScore() {
        i_score++;
        GameController.GetInstance().Score(e_color, i_score);
        if (i_score >= Constants.ObjectiveStats.C_CTFMaxScore) {
            b_isComplete = true;
        }
    }

    // [Param Fix] - Used in Parameters Screen. Will be removed in main game (probably)
    public override void ParamReset(float param_in) {
        i_score = 0;
        GameController.GetInstance().Score(e_color, i_score);
        fc_activeFlag.ResetFlagPosition();
    }

}
