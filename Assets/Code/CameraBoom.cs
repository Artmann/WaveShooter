using UnityEngine;

namespace Code
{
    [ExecuteInEditMode]
    public class CameraBoom : MonoBehaviour
    {
        [Range(0f, 100f)] public float length = 10;
        
        [SerializeField] private new Camera camera;
        [SerializeField] private float pitch = 0;

        private const float maxDistance = 5f;
        
        private Vector3 cameraPosition;
        
        private void OnDrawGizmos()
        {
            var position = transform.position;
        
            Gizmos.color = Color.cyan;
            
            Gizmos.DrawSphere(position, 0.1f);
            Gizmos.DrawSphere(cameraPosition, 0.1f);
            
            Gizmos.color = Color.red;
            
            Gizmos.DrawLine(position, cameraPosition);
        }

        private void Update()
        {
            var transformCache = transform;
            var position = transformCache.position;
            
            (transformCache = transform).localRotation = Quaternion.Euler(pitch, 0, 0);

            var direction = -transformCache.forward;
            var maxCameraDistance = length / 100f * maxDistance;

            var ray = new Ray(position, direction);
            var blocked = Physics.SphereCast(ray, 0.1f, out var hit, maxCameraDistance);

            cameraPosition = blocked ? hit.point : position + direction * maxCameraDistance;

            var cameraTransformCache = camera.transform;
            
            cameraTransformCache.position = cameraPosition;
            cameraTransformCache.LookAt(position);
        }

        public void SetPitch(float pitch)
        {
            this.pitch = pitch;
        }
    }
}