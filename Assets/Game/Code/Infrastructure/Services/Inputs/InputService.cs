using UnityEngine;

namespace Game.Code.Infrastructure.Services.Inputs
{
    public class InputService : IInputService
    {
        public Vector2 InputAxis()
        {
            return new Vector2(Joystick.Instance.Horizontal, Joystick.Instance.Vertical);
        }
    }
}