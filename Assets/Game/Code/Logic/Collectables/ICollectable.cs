using UnityEngine;

namespace Game.Code.Logic.Collectables
{
    public interface ICollectable
    {
        void MoveTo(Vector3 position);
        Transform GetTransform();
        float GetHeight();
    }
}