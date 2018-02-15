/*  Goal Controller - Zak Olyarnik
 * 
 *  Desc:   Holds color information for the CTF and Ice Hockey goal areas
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour {

    [SerializeField] private Constants.Global.Color e_color; // identifies owning team

    // Getters
    public Constants.Global.Color Color {
        get { return e_color; }
    }
}
