/*  Initialize Players - Zak Olyarnik
 * 
 *  Desc:   Configures player models based on values saved into Constants
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitializePlayers : MonoBehaviour {

    [SerializeField] private GameObject[] go_r1Hats, go_r2Hats, go_b1Hats, go_b2Hats;
    [SerializeField] private PlayerController pc_r1, pc_r2, pc_b1, pc_b2;
    [SerializeField] private Text txt_r1Indicator, txt_r2Indicator, txt_b1Indicator, txt_b2Indicator;
    private bool b_r1Used = false, b_r2Used = false, b_b1Used = false, b_b2Used = false;

	/*/////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    void Awake () {
        // p1
        if (Constants.PlayerStats.C_p1Color == Constants.Global.Color.RED)
        {
            if(!b_r1Used)
            {
                pc_r1.i_playerNumber = 0;
                txt_r1Indicator.text = "P1";
                go_r1Hats[Constants.PlayerStats.C_p1Hat].SetActive(true);
                b_r1Used = true;
            }
            else
            {
                pc_r2.i_playerNumber = 0;
                txt_r2Indicator.text = "P1";
                go_r2Hats[Constants.PlayerStats.C_p1Hat].SetActive(true);
                b_r2Used = true;
            }
        }
        else
        {
            if (!b_b1Used)
            {
                pc_b1.i_playerNumber = 0;
                txt_b1Indicator.text = "P1";
                go_b1Hats[Constants.PlayerStats.C_p1Hat].SetActive(true);
                b_b1Used = true;
            }
            else
            {
                pc_b2.i_playerNumber = 0;
                txt_b2Indicator.text = "P1";
                go_b2Hats[Constants.PlayerStats.C_p1Hat].SetActive(true);
                b_b2Used = true;
            }
        }

        // p2
        if (Constants.PlayerStats.C_p2Color == Constants.Global.Color.RED)
        {
            if (!b_r1Used)
            {
                pc_r1.i_playerNumber = 1;
                txt_r1Indicator.text = "P2";
                go_r1Hats[Constants.PlayerStats.C_p2Hat].SetActive(true);
                b_r1Used = true;
            }
            else
            {
                pc_r2.i_playerNumber = 1;
                txt_r2Indicator.text = "P2";
                go_r2Hats[Constants.PlayerStats.C_p2Hat].SetActive(true);
                b_r2Used = true;
            }
        }
        else
        {
            if (!b_b1Used)
            {
                pc_b1.i_playerNumber = 1;
                txt_b1Indicator.text = "P2";
                go_b1Hats[Constants.PlayerStats.C_p2Hat].SetActive(true);
                b_b1Used = true;
            }
            else
            {
                pc_b2.i_playerNumber = 1;
                txt_b2Indicator.text = "P2";
                go_b2Hats[Constants.PlayerStats.C_p2Hat].SetActive(true);
                b_b2Used = true;
            }
        }

        // p3
        if (Constants.PlayerStats.C_p3Color == Constants.Global.Color.RED)
        {
            if (!b_r1Used)
            {
                pc_r1.i_playerNumber = 2;
                txt_r1Indicator.text = "P3";
                go_r1Hats[Constants.PlayerStats.C_p3Hat].SetActive(true);
                b_r1Used = true;
            }
            else
            {
                pc_r2.i_playerNumber = 2;
                txt_r2Indicator.text = "P3";
                go_r2Hats[Constants.PlayerStats.C_p3Hat].SetActive(true);
                b_r2Used = true;
            }
        }
        else
        {
            if (!b_b1Used)
            {
                pc_b1.i_playerNumber = 2;
                txt_b1Indicator.text = "P3";
                go_b1Hats[Constants.PlayerStats.C_p3Hat].SetActive(true);
                b_b1Used = true;
            }
            else
            {
                pc_b2.i_playerNumber = 2;
                txt_b2Indicator.text = "P3";
                go_b2Hats[Constants.PlayerStats.C_p3Hat].SetActive(true);
                b_b2Used = true;
            }
        }

        // p4
        if (Constants.PlayerStats.C_p4Color == Constants.Global.Color.RED)
        {
            if (!b_r1Used)
            {
                pc_r1.i_playerNumber = 3;
                txt_r1Indicator.text = "P4";
                go_r1Hats[Constants.PlayerStats.C_p4Hat].SetActive(true);
                b_r1Used = true;
            }
            else
            {
                pc_r2.i_playerNumber = 3;
                txt_r2Indicator.text = "P4";
                go_r2Hats[Constants.PlayerStats.C_p4Hat].SetActive(true);
                b_r2Used = true;
            }
        }
        else
        {
            if (!b_b1Used)
            {
                pc_b1.i_playerNumber = 3;
                txt_b1Indicator.text = "P4";
                go_b1Hats[Constants.PlayerStats.C_p4Hat].SetActive(true);
                b_b1Used = true;
            }
            else
            {
                pc_b2.i_playerNumber = 3;
                txt_b2Indicator.text = "P4";
                go_b2Hats[Constants.PlayerStats.C_p4Hat].SetActive(true);
                b_b2Used = true;
            }
        }



    }
	
}
