/*  Crystal Destruction Objective - Dana Thompson
 * 
 *  Desc:   Facilitates Crystal Destruction Objective
 * 
 */
 
public class CrystalDestructionObjective : Objective {
#region Variables and Declarations
    public CrystalController cc_activeCrystal;    // active crystal specific to this objective instance
#endregion
    
#region CrystalDestructionObjective Methods
    override protected void SetUI() {
        calligrapher.CrystalDestructionInit(cc_activeCrystal.Color);
    }

    override protected void ResetUI() {
        calligrapher.CrystalDestructionReset(cc_activeCrystal.Color);
    }

    // Update UI and check for completion
    public void UpdateCrystalHealth(float f) {
        calligrapher.UpdateCrystalHealthUI(cc_activeCrystal.Color, f);
        if (f <= 0) {
            b_isComplete = true;
        }
    }
#endregion

#region Unity Overrides
    void OnEnable() {
        maestro.PlayBeginCrystalDestruction();
    }
#endregion
}
