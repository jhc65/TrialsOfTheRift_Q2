/* MenuController.cs - Sam C
 * 
 * Desc: Controls the main menu and passes volume options to the main scene.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    [SerializeField]GameObject go_options;
    [SerializeField]GameObject go_mainMenu;
    [SerializeField]Button butt_optSelect;
    [SerializeField]Button butt_mainSelect;

	public void PlayTheFuckinGame() {
        SceneManager.LoadScene("RegisterPlayers");
    }

    public void OpenOptions() {
        go_options.SetActive(true);
        go_mainMenu.SetActive(false);
        butt_optSelect.Select();
    }

    public void CloseOptions() {
        go_options.SetActive(false);
        go_mainMenu.SetActive(true);
        butt_mainSelect.Select();
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

    public void QuitTheFuckinGame() {
        Application.Quit();
    }
}
