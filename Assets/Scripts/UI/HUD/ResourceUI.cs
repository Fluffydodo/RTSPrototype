using RTS.Resources;
using UnityEngine;
using UnityEngine.UI;

namespace RTS.UI.HUD 
{
    public class ResourceUI : MonoBehaviour
    {
        public static ResourceUI instance;

        [SerializeField] private Text goldAmount;

        private void Awake()
        {
            instance = this;
        }

        private void Update()
        {
            UpdateGold();
        }

        private void UpdateGold()
        {
            goldAmount.text = ResourceHandler.instance.goldStockpile.ToString();
        }
    }
}
