/*  Goal Controller - Zak Olyarnik
 * 
 *  Desc:   Holds color information for the CTF and Ice Hockey goal areas
 * 
 */

using UnityEngine;

public class GoalController : MonoBehaviour {
#region Variables and Declarations
    [SerializeField] private Constants.Global.Color e_color; // identifies owning team
#endregion

#region Getters and Setters
    public Constants.Global.Color Color {
        get { return e_color; }
    }
#endregion
}
