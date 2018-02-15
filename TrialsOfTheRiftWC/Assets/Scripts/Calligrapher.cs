/*  Calligrapher - Sam Caulker
 * 
 *  Desc:   Facilitates UI score updates
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public sealed class Calligrapher : MonoBehaviour {

    public Text txt_redScoreText, txt_blueScoreText;
    public Text txt_redHealthText, txt_blueHealthText;
    public Text txt_redCompletionTimer, txt_blueCompletionTimer;
    public Text txt_redDestructionTimer, txt_blueDestructionTimer;
    public Text txt_redObjvTitle, txt_blueObjvTitle;
    public Text txt_redObjvDescription, txt_blueObjvDescription;
    public Image img_redPopupBacking, img_bluePopupBacking;

    private float f_redStartTime, f_blueStartTime;  // controls UI pop-up fading

    // Singleton
    private static Calligrapher instance;
    public static Calligrapher Instance {
        get { return instance; }
    }

    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    // update score (CTF and Ice Hockey)
    public void UpdateScoreUI(Constants.Global.Color colorIn, int scoreIn) {
        if (colorIn == Constants.Global.Color.RED) {
            txt_redScoreText.text = scoreIn.ToString();
        }
        else if (colorIn == Constants.Global.Color.BLUE) {
            txt_blueScoreText.text = scoreIn.ToString();
        }
    }

    // update health (Crystal Destruction)
	public void UpdateCrystalHealthUI(Constants.Global.Color colorIn, float healthIn) {
        if (colorIn == Constants.Global.Color.RED) {
            txt_redHealthText.text = Mathf.CeilToInt(healthIn).ToString();
        }
        else if (colorIn == Constants.Global.Color.BLUE) {
            txt_blueHealthText.text = Mathf.CeilToInt(healthIn).ToString();
        }
    }

    // update health (Rift Boss)
	public void UpdateRiftBossHealthUI(Constants.Global.Color colorIn, float healthIn) {
        if (colorIn == Constants.Global.Color.RED) {
            txt_redHealthText.text = Mathf.CeilToInt(healthIn).ToString();  //TODO: new ui txt objects
        }
        else if (colorIn == Constants.Global.Color.BLUE) {
            txt_blueHealthText.text = Mathf.CeilToInt(healthIn).ToString();
        }
    }

    // update completion timer (Hot Potato)
    public void UpdateCompletionTimerUI(Constants.Global.Color colorIn, int time) {
        if (colorIn == Constants.Global.Color.RED) {
            txt_redCompletionTimer.text = time.ToString();
        }
        else if (colorIn == Constants.Global.Color.BLUE) {
            txt_blueCompletionTimer.text = time.ToString();
        }
    }

    // update destruction timer (Hot Potato)
    public void UpdateDestructionTimerUI(Constants.Global.Color colorIn, int time) {
        if (colorIn == Constants.Global.Color.RED) {
            txt_redDestructionTimer.text = time.ToString();
        }
        else if (colorIn == Constants.Global.Color.BLUE) {
            txt_blueDestructionTimer.text = time.ToString();
        }
    }

    //----------------------------
    // Initialization of different objectives
    public void CTFInit(Constants.Global.Color colorIn) {
        if (colorIn == Constants.Global.Color.RED) {
            txt_redScoreText.transform.parent.gameObject.SetActive(true);
            txt_redObjvTitle.text = "Capture The Flag";
            txt_redObjvDescription.text = "Pick up the opponent's flag with [Interact] and drag it back to your goal! " + Constants.ObjectiveStats.C_CTFMaxScore + " Goals wins!";
        }
        else {
            txt_blueScoreText.transform.parent.gameObject.SetActive(true);
            txt_blueObjvTitle.text = "Capture The Flag";
            txt_blueObjvDescription.text = "Pick up the opponent's flag with [Interact] and drag it back to your goal! " + Constants.ObjectiveStats.C_CTFMaxScore + " Goals wins!";
        }
        UpdateScoreUI(colorIn, 0);
        PopupFadeIn(colorIn);
    }

    public void IceHockeyInit(Constants.Global.Color colorIn) {
        if (colorIn == Constants.Global.Color.RED) {
            txt_redScoreText.transform.parent.gameObject.SetActive(true);
            txt_redObjvTitle.text = "Ice Hockey";
            txt_redObjvDescription.text = "Shoot and parry your puck into the enemy's goal! Careful, you can't score from behind! " + Constants.ObjectiveStats.C_HockeyMaxScore + " Goals wins!";
        }
        else {
            txt_blueScoreText.transform.parent.gameObject.SetActive(true);
            txt_blueObjvTitle.text = "Ice Hockey";
            txt_blueObjvDescription.text = "Shoot and parry your puck into the enemy's goal! Careful, you can't score from behind! " + Constants.ObjectiveStats.C_HockeyMaxScore + " Goals wins!";
        }
        UpdateScoreUI(colorIn, 0);
        PopupFadeIn(colorIn);
    }

    public void CrystalDestructionInit(Constants.Global.Color colorIn) {
        // colorIn will be crystal color, not objective/team color
        if (colorIn == Constants.Global.Color.RED) {
            txt_redHealthText.transform.parent.gameObject.SetActive(true);
            txt_redObjvTitle.text = "Crystal Destruction";
            txt_redObjvDescription.text = "Cast spells at the enemy team's crystal to destroy it! Heal your own crystal with your own spells!";
            PopupFadeIn(Constants.Global.Color.RED);
        }
        else {
            txt_blueHealthText.transform.parent.gameObject.SetActive(true);
            txt_blueObjvTitle.text = "Crystal Destruction";
            txt_blueObjvDescription.text = "Cast spells at the enemy team's crystal to destroy it! Heal your own crystal with your own spells!";
            PopupFadeIn(Constants.Global.Color.BLUE);
        }
        UpdateCrystalHealthUI(colorIn, Constants.ObjectiveStats.C_CrystalMaxHealth);
    }

    public void HotPotatoInit(Constants.Global.Color colorIn) {
        if (colorIn == Constants.Global.Color.RED) {
            txt_redCompletionTimer.transform.parent.gameObject.SetActive(true);
            txt_redDestructionTimer.transform.parent.gameObject.SetActive(true);
            txt_redObjvTitle.text = "Reverse Capture the Flag";
            txt_redObjvDescription.text = "Shove your flag onto the opponent's side and keep it there. Be careful, if you leave yours on your side for too long, bad things will happen!";
        }
        else {
            txt_blueCompletionTimer.transform.parent.gameObject.SetActive(true);
            txt_blueDestructionTimer.transform.parent.gameObject.SetActive(true);
            txt_blueObjvTitle.text = "Reverse Capture the Flag";
            txt_blueObjvDescription.text = "Shove your flag onto the opponent's side and keep it there. Be careful, if you leave yours on your side for too long, bad things will happen!";
        }
        UpdateCompletionTimerUI(colorIn, Constants.ObjectiveStats.C_PotatoCompletionTimer);
        UpdateDestructionTimerUI(colorIn, Constants.ObjectiveStats.C_PotatoSelfDestructTimer);
        PopupFadeIn(colorIn);
    }

    public void RiftBossInit(Constants.Global.Color colorIn) {
        if (colorIn == Constants.Global.Color.RED) {
            txt_redHealthText.transform.parent.gameObject.SetActive(true);
            txt_redObjvTitle.text = "Rift Boss";
            txt_redObjvDescription.text = "The final Boss!  Wait for its shield to go down to attack!";
            PopupFadeIn(Constants.Global.Color.BLUE);
        }
        else {
            txt_blueHealthText.transform.parent.gameObject.SetActive(true);
            txt_blueObjvTitle.text = "Rift Boss";
            txt_blueObjvDescription.text = "TThe final Boss!  Wait for its shield to go down to attack!";
            PopupFadeIn(Constants.Global.Color.RED);
        }
        UpdateRiftBossHealthUI(colorIn, Constants.ObjectiveStats.C_RiftBossMaxHealth);
    }

    //----------------------------
    // Reset of different UI objects
    public void ScoreReset(Constants.Global.Color colorIn) {
        if (colorIn == Constants.Global.Color.RED) {
            txt_redScoreText.transform.parent.gameObject.SetActive(false);
        }
        else {
            txt_blueScoreText.transform.parent.gameObject.SetActive(false);
        }
    }

    public void CrystalDestructionReset(Constants.Global.Color colorIn) {
        if (colorIn == Constants.Global.Color.RED) {
            txt_redHealthText.transform.parent.gameObject.SetActive(false);
        }
        else {
            txt_blueHealthText.transform.parent.gameObject.SetActive(false);
        }
    }

    public void HotPotatoReset(Constants.Global.Color colorIn) {
        if (colorIn == Constants.Global.Color.RED) {
            txt_redCompletionTimer.transform.parent.gameObject.SetActive(false);
            txt_redDestructionTimer.transform.parent.gameObject.SetActive(false);
        }
        else {
            txt_blueCompletionTimer.transform.parent.gameObject.SetActive(false);
            txt_blueDestructionTimer.transform.parent.gameObject.SetActive(false);
        }
    }

    public void RiftBossReset(Constants.Global.Color colorIn) {
        if (colorIn == Constants.Global.Color.RED) {
            txt_redHealthText.transform.parent.gameObject.SetActive(false); //TODO: new ui txt objects
        }
        else {
            txt_blueHealthText.transform.parent.gameObject.SetActive(false);
        }
    }

    //----------------------------
    // Fade in/out objective description UI at the start of each objective
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
        if (timer > 5f) {
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
        if (timer > 5f) {
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

    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    void Awake() {
        instance = this;
    }
}
