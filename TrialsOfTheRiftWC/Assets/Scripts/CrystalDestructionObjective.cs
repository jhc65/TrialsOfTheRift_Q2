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
        // @Sam - Turn on CD UI
        if (e_color == Constants.Global.Color.RED) {
            GameController.GetInstance().txt_redHealthCounter.transform.parent.gameObject.SetActive(true);
            GameController.GetInstance().txt_redObjvTitle.text = "Crystal Destruction";
            GameController.GetInstance().txt_redObjvDescription.text = "Cast spells at the enemy team's crystal to destroy it! Heal your own crystal with your own spells!";
        } else {
            GameController.GetInstance().txt_blueHealthCounter.transform.parent.gameObject.SetActive(true);
            GameController.GetInstance().txt_blueObjvTitle.text = "Crystal Destruction";
            GameController.GetInstance().txt_blueObjvDescription.text = "Cast spells at the enemy team's crystal to destroy it! Heal your own crystal with your own spells!";
        }
        GameController.GetInstance().CrystalHealth(cc_activeCrystal.Color, Constants.ObjectiveStats.C_CrystalMaxHealth);

        GameController.GetInstance().PopupFadeIn(e_color);
    }

    override protected void ResetUI() {
        // @Sam - Turn off CD UI
        if (e_color == Constants.Global.Color.RED) {
            GameController.GetInstance().txt_redHealthCounter.transform.parent.gameObject.SetActive(false);
        } else {
            GameController.GetInstance().txt_blueHealthCounter.transform.parent.gameObject.SetActive(false);
        }
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
