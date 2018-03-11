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
        calligrapher.CTFInit(e_color);
    }

    override protected void ResetUI() {
        calligrapher.ScoreReset(e_color);
    }

    public void UpdateFlagScore() {
        i_score++;
        calligrapher.UpdateScoreUI(e_color, i_score);
        if (i_score >= Constants.ObjectiveStats.C_CTFMaxScore) {
            b_isComplete = true;
        }
    }

    // [Param Fix] - Used in Parameters Screen. TODO: remove for release.
    public override void ParamReset() {
        i_score = 0;
        calligrapher.UpdateScoreUI(e_color, i_score);
        fc_activeFlag.ResetFlagPosition();
    }
	
	void OnEnable() {
        maestro.PlayBeginCTF();
    }

}
