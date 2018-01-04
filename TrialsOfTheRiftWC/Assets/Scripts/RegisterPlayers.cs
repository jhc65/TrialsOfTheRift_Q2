using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RegisterPlayers : MonoBehaviour {

	public GameObject go_connectMessage;	// "Connect 4 controllers"
	public Text txt_p1Message, txt_p2Message, txt_p3Message, txt_p4Message;	// identifies button to confirm based on connected controller type
	public GameObject go_r1, go_r2, go_b1, go_b2;		// background images displayed when players are "ready"
	private bool b_connected = false;	// set when 4 controllers are detected
	private bool b_p1Ready = false, b_p2Ready = false, b_p3Ready = false, b_p4Ready = false;	// set as each player confirms


	//TODO: something with player color, then have the Main Scene's GameController read the set colors (and character models?)
		// from here to instantiate player prefabs correctly.


	void MapControllers() {
		b_connected = true;
		go_connectMessage.SetActive(false);

		// controller mapping defaults to XBOX, so only change if PS4 controller connected
		if (Input.GetJoystickNames()[0] == "Wireless Controller") { // PS4 connection message
			InputManager.P1_Map = InputManager.P1_PS4;
			Debug.Log("P1 PS4");
			txt_p1Message.text = "Press X";
		}
		else {
			InputManager.P1_Map = InputManager.P1_XBOX;
			Debug.Log("P1 XBOX");
			txt_p1Message.text = "Press A";
		}

		if (Input.GetJoystickNames()[1] == "Wireless Controller") {
			InputManager.P2_Map = InputManager.P2_PS4;
			Debug.Log("P2 PS4");
			txt_p2Message.text = "Press X";
		}
		else {
			InputManager.P2_Map = InputManager.P2_XBOX;
			Debug.Log("P2 XBOX");
			txt_p2Message.text = "Press A";
		}

		if (Input.GetJoystickNames()[2] == "Wireless Controller") {
			InputManager.P3_Map = InputManager.P3_PS4;
			Debug.Log("P3 PS4");
			txt_p3Message.text = "Press X";
		}
		else {
			InputManager.P3_Map = InputManager.P3_XBOX;
			Debug.Log("P3 XBOX");
			txt_p3Message.text = "Press A";
		}

		if (Input.GetJoystickNames()[3] == "Wireless Controller") {
			InputManager.P4_Map = InputManager.P4_PS4;
			Debug.Log("P4 PS4");
			txt_p4Message.text = "Press X";
		}
		else {
			InputManager.P4_Map = InputManager.P4_XBOX;
			Debug.Log("P4 XBOX");
			txt_p4Message.text = "Press A";
		}
	}

	void Update() {
		if (!b_connected){
			if (Input.GetJoystickNames().Length >= 1) {
				txt_p1Message.text = "CONNECTED";
			}
			if (Input.GetJoystickNames().Length >= 2) {
				txt_p2Message.text = "CONNECTED";
			}
			if (Input.GetJoystickNames().Length >= 3) {
				txt_p3Message.text = "CONNECTED";
			}
			if (Input.GetJoystickNames().Length == 4) {     // when 4 controllers are detected, commence mapping
				txt_p4Message.text = "CONNECTED";
				MapControllers();
			}
		}
		else{
			if (InputManager.GetButtonDown(InputManager.Axes.SUBMIT, 1)) {
				b_p1Ready = true;
				txt_p1Message.text = "READY";
				go_r1.SetActive(true);
			}
			if (InputManager.GetButtonDown(InputManager.Axes.SUBMIT, 2)) {
				b_p2Ready = true;
				txt_p2Message.text = "READY";
				go_r2.SetActive(true);
			}
			if (InputManager.GetButtonDown(InputManager.Axes.SUBMIT, 3)) {
				b_p3Ready = true;
				txt_p3Message.text = "READY";
				go_b1.SetActive(true);
			}
			if (InputManager.GetButtonDown(InputManager.Axes.SUBMIT, 4)) {
				b_p4Ready = true;
				txt_p4Message.text = "READY";
				go_b2.SetActive(true);
			}
			if (b_p1Ready && b_p2Ready && b_p3Ready && b_p4Ready) {
				SceneManager.LoadScene("BuildSetUp");
			}
		}
	}
}
