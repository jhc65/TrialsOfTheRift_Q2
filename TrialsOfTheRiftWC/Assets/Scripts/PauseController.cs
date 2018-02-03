
/* PauseController.cs - Sam C
 * 
 * Desc: Facilitates pausing the game and limiting it to only one user.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class PauseController : MonoBehaviour {

    static PlayerController pc_owner;
    public GameObject img_pauseBacking;
    public Text txt_pauseIndicator;
    private Player p_player;
    private float f_unPause;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (pc_owner != null) {
            if (p_player.GetButtonTimedPressUp("Menu",0.3f)) {
                Unpause();
            }
        }
        
	}

    public void Pause(PlayerController pc_in) {
        if (pc_owner == null) {
            pc_owner = pc_in;
            txt_pauseIndicator.text = "P" + (pc_owner.i_playerNumber + 1) + " Pause.\n\nHold and Release Start/Options to Resume.";
            p_player = ReInput.players.GetPlayer(pc_owner.i_playerNumber);
            img_pauseBacking.SetActive(true);
            Time.timeScale = 0;
        }  
    }

    public void Unpause() {
        pc_owner = null;
        img_pauseBacking.SetActive(false);
        Time.timeScale = 1;
    }


}
