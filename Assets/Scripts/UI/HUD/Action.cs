using UnityEngine;

namespace RTS.UI.HUD 
{
    public class Action : MonoBehaviour
    {
        public void OnClick()
        {
            ActionFrame.instance.StartSpawnTimer(name);
        }
    }
}
