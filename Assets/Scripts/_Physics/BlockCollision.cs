using UnityEngine;

namespace RTS._Physics
{
    public class BlockCollision : MonoBehaviour
    {

        public CapsuleCollider unitCollider;
        public CapsuleCollider unitBlockerCollider;

        private void Start()
        {
            Physics.IgnoreCollision(unitCollider, unitBlockerCollider, true);
        }
    }
}