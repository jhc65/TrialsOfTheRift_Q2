/*  Crystal Controller - Dana Thompson
 * 
 *  Desc:   Controls changes to Crystal Destruction Objective's Crystal health
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalController : MonoBehaviour {

    public CrystalDestructionObjective cdo_owner;  // identifies objective crystal is a part of
    [SerializeField] private Constants.Global.Color e_color;     // identifies owning team
    private float f_health;    // indicates how much health the crystal has

    // Getters
    public Constants.Global.Color Color {
        get { return e_color; }
    }
    public float Health {
        get { return f_health; }
        set { f_health = value; }
    }

    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    public void UpdateCrystalHealth(float percentage) { //TODO: rename this in_variable when we decide percentage or no
        f_health += percentage * Constants.ObjectiveStats.C_CrystalMaxHealth;
        if(f_health > Constants.ObjectiveStats.C_CrystalMaxHealth) {
            f_health = Constants.ObjectiveStats.C_CrystalMaxHealth;
        }
        cdo_owner.UpdateCrystalHealth(f_health);
    }

    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    void Start() {
        f_health = Constants.ObjectiveStats.C_CrystalMaxHealth;     // cannot read from Constants.cs in initialization at top
    }
}
