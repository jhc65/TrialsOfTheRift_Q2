/*  Maestro - Noah Nam
 * 
 *  Desc:   Facilitates all in-game music and sound effects.
 * 
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Maestro : MonoBehaviour {
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

	// Use this for initialization
	void Start () {
		as_bgm.clip = ac_bgm0;
		as_bgm.Play();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
