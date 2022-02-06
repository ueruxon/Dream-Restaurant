using Game.Code.Infrastructure.Services.Inputs;
using UnityEngine;

namespace Game.Code.Players
{
    public class PlayerMovement
    {
        private const float Epsilon = 0.001f;
        
        private readonly IInputService _inputService;
        private readonly CharacterController _characterController;
        private readonly PlayerAnimator _playerAnimator;
        private readonly Camera _camera;

        private float _movementSpeed = 5f;
        private bool _isRun = false;

        public PlayerMovement(IInputService inputService, CharacterController characterController,
            PlayerAnimator playerAnimator, Camera camera)
        {
            _inputService = inputService;
            _characterController = characterController;
            _playerAnimator = playerAnimator;
            _camera = camera;
        }
        
        public void Move(Transform playerTransform, float deltaTime)
        {
            Vector3 movementVector = Vector3.zero;

            if (_inputService.InputAxis().sqrMagnitude > Epsilon)
            {
                movementVector = _camera.transform.TransformDirection(_inputService.InputAxis());
                movementVector.y = 0f;
                movementVector.Normalize();

                playerTransform.forward = movementVector;
            }

            movementVector += Physics.gravity;

            _characterController.Move(movementVector * _movementSpeed * deltaTime);
            _playerAnimator.PlayRun(_characterController.velocity.magnitude);
        }
    }
}