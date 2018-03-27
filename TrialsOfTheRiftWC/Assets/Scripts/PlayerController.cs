/*  Player Controller - Sam Caulker
 * 
 *  Desc:   Facilitates player interactions
 * 
 */

using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;

public class PlayerController : SpellTarget {
#region Variables and Declarations
    [SerializeField] private GameObject go_playerCapsule;   // player main body
    [SerializeField] private GameObject go_playerWisp;      // player wisp body
    [SerializeField] private GameObject go_magicMissileShot;    // magic missile object
    [SerializeField] private GameObject go_windShot;            // wind spell object
    [SerializeField] private GameObject go_iceShot;             // ice spell object
    [SerializeField] private GameObject go_electricShot;        // electric spell object
    [SerializeField] private Transform t_spellSpawn;    // location spells are instantiated
    [SerializeField] private Transform t_flagPos;       // location on character model of flag
    [SerializeField] private GameObject go_interactCollider;  // activated with button-press to interact with objectives
    [SerializeField] private GameObject go_parryShield;       // activated with right stick click
    [SerializeField] private PauseController pauc_pause;        // for pausing

    private int i_playerNumber;             // designates player's number for controller mappings
    private Player p_player;                // rewired player for input control
    private Constants.Global.Side e_side;   // identifies which side of the rift player is on
    private float f_canMove = 1;            // identifies if the player is frozen
    private bool isWisp = false;            // player has "died"
    private bool b_iceboltMode = false;     // player is controlling an icebolt
    private GameObject go_icebolt;          // the icebolt the player is controlling
    private GameObject go_flagObj;          // flag game object; if not null, player is carrying flag
    private bool b_stepOk = true;           // time to play next footstep noise has elapsed

    // Spells
    private float f_nextWind = 0;				    // time next wind spell can be cast
	private float f_nextIce = 0;                    // time next ice spell can be cast
	private float f_nextElectric = 0;               // time next electric spell can be cast
	private float f_nextMagicMissile = 0;           // time next magic missile can be cast
    private float f_nextChargedMagicMissile = 0;    // time next charged magic missile can be cast
    private float f_nextCast = 0;                   // time next spell in general can be cast (does not include magic missile)
    private float f_mmCharge;               // current charge of magic missile
    private float f_iceCharge;              // current charge of ice spell
    private float f_windCharge;             // current charge of wind spell
    private float f_electricCharge;         // current charge of electric charge
    private float f_projectileSize;         // size of spell objects

    #region Getters and Setters
    public Constants.Global.Side Side {
        get { return e_side; }
    }

    public bool Wisp {
        get { return isWisp; }
    }

    public int Num {
        get { return i_playerNumber; }
        set { i_playerNumber = value; }
    }

    public float Health {
        get { return f_health; }
    }

    public bool IceBoltMode {
        set { b_iceboltMode = value; }
    }

    public float NextWind {
        get { return f_nextWind; }
    }

    public float NextIce {
        get { return f_nextIce; }
    }

    public float NextElectric {
        get { return f_nextElectric; }
    }
    #endregion
#endregion

#region Player Controller Methods
    override public void ApplySpellEffect(Constants.SpellStats.SpellType spell, Constants.Global.Color color, float damage, Vector3 direction) {
        switch(spell) {
            case Constants.SpellStats.SpellType.WIND:
                DropFlag();
                rb.AddForce(direction * Constants.SpellStats.C_WindForce);
                TakeDamage(damage, Constants.Global.DamageType.WIND);
                anim.SetTrigger("windTrigger");
                break;
            case Constants.SpellStats.SpellType.ICE:
                DropFlag();
                f_canMove = 0;
                TakeDamage(damage, Constants.Global.DamageType.ICE);
                anim.SetTrigger("freezeTrigger");
                anim.SetBool("freezeBool", true);
                Invoke("Unfreeze", Constants.SpellStats.C_IceFreezeTime);
                break;
            case Constants.SpellStats.SpellType.ELECTRICITYAOE:
                if(e_color != color) {
                    DropFlag();
                    f_canMove = Constants.SpellStats.C_ElectricAOESlowDownMultiplier;
                    TakeDamage(damage, Constants.Global.DamageType.ELECTRICITY);
                    anim.SetTrigger("gooTrigger");
                }
                break;
            case Constants.SpellStats.SpellType.MAGICMISSILE:
                if (e_color != color) {
                    DropFlag();
                    TakeDamage(damage, Constants.Global.DamageType.MAGICMISSILE);
                    anim.SetTrigger("hitTrigger");
                }
                else {
                    Heal(Constants.SpellStats.C_MagicMissileHeal);
                }
                break;
        }
    }

