/*  Calligrapher - Sam Caulker
 * 
 *  Desc:   Facilitates UI and score updates
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public sealed class Calligrapher : MonoBehaviour {

    [SerializeField] private Text txt_redScoreText, txt_blueScoreText;
    [SerializeField] private Text txt_redCrystalHealthText, txt_blueCrystalHealthText;
    [SerializeField] private Text txt_redRiftBossHealthText, txt_blueRiftBossHealthText;
    [SerializeField] private Text txt_redCompletionTimer, txt_blueCompletionTimer;
    [SerializeField] private Text txt_redDestructionTimer, txt_blueDestructionTimer;

    [SerializeField] private Text txt_redObjvTitle, txt_blueObjvTitle;
    [SerializeField] private Text txt_redObjvDescription, txt_blueObjvDescription;
    [SerializeField] private Text txt_redPauseObjvTitle, txt_bluePauseObjvTitle;
    [SerializeField] private Text txt_redPauseObjvDescription, txt_bluePauseObjvDescription;
 
    [SerializeField] private Image img_redPopupBacking, img_bluePopupBacking;
    [SerializeField] private Image img_redFlashBacking, img_blueFlashBacking;
    [SerializeField] private Text  txt_redRoomCounter, txt_blueRoomCounter;

    private float f_redStartTime, f_blueStartTime;  // controls UI pop-up fading
    private float f_redFlashTime, f_blueFlashTime;  // separate timers for flash to avoid overwriting, since both animations play at roughly the same time.

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
            txt_redCrystalHealthText.text = Mathf.CeilToInt(healthIn).ToString();
        }
        else if (colorIn == Constants.Global.Color.BLUE) {
            txt_blueCrystalHealthText.text = Mathf.CeilToInt(healthIn).ToString();
        }
    }

    // update health (Rift Boss)
	public void UpdateRiftBossHealthUI(Constants.Global.Color colorIn, float healthIn) {
        if (colorIn == Constants.Global.Color.RED) {
            txt_redRiftBossHealthText.text = Mathf.CeilToInt(healthIn).ToString();  //TODO: new ui txt objects
        }
        else if (colorIn == Constants.Global.Color.BLUE) {
            txt_blueRiftBossHealthText.text = Mathf.CeilToInt(healthIn).ToString();
        }
    }

    // update completion timer (Hot Potato)
    public void UpdateCompletionTimerUI(Constants.Global.Color colorIn, int timeIn) {
        if (colorIn == Constants.Global.Color.RED) {
            txt_redCompletionTimer.text = timeIn.ToString();
        }
        else if (colorIn == Constants.Global.Color.BLUE) {
            txt_blueCompletionTimer.text = timeIn.ToString();
        }
    }

    // update destruction timer (Hot Potato)
    public void UpdateDestructionTimerUI(Constants.Global.Color colorIn, int timeIn) {
        if (colorIn == Constants.Global.Color.RED) {
            txt_redDestructionTimer.text = timeIn.ToString();
        }
        else if (colorIn == Constants.Global.Color.BLUE) {
            txt_blueDestructionTimer.text = timeIn.ToString();
        }
    }

    //----------------------------
    // Initialization of different objectives
    public void CTFInit(Constants.Global.Color colorIn) {
        if (colorIn == Constants.Global.Color.RED) {
            txt_redScoreText.transform.parent.gameObject.SetActive(true);
            txt_redObjvTitle.text = txt_redPauseObjvTitle.text = Constants.ObjectiveText.C_CTFTitle;
            txt_redObjvDescription.text = txt_redPauseObjvDescription.text = Constants.ObjectiveText.C_CTFDescription;
        }
        else {
            txt_blueScoreText.transform.parent.gameObject.SetActive(true);
            txt_blueObjvTitle.text = txt_bluePauseObjvTitle.text = Constants.ObjectiveText.C_CTFTitle;
            txt_blueObjvDescription.text = txt_bluePauseObjvDescription.text = Constants.ObjectiveText.C_CTFDescription;
        }
        UpdateScoreUI(colorIn, 0);
        PopupFadeIn(colorIn);
    }

    public void IceHockeyInit(Constants.Global.Color colorIn) {
        if (colorIn == Constants.Global.Color.RED) {
            txt_redScoreText.transform.parent.gameObject.SetActive(true);
            txt_redObjvTitle.text = txt_redPauseObjvTitle.text = Constants.ObjectiveText.C_HockeyTitle;
            txt_redObjvDescription.text = txt_redPauseObjvDescription.text = Constants.ObjectiveText.C_HockeyDescription;
        }
        else {
            txt_blueScoreText.transform.parent.gameObject.SetActive(true);
            txt_blueObjvTitle.text = txt_bluePauseObjvTitle.text = Constants.ObjectiveText.C_HockeyTitle;
            txt_blueObjvDescription.text = txt_bluePauseObjvDescription.text = Constants.ObjectiveText.C_HockeyDescription;
        }
        UpdateScoreUI(colorIn, 0);
        PopupFadeIn(colorIn);
    }

    public void CrystalDestructionInit(Constants.Global.Color colorIn) {
        // colorIn will be crystal color, not objective/team color
        if (colorIn == Constants.Global.Color.RED) {
            txt_redCrystalHealthText.transform.parent.gameObject.SetActive(true);
            txt_blueObjvTitle.text = txt_bluePauseObjvTitle.text = Constants.ObjectiveText.C_CrystalDestructTitle;
            txt_blueObjvDescription.text = txt_bluePauseObjvDescription.text = Constants.ObjectiveText.C_CrystalDestructDescription;
            PopupFadeIn(Constants.Global.Color.BLUE);
        }
        else {
            txt_blueCrystalHealthText.transform.parent.gameObject.SetActive(true);
            txt_redObjvTitle.text = txt_redPauseObjvTitle.text = Constants.ObjectiveText.C_CrystalDestructTitle;
            txt_redObjvDescription.text = txt_redPauseObjvDescription.text = Constants.ObjectiveText.C_CrystalDestructDescription;
            PopupFadeIn(Constants.Global.Color.RED);
        }
        UpdateCrystalHealthUI(colorIn, Constants.ObjectiveStats.C_CrystalMaxHealth);
    }

    public void HotPotatoInit(Constants.Global.Color colorIn) {
        if (colorIn == Constants.Global.Color.RED) {
            txt_redCompletionTimer.transform.parent.gameObject.SetActive(true);
            txt_redDestructionTimer.transform.parent.gameObject.SetActive(true);
            txt_redObjvTitle.text = txt_redPauseObjvTitle.text = Constants.ObjectiveText.C_PotatoTitle;
            txt_redObjvDescription.text = txt_redPauseObjvDescription.text = Constants.ObjectiveText.C_PotatoDescription;
        }
        else {
            txt_blueCompletionTimer.transform.parent.gameObject.SetActive(true);
            txt_blueDestructionTimer.transform.parent.gameObject.SetActive(true);
            txt_blueObjvTitle.text = txt_bluePauseObjvTitle.text = Constants.ObjectiveText.C_PotatoTitle;
            txt_blueObjvDescription.text = txt_bluePauseObjvDescription.text = Constants.ObjectiveText.C_PotatoDescription;
        }
        UpdateCompletionTimerUI(colorIn, Constants.ObjectiveStats.C_PotatoCompletionTimer);
        UpdateDestructionTimerUI(colorIn, Constants.ObjectiveStats.C_PotatoSelfDestructTimer);
        PopupFadeIn(colorIn);
    }

    public void RiftBossInit(Constants.Global.Color colorIn) {
        if (colorIn == Constants.Global.Color.RED) {
            txt_redRiftBossHealthText.transform.parent.gameObject.SetActive(true);
            txt_redObjvTitle.text = txt_redPauseObjvTitle.text = Constants.ObjectiveText.C_BossTitle;
            txt_redObjvDescription.text = txt_redPauseObjvDescription.text = Constants.ObjectiveText.C_BossDescription;
            PopupFadeIn(Constants.Global.Color.RED);
        }
        else {
            txt_blueRiftBossHealthText.transform.parent.gameObject.SetActive(true);
            txt_blueObjvTitle.text = txt_bluePauseObjvTitle.text = Constants.ObjectiveText.C_BossTitle;
            txt_blueObjvDescription.text = txt_bluePauseObjvDescription.text = Constants.ObjectiveText.C_BossDescription;
            PopupFadeIn(Constants.Global.Color.BLUE);
        }
        UpdateRiftBossHealthUI(colorIn, Constants.ObjectiveStats.C_RiftBossMaxHealth);
    }

    //----------------------------
    // Flash to mask room switching.

    public void Flash(Constants.Global.Color colorIn) {
        if (colorIn == Constants.Global.Color.RED) {
            f_redFlashTime = Time.time;
            InvokeRepeating("RedFlash", 0.05f, 0.075f);
        } else {
            f_blueFlashTime = Time.time;
            InvokeRepeating("BlueFlash", 0.05f, 0.075f);
        }
    }

    public void RoomUpdate(Constants.Global.Color colorIn, int i_currentRoom) {
        if (colorIn == Constants.Global.Color.RED) {
            txt_redRoomCounter.text = "Room:\n" + i_currentRoom + "/5";
        } else {
            txt_blueRoomCounter.text = "Room:\n" + i_currentRoom + "/5";
        }

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
            txt_redCrystalHealthText.transform.parent.gameObject.SetActive(false);
        }
        else {
            txt_blueCrystalHealthText.transform.parent.gameObject.SetActive(false);
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
            txt_redRiftBossHealthText.transform.parent.gameObject.SetActive(false); //TODO: new ui txt objects
        }
        else {
            txt_blueRiftBossHealthText.transform.parent.gameObject.SetActive(false);
        }
    }

    //----------------------------
    // Fade in/out objective description UI at the start of each objective
    private void PopupFadeIn(Constants.Global.Color colorIn) {
        if (colorIn == Constants.Global.Color.RED) {
            f_redStartTime = Time.time;
            img_redFlashBacking.color = Color.white;
            InvokeRepeating("FadeInRed", 0.1f, 0.075f);
        } else {
            f_blueStartTime = Time.time;
            img_blueFlashBacking.color = Color.white;
            InvokeRepeating("FadeInBlue", 0.1f, 0.075f);
        }
    }

    private void PopupFadeOut(Constants.Global.Color colorIn) {
        if (colorIn == Constants.Global.Color.RED) {
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

    private void BlueFlash() {
        float timer = (Time.time - f_blueFlashTime);
        float fracJourney = timer / 0.4f;
        img_blueFlashBacking.color = Color.Lerp(img_blueFlashBacking.color, new Color(1,1,1,0), fracJourney);
        if (timer > 0.4f) {
            CancelInvoke("BlueFlash");
        }
    }

    private void RedFlash() {
        float timer = (Time.time - f_redFlashTime);
        float fracJourney = timer / 0.4f;
        img_redFlashBacking.color = Color.Lerp(img_redFlashBacking.color, new Color(1,1,1,0), fracJourney);
        if (timer > 0.4f) {
            CancelInvoke("RedFlash");
        }
    }

    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    void Awake() {
        instance = this;
    }
}
