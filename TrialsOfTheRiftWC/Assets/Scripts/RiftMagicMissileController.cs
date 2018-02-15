using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiftMagicMissileController : SpellController
{
    protected override void Start() {
        // Do nothing, because the Rift's spells probably shouldn't die until they hit something
    }

    protected override void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag.ToLower() == "player") {
            ApplyEffect(collision.gameObject, collision);
        }
        else {
            Destroy(gameObject);
        }
    }

    protected override void OnTriggerEnter(Collider other) {
        // Just casually override my parent's method. I'm not actually gonna do anything! HA!
    }

    protected override void ApplyEffect(GameObject go_target, Collision collision)
    {
        go_target.GetComponent<PlayerController>().DropFlag();
        //go_target.GetComponent<PlayerController>().TakeDamage(Constants.RiftStats.C_VolatilityMeteorDamage);
    }

    protected override void BuffSpell()
    {
        // This Magic Missile comes from the rift. don't do anything
    }

    public override void Charge(float f_chargeTime) {
        //Do nothing here.
    }
}
