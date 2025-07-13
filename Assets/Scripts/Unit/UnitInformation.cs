using UnityEngine;

namespace RTS.Unit
{
    [CreateAssetMenu(fileName = "New Unit", menuName = "New Unit/Tier 1")]

    public class UnitInformation :ScriptableObject
{

    public enum unitType
    {
        Worker,
        Warrior,
        Archer

    }

    
    [Header("Unit Settings")]
    [Space(15)]
    public unitType type;
    public new string name;
    public GameObject unitPrefab;
    public GameObject icon, unitCard;
    public float spawnTime;

    [Space(40)]
    [Header("Unit Base Stats")]
    
    [Space(15)]
    public UnitStatTypes.Base baseStats;

}
}


