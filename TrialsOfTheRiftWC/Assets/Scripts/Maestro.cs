/*  Maestro - Noah Nam
 * 
 *  Desc:   Facilitates all in-game music and sound effects.
 * 
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public sealed class Maestro : MonoBehaviour {
    public AudioMixer am_masterMix;

	[Header("Audio Sources")]
	public AudioSource as_bgm;			// background music audio source
	public AudioSource as_volatility;	// volatility ambience audio source
	public AudioSource as_sfxHi;		// high priority sound effect audio source
	public AudioSource as_sfxMe;		// medium priority sound effect audio source
	public AudioSource as_sfxLo;		// low priority sound effect audio source
	public AudioSource as_voi;			// voice audio source
	
	[Header("Audio Clips")]
	public AudioClip ac_bgm0;
	
	public AudioClip ac_windShoot;
	public AudioClip ac_iceShoot;
	public AudioClip ac_electricShoot;
	public AudioClip ac_magicMissileShoot;
	
	public AudioClip ac_enemyHit;
	public AudioClip ac_enemyDie;
	public AudioClip ac_enemySpawn;
	
	public AudioClip ac_playerSpawn;
	public AudioClip ac_playerDie;
	
	public AudioClip ac_portal;

    [SerializeField] private AudioClip[] ac_volatility_ambience;
	[SerializeField] private AudioClip[] ac_volatility_noise_0;
	[SerializeField] private AudioClip[] ac_volatility_noise_1;
	[SerializeField] private AudioClip[] ac_volatility_noise_2;
	[SerializeField] private AudioClip[] ac_volatility_noise_3;
	[SerializeField] private AudioClip[] ac_volatility_noise_4;
	[SerializeField] private AudioClip[] ac_player_footstep;
	[SerializeField] private AudioClip[] ac_player_hit;
	[SerializeField] private AudioClip[] ac_player_clothing;
	
	[Header("Audio Clips (Announcer)")]
	[SerializeField] private AudioClip[] ac_generic;
	[SerializeField] private AudioClip[] ac_volatility_up;
	[SerializeField] private AudioClip[] ac_trial_transition;
	[SerializeField] private AudioClip[] ac_wisp_generic;
	[SerializeField] private AudioClip[] ac_intro;
	[SerializeField] private AudioClip[] ac_enemy_hit_player;
	[SerializeField] private AudioClip[] ac_rift_hit_player;
	[SerializeField] private AudioClip[] ac_wind_hit_player;
	[SerializeField] private AudioClip[] ac_ice_hit_player;
	[SerializeField] private AudioClip[] ac_spell_hit_player;
	[SerializeField] private AudioClip[] ac_generic_hit_player;
	
	[Header("Settings")]
	public float f_announcementDelay;		// An announcement can only play after this many seconds have elapsed.
	public float f_genericAnnouncementDelay;
	[Range(0,1)] public float f_announcementChance;	// Announcements have a probability of playing.
	private bool b_announcementOk;
	
    // Singleton
    private static Maestro instance;
	
	void Awake(){
		instance = this;
	}
	
    public static Maestro Instance {
        get { return instance; }
    }

    public void PlayVolatilityAmbience(int i) {
        as_volatility.clip = ac_volatility_ambience[i];
        as_volatility.Play();
    }
	
	public void PlayVolatilityNoise(int i) {
		System.Random r = new System.Random();
		switch(i){
			case(0):
				as_sfxLo.PlayOneShot(ac_volatility_noise_0[r.Next(0, ac_volatility_noise_0.Length)]);
				break;
			case(1):
				as_sfxLo.PlayOneShot(ac_volatility_noise_1[r.Next(0, ac_volatility_noise_1.Length)]);
				break;
			case(2):
				as_sfxLo.PlayOneShot(ac_volatility_noise_2[r.Next(0, ac_volatility_noise_2.Length)]);
				break;
			case(3):
				as_sfxLo.PlayOneShot(ac_volatility_noise_3[r.Next(0, ac_volatility_noise_3.Length)]);
				break;
			case(4):
				as_sfxLo.PlayOneShot(ac_volatility_noise_4[r.Next(0, ac_volatility_noise_4.Length)]);
				break;
		}
    }
	
	public void PlayWindShoot(){
		as_sfxMe.PlayOneShot(ac_windShoot);
	}
	public void PlayIceShoot(){
		as_sfxMe.PlayOneShot(ac_iceShoot);
	}
	public void PlayElectricShoot(){
		as_sfxMe.PlayOneShot(ac_electricShoot);
	}
	public void PlayMagicMissileShoot(){
		as_sfxMe.PlayOneShot(ac_magicMissileShoot);
	}
	public void PlayEnemyHit(){
		as_sfxMe.PlayOneShot(ac_enemyHit);
	}
	public void PlayEnemyDie(){
		as_sfxHi.PlayOneShot(ac_enemyDie);
	}
	public void PlayEnemySpawn(){
		as_sfxHi.PlayOneShot(ac_enemySpawn);
	}
	public void PlayPlayerSpawn(){
		as_sfxHi.PlayOneShot(ac_playerSpawn);
	}
	public void PlayPlayerDie(){
		as_sfxHi.PlayOneShot(ac_playerDie);
	}
	public void PlayPortal(){
		as_sfxMe.PlayOneShot(ac_portal);
	}
	public void PlayPlayerFootstep(){
		System.Random r = new System.Random();
		as_sfxLo.PlayOneShot(ac_player_footstep[r.Next(0, ac_player_footstep.Length)]);
	}
	public void PlayPlayerClothing(){
		System.Random r = new System.Random();
		as_sfxLo.PlayOneShot(ac_player_clothing[r.Next(0, ac_player_clothing.Length)]);
	}
	public void PlayPlayerHit(){
		System.Random r = new System.Random();
		as_sfxMe.PlayOneShot(ac_player_hit[r.Next(0, ac_player_hit.Length)]);
	}
	
	public void PlayAnnouncmentPlayerHit(int playerNum, Constants.Global.DamageType d){
		System.Random r = new System.Random();
		if(b_announcementOk && r.NextDouble() <= f_announcementChance){
			b_announcementOk = false;
			switch(d){
				case(Constants.Global.DamageType.ENEMY):
					as_voi.PlayOneShot(ac_enemy_hit_player[r.Next(0, ac_enemy_hit_player.Length)]);
					break;
				case(Constants.Global.DamageType.RIFT):
					as_voi.PlayOneShot(ac_rift_hit_player[r.Next(0, ac_rift_hit_player.Length)]);
					break;
				case(Constants.Global.DamageType.WIND):
					as_voi.PlayOneShot(ac_wind_hit_player[r.Next(0, ac_wind_hit_player.Length)]);
					break;
				case(Constants.Global.DamageType.ICE):
					as_voi.PlayOneShot(ac_ice_hit_player[r.Next(0, ac_ice_hit_player.Length)]);
					break;
				case(Constants.Global.DamageType.MAGICMISSILE):
				case(Constants.Global.DamageType.ELECTRICITY):
					as_voi.PlayOneShot(ac_spell_hit_player[r.Next(0, ac_spell_hit_player.Length)]);
					break;
				default:
					as_voi.PlayOneShot(ac_generic_hit_player[r.Next(0, ac_generic_hit_player.Length)]);
					break;
			}
			Invoke("AnnouncementOk",f_announcementDelay);
		}
	}
	public void PlayAnnouncementGeneric(){
		System.Random r = new System.Random();
		if(b_announcementOk && r.NextDouble() <= f_announcementChance){
				b_announcementOk = false;
				as_voi.PlayOneShot(ac_generic[r.Next(0, ac_generic.Length)]);
				Invoke("AnnouncementOk",f_announcementDelay);
			}
	}
	public void PlayAnnouncementVolatilityUp(){
		System.Random r = new System.Random();
		if(b_announcementOk && r.NextDouble() <= f_announcementChance){
				b_announcementOk = false;
				as_voi.PlayOneShot(ac_volatility_up[r.Next(0, ac_volatility_up.Length)]);
				Invoke("AnnouncementOk",f_announcementDelay);
			}
	}
	public void PlayAnnouncementTrialTransition(){
		System.Random r = new System.Random();
		if(b_announcementOk && r.NextDouble() <= f_announcementChance){
				b_announcementOk = false;
				as_voi.PlayOneShot(ac_trial_transition[r.Next(0, ac_trial_transition.Length)]);
				Invoke("AnnouncementOk",f_announcementDelay);
			}
	}
	public void PlayAnnouncementWispGeneric(){
		System.Random r = new System.Random();
		if(b_announcementOk && r.NextDouble() <= f_announcementChance){
				b_announcementOk = false;
				as_voi.PlayOneShot(ac_wisp_generic[r.Next(0, ac_wisp_generic.Length)]);
				Invoke("AnnouncementOk",f_announcementDelay);
			}
	}
	
	public void PlayAnnouncementIntro(){
		System.Random r = new System.Random();
		if(b_announcementOk){
				b_announcementOk = false;
				as_voi.PlayOneShot(ac_intro[r.Next(0, ac_intro.Length)]);
				Invoke("AnnouncementOk",f_announcementDelay);
			}
	}

	// Use this for initialization
	void Start () {
		b_announcementOk = true;
		InvokeRepeating("GenericOk",f_genericAnnouncementDelay,f_genericAnnouncementDelay);
		PlayAnnouncementIntro();
		as_bgm.clip = ac_bgm0;
		as_bgm.Play();
        am_masterMix.SetFloat("VolumeMaster",Constants.VolOptions.C_MasterVolume);
        am_masterMix.SetFloat("VolumeVOI",Constants.VolOptions.C_VOIVolume);
        am_masterMix.SetFloat("VolumeBGM",Constants.VolOptions.C_BGMVolume);
        am_masterMix.SetFloat("VolumeSFX",Constants.VolOptions.C_SFXVolume);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	private void AnnouncementOk(){
		b_announcementOk = true;
	}
	
	private void GenericOk(){
		PlayAnnouncementGeneric();
	}

    public void AdjustMasterVolume(float f_volIn) {
		Constants.VolOptions.C_MasterVolume = f_volIn;
        am_masterMix.SetFloat("VolumeMaster",20*Mathf.Log(Constants.VolOptions.C_MasterVolume,10));
    }

    public void AdjustBGMVolume(float f_volIn) {
        Constants.VolOptions.C_BGMVolume = f_volIn;
        am_masterMix.SetFloat("VolumeBGM",20*Mathf.Log(Constants.VolOptions.C_BGMVolume,10));
    }

    public void AdjustSFXVolume(float f_volIn) {
        Constants.VolOptions.C_SFXVolume = f_volIn;
        am_masterMix.SetFloat("VolumeSFX",20*Mathf.Log(Constants.VolOptions.C_SFXVolume,10));
    }

    public void AdjustVOIVolume(float f_volIn) {
        Constants.VolOptions.C_VOIVolume = f_volIn;
        am_masterMix.SetFloat("VolumeVOI",20*Mathf.Log(Constants.VolOptions.C_VOIVolume,10));
    }
}
