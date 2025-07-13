using System.Collections.Generic;
using RTS.Player;
using UnityEngine;

namespace RTS.CapturePoints
{
    public class PlayerMapAreas : MonoBehaviour
    {
        [SerializeField] List<List<Transform>> playerTeams = new List<List<Transform>>();
        [SerializeField] private int teamNumber;

       private void Awake()
       {
            //FindTeam(); 
       }

       private void Start() 
       {
            FindTeam(); 
       }

       private void FindTeam()
       {
           foreach(List<Transform> team in TeamManager.instance.playerTeams)
           {
               teamNumber++;
               if(team.Contains(transform.parent.parent))
               {
                   break;
               }      
           }
       }

       public int GetTeam()
       {
            
            return teamNumber;
       }
    }
}