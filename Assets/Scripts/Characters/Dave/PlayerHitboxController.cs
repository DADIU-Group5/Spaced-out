using UnityEngine;
using System.Collections;

public class PlayerHitboxController : MonoBehaviour {

    public Transform playerTransform;
    public Transform cameraTransform;
    
	void LateUpdate () {
        // Position should always be on top of the player
        this.transform.position = playerTransform.position;

        // Rotation should always face camera with flat surface
        this.transform.LookAt(cameraTransform);
        this.transform.Rotate(new Vector3(180f,0f,0f));
	}
}
