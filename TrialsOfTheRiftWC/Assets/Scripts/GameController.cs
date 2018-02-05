using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameController : MonoBehaviour {

    // Public vars
    public Text txt_redScoreText, txt_blueScoreText;
    public Text txt_redHealthCounter, txt_blueHealthCounter;
    public Text txt_redCompletionTimer, txt_blueCompletionTimer;
    public Text txt_redSelfDestructTimer, txt_blueSelfDestructTimer;
    public Text txt_redObjvTitle, txt_blueObjvTitle;
    public Text txt_redObjvDescription, txt_blueObjvDescription;
    public Image img_redPopupBacking, img_bluePopupBacking;
    private float f_redStartTime, f_blueStartTime;
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
            txt_redHealthCounter.text = health.ToString();
        }
        else if (colorIn == Constants.Global.Color.BLUE) {
            txt_blueHealthCounter.text = health.ToString();
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

    public void PopupFadeIn(Constants.Global.Color e_color) {
        if (e_color == Constants.Global.Color.RED) {
            f_redStartTime = Time.time;
            InvokeRepeating("FadeInRed", 0.1f, 0.075f);
        } else {
            f_blueStartTime = Time.time;
            InvokeRepeating("FadeInBlue", 0.1f, 0.075f);
        }
    }

    public void PopupFadeOut(Constants.Global.Color e_color) {
        if (e_color == Constants.Global.Color.RED) {
            f_redStartTime = Time.time;
            InvokeRepeating("FadeOutRed", 0.1f, 0.075f);
        } else {
            f_blueStartTime = Time.time;
            InvokeRepeating("FadeOutBlue", 0.1f, 0.075f);
        }
    }

    private void FadeInRed() {
        float timer = (Time.time - f_redStartTime);
        float fracJourney = timer / 1f;
        img_redPopupBacking.color = Color.Lerp(img_redPopupBacking.color, new Color(0,0,0,0.2f), fracJourney);
        txt_redObjvTitle.color = Color.Lerp(txt_redObjvTitle.color, new Color(1,1,1,1), fracJourney);
        txt_redObjvDescription.color = Color.Lerp(txt_redObjvDescription.color, new Color(1,1,1,1), fracJourney);
        if (timer > 2f) {
            CancelInvoke("FadeInRed");
            PopupFadeOut(Constants.Global.Color.RED);
        }
    }

    private void FadeInBlue() {
        float timer = (Time.time - f_blueStartTime);
        float fracJourney = timer / 1f;
        img_bluePopupBacking.color = Color.Lerp(img_bluePopupBacking.color, new Color(0,0,0,0.2f), fracJourney);
        txt_blueObjvTitle.color = Color.Lerp(txt_blueObjvTitle.color, new Color(1,1,1,1), fracJourney);
        txt_blueObjvDescription.color = Color.Lerp(txt_blueObjvDescription.color, new Color(1,1,1,1), fracJourney);
        if (timer > 2f) {
            CancelInvoke("FadeInBlue");
            PopupFadeOut(Constants.Global.Color.BLUE);
        }
    }

    private void FadeOutRed() {
        float timer = (Time.time - f_redStartTime);
        float fracJourney = timer / 1f;
        img_redPopupBacking.color = Color.Lerp(img_redPopupBacking.color, new Color(0,0,0,0), fracJourney);
        txt_redObjvTitle.color = Color.Lerp(txt_redObjvTitle.color, new Color(1,1,1,0), fracJourney);
        txt_redObjvDescription.color = Color.Lerp(txt_redObjvDescription.color, new Color(1,1,1,0), fracJourney);
        if (timer > 2f) {
            CancelInvoke("FadeOutRed");
        }
    }

    private void FadeOutBlue() {
        float timer = (Time.time - f_blueStartTime);
        float fracJourney = timer / 1f;
        img_bluePopupBacking.color = Color.Lerp(img_bluePopupBacking.color, new Color(0,0,0,0), fracJourney);
        txt_blueObjvTitle.color = Color.Lerp(txt_blueObjvTitle.color, new Color(1,1,1,0), fracJourney);
        txt_blueObjvDescription.color = Color.Lerp(txt_blueObjvDescription.color, new Color(1,1,1,0), fracJourney);
        if (timer > 2f) {
            CancelInvoke("FadeOutBlue");
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
