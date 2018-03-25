/*  Rift Boss Controller - Dana Thompson
 * 
 *  Desc:   Controls how the Rift Boss works
 * 
 */

using UnityEngine;

public class RiftBossController : SpellTarget {
#region Variables and Declarations
    [SerializeField] private RiftBossObjective rbo_owner;  // identifies objective rift boss is a part of
    [SerializeField] private GameObject go_ForceField;
    [SerializeField] private GameObject[] go_runes;
#endregion

#region RiftBossController Methods
    public void TakeDamage(float damage) {
        if (!go_ForceField.activeSelf) {
            f_health -= damage;
            rbo_owner.UpdateRiftBossHealth(f_health);
        }
    }

    override public void ApplySpellEffect(Constants.SpellStats.SpellType spell, Constants.Global.Color color, float damage, Vector3 direction) {
        if (color == e_color) {
            TakeDamage(damage);
        }
    }
    
    private void SpawnRunes() {
        foreach (GameObject runes in go_runes) {
            if (!runes.activeSelf) {
                runes.SetActive(true);
            }
        }
        anim.SetTrigger("runeTrigger");
    }

    private void FireDeathBolts() {
        riftController.FireDeathBolts(e_color);
        go_ForceField.SetActive(false);
        Invoke("TurnOnForceField", Constants.ObjectiveStats.C_ForceFieldCooldown);
        anim.SetTrigger("deathboltTrigger");
    }

    private void TurnOnForceField() {
        go_ForceField.SetActive(true);
    }
#endregion

#region Unity Overrides
    void Start() {
        riftController = RiftController.Instance;
        f_health = Constants.ObjectiveStats.C_RiftBossMaxHealth;     // cannot read from Constants.cs in initialization at top

        InvokeRepeating("FireDeathBolts", Constants.ObjectiveStats.C_DeathBoltCooldown, Constants.ObjectiveStats.C_DeathBoltCooldown + Constants.ObjectiveStats.C_ForceFieldCooldown);
        InvokeRepeating("SpawnRunes", Constants.ObjectiveStats.C_RuneSpawnInterval, Constants.ObjectiveStats.C_RuneSpawnInterval);
    }
#endregion




    
}
