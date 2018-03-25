/*  Crystal Controller - Dana Thompson
 * 
 *  Desc:   Controls changes to Crystal Destruction Objective's Crystal health
 * 
 */

using UnityEngine;

public class CrystalController : SpellTarget {
#region Variables and Declarations
    [SerializeField] private CrystalDestructionObjective cdo_owner;     // identifies objective crystal is a part of
#endregion

#region CrystalController Methods
    override public void ApplySpellEffect(Constants.SpellStats.SpellType spell, Constants.Global.Color color, float damage, Vector3 direction) {
        if (color != e_color) { // crystal is not affected by spells from the owning team
            CancelInvoke("HealthRegen");    // cancels and restarts health regen timer each time the crystal is hit
            AdjustHealth(-damage / 2.0f);
            InvokeRepeating("HealthRegen", Constants.ObjectiveStats.C_CrystalHealDelay, Constants.ObjectiveStats.C_CrystalHealRate);
        }
    }

    // Crystal can be healed as well as damaged, so this method replaces TakeDamage()
    private void AdjustHealth(float change) {
        f_health += change;
        if (f_health > Constants.ObjectiveStats.C_CrystalMaxHealth) {
            f_health = Constants.ObjectiveStats.C_CrystalMaxHealth;
        }
        cdo_owner.UpdateCrystalHealth(f_health);
    }

    // Called via InvokeRepeating() to heal the crystal
    private void HealthRegen() {
        AdjustHealth(Constants.ObjectiveStats.C_CrystalRegenHeal);
    }

#endregion

#region Unity Overrides
    void Start() {
        f_health = Constants.ObjectiveStats.C_CrystalMaxHealth;     // cannot read from Constants.cs in initialization at top
    }
#endregion
}
