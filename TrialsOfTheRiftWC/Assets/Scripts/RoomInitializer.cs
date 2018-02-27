/*  RoomInitializer - Dana Thompson
 * 
 *  Desc:   Determines what happens when a Room is set to be active
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInitializer : MonoBehaviour
{

    [SerializeField] private GameObject[] go_spawnerPool;         // rooms allowed for this objective to be played in
    [SerializeField] private Constants.Global.Side side;
    private RiftController rc_riftController;

    void Awake() {
        rc_riftController = RiftController.Instance;
    }

    void OnEnable() {
        if (side == Constants.Global.Side.RIGHT) {
            rc_riftController.RightEnemySpawners = go_spawnerPool;
        }
        else {
            rc_riftController.LeftEnemySpawners = go_spawnerPool;
        }
    }

    private void Start() {
    
    }
}
