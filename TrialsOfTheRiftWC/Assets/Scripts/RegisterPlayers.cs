/*  Register Players - Zak Olyarnik
 * 
 *  Desc:   Connects controllers and allows player and team selection.
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Rewired;

public class RegisterPlayers : MonoBehaviour {

    [SerializeField] private Image go_backing1, go_backing2, go_backing3, go_backing4;
    [SerializeField] private Image go_hat1, go_hat2, go_hat3, go_hat4;
    [SerializeField] private Sprite img_red, img_blue;
    [SerializeField] private Sprite[] img_hats;
    [SerializeField] private Text txt_p1Message, txt_p2Message, txt_p3Message, txt_p4Message;
    [SerializeField] private GameObject go_go;
    private Player p_player1, p_player2, p_player3, p_player4;
    private bool b_p1Connected = false, b_p2Connected = false, b_p3Connected = false, b_p4Connected = false;	// set when 4 controllers are detected
    private bool b_p1Ready = false, b_p2Ready = false, b_p3Ready = false, b_p4Ready = false;
    private Constants.Global.Color e_p1Color, e_p2Color, e_p3Color, e_p4Color;
    private int i_numRed = 0, i_numBlue = 0;
    private int i_p1Hat, i_p2Hat, i_p3Hat, i_p4Hat;

	/*/////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
		
    void Awake()
    {
        p_player1 = ReInput.players.GetPlayer(0);
        p_player2 = ReInput.players.GetPlayer(1);
        p_player3 = ReInput.players.GetPlayer(2);
        p_player4 = ReInput.players.GetPlayer(3);
    }

    void Start()
    {
        e_p1Color = Constants.PlayerStats.C_p1Color;
        e_p2Color = Constants.PlayerStats.C_p2Color;
        e_p3Color = Constants.PlayerStats.C_p3Color;
        e_p4Color = Constants.PlayerStats.C_p4Color;
        i_p1Hat = Constants.PlayerStats.C_p1Hat;
        i_p2Hat = Constants.PlayerStats.C_p2Hat;
        i_p3Hat = Constants.PlayerStats.C_p3Hat;
        i_p4Hat = Constants.PlayerStats.C_p4Hat;
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
            SceneManager.LoadScene("WarmUp");

        // test connection
        int connectedControllers = ReInput.controllers.GetControllerCount(ControllerType.Joystick);
        if (connectedControllers >= 1 && !b_p1Connected)
        {
            txt_p1Message.text = "CONNECTED";
            b_p1Connected = true;
            go_backing1.sprite = img_red;
            go_hat1.sprite = img_hats[i_p1Hat];
            //e_p1Color = Constants.Global.Color.RED;
        }
        if (Input.GetJoystickNames().Length >= 2 && !b_p2Connected)
        {
            txt_p2Message.text = "CONNECTED";
            b_p2Connected = true;
            go_backing2.sprite = img_red;
            go_hat2.sprite = img_hats[i_p2Hat];
            //e_p1Color = Constants.Global.Color.RED;
        }
        if (Input.GetJoystickNames().Length >= 3 && !b_p3Connected)
        {
            txt_p3Message.text = "CONNECTED";
            b_p3Connected = true;
            go_backing3.sprite = img_blue;
            go_hat3.sprite = img_hats[i_p3Hat];
            //e_p1Color = Constants.Global.Color.BLUE;
        }
        if (Input.GetJoystickNames().Length == 4 && !b_p4Connected)
        {
            txt_p4Message.text = "CONNECTED";
            b_p4Connected = true;
            go_backing4.sprite = img_blue;
            go_hat4.sprite = img_hats[i_p4Hat];
            //e_p1Color = Constants.Global.Color.BLUE;
        }


        // switch colors
        if ((p_player1.GetButtonDown("UIPageLeft") || p_player1.GetButtonDown("UIPageRight")) && !b_p1Ready)    // AND NOT READY!!!!
        {
            if (go_backing1.sprite == img_red)
            {
                go_backing1.sprite = img_blue;
                e_p1Color = Constants.Global.Color.BLUE;
            }
            else
            {
                go_backing1.sprite = img_red;
                e_p1Color = Constants.Global.Color.RED;
            }
        }
        if ((p_player2.GetButtonDown("UIPageLeft") || p_player2.GetButtonDown("UIPageRight")) && !b_p2Ready)
        {
            if (go_backing2.sprite == img_red)
            {
                go_backing2.sprite = img_blue;
                e_p2Color = Constants.Global.Color.BLUE;
            }
            else
            {
                go_backing2.sprite = img_red;
                e_p2Color = Constants.Global.Color.RED;
            }
        }
        if ((p_player3.GetButtonDown("UIPageLeft") || p_player3.GetButtonDown("UIPageRight")) && !b_p3Ready)
        {
            if (go_backing3.sprite == img_red)
            {
                go_backing3.sprite = img_blue;
                e_p3Color = Constants.Global.Color.BLUE;
            }
            else
            {
                go_backing3.sprite = img_red;
                e_p3Color = Constants.Global.Color.RED;
            }
        }
        if ((p_player4.GetButtonDown("UIPageLeft") || p_player4.GetButtonDown("UIPageRight")) && !b_p4Ready)
        {
            if (go_backing4.sprite == img_red)
            {
                go_backing4.sprite = img_blue;
                e_p4Color = Constants.Global.Color.BLUE;
            }
            else
            {
                go_backing4.sprite = img_red;
                e_p4Color = Constants.Global.Color.RED;
            }
        }


        // switch hats               // AND NOT READY!!!!
        if ((p_player1.GetNegativeButtonDown("UIHorizontal") && !b_p1Ready))
        {
            i_p1Hat--;
            if(i_p1Hat < 0)
            {
                i_p1Hat = 3;
            }
            go_hat1.sprite = img_hats[i_p1Hat];
        }
        if ((p_player1.GetButtonDown("UIHorizontal") && !b_p1Ready))
        {
            i_p1Hat++;
            if (i_p1Hat > 3)
            {
                i_p1Hat = 0;
            }
            go_hat1.sprite = img_hats[i_p1Hat];
        }
        if ((p_player2.GetNegativeButtonDown("UIHorizontal") && !b_p2Ready))
        {
            i_p2Hat--;
            if (i_p2Hat < 0)
            {
                i_p2Hat = 3;
            }
            go_hat2.sprite = img_hats[i_p2Hat];
        }
        if ((p_player2.GetButtonDown("UIHorizontal") && !b_p2Ready))
        {
            i_p2Hat++;
            if (i_p2Hat > 3)
            {
                i_p2Hat = 0;
            }
            go_hat2.sprite = img_hats[i_p2Hat];
        }
        if ((p_player3.GetNegativeButtonDown("UIHorizontal") && !b_p3Ready))
        {
            i_p3Hat--;
            if (i_p3Hat < 0)
            {
                i_p3Hat = 3;
            }
            go_hat3.sprite = img_hats[i_p3Hat];
        }
        if ((p_player3.GetButtonDown("UIHorizontal") && !b_p3Ready))
        {
            i_p3Hat++;
            if (i_p3Hat > 3)
            {
                i_p3Hat = 0;
            }
            go_hat3.sprite = img_hats[i_p3Hat];
        }
        if ((p_player4.GetNegativeButtonDown("UIHorizontal") && !b_p4Ready))
        {
            i_p4Hat--;
            if (i_p4Hat < 0)
            {
                i_p4Hat = 3;
            }
            go_hat4.sprite = img_hats[i_p4Hat];
        }
        if ((p_player4.GetButtonDown("UIHorizontal") && !b_p4Ready))
        {
            i_p4Hat++;
            if (i_p4Hat > 3)
            {
                i_p4Hat = 0;
            }
            go_hat4.sprite = img_hats[i_p4Hat];
        }


        // confirm selection
        if (p_player1.GetButtonDown("UISubmit") && !b_p1Ready)
        {
            if (e_p1Color == Constants.Global.Color.RED && i_numRed < 2)
            {
                i_numRed++;
                txt_p1Message.text = "OK!";
                b_p1Ready = true;
            }
            else if (e_p1Color == Constants.Global.Color.BLUE && i_numBlue < 2)
            {
                i_numBlue++;
                txt_p1Message.text = "OK!";
                b_p1Ready = true;
            }
        }
        if (p_player2.GetButtonDown("UISubmit") && !b_p2Ready)
        {
            if (e_p2Color == Constants.Global.Color.RED && i_numRed < 2)
            {
                i_numRed++;
                txt_p2Message.text = "OK!";
                b_p2Ready = true;
            }
            else if (e_p2Color == Constants.Global.Color.BLUE && i_numBlue < 2)
            {
                i_numBlue++;
                txt_p2Message.text = "OK!";
                b_p2Ready = true;
            }
        }
        if (p_player3.GetButtonDown("UISubmit") && !b_p3Ready)
        {
            if (e_p3Color == Constants.Global.Color.RED && i_numRed < 2)
            {
                i_numRed++;
                txt_p3Message.text = "OK!";
                b_p3Ready = true;
            }
            else if (e_p3Color == Constants.Global.Color.BLUE && i_numBlue < 2)
            {
                i_numBlue++;
                txt_p3Message.text = "OK!";
                b_p3Ready = true;
            }
        }
        if (p_player4.GetButtonDown("UISubmit") && !b_p4Ready)
        {
            if (e_p4Color == Constants.Global.Color.RED && i_numRed < 2)
            {
                i_numRed++;
                txt_p4Message.text = "OK!";
                b_p4Ready = true;
            }
            else if (e_p4Color == Constants.Global.Color.BLUE && i_numBlue < 2)
            {
                i_numBlue++;
                txt_p4Message.text = "OK!";
                b_p4Ready = true;
            }
        }
        

        // reset selection
        if (p_player1.GetButtonDown("UICancel") && b_p1Ready)
        {
            // reduce color number
            if (e_p1Color == Constants.Global.Color.RED)
            {
                i_numRed--;
            }
            else if (e_p1Color == Constants.Global.Color.BLUE)
            {
                i_numBlue--;
            }
            txt_p1Message.text = "CONNECTED!";
            b_p1Ready = false;
        }
        if (p_player2.GetButtonDown("UICancel") && b_p2Ready)
        {
            // reduce color number
            if (e_p2Color == Constants.Global.Color.RED)
            {
                i_numRed--;
            }
            else if (e_p2Color == Constants.Global.Color.BLUE)
            {
                i_numBlue--;
            }
            txt_p2Message.text = "CONNECTED!";
            b_p2Ready = false;
        }
        if (p_player3.GetButtonDown("UICancel") && b_p3Ready)
        {
            // reduce color number
            if (e_p3Color == Constants.Global.Color.RED)
            {
                i_numRed--;
            }
            else if (e_p3Color == Constants.Global.Color.BLUE)
            {
                i_numBlue--;
            }
            txt_p3Message.text = "CONNECTED!";
            b_p3Ready = false;
        }
        if (p_player4.GetButtonDown("UICancel") && b_p4Ready)
        {
            // reduce color number
            if (e_p4Color == Constants.Global.Color.RED)
            {
                i_numRed--;
            }
            else if (e_p4Color == Constants.Global.Color.BLUE)
            {
                i_numBlue--;
            }
            txt_p4Message.text = "CONNECTED!";
            b_p4Ready = false;
        }


        // load next scene
        if (b_p1Ready && b_p2Ready && b_p3Ready && b_p4Ready)
        {
            go_go.SetActive(true);
            if (p_player1.GetButtonDown("MenuUISubmit"))
            {
                // set constants color, hat for all 4 players
                Constants.PlayerStats.C_p1Color = e_p1Color;
                Constants.PlayerStats.C_p2Color = e_p2Color;
                Constants.PlayerStats.C_p3Color = e_p3Color;
                Constants.PlayerStats.C_p4Color = e_p4Color;
                Constants.PlayerStats.C_p1Hat = i_p1Hat;
                Constants.PlayerStats.C_p2Hat = i_p2Hat;
                Constants.PlayerStats.C_p3Hat = i_p3Hat;
                Constants.PlayerStats.C_p4Hat = i_p4Hat;

                SceneManager.LoadScene("WarmUp");
            }
        }
        else
        {
            go_go.SetActive(false);
        }
    }
}
