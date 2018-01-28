using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour {

	public Constants.Global.Color e_color; // identifies owning team

	public Constants.Global.Color GetColor() {
		return e_color;
	}
}
