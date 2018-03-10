using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WarmUpPortalController : MonoBehaviour {
	public int i_players = 4; //number of players in the game, should be 4.
	private int i_remainingPlayers; //decreases as players enter the portal
    [SerializeField] private GameObject go_load;
    [SerializeField] private Text txt_loadFade;
	
    void LoadTextFade() {
        txt_loadFade.color = Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time, 0.5f));
    }

	void Start(){
		i_remainingPlayers = i_players;
        Time.timeScale = 1;
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			other.gameObject.SetActive(false);
			i_remainingPlayers--;
			if(i_remainingPlayers <= 0){
                go_load.SetActive(true);
                InvokeRepeating("LoadTextFade", 0.01f, 0.0165f);
                SceneManager.LoadSceneAsync("BuildSetUp");
			}
		}
	}

    private void Update()
    {
        if (Input.GetKeyDown("space")) {
           go_load.SetActive(true);
            InvokeRepeating("LoadTextFade", 0.01f, 0.0165f);
            SceneManager.LoadSceneAsync("BuildSetUp");
        }
    }

}
