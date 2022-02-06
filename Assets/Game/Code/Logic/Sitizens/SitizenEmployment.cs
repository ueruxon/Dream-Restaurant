using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Game.Code.Logic.Animators;
using Game.Code.Logic.FoodAreas;

namespace Game.Code.Logic.Sitizens
{
    public class SitizenEmployment
    {
        public event Action<bool> EmploymentEnded;
        public event Action StartWaitingOrdered;

        private readonly ICoroutineRunner _coroutineRunner;
        private readonly SitizenAnimator _animator;
        private readonly NavMeshAgent _agent;
        
        private Spot _spot;

        private readonly int _waitingTimeOrder;
        private float _currentWaitingTimer;

        private readonly int _interactionTime = 4;
        private float _currentInteractionTimer;

        public SitizenEmployment(SitizenAnimator animator, NavMeshAgent navMeshAgent, ICoroutineRunner coroutineRunner,
            int waitingTimeOrder)
        {
            _animator = animator;
            _agent = navMeshAgent;
            _coroutineRunner = coroutineRunner;
            
            _waitingTimeOrder = waitingTimeOrder;
            _currentWaitingTimer = 0f;
        }

        private void OnStateExited(SitizenAnimatorState animatorState)
        {
            //Debug.Log("Вышли из анимации " + animatorState);
            if (animatorState == SitizenAnimatorState.StandToSit) 
                StartWaitingOrder();
        }

        public void Start(Spot spot, Transform sitizenTransform)
        {
            _spot = spot;
            _agent.enabled = false;

            _animator.StateExited += OnStateExited;
            _animator.PlaySitDown();
            
            _coroutineRunner.StartCoroutine(LookAtRoutine(sitizenTransform, _spot));
        }

        public void StartInteraction()
        {
            _animator.PlayEat();
        }

        public void WaitingOrder(float deltaTime)
        {
            _currentWaitingTimer += deltaTime;

            if (_currentWaitingTimer >= _waitingTimeOrder)
            {
                _currentWaitingTimer = 0;
                EmploymentEnded?.Invoke(false);
            }
        }

        public void Interaction(float deltaTime)
        {
            _currentInteractionTimer += deltaTime;

            if (_currentInteractionTimer >= _interactionTime)
            {
                _currentInteractionTimer = 0;
                EmploymentEnded?.Invoke(true);
            }
        }

        private void StartWaitingOrder()
        {
            _animator.PlaySittingIdle();
            StartWaitingOrdered?.Invoke();
        }

        private IEnumerator LookAtRoutine(Transform transform, Spot spot)
        {
            Transform foodPoint = spot.GetFoodPoint();
            Transform spotPoint = spot.GetSpotPoint();

            float currentTimer = 0;
            
            while (currentTimer < 1f)
            {
                Vector3 targetPosition = new Vector3(foodPoint.position.x, transform.position.y, foodPoint.position.z);
                transform.LookAt(targetPosition, Vector3.up);

                float step =  1f * Time.deltaTime; 
                transform.position = Vector3.MoveTowards(transform.position, spotPoint.position, step);

                currentTimer += Time.deltaTime;
                yield return null;
            }
        }

        public void Reset() => 
            _animator.StateExited -= OnStateExited;
    }
}