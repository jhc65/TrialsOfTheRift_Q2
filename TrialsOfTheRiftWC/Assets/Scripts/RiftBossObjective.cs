/*  Rift Boss Objective - Dana Thompson
 * 
 *  Desc:   Facilitates Rift Boss Objective
 * 
 */
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiftBossObjective : Objective {

	public RiftBossController cc_activeBoss;    // active Boss Head specific to this objective instance

    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    override protected void SetUI() {
        calligrapher.RiftBossInit(cc_activeBoss.Color);
    }

    override protected void ResetUI() {
        calligrapher.RiftBossReset(cc_activeBoss.Color);
    }

    public void UpdateRiftBossHealth(float f) {
        calligrapher.UpdateRiftBossHealthUI(cc_activeBoss.Color, f);
        if (cc_activeBoss.Health <= 0) {
            b_isComplete = true;
        }
    }

    // [Param Fix] - Used in Parameters Screen. TODO: remove for release.
    public override void ParamReset() {
        cc_activeBoss.Health = Constants.ObjectiveStats.C_RiftBossMaxHealth;
        calligrapher.UpdateRiftBossHealthUI(cc_activeBoss.Color, Constants.ObjectiveStats.C_RiftBossMaxHealth);
    }
	
	void OnEnable() {
        maestro.PlayBeginRiftBoss();
    }

}