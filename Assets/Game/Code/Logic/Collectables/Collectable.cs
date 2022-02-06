using UnityEngine;

namespace Game.Code.Logic.Collectables
{
    public abstract class Collectable : MonoBehaviour
    {
        public abstract void MoveTo(Vector3 position);
        public abstract float GetHeight();

        public abstract void DestroyCollectable();

        public virtual Transform GetTransform() => 
            transform;
    }
}