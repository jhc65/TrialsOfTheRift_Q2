using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarmUpPortalController : MonoBehaviour {
	public int i_players = 4; //number of players in the game, should be 4.
	private int i_remainingPlayers; //decreases as players enter the portal
	
	void Start(){
		i_remainingPlayers = i_players;
        Time.timeScale = 1;
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			other.gameObject.SetActive(false);
			i_remainingPlayers--;
			if(i_remainingPlayers <= 0){
				SceneManager.LoadScene("BuildSetUp");
			}
		}
	}

    private void Update()
    {
        if (Input.GetKeyDown("space")) {
            SceneManager.LoadScene("BuildSetUp");
        }
    }

}
