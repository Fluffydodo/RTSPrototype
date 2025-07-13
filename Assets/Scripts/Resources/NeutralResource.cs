using UnityEngine;

namespace RTS.Resources
{
    [CreateAssetMenu(fileName = "New Resource", menuName = "Resource")]
    public class NeutralResource : ScriptableObject
    {
        public enum ResourceType
        {
            Gold,
            Supplies,
            Steel

        }


        [Header("Resource Settings")]
        [Space(15)]
        public ResourceType type;
        public new string name;
        public GameObject prefab;
        public float resourceSize;
    }
}