    override public void NegateSpellEffect(Constants.SpellStats.SpellType spell) {
        if (spell == Constants.SpellStats.SpellType.ELECTRICITYAOE) {
            StopCoroutine(cor_AOECoroutine);
            f_canMove = 1;
        }
    }

    private void Move() {
        float f_inputX = p_player.GetAxis("MoveHorizontal");
        float f_inputZ = p_player.GetAxis("MoveVertical");
        float f_aimInputX = p_player.GetAxis("AimHorizontal");
        float f_aimInputZ = p_player.GetAxis("AimVertical");
		//float f_lookDirection;

        Vector3 v3_moveDir = new Vector3(f_inputX, 0, f_inputZ).normalized;
		Vector3 v3_aimDir = new Vector3(f_aimInputX, 0, f_aimInputZ).normalized;
		//f_lookDirection = f_inputX + f_aimInputX;

		//anim.SetFloat ("runSpeed", v3_moveDir.magnitude);
		//anim.SetFloat ("lookDirection", v3_aimDir.magnitude);

		if (v3_aimDir.magnitude > 0) {
			transform.rotation = Quaternion.LookRotation(v3_aimDir);
		}

        if (isWisp) {
			GetComponent<Rigidbody>().velocity = (v3_moveDir * Constants.PlayerStats.C_WispMovementSpeed) * f_canMove;
		}
		else {
			GetComponent<Rigidbody>().velocity = (v3_moveDir * Constants.PlayerStats.C_MovementSpeed) * f_canMove;
			if(v3_moveDir.magnitude > 0 && b_stepOk){
				b_stepOk = false;
				maestro.PlayPlayerFootstep();
				maestro.PlayPlayerClothing();
			}	
		}
	}

    #region Reset Helper Functions
    private void TurnOffParryShield() {
        go_parryShield.SetActive(false);
    }

    private void TurnOffInteractCollider() {
        go_interactCollider.transform.localPosition = new Vector3(go_interactCollider.transform.localPosition.x, -1000.0f, go_interactCollider.transform.localPosition.z);
        Invoke("ResetInteractCollider", 0.05f);
    }

    private void ResetInteractCollider() {
        go_interactCollider.SetActive(false);
        go_interactCollider.transform.localPosition = new Vector3(go_interactCollider.transform.localPosition.x, transform.position.y, go_interactCollider.transform.localPosition.z);
    }

    private void Unfreeze() {
		f_canMove = 1;
		anim.SetBool ("freezeBool", false);
    }
    #endregion

    #region Health and Damage
    private void PlayerDeath() {
		maestro.PlayAnnouncementWispGeneric();
        DropFlag();
        TurnOffInteractCollider();
        isWisp = true;
        if(SceneManager.GetActiveScene().name != "WarmUp") {
            riftController.IncreaseVolatility(Constants.RiftStats.C_VolatilityIncrease_PlayerDeath);
        } 
		maestro.PlayPlayerDie();
        go_playerCapsule.SetActive(false);
		go_playerWisp.SetActive(true);
        f_nextMagicMissile = 0;
        f_nextWind = 0;
        f_nextIce = 0;
        f_nextElectric = 0;
        f_nextCast = 0;
        f_nextChargedMagicMissile = 0;
        Invoke("PlayerRespawn", Constants.PlayerStats.C_RespawnTimer);
    }

    private void PlayerRespawn() {
		isWisp = false;
		maestro.PlayPlayerSpawn();
        go_playerCapsule.SetActive(true);
        go_playerWisp.SetActive(false);
        f_health = Constants.PlayerStats.C_MaxHealth;
    }

	public void TakeDamage(float damage, Constants.Global.DamageType d) {
		if (!isWisp) {
			maestro.PlayAnnouncmentPlayerHit(i_playerNumber,d);
			maestro.PlayPlayerHit();
			f_health -= damage;
            //DamageVisualOn();
			if (f_health <= 0.0f) {
                PlayerDeath();
			}
		}
	}
    
    public void Heal(float heal) {
        if (!isWisp) {
            float tempHp = f_health + heal;
            if (tempHp >= Constants.PlayerStats.C_MaxHealth) {
                f_health = Constants.PlayerStats.C_MaxHealth;
            } else {
                f_health = tempHp;
            }
            //HealVisualOn();
        }
    }
    #endregion

    #region Flag Stuff
    // there's no good way to do any of this
    public void Pickup(GameObject flag) {
        if (!isWisp) {
            flag.transform.SetParent(t_flagPos);
            flag.transform.localPosition = Vector3.zero;
            go_flagObj = flag;
        }
	}

