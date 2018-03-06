using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired;

public class PlayerSelectController : MonoBehaviour {

    private Player p_player1, p_player2, p_player3, p_player4;
    bool b_settingsActive = false;

    [SerializeField] Rewired.Integration.UnityUI.RewiredStandaloneInputModule rsim;

    [SerializeField] GameObject go_settingsMenu;
    [SerializeField] GameObject go_optionsMenu;
    [SerializeField] GameObject go_selectMenu;
    [SerializeField] GameObject go_regController;

    [SerializeField] Button butt_settingsSelect;
    [SerializeField] Button butt_optionsSelect;

    public void SettingsFree() {
        b_settingsActive = false;
    }

    public void OpenParams() {
        go_settingsMenu.SetActive(true);
        go_selectMenu.SetActive(false);
        go_regController.SetActive(false);

        butt_settingsSelect.Select();
        butt_settingsSelect.OnSelect(null);
    }

    public void CloseParams() {
        go_selectMenu.SetActive(true);
        go_settingsMenu.SetActive(false);
        go_regController.SetActive(true);
        SettingsFree();
    }

    public void OpenOptions() {
        go_optionsMenu.SetActive(true);
        go_selectMenu.SetActive(false);
        go_regController.SetActive(false);

        butt_optionsSelect.Select();
        butt_optionsSelect.OnSelect(null);
    }

    public void CloseOptions() {
        go_selectMenu.SetActive(true);
        go_optionsMenu.SetActive(false);
        go_regController.SetActive(true);
        SettingsFree();
    }

    public void AdjustMasterVolume(float f_volIn) {
        Constants.VolOptions.C_MasterVolume = f_volIn;
    }

    public void AdjustBGMVolume(float f_volIn) {
        Constants.VolOptions.C_BGMVolume = f_volIn;
    }

    public void AdjustSFXVolume(float f_volIn) {
        Constants.VolOptions.C_SFXVolume = f_volIn;
    }

    public void AdjustVOIVolume(float f_volIn) {
        Constants.VolOptions.C_VOIVolume = f_volIn;
    }

    void Awake() {
        p_player1 = ReInput.players.GetPlayer(0);
    }
	
	void Update () {
        //Check for settings.
		if (p_player1.GetButtonDown("MenuSettings") && !b_settingsActive) {
            b_settingsActive = true;
            OpenParams();
        }


        //check for options.
        if (p_player1.GetButtonDown("MenuOptions") && !b_settingsActive) {
            b_settingsActive = true;
            OpenOptions();
        }

        //check for back.
         if ((p_player1.GetButtonDown("MenuBack")) && !b_settingsActive) {
            SceneManager.LoadScene("MainMenu");
        }
	}

}
