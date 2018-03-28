/*  Ice Spell Controller - Sam Caulker
 * 
 *  Desc:   Facilitates Ice Spell interactions
 * 
 */

using UnityEngine;
using Rewired;

public class IceSpellController : SpellController {
#region Variables and Declarations
    [SerializeField] private GameObject go_iceWall;
    [SerializeField] private GameObject go_WallSpawn;
    private Player p_player;
#endregion

#region Ice Spell Controller Methods
    override protected void Charge(float f_chargeTime) {
        // Ice spell cannot be charged like other spells
        f_damage = Constants.SpellStats.C_IceDamage;
    }

    override protected void BuffSpell() {
        riftController.IncreaseVolatility(Constants.RiftStats.C_VolatilityIncrease_SpellCross);
        f_damage = f_damage * Constants.SpellStats.C_IceRiftDamageMultiplier;
        transform.localScale *= Constants.SpellStats.C_SpellScaleMultiplier;
    }

    void MakeWall() {
        Instantiate(go_iceWall, go_WallSpawn.transform.position, go_WallSpawn.transform.rotation);
    }
#endregion

#region Unity Overrides
    override protected void Start() {
        riftController = RiftController.Instance;
        Invoke("InvokeDestroy", Constants.SpellStats.C_IceLiveTime);
        InvokeRepeating("MakeWall", 0.15f, 0.15f);
    }

    override protected void OnCollisionEnter(Collision collision) {
        pc_owner.IceBoltMode = false;
        base.OnCollisionEnter(collision);
	}

    void FixedUpdate() {
        if (p_player == null) p_player = ReInput.players.GetPlayer(pc_owner.Num);
        float f_inputX = p_player.GetAxis("AimHorizontal");
        float f_inputZ = p_player.GetAxis("AimVertical");
        Vector3 v3_dir = new Vector3(f_inputX, 0, f_inputZ).normalized;
        if (!(v3_dir.Equals(Vector3.zero))) {
            transform.rotation = Quaternion.LookRotation(v3_dir);
        }
        rb.velocity = transform.forward * Constants.SpellStats.C_IceSpeed;
    }
#endregion
}

