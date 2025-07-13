using UnityEngine;

namespace RTS.UI.HUD
{
    public class UIUtils : MonoBehaviour
    {
        public bool isOpen;
        public GameObject ui;

        public virtual void Awake()
        {
            ui.SetActive(false);
        }

        public virtual void OnUIEnter()
        {
            ShowUI();
            isOpen = true;
        }
        public virtual void OnUIExit()
        {
            HideUI();
            isOpen = false;
        }
        public virtual void ShowUI()
        {
            ui.SetActive(true);
        }
        public virtual void HideUI()
        {
            ui.SetActive(false);
        }
    }
}
