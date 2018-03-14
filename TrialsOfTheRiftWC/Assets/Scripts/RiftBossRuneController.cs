using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiftBossRuneController : MonoBehaviour
{
    public string[] s_runeTargetTags;
    void Start()
    {
        //Destroy(gameObject, Constants.SpellStats.C_ElectricAOELiveTime);
        //Invoke("Die", Constants.ObjectiveStats.C_RuneSpawnInterval);
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        foreach (string tag in s_runeTargetTags)
        {
            if (other.gameObject.tag == tag)
            {
                ApplyEffect(other.gameObject);
                //CancelInvoke("Die");
                Die();
                break;
            }
        }
    }

    private void ApplyEffect(GameObject go_target)
    {
        if (go_target.tag == "Player")
        {
            go_target.GetComponent<PlayerController>().TakeDamage(Constants.ObjectiveStats.C_RuneDamage,Constants.Global.DamageType.RUNE);
        }
    }
}