	public void DropFlag() {
		if(go_flagObj) {
            go_flagObj.GetComponent<FlagController>().DropFlag();
			go_flagObj = null;
		}
	}
    #endregion

	private void StepDelay(){
		b_stepOk = true;
	}
    
    //public void DamageVisualOn() {
    //    go_playerCapsule.GetComponent<MeshRenderer>().material.color = Color.yellow;
    //    //Call screenshake here.
    //    Invoke("DamageVisualOff", 0.1666f * 2);
    //}

    //public void DamageVisualOff() {
    //    go_playerCapsule.GetComponent<MeshRenderer>().material.color = col_originalColor;
    //}

    //public void HealVisualOn() {
    //    go_playerCapsule.GetComponent<MeshRenderer>().material.color = Color.green;
    //    //Call screenshake here.
    //    Invoke("HealVisualOff", 0.1666f * 2);
    //}

    //public void HealVisualOff() {
    //    go_playerCapsule.GetComponent<MeshRenderer>().material.color = col_originalColor;
    //}
#endregion

#region Unity Overrides
    void Start() {
        maestro = Maestro.Instance;
        riftController = RiftController.Instance;
        p_player = ReInput.players.GetPlayer(i_playerNumber);
        f_health = Constants.PlayerStats.C_MaxHealth;
        f_projectileSize = Constants.SpellStats.C_PlayerProjectileSize;
		
		if (transform.position.x > 0)
			e_side = Constants.Global.Side.RIGHT;
		else
			e_side = Constants.Global.Side.LEFT;

        InvokeRepeating("StepDelay", Constants.PlayerStats.C_StepSoundDelay, Constants.PlayerStats.C_StepSoundDelay);
    }

