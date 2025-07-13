using UnityEngine;

namespace RTS.Resources
{
    public class ResourceHandler : MonoBehaviour
    {
        public static ResourceHandler instance;

        public float goldStockpile;

        private void Awake()
        {
           instance = this; 
        }

    }
}
