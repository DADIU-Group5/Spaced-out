using UnityEngine;

namespace Prototype
{
    public class InputController : MonoBehaviour
    {
        private bool invertCameraControls = false;
        private Vector2 oldPoint;
        private bool launchMode = false;

        public float cameraRotateSpeed = 4000f;
        public float launchBuffer = 100f;
        public Camera cam;
        public BehindCamera behindCamera;
        //public PlayerController player;
        public FuelController fuel;

        // TODO: remove
        public Transform playerTransform;
        public Transform playerPitchTransform;
        
        private void Awake()
        { }

        private void Update()
        {
            // Quit
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
            
            // See of player was tapped. If he was, set launchMode to true, otherwise to false.
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.tag == "Player")
                    {
                        launchMode = true;
                    }
                    else
                    {
                        launchMode = false;
                    }
                }
            }

            // Check if we are NOT in launchmode
            if (!launchMode)
            {
                // Save starting position of tap
                if (Input.GetMouseButtonDown(0))
                {
                    oldPoint = Input.mousePosition;
                }
                
                // Look around and change position of camera
                if (Input.GetMouseButton(0))
                {
                    Vector2 pos = Input.mousePosition;
                    Vector2 offset = pos - oldPoint;

                    if (invertCameraControls)
                    {
                        offset = -offset;
                    }

                    DirectedRotation(offset);
                    oldPoint = pos;
                }
            }
            else if (fuel.HasFuel())
            {
                // Save starting position of tap
                if (Input.GetMouseButtonDown(0))
                {
                    oldPoint = Input.mousePosition;
                }

                // Rotate player pitch so it faces camera direction and update velocity meter according to where finger is on the screen
                if (Input.GetMouseButton(0))
                {
                    // Rotate player pitch transform so player is launched in correct direction
                    playerPitchTransform.rotation = behindCamera.pitch.transform.rotation;

                    // Commented out by Giorgos, I don't think this is needed
                    //player.SetLaunchForce(GetLaunchForce());
                }

                // Launch 
                if (Input.GetMouseButtonUp(0))
                {
                    //player.Launch(GetLaunchForce());

                    var evt = new ObserverEvent(EventName.PlayerLaunch);
                    
                    evt.payload.Add(PayloadConstants.LAUNCH_SPEED, GetLaunchForce());
                    Subject.instance.Notify(gameObject, evt);

                    launchMode = false;
                }
            }
        }

        // Calculate force from old mouse position and current mouse position
        private float GetLaunchForce()
        {
            float difference = oldPoint.y - Input.mousePosition.y;
            float maxDifference = oldPoint.y - launchBuffer;

            return (difference / maxDifference).Clamp(0f, 1f);
        }

        private void ResetRotation()
        {
            playerTransform.rotation = Quaternion.identity;
            playerPitchTransform.rotation = Quaternion.identity;
        }

        private Vector2 ScreenCenter()
        {
            return new Vector2(cam.pixelWidth / 2f, cam.pixelHeight / 2f);
        }

        private void DirectedRotation(Vector2 offset)
        {
            float xScale = behindCamera.pitch.transform.up.y;
            behindCamera.transform.Rotate(Vector3.up, Time.deltaTime * xScale * cameraRotateSpeed * (offset.x / ScreenCenter().magnitude));
            behindCamera.pitch.transform.Rotate(Vector3.right, Time.deltaTime * cameraRotateSpeed * (-offset.y / ScreenCenter().magnitude));
        }

        public void ToggleCameraControls()
        {
            invertCameraControls = !invertCameraControls;
        }
    }
}