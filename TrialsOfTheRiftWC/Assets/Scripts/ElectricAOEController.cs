using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricAOEController : MonoBehaviour {

	public string[] s_spellTargetTags; // these are the tags of the objects spells should do damage/effect against
	public Constants.Global.Color e_color;
	public float f_electricDamage;	// TODO: set this immediately after instantiation

	void Start () {
		//Destroy(gameObject, Constants.SpellStats.C_ElectricAOELiveTime);
		Invoke("Die", Constants.SpellStats.C_ElectricAOELiveTime);
        f_electricDamage = Constants.SpellStats.C_ElectricDamage;
	}
	
	private void Die(){
		transform.position = new Vector3(transform.position.x,-500f,transform.position.z);
		Destroy(gameObject, 0.1f);
	}
	
	void OnTriggerEnter(Collider other) {
		foreach (string tag in s_spellTargetTags) {
            if (other.gameObject.tag == tag) {  
                ApplyEffect(other.gameObject);
				return;
			}
		}
	}

	void OnTriggerExit(Collider other) {
		foreach (string tag in s_spellTargetTags) {
			if (other.gameObject.tag == tag) {
				NegateEffect(other.gameObject);
				return;
			}
		}
	}

	private void ApplyEffect(GameObject go_target) {
        if (go_target.tag == "Player") {
            if (go_target.GetComponent<PlayerController>().GetColor() != e_color)
            {
                go_target.GetComponent<PlayerController>().DropFlag();
                go_target.GetComponent<PlayerController>().f_canMove = .5f; //TODO: Constants
				go_target.GetComponent<PlayerController>().Animator.SetTrigger("gooTrigger");
                StartCoroutine("ApplyPlayerDamage", go_target);
            }
        }
        else if (go_target.tag == "Enemy") {
            StartCoroutine("ApplyEnemyDamage", go_target);
            go_target.GetComponent<EnemyController>().Slow();
        }
        else if (go_target.tag == "RiftBoss") {
            Debug.Log(go_target.tag);
            StartCoroutine("ApplyRiftBossDamage", go_target);
        }
        else if (go_target.tag == "Crystal") {
            StartCoroutine("ApplyCrystalDamage", go_target);
        }
	}

	private void NegateEffect(GameObject go_target) {
		if (go_target.tag == "Player") {
			go_target.GetComponent<PlayerController>().f_canMove = 1;
		}
		else if (go_target.tag == "Enemy") {
			StopCoroutine("ApplyEnemyDamage");
			go_target.GetComponent<EnemyController>().Unslow();
		}
        else if (go_target.tag == "RiftBoss") {
            StopCoroutine("ApplyRiftBossDamage");
        }
        else if (go_target.tag == "Crystal") { 
			StopCoroutine("ApplyCrystalDamage");	// shouldn't be practically necessary
		}
	}

	// applies AOE damage/heal to crystal every .5s until dissipation
	IEnumerator ApplyCrystalDamage(GameObject go_target) {
		if (go_target) {
			Constants.Global.Color crystalColor = go_target.GetComponent<CrystalController>().Color;
			if (crystalColor != e_color) {
				go_target.GetComponent<CrystalController>().UpdateCrystalHealth(Constants.SpellStats.C_SpellCrystalDamagePercent / 5);
			}
			else if (crystalColor == e_color) {
				go_target.GetComponent<CrystalController>().UpdateCrystalHealth(Constants.SpellStats.C_SpellCrystalHealPercent / 5);
			}
			yield return new WaitForSeconds(0.5f);
			StartCoroutine("ApplyCrystalDamage", go_target);
		}
	}

    // applies AOE damage/heal to Rift Boss every .5s until dissipation
    IEnumerator ApplyRiftBossDamage(GameObject go_target)
    {
        if (go_target)
        {
            Constants.Global.Color riftBossColor = go_target.GetComponent<RiftBossController>().Color;
            if (riftBossColor == e_color)
            {
                go_target.GetComponent<RiftBossController>().TakeDamage(f_electricDamage);
            }

            yield return new WaitForSeconds(0.5f);
            StartCoroutine("ApplyRiftBossDamage", go_target);
        }
    }

    // applies AOE damage to enemy every .5s until dissipation or triggerexit
    IEnumerator ApplyEnemyDamage(GameObject go_target) {
		if(go_target){  //Make sure it's not already dead.
			go_target.GetComponent<EnemyController>().TakeDamage(f_electricDamage);
			yield return new WaitForSeconds(0.5f);
			StartCoroutine("ApplyEnemyDamage", go_target);
		}
	}

    IEnumerator ApplyPlayerDamage(GameObject go_target) {
        if (go_target)
        {  //Make sure it's not already dead.
            go_target.GetComponent<PlayerController>().TakeDamage(f_electricDamage * Constants.SpellStats.C_ElectricPlayerDamageMultiplier,Constants.Global.DamageType.ELECTRICITY);
            yield return new WaitForSeconds(0.5f);
            StartCoroutine("ApplyPlayerDamage", go_target);
        }
    }

}
