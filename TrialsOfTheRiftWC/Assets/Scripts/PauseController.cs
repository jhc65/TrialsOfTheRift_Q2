
/* PauseController.cs - Sam C
 * 
 * Desc: Facilitates pausing the game and limiting it to only one user.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Rewired;

public class PauseController : MonoBehaviour {

    static PlayerController pc_owner;
    public GameObject img_pauseBacking;
    public Text txt_pauseIndicator;
    //private Player p_player;
    private float f_unPause;
    [SerializeField]Button butt_select;
    [SerializeField] Rewired.Integration.UnityUI.RewiredStandaloneInputModule rsim;
    [SerializeField] EventSystem es_master;


    public void Pause(PlayerController pc_in) {
        if (pc_owner == null) {
            pc_owner = pc_in;
            txt_pauseIndicator.text = "P" + (pc_owner.i_playerNumber + 1) + " Pause.";
            img_pauseBacking.SetActive(true);

            rsim.RewiredPlayerIds = new int[] { pc_owner.i_playerNumber };

            //Properly highlight the button.
            butt_select.Select();
            butt_select.OnSelect(null);

            Time.timeScale = 0;
            
        }  
    }

    public void Unpause() {
        pc_owner = null;
        img_pauseBacking.SetActive(false);
        Time.timeScale = 1;
    }

    public void GameReset() {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
