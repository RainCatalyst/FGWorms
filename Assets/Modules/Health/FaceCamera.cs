using UnityEngine;

namespace FGWorms.UI
{
    public class FaceCamera : MonoBehaviour
    {
        private void Update()
        {
            // Look at camera
            var cameraDirection = transform.position - Camera.main.transform.position;
            cameraDirection.y = 0;
            transform.rotation = Quaternion.LookRotation(cameraDirection);
        }
    }
}