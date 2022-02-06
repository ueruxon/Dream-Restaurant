using UnityEngine;

namespace Game.Code.Logic.Camera
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform _following;

        public float RotationAngleX;
        public float Distance;
        public float OffsetY;

        private void LateUpdate() {
            if (_following == null)
                return;

            Quaternion rotation = Quaternion.Euler(RotationAngleX, 0f, 0f);

            Vector3 position = rotation * new Vector3(0, 0, -Distance) + GetFollowingPosition();

            transform.rotation = rotation;
            transform.position = position;
        }

        public void Follow(GameObject following) => _following = following.transform;

        private Vector3 GetFollowingPosition() {
            Vector3 followingPosition = _following.position;
            followingPosition.y += OffsetY;

            return followingPosition;
        }
    }
}