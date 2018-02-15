/*  Maestro - Noah Nam
 * 
 *  Desc:   Facilitates all in-game music and sound effects.
 * 
 */

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

    [SerializeField] private AudioClip[] ac_volatility;
	
    // Singleton
    private static Maestro instance;
    public static Maestro Instance {
        get { return instance; }
    }

    public void PlayVolatility(int i) {
        as_volatility.clip = ac_volatility[i];
        as_volatility.Play();
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
		as_sfxMe.PlayOneShot(ac_enemyDie);
	}
	public void PlayEnemySpawn(){
		as_sfxMe.PlayOneShot(ac_enemySpawn);
	}
	public void PlayPlayerSpawn(){
		as_sfxMe.PlayOneShot(ac_playerSpawn);
	}
	public void PlayPlayerDie(){
		as_sfxMe.PlayOneShot(ac_playerDie);
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
