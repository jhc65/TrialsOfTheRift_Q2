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

    override public void SetUI()
    {
        // @Sam - Turn on Hockey UI
        GameController.GetInstance().Score(e_color, 0);
    }

    override public void ResetUI()
    {
        // @Sam - Turn off Hockey UI
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
