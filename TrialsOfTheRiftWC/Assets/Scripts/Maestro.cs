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
using UnityEngine.SceneManagement;

public sealed class Maestro : MonoBehaviour {
    public AudioMixer am_masterMix;

	[Header("Audio Sources")]
	public AudioSource as_bgmA;			// background music audio source
	public AudioSource as_bgmB;			// background music audio source
	public AudioSource as_volatility;	// volatility ambience audio source
	public AudioSource as_sfxHi;		// high priority sound effect audio source
	public AudioSource as_sfxMe;		// medium priority sound effect audio source
	public AudioSource as_sfxLo;		// low priority sound effect audio source
	public AudioSource as_voi;			// voice audio source
	
	[Header("Audio Clips")]
	public AudioClip ac_windShoot;
	public AudioClip ac_iceShoot;
	public AudioClip ac_electricShoot;
	[SerializeField] private AudioClip[] ac_magicMissileShoot;
	public AudioClip ac_contact_generic;
	public AudioClip ac_spell_charge;
	
	public AudioClip ac_enemyHit;
	[SerializeField] private AudioClip[] ac_skeleton_die;
	[SerializeField] private AudioClip[] ac_heavy_skeleton_die;
	[SerializeField] private AudioClip[] ac_necromancer_die;
	[SerializeField] private AudioClip[] ac_skeleton_spawn;
	public AudioClip ac_necromancer_spawn;
	[SerializeField] private AudioClip[] ac_heavy_skeleton_footstep;
	
	public AudioClip ac_playerSpawn;
	public AudioClip ac_playerDie;
	
	public AudioClip ac_portal;
	
	[SerializeField] private AudioClip[] ac_bgm;

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
	[SerializeField] private AudioClip[] ac_board_clear;
	public AudioClip ac_begin_ctf;
	public AudioClip ac_begin_hockey;
	public AudioClip ac_begin_crystal_destruction;
	public AudioClip ac_begin_rift_boss;
	public AudioClip ac_begin_potato;
	public AudioClip ac_tutorial;
	
	[Header("UI")]
	[SerializeField] private AudioClip[] ac_page_turn;
	[SerializeField] private AudioClip[] ac_tap;
	[SerializeField] private AudioClip[] ac_click;
	[SerializeField] private AudioClip[] ac_buzz;
	
