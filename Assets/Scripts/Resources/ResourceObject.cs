using UnityEngine;

namespace RTS.Resources
{
    public class ResourceObject : MonoBehaviour
    {
        public NeutralResource resourceInfo;

        [SerializeField] private float resourceHealth;

        private void Awake()
        {
            resourceHealth = resourceInfo.resourceSize;
        }

        private void Update()
        {
            HandleHealth();
        }

        public void GiveResource(float dmg)
        {
            resourceHealth -= dmg; //change that to worker dps

            ResourceHandler.instance.goldStockpile += dmg;
        }

        private void HandleHealth()
        {
            if(gameObject)
            {
                if(resourceHealth <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
