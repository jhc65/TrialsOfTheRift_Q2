/*  Crystal Destruction Objective - Zak Olyarnik
 * 
 *  Desc:   Abstracts scene loading functionality
 * 
 */

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour {
#region Variables and Declarations
    [SerializeField] protected GameObject go_load;
    [SerializeField] protected Text txt_loadFade;
#endregion

#region SceneLoader Shared Methods
    protected void LoadTextFade() {
        txt_loadFade.color = Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time, 0.5f));
    }

    protected void LoadNextScene(string nextScene) {
        go_load.SetActive(true);
        InvokeRepeating("LoadTextFade", 0.01f, 0.0165f);
        SceneManager.LoadSceneAsync(nextScene);
    }
#endregion
}
