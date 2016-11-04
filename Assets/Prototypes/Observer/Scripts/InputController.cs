using UnityEngine;

namespace ObserverPattern
{
    public class InputController : MonoBehaviour
    {
        private bool invertCameraControls = false;
        private Vector2 oldPoint;

        public float cameraRotateSpeed = 4000f;
        public Camera cam;
        public BehindCamera behindCamera;
        public PlayerController player;

        // TODO: remove
        public Transform playerTransform;
        public Transform playerPitchTransform;

        public Subject subject = Subject.instance;

        public float forceScaling = 6.0f;

        private void Update()
        {
            // Quit
            if (Input.GetKey(KeyCode.Escape))
            {
                // TODO: remove Quit(), instead send event and close down gracefully
                // perhaps by also prompting user to save progress
                // subject.Notify(gameObject, new Event(EventName.QuitGame));
                Application.Quit();
            }

            if (Input.GetMouseButtonDown(0) && !Input.GetMouseButton(1))
            {
                oldPoint = Input.mousePosition;
            }

            // Look
            if (Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButtonUp(1))
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

            // Move
            if (Input.GetMouseButtonDown(1))
            {
                // Hide save starting positions
                oldPoint = Input.mousePosition;
                player.SetHoldControl(false);
            }

            // Rotate player so it faces camera direction
            if (Input.GetMouseButton(1))
            {
                // TODO: remove
                player.GetComponent<Rigidbody>().freezeRotation = true;

                playerTransform.rotation = behindCamera.transform.rotation;
                playerPitchTransform.rotation = behindCamera.pitch.transform.rotation;

                player.SetLaunchForce(GetLaunchForce());
            }

            // Launch
            if (Input.GetMouseButtonUp(1))
            {
                // TODO: remove
                player.GetComponent<Rigidbody>().freezeRotation = false;

                var evt = new Event(EventName.PlayerLaunch);

                evt.payload.Add(PayloadConstants.LAUNCH_SPEED, GetLaunchForce());

                subject.Notify(gameObject, evt);

                oldPoint = Input.mousePosition;
            }
        }

        private float GetLaunchForce()
        {
            Vector2 difference = oldPoint - (Vector2)Input.mousePosition;

            float launchForce = difference.y * forceScaling;

            return launchForce.Clamp(player.minLaunchForce, player.maxLaunchForce);
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