using UnityEngine;

namespace RTS.Interactables
{
    public class Interactable : MonoBehaviour
    {
        public bool isInteracting;
        public GameObject highlight;

        public virtual void Awake()
        {
            highlight.SetActive(false);
        }

        public virtual void OnInteractEnter()
        {
            ShowHighlight();
            isInteracting = true;
        }

        public virtual void OnInteractExit()
        {
            HideHighlight();
            isInteracting = false;
        }

        public virtual void ShowHighlight()
        {
            highlight.SetActive(true);
        }

        public virtual void HideHighlight()
        {
            highlight.SetActive(false);
        }
    }
}
