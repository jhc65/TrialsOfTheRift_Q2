/*  Capture The Flag Objective - Zak Olyarnik
 * 
 *  Desc:   Facilitates Capture The Flag Objective
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureTheFlagObjective : Objective {

    public FlagController fc_activeFlag; // TODO: remove?
	private int i_score = 0;     // current progress towards i_maxScore

    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    override public void SetUI() {
        // @Sam - Turn on CTF UI
        GameController.GetInstance().Score(e_color, 0);
    }

    override public void ResetUI() {
        // @Sam - Turn on CTF UI
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
