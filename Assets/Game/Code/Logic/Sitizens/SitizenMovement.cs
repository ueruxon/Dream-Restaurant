using System;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Code.Logic.Sitizens
{
    public class SitizenMovement
    {
        public event Action ReachedToWaypoint;
        public event Action ReachedToLastPoint;

        private readonly NavMeshAgent _agent;
        private readonly SitizenAnimator _animator;
        private readonly SitizenWaypointNetwork _waypointNetwork;

        private bool _alreadyMove;

        public SitizenMovement(SitizenAnimator animator, NavMeshAgent agent, SitizenWaypointNetwork sitizenWaypointNetwork)
        {
            _animator = animator;
            _agent = agent;
            _alreadyMove = false;
            _waypointNetwork = sitizenWaypointNetwork;

            _agent.enabled = false;
        }

        public void Move(float remainingDistance = 0.5f)
        {
            if (_agent.enabled == false)
                _agent.enabled = true;
            
            if (_alreadyMove)
            {
                if (!_agent.pathPending && _agent.remainingDistance < remainingDistance)
                {
                    _alreadyMove = false;

                    //_animator.SetBool(Run, false);
                    
                    ReachedToWaypoint?.Invoke();
                }
            }
            else
                _alreadyMove = TryMoveToNextDestination();
        }

        private bool TryMoveToNextDestination()
        {
            Transform target = _waypointNetwork.GetNextWaypoint();

            if (target == null)
            {
                ReachedToLastPoint?.Invoke();
                return false;
            }
            
            SetDestination(target.position);

            return true;
        }

        public void MoveTo(Transform target, float remainingDistance = 0.5f)
        {
            if (_agent.enabled == false)
                _agent.enabled = true;
            
            if (_alreadyMove)
            {
                if (!_agent.pathPending && _agent.remainingDistance < remainingDistance) 
                    _alreadyMove = false;
            }
            else
            {
                _alreadyMove = true;
                SetDestination(target.position);
            }
        }
        
        public void StopMove()
        {
            _agent.isStopped = false;
            _agent.enabled = false;
            
            _alreadyMove = false;
        }

        private void SetDestination(Vector3 destinationPoint)
        {
            _agent.SetDestination(destinationPoint);
            _animator.PlayMove();
        }
    }
}