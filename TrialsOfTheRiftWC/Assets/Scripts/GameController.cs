using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameController : MonoBehaviour {

    // Public vars
    public Text txt_redScoreText, txt_blueScoreText;
    public Text txt_redScoreText2, txt_blueScoreText2;
    public Text txt_redCompletionTimer, txt_blueCompletionTimer;
    public Text txt_redSelfDestructTimer, txt_blueSelfDestructTimer;
    public GameObject go_canvas;

    //Singleton
    static GameController instance;

	public static GameController GetInstance() {
        return instance;
    }

    // update the score
    public void Score(Constants.Global.Color colorIn, int score) {
        if (colorIn == Constants.Global.Color.RED) {
            txt_redScoreText.text = Mathf.CeilToInt(score).ToString();

        }
        else if (colorIn == Constants.Global.Color.BLUE) {
            txt_blueScoreText.text = Mathf.CeilToInt(score).ToString();
        }
    }

	public void CrystalHealth(Constants.Global.Color colorIn, float health) {
        if (colorIn == Constants.Global.Color.RED) {
            txt_redScoreText2.text = health.ToString();
        }
        else if (colorIn == Constants.Global.Color.BLUE) {
            txt_blueScoreText2.text = health.ToString();
        }
    }

    public void CompletionProgress(Constants.Global.Color colorIn, int time) {
        if (colorIn == Constants.Global.Color.RED)
        {
            txt_redCompletionTimer.text = time.ToString();
        }
        else if (colorIn == Constants.Global.Color.BLUE)
        {
            txt_blueCompletionTimer.text = time.ToString();
        }
    }

    public void SelfDestructProgress(Constants.Global.Color colorIn, int time)
    {
        if (colorIn == Constants.Global.Color.RED)
        {
            txt_redSelfDestructTimer.text = time.ToString();
        }
        else if (colorIn == Constants.Global.Color.BLUE)
        {
            txt_blueSelfDestructTimer.text = time.ToString();
        }
    }

    public void SetConnectMessage(GameObject go_connectMessageIn) {
        //go_connectMessage = go_connectMessageIn;
    }

    public void InitGame() {
        go_canvas.SetActive(false);
        Time.timeScale = 1f;
    }

	public void Params() {
		go_canvas.SetActive(true);
		Time.timeScale = 0f;
	}

	public void Exit() {
		Application.Quit();
	}

	void Awake() {
        if (instance == null) {
            instance = this;
        }

        if (instance != null && instance != this) {
            Debug.Log("Destroying non-primary GC.");
            Destroy(this);
        }

        Time.timeScale = 0;
    }

	void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }
}
