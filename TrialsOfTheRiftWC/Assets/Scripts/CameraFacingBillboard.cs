using UnityEngine;
using System.Collections;
 
public class CameraFacingBillboard : MonoBehaviour {

	public Camera cam_Camera;
    public PlayerController playc_trackedPlayer;
    //Set in editor.
    public Vector3 v3_offset;

    void Update() {
        transform.LookAt(transform.position + cam_Camera.transform.rotation * Vector3.forward,
            cam_Camera.transform.rotation * Vector3.up);
        transform.position = playc_trackedPlayer.transform.position + v3_offset;
    }
}