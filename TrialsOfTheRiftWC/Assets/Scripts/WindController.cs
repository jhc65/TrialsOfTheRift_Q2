using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindController : SpellController {

    private float f_windDamage = Constants.SpellStats.C_WindDamage;

    protected override void ApplyEffect(GameObject go_target) {
        if (go_target.tag == "Player")
        {
            Vector3 v3_direction = transform.forward.normalized;
            go_target.GetComponent<Rigidbody>().AddForce(v3_direction * Constants.SpellStats.C_WindForce);
            go_target.GetComponent<PlayerController>().Drop();
        }
        else if (go_target.tag == "Enemy")
        {
            Vector3 v3_direction = transform.forward.normalized;
            go_target.GetComponent<Rigidbody>().AddForce(v3_direction * Constants.SpellStats.C_WindForce);
            go_target.GetComponent<EnemyController>().TakeDamage(f_windDamage);
        }
        else if (go_target.tag == "Crystal")
        {
            Constants.Color crystalColor = go_target.GetComponent<CrystalController>().e_color;
            if (crystalColor != e_color)
            {
                go_target.GetComponent<CrystalController>().ChangeHealth(Constants.SpellStats.C_SpellCrystalDamagePercent);
            }
            else if (crystalColor == e_color)
            {
                go_target.GetComponent<CrystalController>().ChangeHealth(Constants.SpellStats.C_SpellCrystalHealPercent);
            }
        }
        else if (go_target.tag == "Potato") {
            go_target.GetComponent<Rigidbody>().isKinematic = false;
            Vector3 v3_direction = transform.forward.normalized;
            go_target.GetComponent<Rigidbody>().AddForce(v3_direction * Constants.SpellStats.C_WindForce);
        }
    }

    protected override void BuffSpell() {
        // Increase Volatility by 0.5%
        RiftController.GetInstance().IncreaseVolatility(Constants.RiftStats.C_VolatilityIncrease_SpellCross);
        f_windDamage = f_windDamage * Constants.SpellStats.C_WindDamageMultiplier;
        transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
    }
}
