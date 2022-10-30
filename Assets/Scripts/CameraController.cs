using Cinemachine;
using UnityEngine;

namespace Pavos.Archer
{
    public class CameraController : MonoBehaviour
    {
        
        [Header("Virtual Cameras")]
        [SerializeField] private CinemachineVirtualCamera playerVirtualCamera;
        [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
        
        [Header("Target")]
        [SerializeField] private Transform followTransform;

        public void HandleRotation(Vector2 input, float rotationPower)
        {
            
            followTransform.transform.rotation *= Quaternion.AngleAxis(input.x * rotationPower, Vector3.up);
            followTransform.transform.rotation *= Quaternion.AngleAxis(input.y * rotationPower, Vector3.right);

            var angles = followTransform.transform.localEulerAngles;
            angles.z = 0;
        
            if (angles.x is > 180 and < 340)
            {
                angles.x = 340;
            } else if (angles.x is < 180 and > 40)
            {
                angles.x = 40;
            }

            followTransform.transform.localEulerAngles = angles;

        }
        
        public void HandleAiming(bool isAiming)
        {
            // playerVirtualCamera.Priority = isAiming ? 9 : 11;
            aimVirtualCamera.Priority = isAiming ? 11 : 9;
        }
        
    }
}