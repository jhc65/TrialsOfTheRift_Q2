﻿using System.Collections;
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
    private Vector2 v2_origIceSize;
    private Vector2 v2_origWindSize;
    private Vector2 v2_origElecSize;
    private Color col_UIBacking;

    private void Start() {
        col_origElecColor = img_electricbar.color;
        col_origIceColor = img_icebar.color;
        col_origWindColor = img_windbar.color;
        v2_origIceSize = img_icebar.rectTransform.sizeDelta;
        v2_origElecSize = img_electricbar.rectTransform.sizeDelta;
        v2_origWindSize = img_windbar.rectTransform.sizeDelta;
        col_UIBacking = GetComponent<Image>().color;
    }

    void Update () {
		img_windbar.fillAmount = playc_trackedPlayer.GetNextWind() / Constants.SpellStats.C_WindCooldown;
        img_icebar.fillAmount = playc_trackedPlayer.GetNextIce() / Constants.SpellStats.C_IceCooldown;
		img_electricbar.fillAmount = playc_trackedPlayer.GetNextElectric() / Constants.SpellStats.C_ElectricCooldown;
		img_healthbar.fillAmount = playc_trackedPlayer.GetCurrentHealth() / Constants.PlayerStats.C_MaxHealth;


        //Statements for doing visual things to the spell counters.
        if (img_icebar.fillAmount == 1) {
            img_icebar.color = Color.Lerp(col_origIceColor, Color.white, Mathf.PingPong(Time.time, 1));
            img_icebar.rectTransform.sizeDelta = Vector2.Lerp(v2_origIceSize, v2_origIceSize + new Vector2(10f,10f), Mathf.PingPong(Time.time, 1));
        } else {
            img_icebar.color = col_origIceColor;
            img_icebar.rectTransform.sizeDelta = v2_origIceSize;
        }

        if (img_electricbar.fillAmount == 1) {
            img_electricbar.color = Color.Lerp(col_origElecColor, Color.white, Mathf.PingPong(Time.time, 1));
            img_electricbar.rectTransform.sizeDelta = Vector2.Lerp(v2_origElecSize, v2_origElecSize + new Vector2(10f,10f), Mathf.PingPong(Time.time, 1));
        } else {
            img_electricbar.color = col_origElecColor;
            img_electricbar.rectTransform.sizeDelta = v2_origElecSize;
        }

        if (img_windbar.fillAmount == 1) {
            img_windbar.color = Color.Lerp(col_origWindColor, Color.white, Mathf.PingPong(Time.time, 1));
            img_windbar.rectTransform.sizeDelta = Vector2.Lerp(v2_origWindSize, v2_origWindSize + new Vector2(10f,10f), Mathf.PingPong(Time.time, 1));
        } else {
            img_windbar.color = col_origWindColor;
            img_windbar.rectTransform.sizeDelta = v2_origWindSize;
        }

        //Statements for doing visual things to the UI itself.
        if (img_healthbar.fillAmount <= 0) {
            GetComponent<Image>().color = Color.gray;
        } else {
            GetComponent<Image>().color = col_UIBacking;
        }
	}

    public void ShakeUI() {
        sshk_shaker.SetShake(1);
        sshk_shaker.StartShake();
    }
}
