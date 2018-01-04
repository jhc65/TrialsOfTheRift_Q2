using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour {

	public Constants.Color e_color; // identifies owning team

	public Constants.Color GetColor() {
		return e_color;
	}
}
