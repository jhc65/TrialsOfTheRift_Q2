/*  Hot Potato Objective - Dana Thompson
 * 
 *  Desc:   Facilitates Hot Potato Objective
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotPotatoObjective : Objective {

    public HotPotatoController hpc_activePotato;    // active potato specific to this objective instance

    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    override protected void SetUI() {
        calligrapher.HotPotatoInit(e_color);
    }

    override protected void ResetUI() {
        calligrapher.HotPotatoReset(e_color);
    }

    // Update UI and check for completion
    public void UpdateCompletionTimer(int i) {
        calligrapher.UpdateCompletionTimerUI(e_color, i);
        if (i <= 0) {
            b_isComplete = true;
        }
    }

    // Update UI and check to self-destruct
    public void UpdateDestructionTimer(int i) {
        calligrapher.UpdateDestructionTimerUI(e_color, i);
        if (i <= 0) {
            calligrapher.UpdateDestructionTimerUI(e_color, Constants.ObjectiveStats.C_PotatoSelfDestructTimer);
            hpc_activePotato.SelfDestruct();
        }
    }

    // [Param Fix] - Used in Parameters Screen. TODO: remove for release.
    public override void ParamReset() {
        hpc_activePotato.ResetPotatoPosition();
        hpc_activePotato.UpdateCompletionTimer(Constants.ObjectiveStats.C_PotatoCompletionTimer);
        hpc_activePotato.UpdateDestructionTimer(Constants.ObjectiveStats.C_PotatoSelfDestructTimer);
    }
	
	void OnEnable() {
        maestro.PlayBeginPotato();
    }
}
