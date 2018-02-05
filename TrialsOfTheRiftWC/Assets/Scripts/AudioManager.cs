using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class AudioManager : MonoBehaviour {
	public AudioSource as_bgm;
	public AudioSource as_volatility;
	public AudioSource as_sfx;
	public AudioSource as_voi;
	
	public AudioClip ac_bgm0;
	
	public AudioClip ac_windShoot;
	public AudioClip ac_iceShoot;
	public AudioClip ac_electricShoot;
	public AudioClip ac_magicMissileShoot;
	
	public AudioClip ac_enemyHit;
	public AudioClip ac_enemyDie;
	
	public AudioClip ac_volatility0;
	public AudioClip ac_volatility1;
	public AudioClip ac_volatility2;
	public AudioClip ac_volatility3;
	public AudioClip ac_volatility4;
	
	//Singleton
    static AudioManager instance;


	public static AudioManager Instance{
		get{
			return instance;
		}
	}
	
	void Awake() {
        instance = this;
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
