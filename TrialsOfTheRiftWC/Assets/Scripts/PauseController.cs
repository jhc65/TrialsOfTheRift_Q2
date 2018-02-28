
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
    [SerializeField] GameObject go_options;
    [SerializeField] GameObject go_pause;

    [SerializeField]Button butt_select;
    [SerializeField]Button butt_optSelect;
    //private Player p_player;
    private float f_unPause;
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

    public void OpenOptions() {
        go_options.SetActive(true);
        go_pause.SetActive(false);
        butt_optSelect.Select();
        butt_optSelect.OnSelect(null);
    }

    public void CloseOptions() {
        go_options.SetActive(false);
        go_pause.SetActive(true);
        butt_select.Select();
        butt_select.OnSelect(null);
    }

    public void GameReset() {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void MatchRestart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
