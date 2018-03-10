using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMissileController : SpellController {

    bool i_reflect = false;

    private void OnDestroy() {
        ParticleSystem ps_particle = Instantiate(ps_onDestroyParticles, gameObject.transform.position, Quaternion.identity);
        ps_particle.GetComponent<ParticleSystemController>().enabled = true;
    }

    protected override void ApplyEffect(GameObject go_target, Collision collision) {
        if (go_target.tag == "Player") {
            if (go_target.GetComponent<PlayerController>().GetColor() != this.e_color)
            {
                go_target.GetComponent<PlayerController>().TakeDamage(Constants.SpellStats.C_MagicMissileDamage * f_charged,Constants.Global.DamageType.MAGICMISSILE);
                go_target.GetComponent<PlayerController>().DropFlag();
				go_target.GetComponent<PlayerController> ().Animator.SetTrigger ("hitTrigger");
            }
            else {
                go_target.GetComponent<PlayerController>().Heal(Constants.SpellStats.C_MagicMissileHeal * f_charged);
            }
        }
		else if (go_target.tag == "Enemy") {
            go_target.GetComponent<EnemyController>().TakeDamage(Constants.SpellStats.C_MagicMissileDamage * f_charged);
        }
        else if (go_target.tag == "RiftBoss")
        {
            if (go_target.gameObject.GetComponent<RiftBossController>().Color == e_color) {
                go_target.GetComponent<RiftBossController>().TakeDamage(Constants.SpellStats.C_MagicMissileDamage * f_charged);
            }
        }
        else if (go_target.tag == "Crystal"){
			Constants.Global.Color crystalColor = go_target.GetComponent<CrystalController>().Color;
			if (crystalColor != e_color){
				go_target.GetComponent<CrystalController>().UpdateCrystalHealth(Constants.SpellStats.C_MagicMissileCrystalDamage * f_charged);
			}
		} else if (go_target.tag == "Wall") {
            if (i_reflect) {
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
        if (f_chargeTime > 0) {
            i_reflect = true;
        }
        
    }
}
