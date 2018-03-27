/*  Wind Spell Controller - Zak Olyarnik
 * 
 *  Desc:   Facilitates Wind Spell interactions
 * 
 */

using UnityEngine;

public class WindSpellController : SpellController {
#region Wind Spell Controller Methods
    protected override void Charge(float f_chargeTime) {
        f_charge = (f_chargeTime * 1/3) + 1;
        if (f_charge > 2f) {
            f_charge = 2f;
        }
        f_damage = Constants.SpellStats.C_WindDamage;
        f_damage *= ((f_charge*1/12) + 1);
        transform.localScale *= f_charge*0.7f;
    }

    protected override void BuffSpell() {
        riftController.IncreaseVolatility(Constants.RiftStats.C_VolatilityIncrease_SpellCross);
        f_damage = f_damage * Constants.SpellStats.C_WindRiftDamageMultiplier;
        transform.localScale *= Constants.SpellStats.C_SpellScaleMultiplier;
    }
#endregion
    
#region Unity Overrides
    void OnDestroy() {
        Instantiate(go_onDestroyParticles, transform.position, Quaternion.identity);
    }
#endregion
}
