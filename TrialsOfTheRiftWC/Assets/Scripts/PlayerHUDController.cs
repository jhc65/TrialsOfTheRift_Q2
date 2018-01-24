using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDController : MonoBehaviour {

    [SerializeField] PlayerController playc_trackedPlayer;
    [SerializeField] Screenshake sshk_shaker;
    [SerializeField] Image img_healthbar;
    [SerializeField] Image img_windbar;
    [SerializeField] Image img_icebar;
	[SerializeField] Image img_electricbar;

    private Color col_origIceColor;
    private Color col_origWindColor;
    private Color col_origElecColor;

    private void Start() {
        col_origElecColor = img_electricbar.color;
        col_origIceColor = img_icebar.color;
        col_origWindColor = img_windbar.color;
    }

    void Update () {
		img_windbar.fillAmount = playc_trackedPlayer.GetNextWind() / Constants.SpellStats.C_WindCooldown;
        img_icebar.fillAmount = playc_trackedPlayer.GetNextIce() / Constants.SpellStats.C_IceCooldown;
		img_electricbar.fillAmount = playc_trackedPlayer.GetNextElectric() / Constants.SpellStats.C_ElectricCooldown;
		img_healthbar.fillAmount = playc_trackedPlayer.GetCurrentHealth() / Constants.PlayerStats.C_MaxHealth;

        if (img_icebar.fillAmount == 1) {
            img_icebar.color = Color.Lerp(col_origIceColor, Color.white, Mathf.PingPong(Time.time, 1));
        } else {
            img_icebar.color = col_origIceColor;
        }

        if (img_electricbar.fillAmount == 1) {
            img_electricbar.color = Color.Lerp(col_origElecColor, Color.white, Mathf.PingPong(Time.time, 1));
        } else {
            img_electricbar.color = col_origElecColor;
        }

        if (img_windbar.fillAmount == 1) {
            img_windbar.color = Color.Lerp(col_origWindColor, Color.white, Mathf.PingPong(Time.time, 1));
        } else {
            img_windbar.color = col_origWindColor;
        }
	}

    public void ShakeUI() {
        sshk_shaker.SetShake(1);
        sshk_shaker.StartShake();
    }
}
