/*  Rift Boss Objective - Dana Thompson
 * 
 *  Desc:   Facilitates Rift Boss Objective
 * 
 */
 
public class RiftBossObjective : Objective {

#region RiftBossObjective Methods
    override protected void SetUI() {
        calligrapher.RiftBossInit(e_color);
    }

    override protected void ResetUI() {
        calligrapher.RiftBossReset(e_color);
    }

    // Update UI and check for completion
    public void UpdateRiftBossHealth(float f) {
        calligrapher.UpdateRiftBossHealthUI(e_color, f);
        if (f <= 0) {
            b_isComplete = true;
        }
    }
#endregion

#region Unity Overrides
    void OnEnable() {
        maestro.PlayBeginRiftBoss();
    }
#endregion

}