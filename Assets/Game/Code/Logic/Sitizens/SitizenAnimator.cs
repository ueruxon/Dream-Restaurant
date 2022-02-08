using System;
using UnityEngine;
using Game.Code.Logic.Animators;

namespace Game.Code.Logic.Sitizens
{
    [RequireComponent(typeof(Animator))]
    public class SitizenAnimator : MonoBehaviour, IAnimationStateReader
    {
        private readonly int _walkingStateHash = Animator.StringToHash("Walking");
        private readonly int _standToSitHash = Animator.StringToHash("StandToSit");
        private readonly int _sittingIdleHash = Animator.StringToHash("SittingIdle");
        private readonly int _eatingHash = Animator.StringToHash("Eating");
        
        public event Action<SitizenAnimatorState> StateEntered;
        public event Action<SitizenAnimatorState> StateExited;

        private Animator _animator;
        private SitizenAnimatorState _animatorState;

        private void Awake() => 
            _animator = GetComponent<Animator>();
        
        public void PlayMove() => 
            _animator.SetTrigger(_walkingStateHash);

        public void PlaySitDown() =>
            _animator.SetTrigger(_standToSitHash);
        
        public void PlaySittingIdle() =>
            _animator.SetTrigger(_sittingIdleHash);

        public void PlayEat() => _animator.SetTrigger(_eatingHash);

        public void EnteredState(int stateHash)
        {
            _animatorState = StateFor(stateHash);
            StateEntered?.Invoke(_animatorState);
        }

        public void ExitedState(int stateHash) => 
            StateExited?.Invoke(StateFor(stateHash));

        private SitizenAnimatorState StateFor(int stateHash)
        {
            SitizenAnimatorState state;

            if (stateHash == _walkingStateHash)
                state = SitizenAnimatorState.Walking;
            else if (stateHash == _standToSitHash)
                state = SitizenAnimatorState.StandToSit;
            else if (stateHash == _sittingIdleHash)
                state = SitizenAnimatorState.SittingIdle;
            else if (stateHash == _eatingHash)
                state = SitizenAnimatorState.Eating;
            else
                state = SitizenAnimatorState.None;

            return state;
        }
    }
}