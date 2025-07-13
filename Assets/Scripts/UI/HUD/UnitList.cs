using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RTS.UI.HUD
{
    public class UnitList : MonoBehaviour
    {
        public static UnitList instance;

        [SerializeField] private Transform layoutGroup;

        [SerializeField] private PlayerActions actions;

        public List<Button> unitListUIButtons = new List<Button>();


        private void Awake()
        {
            instance = this;
        }

        private void Update()
        {
            ClearOnEmptyList();
        }

        private void ClearOnEmptyList()
        {
            if(layoutGroup.childCount <= 0)
            {
                gameObject.GetComponent<UIVisibility>().OnUIExit();
            }
        }

    }
}
