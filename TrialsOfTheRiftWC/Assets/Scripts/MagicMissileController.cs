using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMissileController : SpellController {

    int i_reflects = 2;

    protected override void ApplyEffect(GameObject go_target, Collision collision) {
        if (go_target.tag == "Player") {
            if (go_target.GetComponent<PlayerController>().GetColor() != this.e_color)
            {
                go_target.GetComponent<PlayerController>().TakeDamage(Constants.SpellStats.C_MagicMissileDamage * f_charged);
                go_target.GetComponent<PlayerController>().Drop();
            }
            else {
                go_target.GetComponent<PlayerController>().Heal(Constants.SpellStats.C_MagicMissileHeal * f_charged);
            }
        }
		else if (go_target.tag == "Enemy") {
            go_target.GetComponent<EnemyController>().TakeDamage(Constants.SpellStats.C_MagicMissileDamage * f_charged);
        }
		else if (go_target.tag == "Crystal"){
			Constants.Color crystalColor = go_target.GetComponent<CrystalController>().e_color;
			if (crystalColor != e_color){
				go_target.GetComponent<CrystalController>().ChangeHealth(Constants.SpellStats.C_MMCrystalDamagePercent * f_charged);
			}
			else if (crystalColor == e_color) {
				go_target.GetComponent<CrystalController>().ChangeHealth(Constants.SpellStats.C_MMCrystalHealPercent * f_charged);
			}
		} else if (go_target.tag == "Wall") {
            Debug.Log(i_reflects);
            if (i_reflects > 0) {
                i_reflects -= 1;
                Vector3 v = Vector3.Reflect(transform.forward, collision.contacts[0].normal);
                float rot = 90 - Mathf.Atan2(v.z, v.x) * Mathf.Rad2Deg;
                transform.eulerAngles = new Vector3(0,rot,0);
                GetComponent<Rigidbody>().velocity = transform.forward * Constants.SpellStats.C_MagicMissileSpeed;
            } else {
                Destroy(gameObject);
            }
        }
    }

    protected override void BuffSpell() {
        // Magic Missile doesn't cross the rift. Destroy it
        Destroy(gameObject);
    }

    public override void Charge(float f_chargeTime) {
        f_charged = (f_chargeTime * 1/3) + 1;
        if (f_charged > 2f) {
            f_charged = 2f;
        }
        transform.localScale *= f_charged;
    }
}
