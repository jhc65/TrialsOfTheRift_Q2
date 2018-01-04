using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants {
    // Player Constants
    public static class PlayerStats {
        public static float C_MovementSpeed = 13.0f;
        public static float C_WispMovementSpeed = 2.0f;
        public static float C_RespawnTimer = 5.0f;
        public static int C_MaxHealth = 300;
    }

    // Spell Constants
    public static class SpellStats {
		public static float C_SpellLiveTime = 2.0f;
		public static float C_MagicMissileSpeed = 20.0f;
        public static float C_IceSpeed = 25.0f;
        public static float C_WindSpeed = 25.0f;
		public static float C_ElectricSpeed = 25.0f;
		public static int C_MagicMissileDamage = 50;       // Currently want all spells, including MM, to kill enemies in 2 hits
        public static int C_IceDamage = 50;
        public static int C_WindDamage = 50;
		public static int C_ElectricDamage = 10;		// less because this value is repeatedly applied while enemies are in the AOE
		public static float C_MagicMissileCooldown = 0.25f;
		public static float C_IceCooldown = 5.0f;
		public static float C_WindCooldown = 2.0f;
		public static float C_ElectricCooldown = 8.0f;
		public static float C_WindForce = 6000.0f;                // [Param Fix]
        public static float C_IceFreezeTime = 2.0f;               // [Param Fix]
		public static float C_ElectricAOELiveTime = 3.0f;
        public static float C_NextSpellDelay = 0.5f;			// this is separate from any of the other cooldowns
        public static float C_PlayerProjectileSize = 0.75f;

		// Spell Buffs when crossing Rift
		//public static float C_IceSpeedMultiplier = 1.5f;
        //public static float C_WindSpeedMultiplier = 1.5f;
		//public static float C_ElectricSpeedMultiplier = 1.5f;
		public static int C_IceDamageMultiplier = 2;
        public static int C_WindDamageMultiplier = 2;
		public static int C_ElectricDamageMultiplier = 2;
		public static float C_SpellScaleMultiplier = 1.15f;

        //Crystal based percentages
        public static float C_SpellCrystalDamagePercent = -0.1f;
        public static float C_SpellCrystalHealPercent = 0.05f;
        public static float C_MMCrystalDamagePercent = -0.01f;
        public static float C_MMCrystalHealPercent = 0.005f;
    }
       
    // Non-Player Constants.
    // Deals with Objectives and Enemies.
    public static class EnviroStats {
        public static float C_EnemySpawnTime = 7.0f;             
        public static float C_EnemySpeed = 3.5f;
		public static float C_EnemyAttackRange = 1.5f;
        public static int C_EnemyHealth = 100;
        public static int C_EnemyDamage = 25;
        public static int C_CrystalMaxHealth = 500;
        public static int C_CTFMaxScore = 3;
        public static int C_CompletionTimer = 30;       //tracks progress of objective being in enemy territory
        public static int C_SelfDestructThreshold = 10; //threshold for objective to disappear and spawn enemies
    }

    public static class RiftStats {
        public static float C_Volatility_ResetTime = 5.0f;
        public static float C_Volatility_CameraFlipTime = 5.0f;
        public static float C_Volatility_EnemySpawnTimer = 7.0f;
        public static float C_Volatility_SpellDamage = 50.0f;

        public static float C_VolatilityIncrease_RoomAdvance = 5.0f;
        public static float C_VolatilityIncrease_SpellCross = 0.5f;
        public static float C_VolatilityIncraese_PlayerDeath = 2.5f;

        public static float C_VolatilityMultiplier_L1 = 0.0f;
        public static float C_VolatilityMultiplier_L2 = 0.2f;
        public static float C_VolatilityMultiplier_L3 = 0.5f;
        public static float C_VolatilityMultiplier_L4 = 1.0f;

        public enum Volatility { ZERO, FIVE, TWENTYFIVE, THIRTYFIVE, FIFTY, SIXTYFIVE, SEVENTYFIVE, ONEHUNDRED };
    }

    // Team Constants
	public enum Color { RED, BLUE };
	public enum Side { LEFT = -1, RIGHT = 1 };
	public static Vector3 C_RedObjectiveSpawn = new Vector3(20.0f, 0.5f, 0f);	//i.e. the Blue Flag Red Players are trying to retrieve
	public static Vector3 C_BlueObjectiveSpawn = new Vector3(-20.0f, 0.5f, 0f);
    public static Vector3 C_RedHotCrystalSpawn = new Vector3(-3.0f, 0.5f, 0f);
    public static Vector3 C_BlueHotCrystalSpawn = new Vector3(3.0f, 0.5f, 0f);
    public static Vector3 C_RedObjectiveGoal = new Vector3(-3.0f, .01f, 0f);	//i.e. Red's base the Blue's Flag must be returned to
	public static Vector3 C_BlueObjectiveGoal = new Vector3(3.0f, .01f, 0f);
	public static GameObject[] C_Players = GameObject.FindGameObjectsWithTag("Player");
}
