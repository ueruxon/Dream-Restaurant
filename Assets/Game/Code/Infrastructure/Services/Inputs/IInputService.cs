using UnityEngine;

namespace Game.Code.Infrastructure.Services.Inputs
{
    public interface IInputService : IService
    {
        public Vector2 InputAxis();
    }
}