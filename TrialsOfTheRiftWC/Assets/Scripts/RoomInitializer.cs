/*  RoomInitializer - Dana Thompson
 * 
 *  Desc:   Updates enemy spawn points when a new room is set active
 * 
 */

using UnityEngine;

public class RoomInitializer : MonoBehaviour {
#region Variables and Declarations
    [SerializeField] private Constants.Global.Side e_side;
    [SerializeField] private GameObject[] go_enemySpawnPool;// rooms allowed for this objective to be played in
#endregion

#region Unity Overrides
    void OnEnable() {   // OnEnable() called before Start(), so we can't set the RiftController instance there
        if (e_side == Constants.Global.Side.RIGHT) {
            RiftController.Instance.RightEnemySpawners = go_enemySpawnPool;
        }
        else {
            RiftController.Instance.LeftEnemySpawners = go_enemySpawnPool;
        }
    }
#endregion
}
