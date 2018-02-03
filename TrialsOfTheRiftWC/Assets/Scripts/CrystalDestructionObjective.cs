/*  Crystal Destruction Objective - Dana Thompson
 * 
 *  Desc:   Facilitates Crystal Destruction Objective
 * 
 */
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalDestructionObjective : Objective {

	public CrystalController cc_activeCrystal;    // active crystal specific to this objective instance

    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    override public void SetUI() {
        // @Sam - Turn on CD UI
        GameController.GetInstance().CrystalHealth(cc_activeCrystal.Color, Constants.ObjectiveStats.C_CrystalMaxHealth);
    }

    override public void ResetUI() {
        // @Sam - Turn off CD UI
    }

    public void UpdateCrystalHealth(float f) {
        // @Sam - ideally this gets changed to just e_color when the UI changes but remember this # represents the opposite team's crystal health 
        GameController.GetInstance().CrystalHealth(cc_activeCrystal.Color, (int)f);   // @Sam - we def want this to be an int, right?
        if (cc_activeCrystal.Health <= 0) {
            b_isComplete = true;
        }
    }

    // [Param Fix] - Used in Parameters Screen. Will be removed in main game (probably)
    public override void ParamReset(float param_in) {
        cc_activeCrystal.Health = param_in;
        GameController.GetInstance().CrystalHealth(cc_activeCrystal.Color, (int)param_in);
    }
}
