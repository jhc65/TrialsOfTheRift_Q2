/*  Hot Potato Objective - Dana Thompson
 * 
 *  Desc:   Facilitates Hot Potato Objective
 * 
 */

public class HotPotatoObjective : Objective {
#region Variables and Declarations
    public HotPotatoController hpc_activePotato;    // active potato specific to this objective instance
#endregion

#region HotPotatoObjective Methods
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
#endregion

#region Unity Overrides
    void OnEnable() {
        maestro.PlayBeginPotato();
    }
#endregion
}
