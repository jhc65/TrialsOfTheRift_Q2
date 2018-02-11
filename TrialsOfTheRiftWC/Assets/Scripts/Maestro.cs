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
	public AudioSource as_sfx;
	public AudioSource as_voi;
	
	public AudioClip ac_bgm0;
	
	public AudioClip ac_windShoot;
	public AudioClip ac_iceShoot;
	public AudioClip ac_electricShoot;
	public AudioClip ac_magicMissileShoot;
	
	public AudioClip ac_enemyHit;
	public AudioClip ac_enemyDie;

    [SerializeField] private AudioClip[] ac_volatility;
	
    // Singleton
    private static Maestro instance;
    public static Maestro Instance {
        get { return instance; }
    }

    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    public void PlayVolatility(int i) {
        as_volatility.clip = ac_volatility[i];
        as_volatility.Play();
    }


    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

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
