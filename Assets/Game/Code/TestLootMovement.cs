using System.Collections;
using UnityEngine;
using DG.Tweening;


namespace Game.Code
{
    public class TestLootMovement : MonoBehaviour
    {
        [SerializeField] private float _maxDistance = 3;
        
        private float _speed = 1.5f;

        private Vector3 _startPosition;
        private Vector3 _centerPosition;
        private Vector3 _endPosition;
        
        private Vector3 _startRelativeCenter;
        private Vector3 _endRelativeCenter;

        private float _startTime;
        private float _currentPathValue;

        public void Init(Vector3 center, Vector3 end)
        {
            _startPosition = transform.position;
            _centerPosition = center;
            _endPosition = end;

            _startRelativeCenter = transform.position - _centerPosition;
            _endRelativeCenter = _endPosition - _centerPosition;
            
            _startTime = Time.time;
        }

        public void StartMoving()
        {
            StartCoroutine(MovementSlerpRoutine());
            RandomRotation();
        }

        private IEnumerator MovementSlerpRoutine()
        {
            while (_currentPathValue < 1.1f)
            {
                _currentPathValue = (Time.time - _startTime) / 1f * _speed;
                
                transform.position = Vector3.Slerp(_startRelativeCenter, _endRelativeCenter, _currentPathValue * _speed);
                transform.position += _centerPosition;
                
                
                yield return null;
            }
        }

        private void RandomRotation()
        {
            float rotationSpeed = Random.Range(1f, 5f);
            
            Vector3 randomRotate = new Vector3(0f, 360f, 0f);
            transform.DORotate(randomRotate, rotationSpeed, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear);
        }
    }
}