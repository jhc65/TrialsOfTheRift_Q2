/*  Spell Controller - Sam Caulker
 * 
 *  Desc:   Destroys ice wall a set time after creation
 * 
 */

using UnityEngine;

public class IceWallController : MonoBehaviour {
#region Unity Overrides
    void Start () {
		Destroy(gameObject, Constants.SpellStats.C_IceWallLiveTime);
	}
#endregion
}
