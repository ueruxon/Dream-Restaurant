using System.Collections;
using Game.Code.Common;
using UnityEngine;

namespace Game.Code.Logic.Collectables
{
    public class Food : Collectable
    {
        [SerializeField] private Collider _collider;
        
        public FoodType Type;

        private float _speed = 1.5f;
        private float _startTime;
        
        private Vector3 _centerPosition;
        private Vector3 _endPosition;
        private Vector3 _startRelativeCenter;
        private Vector3 _endRelativeCenter;

        public void Init(FoodType foodType)
        {
            Type = foodType;
        }

        public override void MoveTo(Vector3 position)
        {
            CalculatePositionsForSlerpMovement(position);
            StartCoroutine(MovementSlerpRoutine());
            //StartCoroutine(MovementRoutine(position));
        }

        public override float GetHeight() => 
            _collider.bounds.size.y;

        public override void DestroyCollectable() => 
            Destroy(gameObject);

        private void CalculatePositionsForSlerpMovement(Vector3 endPosition)
        {
            _startTime = Time.time;
            
            Vector3 centerPosition = (transform.localPosition + endPosition) * .5f;
            Vector3 centerPositionWithOffset = (centerPosition + centerPosition) * .5f;

            endPosition += new Vector3(0, 0.05f);
            
            _centerPosition = centerPositionWithOffset;
            _endPosition = endPosition;
            
            _startRelativeCenter = transform.localPosition - _centerPosition;
            _endRelativeCenter = _endPosition - _centerPosition;
        }

        private IEnumerator MovementRoutine(Vector3 targetPosition)
        {
            float currentPathValue = 0;
            float target = 1f;

            while (currentPathValue < 1f)
            {
                currentPathValue = Mathf.MoveTowards(currentPathValue, target, _speed * Time.deltaTime);
                transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, currentPathValue);
                
                yield return null;
            }
        }
        
        private IEnumerator MovementSlerpRoutine()
        {
            float currentPathValue = 0;

            while (currentPathValue < 1f)
            {
                currentPathValue = (Time.time - _startTime) / 1f * _speed;

                transform.localPosition = Vector3.Slerp(_startRelativeCenter, _endRelativeCenter, currentPathValue * _speed);
                transform.localPosition += _centerPosition;
                yield return null;
            }
        }
    }
}