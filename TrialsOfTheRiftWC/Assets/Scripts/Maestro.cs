/*  Maestro - Noah Nam
 * 
 *  Desc:   Facilitates all in-game music and sound effects.
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Maestro : MonoBehaviour {
	public AudioSource as_bgm;
	public AudioSource as_volatility;
	public AudioSource as_sfxHi;
	public AudioSource as_sfxMe;
	public AudioSource as_sfxLo;
	public AudioSource as_voi;
	
	public AudioClip ac_bgm0;
	
	public AudioClip ac_windShoot;
	public AudioClip ac_iceShoot;
	public AudioClip ac_electricShoot;
	public AudioClip ac_magicMissileShoot;
	
	public AudioClip ac_enemyHit;
	public AudioClip ac_enemyDie;
	
	public AudioClip ac_playerRespawn;
	public AudioClip ac_playerDie;
	
	public AudioClip ac_portal;
	
	//I think this means I load every sound twice...
	private Dictionary<AudioClip,AudioSource> d_sourceMap;

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
	
	public void Play(AudioClip clip){
		d_sourceMap[clip].PlayOneShot(clip);
	}

    void Awake() {
        instance = this;
		d_sourceMap = new Dictionary<AudioClip,AudioSource>{
			{ac_bgm0,				as_bgm},
			{ac_windShoot,			as_sfxHi},
			{ac_iceShoot,			as_sfxHi},
			{ac_electricShoot,		as_sfxHi},
			{ac_magicMissileShoot,	as_sfxHi},
			{ac_enemyHit,			as_sfxMe},
			{ac_enemyDie,			as_sfxHi},
			{ac_playerRespawn,		as_sfxHi},
			{ac_playerDie,			as_sfxHi},
			{ac_portal,				as_sfxHi},
		};
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
