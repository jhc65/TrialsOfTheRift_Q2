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
        // @Sam - Turn on Potato UI and verify these
        GameController.GetInstance().CompletionProgress(e_color, Constants.ObjectiveStats.C_PotatoCompletionTimer);
        GameController.GetInstance().SelfDestructProgress(e_color, Constants.ObjectiveStats.C_PotatoSelfDestructTimer);
    }

    override protected void ResetUI() {
        // @Sam - Turn off Potato UI
    }

    // Update UI and check for completion
    public void UpdateCompletionTimer(int i) {
        GameController.GetInstance().CompletionProgress(e_color, i);
        if (i <= 0) {
            b_isComplete = true;
        }
    }

    // Update UI and check to self-destruct
    public void UpdateDestructionTimer(int i) {
        GameController.GetInstance().SelfDestructProgress(e_color, i);
        if (i <= 0) {
            GameController.GetInstance().SelfDestructProgress(e_color, Constants.ObjectiveStats.C_PotatoSelfDestructTimer);
            hpc_activePotato.SelfDestruct();
        }
    }

    // [Param Fix] - Used in Parameters Screen. Will be removed in main game (probably)
    public override void ParamReset(float param)
    {       // uh... TODO ?
    }
}
