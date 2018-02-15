/*  Rift Boss Controller - Dana Thompson
 * 
 *  Desc:   Controls how the Rift Boss works
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiftBossController : MonoBehaviour {

    public RiftBossObjective rbo_owner;  // identifies objective crystal is a part of
    public GameObject go_ForceField;
    public GameObject[] go_runes;
    [SerializeField]
    private Constants.Global.Color e_color;     // identifies owning team
    private float f_health;    // indicates how much health the boss has
    private RiftController rc_riftController;

    // Getters
    public Constants.Global.Color Color {
        get { return e_color; }
    }
    public float Health {
        get { return f_health; }
        set { f_health = value; }
    }

    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    public void TakeDamage(float damage) { //TODO: rename this in_variable when we decide percentage or no
        if (!go_ForceField.activeSelf) {
            f_health -= damage;
            rbo_owner.UpdateRiftBossHealth(f_health);
        }
    }

    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    void Start() {
        f_health = Constants.ObjectiveStats.C_RiftBossMaxHealth;     // cannot read from Constants.cs in initialization at top
        rc_riftController = RiftController.Instance;

        InvokeRepeating("FireDeathBolts", Constants.ObjectiveStats.C_DeathBoltCooldown, 
            Constants.ObjectiveStats.C_DeathBoltCooldown + Constants.ObjectiveStats.C_ForceFieldCooldown);

        InvokeRepeating("SpawnRunes", Constants.ObjectiveStats.C_RuneSpawnInterval, Constants.ObjectiveStats.C_RuneSpawnInterval);
    }

    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    //responsible for activating and deactivating runes based on the rune spawn interval
    private void SpawnRunes() {
        foreach (GameObject runes in go_runes) {
            if (!runes.activeSelf) {
                runes.SetActive(true);
            }
            else {
                runes.SetActive(false);
            }
        }
    }

    private void FireDeathBolts() {
        rc_riftController.FireDeathBolts(e_color);
        go_ForceField.SetActive(false);
        Invoke("TurnOnForceField", Constants.ObjectiveStats.C_ForceFieldCooldown);
    }

    private void TurnOnForceField() {
        go_ForceField.SetActive(true);
    }
}
