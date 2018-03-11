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

    override protected void SetUI() {
        calligrapher.CrystalDestructionInit(cc_activeCrystal.Color);
    }

    override protected void ResetUI() {
        calligrapher.CrystalDestructionReset(cc_activeCrystal.Color);
    }

    public void UpdateCrystalHealth(float f) {
        calligrapher.UpdateCrystalHealthUI(cc_activeCrystal.Color, f);
        if (cc_activeCrystal.Health <= 0) {
            b_isComplete = true;
        }
    }

    // [Param Fix] - Used in Parameters Screen. TODO: remove for release.
    public override void ParamReset() {
        cc_activeCrystal.Health = Constants.ObjectiveStats.C_CrystalMaxHealth;
        calligrapher.UpdateCrystalHealthUI(cc_activeCrystal.Color, Constants.ObjectiveStats.C_CrystalMaxHealth);
    }

	void OnEnable() {
        maestro.PlayBeginCrystalDestruction();
    }
}
