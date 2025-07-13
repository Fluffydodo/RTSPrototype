using RTS.Unit;
using UnityEngine;

namespace RTS.UI.HUD 
{

    [CreateAssetMenu(fileName = "NewUnitCard", menuName = "UnitCard")]
    public class UnitListCard : ScriptableObject
    {
        public UnitInformation unitInformation;
        public GameObject prefab;
    }
}
