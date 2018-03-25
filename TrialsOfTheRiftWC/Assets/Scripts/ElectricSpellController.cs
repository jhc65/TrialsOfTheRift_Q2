/*  Electric Spell Controller - Dana Thompson
 * 
 *  Desc:   Facilitates Electric AOE interactions
 * 
 */

using UnityEngine;

public class ElectricSpellController : SpellController {
#region Variables and Declarations
    public GameObject go_aoe;
#endregion

#region ElectricSpellController Methods
    protected override void Charge(float f_chargeTime) {
        f_damage = Constants.SpellStats.C_ElectricDamage;
        f_charge = ((1f / 12f) * f_chargeTime) + 0.05f;
        if (f_charge > 1f) {
            f_charge = 1f;
        }
        Invoke("SpawnAOE", Constants.SpellStats.C_SpellLiveTime * f_charge);
    }

    protected override void BuffSpell() {
        RiftController.Instance.IncreaseVolatility(Constants.RiftStats.C_VolatilityIncrease_SpellCross);
		f_damage *= Constants.SpellStats.C_ElectricRiftDamageMultiplier;
        transform.localScale *= Constants.SpellStats.C_SpellScaleMultiplier;
    }

	void SpawnAOE() {
        ElectricAOEController newAOE = Instantiate(go_aoe, new Vector3(transform.position.x, 0.1f, transform.position.z), Quaternion.identity).GetComponent<ElectricAOEController>();
		newAOE.Init(e_color, f_damage);
		Destroy(gameObject);
	}
#endregion
    
#region Unity Overrides
    override protected void Start() {
        // Electric Spell live time is determined by charge time, so we don't want the auto-InvokeDestroy() in SpellController's Start()
	}

	override protected void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Spell")) {
            Constants.Global.Color spellColor = collision.gameObject.GetComponent<SpellController>().Color;
            if (spellColor != e_color) {    // opposing spells destroy each other
                CancelInvoke();
                Destroy(gameObject);
            }
            else {              // ignore collisions between spells of the same color
                Physics.IgnoreCollision(GetComponent<Collider>(), collision.gameObject.GetComponent<Collider>());
            }
        }
        else {  // spawn AOE on collision with anything else (Rift and portal interactions are controlled by OnTriggerEnter)
            CancelInvoke();
            SpawnAOE();
        }
	}
#endregion
}
