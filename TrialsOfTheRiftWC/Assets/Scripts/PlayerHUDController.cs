using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDController : MonoBehaviour {

    [SerializeField] PlayerController playc_trackedPlayer;
    [SerializeField] Image img_healthbar;
    [SerializeField] Image img_windbar;
    [SerializeField] Image img_icebar;
	[SerializeField] Image img_electricbar;

	void Update () {
		img_windbar.fillAmount = playc_trackedPlayer.GetNextWind() / Constants.SpellStats.C_WindCooldown;
        img_icebar.fillAmount = playc_trackedPlayer.GetNextIce() / Constants.SpellStats.C_IceCooldown;
		img_electricbar.fillAmount = playc_trackedPlayer.GetNextElectric() / Constants.SpellStats.C_ElectricCooldown;
		img_healthbar.fillAmount = playc_trackedPlayer.GetCurrentHealth() / Constants.PlayerStats.C_MaxHealth;
	}
}
