﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class DebugParametersController : MonoBehaviour
{
    // Public Vars
    // Controllers (probably set in editor this time? No instances to pull from in this Playable)
    public GameController GC;
    public DarkMagician DM;
    public List<PlayerController> l_playerControllers = new List<PlayerController>();

    //Menu Buttons
    public Button butt_playerSelect;
    public Button butt_spellSelect;
    public Button butt_enemySelect;
    public Button butt_objectiveSelect;
    private Button[] butt_buttonArray = new Button[4];

    //Menu Organization
    public GameObject go_playerMenu;
    public GameObject go_spellMenu;
    public GameObject go_enemyMenu;
    public GameObject go_objectiveMenu;
    private GameObject[] go_menuArray = new GameObject[4];

    // UI Sliders (Set in editor)
    public Slider slider_playerMoveSpeed;
    public Slider slider_windSpeed;
    public Slider slider_iceSpeed;
    public Slider slider_electricSpeed;
    public Slider slider_windCooldown;
    public Slider slider_iceCooldown;
    public Slider slider_electricCooldown;
    public Slider slider_magicMissileSpeed;
    public Slider slider_magicMissileHeal;
    public Slider slider_projSize;
    public Slider slider_projLife;
    public Slider slider_windForce;
    public Slider slider_iceFreeze;
    public Slider slider_electricLiveTime;
    public Slider slider_enemySpawn;
    public Slider slider_enemySpeed;
    public Slider slider_enemyHealth;
    public Slider slider_enemyDamage;
    public Slider slider_respawnTime;
    public Slider slider_wispMoveSpeed;
    public Slider slider_playerHealth;
    public Slider slider_crystalHealth;
    public Slider slider_CTFScore;
    public Slider slider_completionTimer;
    public Slider slider_selfDestructTimer;
    public Slider slider_enemySpawnCap;
    public Slider slider_icePlayerDamage;
    public Slider slider_windPlayerDamage;
    public Slider slider_electricPlayerDamage;
    public Slider slider_hockeyMaxScore;
    public Slider slider_puckDamage;
    public Slider slider_puckSpeedDecayRate;
    public Slider slider_puckSpeedDecreaseRate;
    public Slider slider_puckBaseSpeed;
    public Slider slider_puckHitIncreaseSpeed;

    // UI txt (Set in editor)
    public Text txt_playerMoveSpeed;
    public Text txt_windSpeed;
    public Text txt_iceSpeed;
    public Text txt_electricSpeed;
    public Text txt_windCooldown;
    public Text txt_iceCooldown;
    public Text txt_electricCooldown;
    public Text txt_magicMissileSpeed;
    public Text txt_magicMissileHeal;
    public Text txt_projSize;
    public Text txt_projLife;
    public Text txt_windForce;
    public Text txt_iceFreeze;
    public Text txt_electricLiveTime;
    public Text txt_enemySpawn;
    public Text txt_enemySpeed;
    public Text txt_enemyHealth;
    public Text txt_enemyDamage;
    public Text txt_respawnTime;
    public Text txt_wispMoveSpeed;
    public Text txt_playerHealth;
    public Text txt_crystalHealth;
    public Text txt_CTFScore;
    public Text txt_completionTimer;
    public Text txt_selfDestructTimer;
    public Text txt_enemySpawnCap;
    public Text txt_icePlayerDamage;
    public Text txt_windPlayerDamage;
    public Text txt_electricPlayerDamage;
    public Text txt_hockeyMaxScore;
    public Text txt_puckDamage;
    public Text txt_puckSpeedDecayRate;
    public Text txt_puckSpeedDecreaseRate;
    public Text txt_puckBaseSpeed;
    public Text txt_puckHitIncreaseSpeed;

    // Public Helper Methods
    // currently unused - GC set in inspector
    public void SetGameController(GameController game_controllerIn)
    {
        GC = game_controllerIn;
    }

    // currently unused - players set in inspector
    public void AddPlayerController(PlayerController play_controllerIn)
    {
        l_playerControllers.Add(play_controllerIn);
    }

    // Private Helper Methods
    public void ChangePlayerSpeed(float f_playerSpeedIn)
    {
        txt_playerMoveSpeed.text = slider_playerMoveSpeed.value.ToString();
        Constants.PlayerStats.C_MovementSpeed = f_playerSpeedIn;
    }

    public void ChangePlayerWispSpeed(float f_playerWispSpeedIn)
    {
        txt_wispMoveSpeed.text = slider_wispMoveSpeed.value.ToString();
        Constants.PlayerStats.C_WispMovementSpeed = f_playerWispSpeedIn;
    }

    public void ChangeMagicMissileSpeed(float f_magicMissileSpeedIn)
    {
        txt_magicMissileSpeed.text = slider_magicMissileSpeed.value.ToString();
        Constants.SpellStats.C_MagicMissileSpeed = f_magicMissileSpeedIn;
    }

    public void ChangeMagicMissileHeal(float f__magicMissileHealIn)
    {
        txt_magicMissileHeal.text = slider_magicMissileHeal.value.ToString();
        Constants.SpellStats.C_MagicMissileHeal = (int)f__magicMissileHealIn;
    }

    public void ChangeWindSpeed(float f_windSpeedIn)
    {
        txt_windSpeed.text = slider_windSpeed.value.ToString();
        Constants.SpellStats.C_WindSpeed = f_windSpeedIn;
    }

    public void ChangeIceSpeed(float f_iceSpeedIn)
    {
        txt_iceSpeed.text = slider_iceSpeed.value.ToString();
        Constants.SpellStats.C_IceSpeed = f_iceSpeedIn;
    }

    public void ChangeElectricSpeed(float f_electricSpeedIn)
    {
        txt_electricSpeed.text = slider_electricSpeed.value.ToString();
        Constants.SpellStats.C_ElectricSpeed = f_electricSpeedIn;
    }

    public void ChangeWindCooldown(float f_windCooldownIn)
    {
        txt_windCooldown.text = slider_windCooldown.value.ToString();
        Constants.SpellStats.C_WindCooldown = f_windCooldownIn;
    }

    public void ChangeIceCooldown(float f_iceCooldownIn)
    {
        txt_iceCooldown.text = slider_iceCooldown.value.ToString();
        Constants.SpellStats.C_IceCooldown = f_iceCooldownIn;
    }

    public void ChangeElectricCooldown(float f_electricCooldownIn)
    {
        txt_electricCooldown.text = slider_electricCooldown.value.ToString();
        Constants.SpellStats.C_ElectricCooldown = f_electricCooldownIn;
    }

    public void ChangeProjectileSize(float f_projSizeIn)
    {
        float roundedVal = Mathf.Round(slider_projSize.value * 100f) / 100f;
        txt_projSize.text = roundedVal.ToString();
        Constants.SpellStats.C_PlayerProjectileSize = roundedVal;
    }

    public void ChangeSpellLifetime(float f_projLifeIn)
    {
        txt_projLife.text = slider_projLife.value.ToString();
        Constants.SpellStats.C_SpellLiveTime = f_projLifeIn;
    }

    public void ChangeWindForce(float f_windForceIn)
    {
        txt_windForce.text = slider_windForce.value.ToString();
        Constants.SpellStats.C_WindForce = f_windForceIn;
    }

    public void ChangeFreezeDuration(float f_iceFreezeIn)
    {
        txt_iceFreeze.text = slider_iceFreeze.value.ToString();
        Constants.SpellStats.C_IceFreezeTime = f_iceFreezeIn;
    }

    public void ChangeElectricLiveTime(float f_electricLiveTimeIn)
    {
        txt_electricLiveTime.text = slider_electricLiveTime.value.ToString();
        Constants.SpellStats.C_ElectricAOELiveTime = f_electricLiveTimeIn;
    }

    // TODO: Change in RiftController.
    public void ChangeEnemySpawnRate(float f_enemySpawnIn)
    {
        txt_enemySpawn.text = slider_enemySpawn.value.ToString();
        Constants.RiftStats.C_VolatilityEnemySpawnTimer = f_enemySpawnIn;
    }

    public void ChangeEnemySpeed(float f_enemySpeedIn)
    {
        txt_enemySpeed.text = slider_enemySpeed.value.ToString();
        Constants.EnemyStats.C_EnemySpeed = f_enemySpeedIn;
        RiftController.GetInstance().setEnemyStartingSpeed();
    }

    public void ChangeEnemyHealth(float f_enemyHealthIn)
    {
        txt_enemyHealth.text = slider_enemyHealth.value.ToString();
        Constants.EnemyStats.C_EnemyHealth = (int)f_enemyHealthIn;
    }

    public void ChangeEnemyDamage(float f_enemyDamageIn)
    {
        txt_enemyDamage.text = slider_enemyDamage.value.ToString();
        Constants.EnemyStats.C_EnemyDamage = (int)f_enemyDamageIn;
    }

    public void ChangePlayerHealth(float f_playerHealthIn)
    {
        txt_playerHealth.text = slider_playerHealth.value.ToString();
        Constants.PlayerStats.C_MaxHealth = (int)f_playerHealthIn;
    }

    public void ChangeRespawnTime(float f_respawnTimeIn)
    {
        txt_respawnTime.text = slider_respawnTime.value.ToString();
        Constants.PlayerStats.C_RespawnTimer = f_respawnTimeIn;
    }

    public void ChangeCrystalHealth(float f_crystalHealthIn)
    {
        txt_crystalHealth.text = slider_crystalHealth.value.ToString();
        Constants.ObjectiveStats.C_CrystalMaxHealth = (int)f_crystalHealthIn;
        if (DM.objv_blueObjective.gameObject.GetComponent<CrystalDestructionObjective>()) {
            DM.objv_blueObjective.gameObject.GetComponent<CrystalDestructionObjective>().ParamReset(f_crystalHealthIn);
        }
        if (DM.objv_redObjective.gameObject.GetComponent<CrystalDestructionObjective>()) {
            DM.objv_redObjective.gameObject.GetComponent<CrystalDestructionObjective>().ParamReset(f_crystalHealthIn);
        }
    }

    public void ChangeCTFMaxScore(float f_CTFScoreIn)
    {
        txt_CTFScore.text = slider_CTFScore.value.ToString();
        Constants.ObjectiveStats.C_CTFMaxScore = (int)f_CTFScoreIn;
    }

    public void ChangeCompletionTimer(float f_timerIn)
    {
        txt_completionTimer.text = slider_completionTimer.value.ToString();
        Constants.ObjectiveStats.C_PotatoCompletionTimer = (int)f_timerIn;
    }

    public void ChangeSelfDestructTimer(float f_timerIn)
    {
        txt_selfDestructTimer.text = slider_selfDestructTimer.value.ToString();
        Constants.ObjectiveStats.C_PotatoSelfDestructTimer = (int)f_timerIn;
    }

    public void ChangeEnemySpawnCap(float f_capIn)
    {
        txt_enemySpawnCap.text = slider_enemySpawnCap.value.ToString();
        Constants.EnemyStats.C_EnemySpawnCapPerSide = (int)(f_capIn - 1);
    }

    public void ChangeIcePlayerDamage(float f_damageIn)
    {
        txt_icePlayerDamage.text = slider_icePlayerDamage.value.ToString();
        Constants.SpellStats.C_IcePlayerDamageMultiplier = f_damageIn;
    }

    public void ChangeWindPlayerDamage(float f_damageIn)
    {
        txt_windPlayerDamage.text = slider_windPlayerDamage.value.ToString();
        Constants.SpellStats.C_WindPlayerDamageMultiplier = f_damageIn;
    }

    public void ChangeElectricPlayerDamage(float f_damageIn)
    {
        txt_electricPlayerDamage.text = slider_electricPlayerDamage.value.ToString();
        Constants.SpellStats.C_ElectricPlayerDamageMultiplier = f_damageIn;
    }

    public void ChangeHockeyMaxScore(float f_score)
    {
        txt_hockeyMaxScore.text = slider_hockeyMaxScore.value.ToString();
        Constants.ObjectiveStats.C_HockeyMaxScore = (int)f_score;
    }

    public void ChangePuckDamage(float f_damage)
    {
        txt_puckDamage.text = slider_puckDamage.value.ToString();
        Constants.ObjectiveStats.C_PuckDamage = (int)f_damage;
    }

    public void ChangePuckSpeedDecayRate(float f_decay)
    {
        txt_puckSpeedDecayRate.text = slider_puckSpeedDecayRate.value.ToString();
        Constants.ObjectiveStats.C_PuckSpeedDecayRate = (int)f_decay;
    }

    public void ChangePuckSpeedDecreaseRate(float f_decrease)
    {
        txt_puckSpeedDecreaseRate.text = slider_puckSpeedDecreaseRate.value.ToString();
        Constants.ObjectiveStats.C_PuckSpeedDecreaseRate = (int)f_decrease;
    }

    public void ChangePuckBaseSpeed(float f_speed)
    {
        txt_puckBaseSpeed.text = slider_puckBaseSpeed.value.ToString();
        Constants.ObjectiveStats.C_PuckBaseSpeed = (int)f_speed;
    }

    public void ChangePuckHitIncreaseSpeed(float f_hit)
    {
        txt_puckHitIncreaseSpeed.text = slider_puckHitIncreaseSpeed.value.ToString();
        Constants.ObjectiveStats.C_PuckHitIncreaseSpeed = (int)f_hit;
    }

    public void ObjectiveReset()
    {
        //Drop the flag before resetting (for CTF)
        foreach (PlayerController playerController in l_playerControllers) {
            playerController.Drop();
        }
        //Then reset. 
        DM.objv_redObjective.ParamReset(Constants.ObjectiveStats.C_CrystalMaxHealth);
        DM.objv_blueObjective.ParamReset(Constants.ObjectiveStats.C_CrystalMaxHealth);
    }

    public void GameReset()
    {
        //Loads the first scene... which should be the first scene in the build.
        //Will reload the same scene otherwise.
        RiftController.GetInstance().Restart();
        SceneManager.LoadScene("WarmUp");
    }

    //Light buttons up as their selected.
    public void LightUp(int which)
    {
        for (int i = 0; i < 4; i++) {
            ColorBlock cb = butt_buttonArray[i].colors;
            if (i == which) {
                cb.normalColor = Color.cyan;
                cb.highlightedColor = Color.cyan;
            }
            else {
                cb.normalColor = Color.white;
                cb.highlightedColor = Color.white;
            }
            butt_buttonArray[i].colors = cb;
        }
    }

    //Show the proper menu on click.
    public void MenuSwitch(int which)
    {
        for (int i = 0; i < 4; i++) {
            if (i == which) {
                go_menuArray[i].SetActive(true);
            }
            else {
                go_menuArray[i].SetActive(false);
            }
        }
    }

    // Get initial values from Constants, or from editor if they're not in Constants.
    void Start()
    {
        // Player Speed
        txt_playerMoveSpeed.text = Constants.PlayerStats.C_MovementSpeed.ToString();
        slider_playerMoveSpeed.value = Constants.PlayerStats.C_MovementSpeed;

        // Player Wisp Speed
        txt_wispMoveSpeed.text = Constants.PlayerStats.C_WispMovementSpeed.ToString();
        slider_wispMoveSpeed.value = Constants.PlayerStats.C_WispMovementSpeed;

        // Wind Spell Speed
        txt_windSpeed.text = Constants.SpellStats.C_WindSpeed.ToString();
        slider_windSpeed.value = Constants.SpellStats.C_WindSpeed;

        // Ice Spell Speed
        txt_iceSpeed.text = Constants.SpellStats.C_IceSpeed.ToString();
        slider_iceSpeed.value = Constants.SpellStats.C_IceSpeed;

        // Electric Spell Speed
        txt_electricSpeed.text = Constants.SpellStats.C_ElectricSpeed.ToString();
        slider_electricSpeed.value = Constants.SpellStats.C_ElectricSpeed;

        // Wind Spell Cooldown
        txt_windCooldown.text = Constants.SpellStats.C_WindCooldown.ToString();
        slider_windCooldown.value = Constants.SpellStats.C_WindCooldown;

        // Ice Spell Cooldown
        txt_iceCooldown.text = Constants.SpellStats.C_IceCooldown.ToString();
        slider_iceCooldown.value = Constants.SpellStats.C_IceCooldown;

        // Electric Spell Cooldown
        txt_electricCooldown.text = Constants.SpellStats.C_ElectricCooldown.ToString();
        slider_electricCooldown.value = Constants.SpellStats.C_ElectricCooldown;

        //----------------------------
        // Magic Missile Speed
        txt_magicMissileSpeed.text = Constants.SpellStats.C_MagicMissileSpeed.ToString();
        slider_magicMissileSpeed.value = Constants.SpellStats.C_MagicMissileSpeed;

        // Magic Missile Heal
        txt_magicMissileHeal.text = Constants.SpellStats.C_MagicMissileHeal.ToString();
        slider_magicMissileHeal.value = Constants.SpellStats.C_MagicMissileHeal;

        // Projectile Size 
        txt_projSize.text = Constants.SpellStats.C_PlayerProjectileSize.ToString();
        slider_projSize.value = Constants.SpellStats.C_PlayerProjectileSize;

        // Projectile Live Time
        txt_projLife.text = Constants.SpellStats.C_SpellLiveTime.ToString();
        slider_projLife.value = Constants.SpellStats.C_SpellLiveTime;

        // Wind Force
        txt_windForce.text = Constants.SpellStats.C_WindForce.ToString();
        slider_windForce.value = Constants.SpellStats.C_WindForce;

        // Ice Freeze Duration
        txt_iceFreeze.text = Constants.SpellStats.C_IceFreezeTime.ToString();
        slider_iceFreeze.value = Constants.SpellStats.C_IceFreezeTime;

        // Electric AOE live-time Freeze Duration
        txt_electricLiveTime.text = Constants.SpellStats.C_ElectricAOELiveTime.ToString();
        slider_electricLiveTime.value = Constants.SpellStats.C_ElectricAOELiveTime;

        //----------------------------

        // Enemy Spawn Rate
        txt_enemySpawn.text = Constants.RiftStats.C_VolatilityEnemySpawnTimer.ToString();
        slider_enemySpawn.value = Constants.RiftStats.C_VolatilityEnemySpawnTimer;

        // Enemy Speed
        txt_enemySpeed.text = Constants.EnemyStats.C_EnemySpeed.ToString();
        slider_enemySpeed.value = Constants.EnemyStats.C_EnemySpeed;

        // Enemy Health
        txt_enemyHealth.text = Constants.EnemyStats.C_EnemyHealth.ToString();
        slider_enemyHealth.value = Constants.EnemyStats.C_EnemyHealth;

        // Enemy Damage
        txt_enemyDamage.text = Constants.EnemyStats.C_EnemyDamage.ToString();
        slider_enemyDamage.value = Constants.EnemyStats.C_EnemyDamage;

        // Respawn Rate
        txt_respawnTime.text = Constants.PlayerStats.C_RespawnTimer.ToString();
        slider_respawnTime.value = Constants.PlayerStats.C_RespawnTimer;

        // Player Health
        txt_playerHealth.text = Constants.PlayerStats.C_MaxHealth.ToString();
        slider_playerHealth.value = Constants.PlayerStats.C_MaxHealth;

        //---------------------------
        // Crystal Health
        txt_crystalHealth.text = Constants.ObjectiveStats.C_CrystalMaxHealth.ToString();
        slider_crystalHealth.value = Constants.ObjectiveStats.C_CrystalMaxHealth;

        // CTF Score
        txt_CTFScore.text = Constants.ObjectiveStats.C_CTFMaxScore.ToString();
        slider_CTFScore.value = Constants.ObjectiveStats.C_CTFMaxScore;

        //Completion Timer for Hot Potato
        txt_completionTimer.text = Constants.ObjectiveStats.C_PotatoCompletionTimer.ToString();
        slider_completionTimer.value = Constants.ObjectiveStats.C_PotatoCompletionTimer;

        //Self Destruct Timer for Hot Potato
        txt_selfDestructTimer.text = Constants.ObjectiveStats.C_PotatoSelfDestructTimer.ToString();
        slider_selfDestructTimer.value = Constants.ObjectiveStats.C_PotatoSelfDestructTimer;

        //---------------------------
        //Ice Player Damage Multiplier
        txt_icePlayerDamage.text = Constants.SpellStats.C_IcePlayerDamageMultiplier.ToString();
        slider_icePlayerDamage.value = Constants.SpellStats.C_IcePlayerDamageMultiplier;

        //wind Player Damage Multiplier
        txt_windPlayerDamage.text = Constants.SpellStats.C_WindPlayerDamageMultiplier.ToString();
        slider_windPlayerDamage.value = Constants.SpellStats.C_WindPlayerDamageMultiplier;

        //electric Player Damage Multiplier
        txt_electricPlayerDamage.text = Constants.SpellStats.C_ElectricPlayerDamageMultiplier.ToString();
        slider_electricPlayerDamage.value = Constants.SpellStats.C_ElectricPlayerDamageMultiplier;

        //Hockey Puck damage
        txt_puckDamage.text = Constants.ObjectiveStats.C_PuckDamage.ToString();
        slider_puckDamage.value = Constants.ObjectiveStats.C_PuckDamage;

        //Hockey Max Score
        txt_hockeyMaxScore.text = Constants.ObjectiveStats.C_HockeyMaxScore.ToString();
        slider_hockeyMaxScore.value = Constants.ObjectiveStats.C_HockeyMaxScore;

        //Hockey Puck Speed Decay Rate
        txt_puckSpeedDecayRate.text = Constants.ObjectiveStats.C_PuckSpeedDecayRate.ToString();
        slider_puckSpeedDecayRate.value = Constants.ObjectiveStats.C_PuckSpeedDecayRate;

        //Hockey Puck Decreasse Rate
        txt_puckSpeedDecreaseRate.text = Constants.ObjectiveStats.C_PuckSpeedDecreaseRate.ToString();
        slider_puckSpeedDecreaseRate.value = Constants.ObjectiveStats.C_PuckSpeedDecreaseRate;

        //Hockey Puck Base Speed
        txt_puckBaseSpeed.text = Constants.ObjectiveStats.C_PuckBaseSpeed.ToString();
        slider_puckBaseSpeed.value = Constants.ObjectiveStats.C_PuckBaseSpeed;

        //Hockey Puck Hit Increase Speed
        txt_hockeyMaxScore.text = Constants.ObjectiveStats.C_PuckHitIncreaseSpeed.ToString();
        slider_hockeyMaxScore.value = Constants.ObjectiveStats.C_PuckHitIncreaseSpeed;


        //---------------------------
        //Get buttons to put into an array.
        butt_buttonArray[0] = butt_playerSelect;
        butt_buttonArray[1] = butt_spellSelect;
        butt_buttonArray[2] = butt_enemySelect;
        butt_buttonArray[3] = butt_objectiveSelect;

        LightUp(0);

        //Get menus to put into an array.
        go_menuArray[0] = go_playerMenu;
        go_menuArray[1] = go_spellMenu;
        go_menuArray[2] = go_enemyMenu;
        go_menuArray[3] = go_objectiveMenu;

        MenuSwitch(0);
    }
}
