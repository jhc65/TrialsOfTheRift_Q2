using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceController : SpellController {

	private float f_iceDamage = Constants.SpellStats.C_IceDamage;

	//TODO: explore this more:
	/*
	// SendMessage() calls all functions named the parameter that exist in MonoBehavior 
    // scripts on the GameObject. This way, we don't have worry about differentiating 
    // between freezing a player or freezing an enemy. It'll just find the function
    // named Freeze() on the GameObject's controller script.
    protected override void ApplyEffect(GameObject go_target){
		go_target.SendMessage("Freeze"); 
	}
	*/

	protected override void ApplyEffect(GameObject go_target) {
        if (go_target.tag == "Player")
        {
            go_target.GetComponent<PlayerController>().Freeze();
        }
        else if (go_target.tag == "Enemy")
        {
            go_target.GetComponent<EnemyController>().TakeDamage(f_iceDamage);
            go_target.GetComponent<EnemyController>().Freeze(0f);
        }
        else if (go_target.tag == "Crystal")
        {
            Constants.Color crystalColor = go_target.GetComponent<CrystalController>().e_color;
			if (crystalColor != e_color){
				go_target.GetComponent<CrystalController>().ChangeHealth(Constants.SpellStats.C_SpellCrystalDamagePercent);
			}
			else if (crystalColor == e_color) {
				go_target.GetComponent<CrystalController>().ChangeHealth(Constants.SpellStats.C_SpellCrystalHealPercent);
			}
        }
    }

    protected override void BuffSpell() {
        // Increase Volatility by 0.5%
        RiftController.GetInstance().IncreaseVolatility(Constants.RiftStats.C_VolatilityIncrease_SpellCross);
        f_iceDamage = f_iceDamage * Constants.SpellStats.C_IceDamageMultiplier;
        transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
    }
}

