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
        if (e_color == Constants.Global.Color.RED) {
            GameController.GetInstance().txt_redCompletionTimer.transform.parent.gameObject.SetActive(true);
            GameController.GetInstance().txt_redSelfDestructTimer.transform.parent.gameObject.SetActive(true);
            GameController.GetInstance().txt_redObjvTitle.text = "Reverse Capture the Flag";
            GameController.GetInstance().txt_redObjvDescription.text = "Shove your flag onto the opponent's side and keep it there. Be careful, if you leave yours on your side for too long, bad things'll happen!";
        } else {
            GameController.GetInstance().txt_blueCompletionTimer.transform.parent.gameObject.SetActive(true);
            GameController.GetInstance().txt_blueSelfDestructTimer.transform.parent.gameObject.SetActive(true);
            GameController.GetInstance().txt_blueObjvTitle.text = "Reverse Capture the Flag";
            GameController.GetInstance().txt_blueObjvDescription.text = "Shove your flag onto the opponent's side and keep it there. Be careful, if you leave yours on your side for too long, bad things'll happen!";
        }
        GameController.GetInstance().CompletionProgress(e_color, Constants.ObjectiveStats.C_PotatoCompletionTimer);
        GameController.GetInstance().SelfDestructProgress(e_color, Constants.ObjectiveStats.C_PotatoSelfDestructTimer);

        GameController.GetInstance().PopupFadeIn(e_color);
    }

    override protected void ResetUI() {
        // @Sam - Turn off Potato UI
        if (e_color == Constants.Global.Color.RED) {
            GameController.GetInstance().txt_redCompletionTimer.transform.parent.gameObject.SetActive(false);
            GameController.GetInstance().txt_redSelfDestructTimer.transform.parent.gameObject.SetActive(false);
        } else {
            GameController.GetInstance().txt_blueCompletionTimer.transform.parent.gameObject.SetActive(false);
            GameController.GetInstance().txt_blueSelfDestructTimer.transform.parent.gameObject.SetActive(false);
        }
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