	void FixedUpdate() {
        // position
        if (transform.position.x > 0)
            e_side = Constants.Global.Side.RIGHT;
        else
            e_side = Constants.Global.Side.LEFT;

        if (!b_iceboltMode) {
            Move();
        }

        // pause
        if (p_player.GetButtonDown("Menu") && Time.timeScale == 1) {
            pauc_pause.Pause(this);
        }

        if (isWisp)
            return;

        // update spell timers
		f_nextMagicMissile += Time.deltaTime;
		f_nextWind += Time.deltaTime;
        f_nextIce += Time.deltaTime;
        f_nextElectric += Time.deltaTime;
		f_nextCast += Time.deltaTime;
        f_nextChargedMagicMissile += Time.deltaTime;

        // interact
        if (p_player.GetButtonDown("Interact")) {
            if (go_flagObj) {
                DropFlag();
            }
            else {
                go_interactCollider.SetActive(true);
            }
        }
        if (p_player.GetButtonUp("Interact")) {
            TurnOffInteractCollider();
        }

        // spells
        if (p_player.GetButtonUp("IceSpell")) {
            b_iceboltMode = false;
            Destroy(go_icebolt);
        }

        if (go_flagObj)
            return;

        // Magic Missile (Auto-fire)
        if (f_nextMagicMissile > Constants.SpellStats.C_MagicMissileCooldown) {
            if (p_player.GetButtonDown("MagicMissile"))
				maestro.PlaySpellCharge();
            if (p_player.GetButtonTimePressed("MagicMissile") != 0) {
                f_mmCharge += p_player.GetButtonTimePressed("MagicMissile");
            }
            if (p_player.GetButton("MagicMissile")) {
                maestro.PlayMagicMissileShoot();
				anim.SetTrigger ("attackTrigger");
                f_nextMagicMissile = 0;
				GameObject go_spell = Instantiate(go_magicMissileShot, t_spellSpawn.position, t_spellSpawn.rotation);
                go_spell.transform.localScale = new Vector3(f_projectileSize, f_projectileSize, f_projectileSize);
                go_spell.GetComponent<Rigidbody>().velocity = transform.forward * Constants.SpellStats.C_MagicMissileSpeed;
                SpellController sc_firing = go_spell.GetComponent<SpellController>();
                sc_firing.Init(this, e_color, 0);
            }                
	    }
        // Charged Magic Missile (Release)
        if (p_player.GetButtonUp("MagicMissile") && f_nextChargedMagicMissile > Constants.SpellStats.C_MagicMissileChargeCooldown) {
			maestro.PlayMagicMissileShoot();
            f_nextChargedMagicMissile = 0;
			GameObject go_spell = Instantiate(go_magicMissileShot, t_spellSpawn.position, t_spellSpawn.rotation);
            go_spell.transform.localScale = new Vector3(f_projectileSize, f_projectileSize, f_projectileSize);
            go_spell.GetComponent<Rigidbody>().velocity = transform.forward * Constants.SpellStats.C_MagicMissileSpeed;
            SpellController sc_firing = go_spell.GetComponent<SpellController>();
            sc_firing.Init(this, e_color, f_mmCharge);
            f_mmCharge = 0;
        }
        // Wind Spell
        if (f_nextWind > Constants.SpellStats.C_WindCooldown && f_nextCast > Constants.SpellStats.C_NextSpellDelay) {
		    if (p_player.GetButtonDown("WindSpell"))
				maestro.PlaySpellCharge();
            if (p_player.GetButtonTimePressed("WindSpell") != 0) {
                f_windCharge += p_player.GetButtonTimePressed("WindSpell");
			    //anim.SetTrigger ("windChargeTrigger");
				//anim.SetFloat ("windCharge", f_windCharge);
            }
            if (p_player.GetButtonUp("WindSpell")) {
				maestro.PlayWindShoot();
				//anim.SetTrigger("windSpellTrigger");
                f_nextWind = 0;
			    f_nextCast = 0;
                for (int i = -30; i <= 30; i += 30) {
                    GameObject go_spell = Instantiate(go_windShot, t_spellSpawn.position, t_spellSpawn.rotation);
                    go_spell.transform.eulerAngles = go_spell.transform.eulerAngles + new Vector3(0, i, 0);
                    go_spell.transform.localScale = new Vector3(f_projectileSize, f_projectileSize, f_projectileSize);
                    go_spell.GetComponent<Rigidbody>().velocity = go_spell.transform.forward * Constants.SpellStats.C_WindSpeed;
                    SpellController sc_firing = go_spell.GetComponent<SpellController>();
                    sc_firing.Init(this, e_color, f_windCharge);
                }
                //anim.SetFloat("windCharge", f_windCharge);
                f_windCharge = 0;
			} 
		}
        // Ice Spell
        if (f_nextIce > Constants.SpellStats.C_IceCooldown && f_nextCast > Constants.SpellStats.C_NextSpellDelay) {
            if (p_player.GetButtonDown("IceSpell")) {
				maestro.PlaySpellCharge();
				maestro.PlayIceShoot();
                b_iceboltMode = true;
                f_nextIce = 0;
                f_nextCast = 0;
                GameObject go_spell = Instantiate(go_iceShot, t_spellSpawn.position, t_spellSpawn.rotation);
                go_spell.transform.localScale = new Vector3(f_projectileSize, f_projectileSize, f_projectileSize);
                go_spell.GetComponent<Rigidbody>().velocity = transform.forward * Constants.SpellStats.C_IceSpeed;
                go_icebolt = go_spell;
                IceSpellController sc_firing = go_spell.GetComponent<IceSpellController>();
                sc_firing.Init(this, e_color, 0);
            }
		}
        // Electric Spell
        if (f_nextElectric > Constants.SpellStats.C_ElectricCooldown && f_nextCast > Constants.SpellStats.C_NextSpellDelay) {
            if (p_player.GetButtonDown("ElectricitySpell"))
				maestro.PlaySpellCharge();
            if (p_player.GetButtonTimePressed("ElectricitySpell") != 0) {
                f_electricCharge += p_player.GetButtonTimePressed("ElectricitySpell");
		    	anim.SetFloat ("gooCharge", f_electricCharge);
            }
            if (p_player.GetButtonUp("ElectricitySpell")) {
				maestro.PlayElectricShoot();
				anim.SetTrigger ("goospellTrigger");
                f_nextElectric = 0;
			    f_nextCast = 0;
			    GameObject go_spell = Instantiate(go_electricShot, t_spellSpawn.position, t_spellSpawn.rotation);
			    go_spell.transform.localScale = new Vector3(f_projectileSize, f_projectileSize, f_projectileSize);
                go_spell.GetComponent<Rigidbody>().velocity = transform.forward * Constants.SpellStats.C_ElectricSpeed;
                SpellController sc_firing = go_spell.GetComponent<SpellController>();
                sc_firing.Init(this, e_color, f_electricCharge);
                anim.SetFloat("gooCharge", f_electricCharge);
                f_electricCharge = 0;
            }        
		}
        
        // parry
        if (p_player.GetButtonDown("Parry")) {
            go_parryShield.SetActive(true);
            Invoke("TurnOffParryShield", 0.25f);
        }
    }
#endregion
}
