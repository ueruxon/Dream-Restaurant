using System;
using Game.Code.Logic.Animators;
using UnityEngine;

namespace Game.Code.Players
{
    public class PlayerAnimator : MonoBehaviour, IAnimationStateReader
    {
        private const string IDLE_LAYER = "Idle Layer";
        
        private readonly int _idleStateHash = Animator.StringToHash("Idle");
        private readonly int _runStateHash = Animator.StringToHash("Speed");

        public event Action<PlayerAnimatorState> StateEntered;
        public event Action<PlayerAnimatorState> StateExited;
        
        private Animator _animator;
        private PlayerAnimatorState _animatorState;

        private int _idleLayerIndex;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();

            _idleLayerIndex = _animator.GetLayerIndex(IDLE_LAYER);
        }

        public void SelectLayerWeight(int resultWeight) => 
            _animator.SetLayerWeight(_idleLayerIndex, resultWeight);

        public void PlayIdle() => 
            _animator.SetTrigger(_idleStateHash);

        public void PlayRun(float speed) => 
            _animator.SetFloat(_runStateHash, speed);

        public void EnteredState(int stateHash)
        {
            _animatorState = StateFor(stateHash);
            StateEntered?.Invoke(_animatorState);
        }

        public void ExitedState(int stateHash) => 
            StateExited?.Invoke(StateFor(stateHash));

        private PlayerAnimatorState StateFor(int stateHash)
        {
            PlayerAnimatorState state;
            
            if (stateHash == _runStateHash)
                state = PlayerAnimatorState.Run;
            else if (stateHash == _idleStateHash)
                state = PlayerAnimatorState.Idle;
            else
                state = PlayerAnimatorState.Idle;

            return state;
        }
    }
}