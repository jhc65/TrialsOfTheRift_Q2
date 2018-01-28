using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceWallController : MonoBehaviour {

    private float f_iceDamage;

	// Use this for initialization
	void Start () {
        f_iceDamage = Constants.SpellStats.C_IceDamage;
		Destroy(gameObject,5f);
	}

    private void OnCollisionEnter(Collision collision) {
        ApplyEffect(collision.gameObject, collision);
    }

    protected void ApplyEffect(GameObject go_target, Collision collision) {
        if (go_target.tag == "Player")
        {
            go_target.GetComponent<PlayerController>().Freeze();
            go_target.GetComponent<PlayerController>().TakeDamage(f_iceDamage * Constants.SpellStats.C_IcePlayerDamageMultiplier);
        }
        else if (go_target.tag == "Enemy")
        {
            go_target.GetComponent<EnemyController>().TakeDamage(f_iceDamage);
            go_target.GetComponent<EnemyController>().Freeze(0f);
        }
        /*else if (go_target.tag == "Crystal")
        {
            Constants.Color crystalColor = go_target.GetComponent<CrystalController>().e_color;
			if (crystalColor != e_color){
				go_target.GetComponent<CrystalController>().ChangeHealth(Constants.SpellStats.C_SpellCrystalDamagePercent);
			}
			else if (crystalColor == e_color) {
				go_target.GetComponent<CrystalController>().ChangeHealth(Constants.SpellStats.C_SpellCrystalHealPercent);
			}
        }*/
    }

}
