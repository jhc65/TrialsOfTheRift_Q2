/*  Debug Parameters Controller - Sam Caulker
 * 
 *  Desc:   Allows players to change the values stored in Constants.cs via interactable sliders
 * 
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class DebugParametersController : MonoBehaviour {
    // Public Vars
    [SerializeField] PlayerSelectController psc_master;

    // Menu buttons
    [SerializeField] private Button butt_playerSelect;
    [SerializeField] private Button butt_spellSelect;
    [SerializeField] private Button butt_enemySelect;
    [SerializeField] private Button butt_objectiveSelect;
    private Button[] butt_buttonArray = new Button[4];
    
    //Other button references (for navigation)
    [SerializeField]private Button butt_go;

    // Menu organization
    [SerializeField] private GameObject go_topMenu;
    [SerializeField] private GameObject go_select;
    [SerializeField] private GameObject go_regController;

    [SerializeField] private GameObject go_playerMenu;
    [SerializeField] private GameObject go_spellMenu;
    [SerializeField] private GameObject go_enemyMenu;
    [SerializeField] private GameObject go_objectiveMenu;
    private GameObject[] go_menuArray = new GameObject[4];

    // UI sliders (set in editor)
    [SerializeField] private Slider slider_playerMoveSpeed;
    [SerializeField] private Slider slider_windSpeed;
    [SerializeField] private Slider slider_iceSpeed;
    [SerializeField] private Slider slider_electricSpeed;
    [SerializeField] private Slider slider_windCooldown;
    [SerializeField] private Slider slider_iceCooldown;
    [SerializeField] private Slider slider_electricCooldown;
    [SerializeField] private Slider slider_magicMissileSpeed;
    [SerializeField] private Slider slider_magicMissileHeal;
    [SerializeField] private Slider slider_projSize;
    [SerializeField] private Slider slider_projLife;
    [SerializeField] private Slider slider_windForce;
    [SerializeField] private Slider slider_iceFreeze;
    [SerializeField] private Slider slider_electricLiveTime;
    [SerializeField] private Slider slider_enemySpawn;
    [SerializeField] private Slider slider_enemySpeed;
    [SerializeField] private Slider slider_enemyHealth;
    [SerializeField] private Slider slider_enemyDamage;
    [SerializeField] private Slider slider_respawnTime;
    [SerializeField] private Slider slider_wispMoveSpeed;
    [SerializeField] private Slider slider_playerHealth;
    [SerializeField] private Slider slider_crystalHealth;
    [SerializeField] private Slider slider_crystalHealthRegen;
    [SerializeField] private Slider slider_crystalHealthRegenRate;
    [SerializeField] private Slider slider_crystalHealthRegenDelay;
    [SerializeField] private Slider slider_CTFScore;
    [SerializeField] private Slider slider_completionTimer;
    [SerializeField] private Slider slider_selfDestructTimer;
    [SerializeField] private Slider slider_enemySpawnCap;
    [SerializeField] private Slider slider_hockeyMaxScore;
    [SerializeField] private Slider slider_puckDamage;
    [SerializeField] private Slider slider_puckSpeedDecayRate;
    [SerializeField] private Slider slider_puckSpeedDecreaseRate;
    [SerializeField] private Slider slider_puckBaseSpeed;
    [SerializeField] private Slider slider_puckHitIncreaseSpeed;
    [SerializeField] private Slider slider_riftBossHealth;
    [SerializeField] private Slider slider_runeSpawnInterval;
    [SerializeField] private Slider slider_deathBoltCooldown;
    [SerializeField] private Slider slider_forceFieldCooldown;

    // UI text (set in editor)
    [SerializeField] private Text txt_playerMoveSpeed;
    [SerializeField] private Text txt_windSpeed;
    [SerializeField] private Text txt_iceSpeed;
    [SerializeField] private Text txt_electricSpeed;
    [SerializeField] private Text txt_windCooldown;
    [SerializeField] private Text txt_iceCooldown;
    [SerializeField] private Text txt_electricCooldown;
    [SerializeField] private Text txt_magicMissileSpeed;
    [SerializeField] private Text txt_magicMissileHeal;
    [SerializeField] private Text txt_projSize;
    [SerializeField] private Text txt_projLife;
    [SerializeField] private Text txt_windForce;
    [SerializeField] private Text txt_iceFreeze;
    [SerializeField] private Text txt_electricLiveTime;
    [SerializeField] private Text txt_enemySpawn;
    [SerializeField] private Text txt_enemySpeed;
    [SerializeField] private Text txt_enemyHealth;
    [SerializeField] private Text txt_enemyDamage;
    [SerializeField] private Text txt_respawnTime;
    [SerializeField] private Text txt_wispMoveSpeed;
    [SerializeField] private Text txt_playerHealth;
    [SerializeField] private Text txt_crystalHealth;
    [SerializeField] private Text txt_crystalHealthRegen;
    [SerializeField] private Text txt_crystalHealthRegenRate;
    [SerializeField] private Text txt_crystalHealthRegenDelay;
    [SerializeField] private Text txt_CTFScore;
    [SerializeField] private Text txt_completionTimer;
    [SerializeField] private Text txt_selfDestructTimer;
    [SerializeField] private Text txt_enemySpawnCap;
    [SerializeField] private Text txt_hockeyMaxScore;
    [SerializeField] private Text txt_puckDamage;
    [SerializeField] private Text txt_puckSpeedDecayRate;
    [SerializeField] private Text txt_puckSpeedDecreaseRate;
    [SerializeField] private Text txt_puckBaseSpeed;
    [SerializeField] private Text txt_puckHitIncreaseSpeed;
    [SerializeField] private Text txt_riftBossHealth;
    [SerializeField] private Text txt_runeSpawnInterval;
    [SerializeField] private Text txt_deathBoltCooldown;
    [SerializeField] private Text txt_forceFieldCooldown;

    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    // Public Helper Methods
    // currently unused - GC set in inspector
    //[SerializeField] private void SetGameController(GameController game_controllerIn)
    //{
    //    GC = game_controllerIn;
    //}

    //// currently unused - players set in inspector
    //[SerializeField] private void AddPlayerController(PlayerController play_controllerIn)
    //{
    //    l_playerControllers.Add(play_controllerIn);
    //}

    // Slider change callbacks
    [SerializeField] private void ChangePlayerSpeed(float f_playerSpeedIn) {
        txt_playerMoveSpeed.text = slider_playerMoveSpeed.value.ToString();
        Constants.PlayerStats.C_MovementSpeed = f_playerSpeedIn;
    }

    [SerializeField] private void ChangePlayerWispSpeed(float f_playerWispSpeedIn) {
        txt_wispMoveSpeed.text = slider_wispMoveSpeed.value.ToString();
        Constants.PlayerStats.C_WispMovementSpeed = f_playerWispSpeedIn;
    }

    [SerializeField] private void ChangeMagicMissileSpeed(float f_magicMissileSpeedIn) {
        txt_magicMissileSpeed.text = slider_magicMissileSpeed.value.ToString();
        Constants.SpellStats.C_MagicMissileSpeed = f_magicMissileSpeedIn;
    }

    [SerializeField] private void ChangeMagicMissileHeal(float f__magicMissileHealIn) {
        txt_magicMissileHeal.text = slider_magicMissileHeal.value.ToString();
        Constants.SpellStats.C_MagicMissileHeal = (int)f__magicMissileHealIn;
    }

    [SerializeField] private void ChangeWindSpeed(float f_windSpeedIn) {
        txt_windSpeed.text = slider_windSpeed.value.ToString();
        Constants.SpellStats.C_WindSpeed = f_windSpeedIn;
    }

    [SerializeField] private void ChangeIceSpeed(float f_iceSpeedIn) {
        txt_iceSpeed.text = slider_iceSpeed.value.ToString();
        Constants.SpellStats.C_IceSpeed = f_iceSpeedIn;
    }

    [SerializeField] private void ChangeElectricSpeed(float f_electricSpeedIn) {
        txt_electricSpeed.text = slider_electricSpeed.value.ToString();
        Constants.SpellStats.C_ElectricSpeed = f_electricSpeedIn;
    }

    [SerializeField] private void ChangeWindCooldown(float f_windCooldownIn) {
        txt_windCooldown.text = slider_windCooldown.value.ToString();
        Constants.SpellStats.C_WindCooldown = f_windCooldownIn;
    }

    [SerializeField] private void ChangeIceCooldown(float f_iceCooldownIn) {
        txt_iceCooldown.text = slider_iceCooldown.value.ToString();
        Constants.SpellStats.C_IceCooldown = f_iceCooldownIn;
    }

    [SerializeField] private void ChangeElectricCooldown(float f_electricCooldownIn) {
        txt_electricCooldown.text = slider_electricCooldown.value.ToString();
        Constants.SpellStats.C_ElectricCooldown = f_electricCooldownIn;
    }

    [SerializeField] private void ChangeProjectileSize(float f_projSizeIn) {
        float roundedVal = Mathf.Round(slider_projSize.value * 100f) / 100f;
        txt_projSize.text = roundedVal.ToString();
        Constants.SpellStats.C_PlayerProjectileSize = roundedVal;
    }

    [SerializeField] private void ChangeSpellLifetime(float f_projLifeIn) {
        txt_projLife.text = slider_projLife.value.ToString();
        Constants.SpellStats.C_SpellLiveTime = f_projLifeIn;
    }

    [SerializeField] private void ChangeWindForce(float f_windForceIn) {
        float value = f_windForceIn * 250.0f;
        txt_windForce.text = value.ToString();
        Constants.SpellStats.C_WindForce = value;
    }

    [SerializeField] private void ChangeFreezeDuration(float f_iceFreezeIn) {
        txt_iceFreeze.text = slider_iceFreeze.value.ToString();
        Constants.SpellStats.C_IceFreezeTime = f_iceFreezeIn;
    }

    [SerializeField] private void ChangeElectricLiveTime(float f_electricLiveTimeIn) {
        txt_electricLiveTime.text = slider_electricLiveTime.value.ToString();
        Constants.SpellStats.C_ElectricAOELiveTime = f_electricLiveTimeIn;
    }

    [SerializeField] private void ChangeEnemySpawnRate(float f_enemySpawnIn) {
        txt_enemySpawn.text = slider_enemySpawn.value.ToString();
        Constants.RiftStats.C_VolatilityEnemySpawnTimer = f_enemySpawnIn;
    }

    [SerializeField] private void ChangeEnemySpeed(float f_enemySpeedIn) {
        txt_enemySpeed.text = slider_enemySpeed.value.ToString();
        Constants.EnemyStats.C_EnemyBaseSpeed = f_enemySpeedIn;
    }

    [SerializeField] private void ChangeEnemyHealth(float f_enemyHealthIn) {
        float value = f_enemyHealthIn * 25.0f;
        txt_enemyHealth.text = value.ToString();
        Constants.EnemyStats.C_EnemyHealth = (int)value;
    }

    [SerializeField] private void ChangeEnemyDamage(float f_enemyDamageIn) {
        float value = f_enemyDamageIn * 5.0f;
        txt_enemyDamage.text = value.ToString();
        Constants.EnemyStats.C_EnemyDamage = (int)value;
    }

    [SerializeField] private void ChangePlayerHealth(float f_playerHealthIn) {
        float value = f_playerHealthIn * 50.0f;
        txt_playerHealth.text = value.ToString();
        Constants.PlayerStats.C_MaxHealth = (int)value;
    }

    [SerializeField] private void ChangeRespawnTime(float f_respawnTimeIn) {
        txt_respawnTime.text = slider_respawnTime.value.ToString();
        Constants.PlayerStats.C_RespawnTimer = f_respawnTimeIn;
    }

    [SerializeField] private void ChangeCrystalHealth(float f_crystalHealthIn) {
        float value = f_crystalHealthIn * 50.0f;
        txt_crystalHealth.text = value.ToString();
        Constants.ObjectiveStats.C_CrystalMaxHealth = (int)value;
    }

    [SerializeField] private void ChangeCrystalHealthRegen(float f_crystalHealthIn)
    {
        float value = f_crystalHealthIn;
        txt_crystalHealthRegen.text = value.ToString();
        Constants.ObjectiveStats.C_CrystalRegenHeal = (int)value;
    }

    [SerializeField] private void ChangeCrystalHealthRegenRate(float f_crystalHealthIn)
    {
        float value = f_crystalHealthIn;
        txt_crystalHealthRegenRate.text = value.ToString();
        Constants.ObjectiveStats.C_CrystalHealRate = (int)value;
    }

    [SerializeField] private void ChangeCrystalHealthRegenDelay(float f_crystalHealthIn)
    {
        float value = f_crystalHealthIn;
        txt_crystalHealthRegenDelay.text = value.ToString();
        Constants.ObjectiveStats.C_CrystalHealDelay = (int)value;
    }

    [SerializeField] private void ChangeCTFMaxScore(float f_CTFScoreIn) {
        txt_CTFScore.text = slider_CTFScore.value.ToString();
        Constants.ObjectiveStats.C_CTFMaxScore = (int)f_CTFScoreIn;
    }

    [SerializeField] private void ChangeCompletionTimer(float f_timerIn) {
        float value = f_timerIn * 5.0f;
        txt_completionTimer.text = value.ToString();
        Constants.ObjectiveStats.C_PotatoCompletionTimer = (int)value;
    }

    [SerializeField] private void ChangeSelfDestructTimer(float f_timerIn) {
        float value = f_timerIn * 5.0f;
        txt_selfDestructTimer.text = value.ToString();
        Constants.ObjectiveStats.C_PotatoSelfDestructTimer = (int)value;
    }

    [SerializeField] private void ChangeEnemySpawnCap(float f_capIn) {
        txt_enemySpawnCap.text = slider_enemySpawnCap.value.ToString();
        Constants.EnemyStats.C_EnemySpawnCapPerSide = (int)(f_capIn - 1);
    }

    [SerializeField] private void ChangeHockeyMaxScore(float f_score) {
        txt_hockeyMaxScore.text = slider_hockeyMaxScore.value.ToString();
        Constants.ObjectiveStats.C_HockeyMaxScore = (int)f_score;
    }

    [SerializeField] private void ChangePuckDamage(float f_damage) {
        float value = f_damage * 10.0f;
        txt_puckDamage.text = value.ToString();
        Constants.ObjectiveStats.C_PuckDamage = (int)value;
    }

    [SerializeField] private void ChangePuckSpeedDecayRate(float f_decay) {
        txt_puckSpeedDecayRate.text = slider_puckSpeedDecayRate.value.ToString();
        Constants.ObjectiveStats.C_PuckSpeedDecayRate = (int)f_decay;
    }

    [SerializeField] private void ChangePuckSpeedDecreaseRate(float f_decrease) {
        txt_puckSpeedDecreaseRate.text = slider_puckSpeedDecreaseRate.value.ToString();
        Constants.ObjectiveStats.C_PuckSpeedDecreaseAmount = (int)f_decrease;
    }

    [SerializeField] private void ChangePuckBaseSpeed(float f_speed) {
        txt_puckBaseSpeed.text = slider_puckBaseSpeed.value.ToString();
        Constants.ObjectiveStats.C_PuckBaseSpeed = (int)f_speed;
    }

    [SerializeField] private void ChangePuckHitIncreaseSpeed(float f_hit) {
        txt_puckHitIncreaseSpeed.text = slider_puckHitIncreaseSpeed.value.ToString();
        Constants.ObjectiveStats.C_PuckSpeedHitIncrease = (int)f_hit;
    }

    [SerializeField] private void ChangeRiftBossHealth(float f_riftBossHealthIn) {
        float value = f_riftBossHealthIn * 250.0f;
        txt_riftBossHealth.text = value.ToString();
        Constants.ObjectiveStats.C_RiftBossMaxHealth = (int)value;
    }

    [SerializeField] private void ChangeRuneSpawnInterval(float f_interval)
    {
        txt_runeSpawnInterval.text = slider_runeSpawnInterval.value.ToString();
        Constants.ObjectiveStats.C_RuneSpawnInterval = (int)f_interval;
    }

    [SerializeField] private void ChangeDeathBoltCooldown(float f_cooldown)
    {
        txt_deathBoltCooldown.text = slider_deathBoltCooldown.value.ToString();
        Constants.ObjectiveStats.C_DeathBoltCooldown = (int)f_cooldown;
    }

    [SerializeField] private void ChangeForceFieldCooldown(float f_cooldown)
    {
        txt_forceFieldCooldown.text = slider_forceFieldCooldown.value.ToString();
        Constants.ObjectiveStats.C_ForceFieldCooldown = (int)f_cooldown;
    }

    // Light buttons up as they are selected
    [SerializeField] private void LightUp(int which) {
        for (int i = 0; i < 4; i++) {
            ColorBlock cb = butt_buttonArray[i].colors;
            if (i == which) {
                cb.normalColor = Color.cyan;
            }
            else {
                cb.normalColor = Color.white;
            }
            butt_buttonArray[i].colors = cb;
        }
    }

    // Show the proper menu on click
    [SerializeField] private void MenuSwitch(int which) {
        for (int i = 0; i < 4; i++) {
            if (i == which) {
                go_menuArray[i].SetActive(true);
                Navigation nav_goNav = butt_go.navigation;
                switch (i) {
                    case 0:
                        nav_goNav.selectOnUp = slider_playerHealth;
                        nav_goNav.selectOnRight = slider_respawnTime;
                        break;
                    case 1:
                        nav_goNav.selectOnUp = slider_electricCooldown;
                        nav_goNav.selectOnRight = slider_electricLiveTime;
                        break;
                    case 2:
                        nav_goNav.selectOnUp = slider_enemyHealth;
                        nav_goNav.selectOnRight = slider_enemyDamage;
                        break;
                    case 3:
                        nav_goNav.selectOnUp = slider_puckSpeedDecayRate;
                        nav_goNav.selectOnRight = slider_selfDestructTimer;
                        break;
                }
                butt_go.navigation = nav_goNav;
            }
            else {
                go_menuArray[i].SetActive(false);
            }
        }
    }

    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    // Get initial values from Constants.cs
    void Start() {

        //----------------------------
        // Player

        // Player Speed
        txt_playerMoveSpeed.text = Constants.PlayerStats.C_MovementSpeed.ToString();
        slider_playerMoveSpeed.value = Constants.PlayerStats.C_MovementSpeed;

        // Player Wisp Speed
        txt_wispMoveSpeed.text = Constants.PlayerStats.C_WispMovementSpeed.ToString();
        slider_wispMoveSpeed.value = Constants.PlayerStats.C_WispMovementSpeed;

        // Player Health
        txt_playerHealth.text = Constants.PlayerStats.C_MaxHealth.ToString();
        slider_playerHealth.value = Constants.PlayerStats.C_MaxHealth / 50;
        
        // Respawn Rate
        txt_respawnTime.text = Constants.PlayerStats.C_RespawnTimer.ToString();
        slider_respawnTime.value = Constants.PlayerStats.C_RespawnTimer;


        //----------------------------
        // Enemy

        // Enemy Spawn Rate
        txt_enemySpawn.text = Constants.RiftStats.C_VolatilityEnemySpawnTimer.ToString();
        slider_enemySpawn.value = Constants.RiftStats.C_VolatilityEnemySpawnTimer;

        // Enemy Speed
        txt_enemySpeed.text = Constants.EnemyStats.C_EnemyBaseSpeed.ToString();
        slider_enemySpeed.value = Constants.EnemyStats.C_EnemyBaseSpeed;

        // Enemy Health
        txt_enemyHealth.text = Constants.EnemyStats.C_EnemyHealth.ToString();
        slider_enemyHealth.value = Constants.EnemyStats.C_EnemyHealth / 25;

        // Enemy Damage
        txt_enemyDamage.text = Constants.EnemyStats.C_EnemyDamage.ToString();
        slider_enemyDamage.value = Constants.EnemyStats.C_EnemyDamage / 5;

        // Enemy Spawn Cap
        txt_enemySpawnCap.text = Constants.EnemyStats.C_EnemySpawnCapPerSide.ToString();
        slider_enemySpawnCap.value = Constants.EnemyStats.C_EnemySpawnCapPerSide;

        //----------------------------
        // Spell

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
        slider_windForce.value = Constants.SpellStats.C_WindForce / 250;

        // Ice Freeze Duration
        txt_iceFreeze.text = Constants.SpellStats.C_IceFreezeTime.ToString();
        slider_iceFreeze.value = Constants.SpellStats.C_IceFreezeTime;

        // Electric AOE Live-Time
        txt_electricLiveTime.text = Constants.SpellStats.C_ElectricAOELiveTime.ToString();
        slider_electricLiveTime.value = Constants.SpellStats.C_ElectricAOELiveTime;


        //----------------------------
        // Objective

        // CTF Score
        txt_CTFScore.text = Constants.ObjectiveStats.C_CTFMaxScore.ToString();
        slider_CTFScore.value = Constants.ObjectiveStats.C_CTFMaxScore;
        
        // Crystal Health
        txt_crystalHealth.text = Constants.ObjectiveStats.C_CrystalMaxHealth.ToString();
        slider_crystalHealth.value = Constants.ObjectiveStats.C_CrystalMaxHealth / 50;

        // Crystal Health Regen
        txt_crystalHealthRegen.text = Constants.ObjectiveStats.C_CrystalRegenHeal.ToString();
        slider_crystalHealthRegen.value = Constants.ObjectiveStats.C_CrystalRegenHeal;

        // Crystal Health Regen Rate
        txt_crystalHealthRegenRate.text = Constants.ObjectiveStats.C_CrystalHealRate.ToString();
        slider_crystalHealthRegenRate.value = Constants.ObjectiveStats.C_CrystalHealRate;

        // Crystal Health Regen Delay
        txt_crystalHealthRegenDelay.text = Constants.ObjectiveStats.C_CrystalHealDelay.ToString();
        slider_crystalHealthRegenDelay.value = Constants.ObjectiveStats.C_CrystalHealDelay;

        // Hot Potato Completion Timer 
        txt_completionTimer.text = Constants.ObjectiveStats.C_PotatoCompletionTimer.ToString();
        slider_completionTimer.value = Constants.ObjectiveStats.C_PotatoCompletionTimer / 5;

        // Hot Potato Self Destruct Timer
        txt_selfDestructTimer.text = Constants.ObjectiveStats.C_PotatoSelfDestructTimer.ToString();
        slider_selfDestructTimer.value = Constants.ObjectiveStats.C_PotatoSelfDestructTimer / 5;

        // Hockey Max Score
        txt_hockeyMaxScore.text = Constants.ObjectiveStats.C_HockeyMaxScore.ToString();
        slider_hockeyMaxScore.value = Constants.ObjectiveStats.C_HockeyMaxScore;

        // Hockey Puck Damage
        txt_puckDamage.text = Constants.ObjectiveStats.C_PuckDamage.ToString();
        slider_puckDamage.value = Constants.ObjectiveStats.C_PuckDamage / 10;

        // Hockey Puck Base Speed
        txt_puckBaseSpeed.text = Constants.ObjectiveStats.C_PuckBaseSpeed.ToString();
        slider_puckBaseSpeed.value = Constants.ObjectiveStats.C_PuckBaseSpeed;

        // Hockey Puck Hit Increase Speed
        txt_hockeyMaxScore.text = Constants.ObjectiveStats.C_PuckSpeedHitIncrease.ToString();
        slider_hockeyMaxScore.value = Constants.ObjectiveStats.C_PuckSpeedHitIncrease;

        // Hockey Puck Speed Decay Rate
        txt_puckSpeedDecayRate.text = Constants.ObjectiveStats.C_PuckSpeedDecayRate.ToString();
        slider_puckSpeedDecayRate.value = Constants.ObjectiveStats.C_PuckSpeedDecayRate;

        // Hockey Puck Speed Decrease Amount
        txt_puckSpeedDecreaseRate.text = Constants.ObjectiveStats.C_PuckSpeedDecreaseAmount.ToString();
        slider_puckSpeedDecreaseRate.value = Constants.ObjectiveStats.C_PuckSpeedDecreaseAmount;

        // Rift Boss Max Health
        txt_riftBossHealth.text = Constants.ObjectiveStats.C_RiftBossMaxHealth.ToString();
        slider_riftBossHealth.value = Constants.ObjectiveStats.C_RiftBossMaxHealth / 250;

        // Rift Boss Rune Spawn Interval
        txt_runeSpawnInterval.text = Constants.ObjectiveStats.C_RuneSpawnInterval.ToString();
        slider_runeSpawnInterval.value = Constants.ObjectiveStats.C_RuneSpawnInterval;

        // Rift Boss Death Bolt Cooldown
        txt_deathBoltCooldown.text = Constants.ObjectiveStats.C_DeathBoltCooldown.ToString();
        slider_deathBoltCooldown.value = Constants.ObjectiveStats.C_DeathBoltCooldown;

        // Force Field Cooldown
        txt_forceFieldCooldown.text = Constants.ObjectiveStats.C_ForceFieldCooldown.ToString();
        slider_forceFieldCooldown.value = Constants.ObjectiveStats.C_ForceFieldCooldown;

        //---------------------------
        // Organize menu buttons
        butt_buttonArray[0] = butt_playerSelect;
        butt_buttonArray[1] = butt_spellSelect;
        butt_buttonArray[2] = butt_enemySelect;
        butt_buttonArray[3] = butt_objectiveSelect;

        LightUp(0);
        butt_playerSelect.Select();

        // Organize menus
        go_menuArray[0] = go_playerMenu;
        go_menuArray[1] = go_spellMenu;
        go_menuArray[2] = go_enemyMenu;
        go_menuArray[3] = go_objectiveMenu;

        MenuSwitch(0);
    }
}
