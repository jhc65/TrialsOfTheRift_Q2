using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricController : SpellController {

	public GameObject go_aoe;
	private float f_electricDamage = Constants.SpellStats.C_ElectricDamage;

	protected override void BuffSpell() {
        // Increase Volatility by 0.5%
        RiftController.GetInstance().IncreaseVolatility(Constants.RiftStats.C_VolatilityIncrease_SpellCross);
		f_electricDamage = f_electricDamage * Constants.SpellStats.C_ElectricDamageMultiplier;
		transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
	}

	protected override void ApplyEffect(GameObject go_target) {
		GameObject go_newAOE = Instantiate(go_aoe, new Vector3(transform.position.x, 0.1f, transform.position.z), Quaternion.identity);
		go_newAOE.GetComponent<ElectricAOEController>().e_color = e_color;
		go_newAOE.GetComponent<ElectricAOEController>().f_electricDamage = f_electricDamage;
		Destroy(gameObject);
	}

	protected override void Start() {
		Invoke("ApplyEffect", Constants.SpellStats.C_SpellLiveTime);
	}

	protected override void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Spell") {
			Constants.Color spellColor = collision.gameObject.GetComponent<SpellController>().e_color;
			if (spellColor != e_color) {
				Destroy(gameObject);
			}
			else {
				//ignores any collision detection between the two spells
				Physics.IgnoreCollision(GetComponent<Collider>(), collision.gameObject.GetComponent<Collider>());
			}
		}
		else if (collision.gameObject.tag != "Portal") {
			ApplyEffect(collision.gameObject);
		}
	}
}
