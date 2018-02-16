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
        calligrapher.IceHockeyInit(e_color);
    }

    override protected void ResetUI() {
        calligrapher.ScoreReset(e_color);
    }

    public void UpdatePuckScore() {
        i_score++;
        calligrapher.UpdateScoreUI(e_color, i_score);
        if (i_score >= Constants.ObjectiveStats.C_HockeyMaxScore) {
            b_isComplete = true;
        }
    }

    // [Param Fix] - Used in Parameters Screen. TODO: remove for release.
    public override void ParamReset() {
        i_score = 0;
        calligrapher.UpdateScoreUI(e_color, i_score);
        hpc_activePuck.ResetPuckPosition();
    }
}
