using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindController : SpellController {

    private float f_windDamage = Constants.SpellStats.C_WindDamage;

    private void OnDestroy()
    {
        ParticleSystem ps_particle = Instantiate(ps_onDestroyParticles, gameObject.transform.position, Quaternion.identity);
        ps_particle.GetComponent<ParticleSystemController>().enabled = true;
    }

    protected override void ApplyEffect(GameObject go_target, Collision collision) {
        if (go_target.tag == "Player")
        {
            Vector3 v3_direction = transform.forward.normalized;
            go_target.GetComponent<Rigidbody>().AddForce(v3_direction * Constants.SpellStats.C_WindForce * f_charged);
            go_target.GetComponent<PlayerController>().DropFlag();

            go_target.GetComponent<PlayerController>().TakeDamage(f_windDamage * Constants.SpellStats.C_WindPlayerDamageMultiplier,Constants.Global.DamageType.WIND);
            go_target.GetComponent<PlayerController> ().Animator.SetTrigger ("windTrigger");           
        }
        else if (go_target.tag == "Enemy")
        {
            Vector3 v3_direction = transform.forward.normalized;
            go_target.GetComponent<Rigidbody>().AddForce(v3_direction * Constants.SpellStats.C_WindForce * f_charged);
            go_target.GetComponent<EnemyController>().TakeDamage(f_windDamage * Constants.SpellStats.C_WindPlayerDamageMultiplier);
        }
        else if (go_target.tag == "RiftBoss") {
            if (go_target.gameObject.GetComponent<RiftBossController>().Color == e_color) {
                go_target.GetComponent<RiftBossController>().TakeDamage(f_windDamage * Constants.SpellStats.C_WindPlayerDamageMultiplier);
            }
        }
        else if (go_target.tag == "Crystal")
        {
            Constants.Global.Color crystalColor = go_target.GetComponent<CrystalController>().Color;
            if (crystalColor != e_color)
            {
                go_target.GetComponent<CrystalController>().UpdateCrystalHealth(Constants.SpellStats.C_SpellCrystalDamagePercent);
            }
            else if (crystalColor == e_color)
            {
                go_target.GetComponent<CrystalController>().UpdateCrystalHealth(Constants.SpellStats.C_SpellCrystalHealPercent);
            }
        }
        else if (go_target.tag == "Potato")
        {
            go_target.GetComponent<Rigidbody>().isKinematic = false;
            Vector3 v3_direction = transform.forward.normalized;
            go_target.GetComponent<Rigidbody>().AddForce(v3_direction * Constants.SpellStats.C_WindForce * f_charged);
        }
    }

    protected override void BuffSpell() {
        // Increase Volatility by 0.5%
        RiftController.Instance.IncreaseVolatility(Constants.RiftStats.C_VolatilityIncrease_SpellCross);
        f_windDamage = f_windDamage * Constants.SpellStats.C_WindRiftDamageMultiplier;
        transform.localScale *= Constants.SpellStats.C_SpellScaleMultiplier;
    }

    public override void Charge(float f_chargeTime) {
        f_charged = (f_chargeTime * 1/3) + 1;
        if (f_charged > 2f) {
            f_charged = 2f;
        }
        f_windDamage *= ((f_charged*1/12) + 1);
        transform.localScale *= f_charged*0.7f;
    }
}
