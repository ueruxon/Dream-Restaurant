using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Game.Code.Players;
using Game.Code.StaticData;

namespace Game.Code.Logic.Loots
{
    public class Loot : MonoBehaviour
    {
        [SerializeField] private List<LootView> _lootVisuals;
        [SerializeField] private TrailRenderer _trail;

        private LootView _currentLootView;
        private LootStaticData _lootData;

        // private float _slerpSpeed = 1f;
        private float _speed = 3f;

        private Vector3 _endPosition;
        // private Vector3 _centerPosition;
        // private Vector3 _startRelativeCenter;
        // private Vector3 _endRelativeCenter;
        //
        // private float _startTime;

        private ICollector _target;
        private bool _isFollowing;
        private Vector3 _velocity;
        
        private Tween _rotationTween;
        private Sequence _jumpSequence;

        public void Init(LootStaticData lootData, Vector3 center, Vector3 end)
        {
            _lootData = lootData;

            if (_trail != null)
            {
                _trail.startColor = _lootData.Color;
                _trail.endColor = _lootData.Color;
            }

            _endPosition = end;
            // _centerPosition = center;

            // _startRelativeCenter = transform.position - _centerPosition;
            // _endRelativeCenter = _endPosition - _centerPosition;
            //
            // _startTime = Time.time;
            
            ActiveView();
        }

        public void StartMoving()
        {
            JumpTo(_endPosition);
            RandomRotation();
            
            //StartCoroutine(MovementSlerpRoutine());
        }

        private void Update()
        {
            if (_isFollowing)
            {
                transform.position = Vector3.SmoothDamp(transform.position, 
                    _target.GetCollectablePoint(), ref _velocity, Time.deltaTime * _speed);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isFollowing == false)
            {
                if (other.TryGetComponent(out ICollector collector))
                {
                    _isFollowing = true;
                    _target = collector;
                }
            }
        }

        private void ActiveView()
        {
            foreach (LootView lootView in _lootVisuals)
            {
                if (lootView.LootType == _lootData.LootType)
                {
                    _currentLootView = lootView;
                    _currentLootView.Collected += OnCollected;
                    _currentLootView.Show();
                    
                    break;
                }
            }
        }
        
        private void OnCollected(ICollector collector)
        {
            collector.AddMoney(_lootData.Amount);
            
            _rotationTween.Kill();
            _jumpSequence.Kill();
            
            _currentLootView.Collected -= OnCollected;
            _currentLootView.Hide();

            StartCoroutine(DestroyLootRoutine());
        }
        
        private void JumpTo(Vector3 endPosition)
        {
            _jumpSequence = transform.DOLocalJump(
                endValue: endPosition,
                jumpPower: 1,
                numJumps: 2,
                duration: 1f
            ).SetEase(Ease.Linear);
        }

        // private IEnumerator MovementSlerpRoutine()
        // {
        //     float currentPathValue = 0;
        //     
        //     while (currentPathValue < 1f)
        //     {
        //         currentPathValue = (Time.time - _startTime) / 1f * _slerpSpeed;
        //         
        //         transform.position = Vector3.Slerp(_startRelativeCenter, _endRelativeCenter, currentPathValue * _slerpSpeed);
        //         transform.position += _centerPosition;
        //         yield return null;
        //     }
        // }
        
        private void RandomRotation()
        {
            float rotationSpeed = Random.Range(1f, 5f);
            
            Vector3 randomRotate = new Vector3(0f, 360f, 0f);
            _rotationTween = _currentLootView.transform.DORotate(randomRotate, rotationSpeed, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear);
        }

        private IEnumerator DestroyLootRoutine()
        {
            yield return new WaitForSeconds(.2f);
            Destroy(gameObject);
        }
    }
}