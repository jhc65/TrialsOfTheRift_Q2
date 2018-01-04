using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControlsTest : MonoBehaviour {

	public Text t;
	public int playerNum;
	public InputManager.Axes horizontal, vertical, wind, ice, interact, menu, submit, cancel, electric, mm, aimh, aimv;

	// Use this for initialization
	void Start () {
        // controller mapping defaults to XBOX, so only change if PS4 controller connected
        if (Input.GetJoystickNames()[0] == "Wireless Controller")
        { // PS4 connection message
            InputManager.P1_Map = InputManager.P1_PS4;
            Debug.Log("P1 PS4");
            t.text = "Press X";
        }
        else
        {
            InputManager.P1_Map = InputManager.P1_XBOX;
            Debug.Log("P1 XBOX");
            t.text = "Press A";
        }
    }

    // Update is called once per frame
    void Update()
    {
        float speed = 10.0F;
        float h = speed * InputManager.GetAxis(aimh, 1);
        float v = speed * InputManager.GetAxis(aimv, 1);
        t.transform.Translate(h, v, 0);

        if (InputManager.GetButtonDown(wind, playerNum))
        {
            t.text = "P" + playerNum + " wind";
        }
        if (InputManager.GetButtonDown(ice, playerNum))
        {
            t.text = "P" + playerNum + " ice";
        }
        if (InputManager.GetButtonDown(interact, playerNum))
        {
            t.text = "P" + playerNum + " interact";
        }
        if (InputManager.GetButtonDown(menu, playerNum))
        {
            t.text = "P" + playerNum + " menu";
        }
        if (InputManager.GetButtonDown(submit, playerNum))
        {
            t.text = "P" + playerNum + " submit";
        }
        if (InputManager.GetButtonDown(cancel, playerNum))
        {
            t.text = "P" + playerNum + " cancel";
        }
        if (InputManager.GetButtonDown(electric, playerNum))
        {
            t.text = "P" + playerNum + " electric";
        }
        if (InputManager.GetButtonDown(mm, playerNum))
        {
            t.text = "P" + playerNum + " mm";
        }

        for (int i = 3; i < 29; i++)
        {
            if(Input.GetAxis("Axis" + i) != 0 && Input.GetAxis("Axis" + i) != -1)
            {
                Debug.Log("Axis" + i + " - " + Input.GetAxis("Axis" + i));
            }
        }
    }
}