	[Header("Settings")]
	public float f_announcementDelay;		// An announcement can only play after this many seconds have elapsed.
	public float f_genericAnnouncementDelay;
	[Range(0,1)] public float f_announcementChance;	// Announcements have a probability of playing.
	private bool b_announcementOk = false;
	private bool b_ctfExplained = false;
	private bool b_hockeyExplained = false;
	private bool b_potatoExplained = false;
	private bool b_destroyExplained = false;
	private bool b_bossExplained = false;
	private bool b_bgmAIsOn = true;	// BGM switches between Sources A and B for crossfade. Which one is the current source we're hearing from?
	private bool b_alreadyFading = false;
	
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
				as_sfxMe.PlayOneShot(ac_volatility_noise_0[r.Next(0, ac_volatility_noise_0.Length)],0.5f);
				break;
			case(1):
				as_sfxMe.PlayOneShot(ac_volatility_noise_1[r.Next(0, ac_volatility_noise_1.Length)],0.5f);
				break;
			case(2):
				as_sfxMe.PlayOneShot(ac_volatility_noise_2[r.Next(0, ac_volatility_noise_2.Length)],0.5f);
				break;
			case(3):
				as_sfxMe.PlayOneShot(ac_volatility_noise_3[r.Next(0, ac_volatility_noise_3.Length)],0.5f);
				break;
			case(4):
				as_sfxMe.PlayOneShot(ac_volatility_noise_4[r.Next(0, ac_volatility_noise_4.Length)],0.5f);
				break;
		}
    }
	
	private void PlaySingle(AudioSource s, AudioClip c){
		s.PlayOneShot(c);
	}
	
	private void PlayRandom(AudioSource s, AudioClip[] c){
		System.Random r = new System.Random();
		s.PlayOneShot(c[r.Next(0, c.Length)]);
	}
	
	private void PlaySingleAnnouncement(AudioClip c){
		System.Random r = new System.Random();
		if(b_announcementOk && r.NextDouble() <= f_announcementChance && !as_voi.isPlaying){
				b_announcementOk = false;
				as_voi.clip = c;
				as_voi.Play();
				Invoke("AnnouncementOk",f_announcementDelay);
		}
	}
	private void PlayRandomAnnouncement(AudioClip[] c){
		System.Random r = new System.Random();
		if(b_announcementOk && r.NextDouble() <= f_announcementChance && !as_voi.isPlaying){
				b_announcementOk = false;
				as_voi.clip = c[r.Next(0, c.Length)];
				as_voi.Play();
				Invoke("AnnouncementOk",f_announcementDelay);
		}
	}
	
	public void PlayWindShoot(){
		PlaySingle(as_sfxHi,ac_windShoot);
	}
	public void PlayIceShoot(){
		PlaySingle(as_sfxHi,ac_iceShoot);
	}
	public void PlayElectricShoot(){
		PlaySingle(as_sfxHi,ac_electricShoot);
	}
	public void PlayMagicMissileShoot(){
		PlayRandom(as_sfxHi,ac_magicMissileShoot);
	}
	public void PlayContactGeneric(){
		PlaySingle(as_sfxHi,ac_contact_generic);
	}
	public void PlaySpellCharge(){
		//PlaySingle(as_sfxHi,ac_spell_charge);
	}
	public void PlayEnemyHit(){
		//PlaySingle(as_sfxLo,ac_enemyHit);
		as_sfxLo.PlayOneShot(ac_enemyHit,0.5f);
	}
	public void PlaySkeletonDie(){
		PlayRandom(as_sfxLo,ac_skeleton_die);
	}
	public void PlayHeavySkeletonDie(){
		PlayRandom(as_sfxLo,ac_heavy_skeleton_die);
	}
	public void PlayNecromancerDie(){
		PlayRandom(as_sfxHi,ac_necromancer_die);
	}
	public void PlayNecromancerSpawn(){
		//PlaySingle(as_sfxHi,ac_necromancer_spawn);
		as_sfxHi.PlayOneShot(ac_necromancer_spawn,1.5f);
	}
	public void PlaySkeletonSpawn(){
		PlayRandom(as_sfxMe,ac_skeleton_spawn);
	}
	public void PlayHeavySkeletonFootstep(){
		PlayRandom(as_sfxLo,ac_heavy_skeleton_footstep);
	}
	public void PlayPlayerSpawn(){
		//PlaySingle(as_sfxHi,ac_playerSpawn);
		as_sfxHi.PlayOneShot(ac_playerSpawn,1.5f);
	}
	public void PlayPlayerDie(){
		//PlaySingle(as_sfxHi,ac_playerDie);
		as_sfxHi.PlayOneShot(ac_playerDie,1.5f);
	}
	public void PlayPortal(){
		PlaySingle(as_sfxHi,ac_portal);
	}
	public void PlayPlayerFootstep(){
		//PlayRandom(as_sfxMe,ac_player_footstep);
	}
	public void PlayPlayerClothing(){
		//PlayRandom(as_sfxLo,ac_player_clothing);
	}
	public void PlayPlayerHit(){
		System.Random r = new System.Random();
		if(r.NextDouble() <= .2f){
				//PlayRandom(as_sfxMe,ac_player_hit);
				as_sfxMe.PlayOneShot(ac_player_hit[r.Next(0, ac_player_hit.Length)],0.4f);
			}
		//PlayRandom(as_sfxMe,ac_player_hit);
	}
	
	public void PlayAnnouncmentPlayerHit(int playerNum, Constants.Global.DamageType d){
		System.Random r = new System.Random();
		if(b_announcementOk && r.NextDouble() <= f_announcementChance && !as_voi.isPlaying){
			b_announcementOk = false;
			switch(d){
				case(Constants.Global.DamageType.ENEMY):
					if(r.NextDouble() <= .2f){
						as_voi.clip = ac_enemy_hit_player[r.Next(0, ac_enemy_hit_player.Length)];
						as_voi.Play();
					}
					break;
				case(Constants.Global.DamageType.RIFT):
					as_voi.clip = ac_rift_hit_player[r.Next(0, ac_rift_hit_player.Length)];
					as_voi.Play();
					break;
				case(Constants.Global.DamageType.WIND):
					as_voi.clip = ac_wind_hit_player[r.Next(0, ac_wind_hit_player.Length)];
					as_voi.Play();
					break;
				case(Constants.Global.DamageType.ICE):
					as_voi.clip = ac_ice_hit_player[r.Next(0, ac_ice_hit_player.Length)];
					as_voi.Play();
					break;
				case(Constants.Global.DamageType.MAGICMISSILE):
				case(Constants.Global.DamageType.ELECTRICITY):
					as_voi.clip = ac_spell_hit_player[r.Next(0, ac_spell_hit_player.Length)];
					as_voi.Play();
					break;
				default:
					as_voi.clip = ac_generic_hit_player[r.Next(0, ac_generic_hit_player.Length)];
					as_voi.Play();
					break;
			}
			Invoke("AnnouncementOk",f_announcementDelay);
		}
	}
	public void PlayAnnouncementGeneric(){
		PlayRandomAnnouncement(ac_generic);
	}
	public void PlayAnnouncementVolatilityUp(){
		PlayRandomAnnouncement(ac_volatility_up);
	}
	public void PlayAnnouncementTrialTransition(){
		PlayRandomAnnouncement(ac_trial_transition);
	}
	public void PlayAnnouncementWispGeneric(){
		PlayRandomAnnouncement(ac_wisp_generic);
	}
	public void PlayAnnouncementTutorial(){
		if(!as_voi.isPlaying){
			b_announcementOk = false;
			as_voi.clip = ac_tutorial;
			as_voi.Play();
			Invoke("AnnouncementOk",f_announcementDelay);
		}
		else
			Invoke("PlayAnnouncementTutorial",1);
	}
	public void PlayAnnouncementIntro(){
		System.Random r = new System.Random();
		if(!as_voi.isPlaying){
			b_announcementOk = false;
			as_voi.clip = ac_intro[r.Next(0, ac_intro.Length)];
			as_voi.Play();
			Invoke("AnnouncementOk",f_announcementDelay);
		}
		else
			Invoke("PlayAnnouncementIntro",1);
	}
	public void PlayAnnouncementBoardClear(){
		PlayRandomAnnouncement(ac_board_clear);
	}
	public void PlayBeginCTF(){
		if(b_ctfExplained)return;
		if(!as_voi.isPlaying){
			b_ctfExplained = true;
			as_voi.clip = ac_begin_ctf;
			as_voi.Play();
		}
		else
			Invoke("PlayBeginCTF",1);
	}
	public void PlayBeginCrystalDestruction(){
		if(b_destroyExplained)return;
		if(!as_voi.isPlaying){
			b_destroyExplained = true;
			as_voi.clip = ac_begin_crystal_destruction;
			as_voi.Play();
		}
		else
			Invoke("PlayBeginCrystalDestruction",1);
	}
	public void PlayBeginHockey(){
		if(b_hockeyExplained)return;
		if(!as_voi.isPlaying){
			b_hockeyExplained = true;
			as_voi.clip = ac_begin_hockey;
			as_voi.Play();
		}
		else
			Invoke("PlayBeginHockey",1);
	}
	public void PlayBeginRiftBoss(){
		if(b_bossExplained)return;
		if(!as_voi.isPlaying){
			b_bossExplained = true;
			as_voi.clip = ac_begin_rift_boss;
			as_voi.Play();
		}
		else
			Invoke("PlayBeginRiftBoss",1);
	}
	public void PlayBeginPotato(){
		if(b_potatoExplained)return;
		if(!as_voi.isPlaying){
			b_potatoExplained = true;
			as_voi.clip = ac_begin_potato;
			as_voi.Play();
		}
		else
			Invoke("PlayBeginPotato",1);
	}
	
	
	public void PlayUIMove(){
		PlayRandom(as_sfxHi,ac_click);
	}
	public void PlayUISubmit(){
		PlayRandom(as_sfxHi,ac_tap);
	}

	// Use this for initialization
	void Start () {
		b_announcementOk = true;
		InvokeRepeating("GenericOk",f_genericAnnouncementDelay,f_genericAnnouncementDelay);
		if(SceneManager.GetActiveScene().name == "WarmUp")
			PlayAnnouncementTutorial();
		/* if(SceneManager.GetActiveScene().name == "BuildSetUp")
			PlayAnnouncementIntro(); */
        am_masterMix.SetFloat("VolumeMaster",Constants.VolOptions.C_MasterVolume);
        am_masterMix.SetFloat("VolumeVOI",Constants.VolOptions.C_VOIVolume);
        am_masterMix.SetFloat("VolumeBGM",Constants.VolOptions.C_BGMVolume);
        am_masterMix.SetFloat("VolumeSFX",Constants.VolOptions.C_SFXVolume);
	}
	
	// Takes volatility level as a parameter.
	public void ChangeBGM(int i){
		AudioSource from, to;
		if(b_bgmAIsOn){
			from = as_bgmA;
			to = as_bgmB;;
		}
		else{
			from = as_bgmB;
			to = as_bgmA;
		}
		
		
		if(!b_alreadyFading){
			to.clip = ac_bgm[i];
			to.timeSamples = from.timeSamples;
			to.volume = 0;
			from.volume = 1;
			to.Play();
			StartCoroutine(Crossfade(from,to));
			b_bgmAIsOn = !b_bgmAIsOn;
		}
		else{
			StartCoroutine("Wait",i);
		}
	}
	
	IEnumerator Crossfade(AudioSource from, AudioSource to){
		b_alreadyFading = true;
		while(to.volume < 1f){
			from.volume -= 0.1f;
			to.volume += 0.1f;
			yield return new WaitForSeconds(.02f);
		}
		from.Stop();
		b_alreadyFading = false;
	}
	
	IEnumerator Wait(int i){
		yield return new WaitForSeconds(1f);
		ChangeBGM(i);
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
