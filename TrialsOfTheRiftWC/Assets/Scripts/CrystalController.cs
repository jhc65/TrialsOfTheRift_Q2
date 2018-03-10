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

    //repsonsible for reseting the regen timer everytime it gets hit
    public void UpdateCrystalHealth(float f_variable) { //TODO: rename this in_variable when we decide percentage or no
        CancelInvoke();
        AdjustHealth(f_variable);

        InvokeRepeating("HealthRegen", Constants.ObjectiveStats.C_CrystalHealDelay, Constants.ObjectiveStats.C_CrystalHealRate);
    }

    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    void Start() {
        f_health = Constants.ObjectiveStats.C_CrystalMaxHealth;     // cannot read from Constants.cs in initialization at top
    }

    private void AdjustHealth(float f_variable) {
        f_health += f_variable;
        if (f_health > Constants.ObjectiveStats.C_CrystalMaxHealth)
        {
            f_health = Constants.ObjectiveStats.C_CrystalMaxHealth;
        }
        cdo_owner.UpdateCrystalHealth(f_health);
    }

    //Is called repeatedly by InvokeRepeating to regen the health of the crystal
    private void HealthRegen() {
        AdjustHealth(Constants.ObjectiveStats.C_CrystalRegenHeal);
    }
}
