using System.Collections.Generic;
using UnityEngine;

namespace RTS.CapturePoints
{
    public class AreaCollider : MonoBehaviour
    {
        public int teamNumber;

        [SerializeField] private List<PlayerMapAreas> playerMapAreaList = new List<PlayerMapAreas>();
       private void OnTriggerEnter(Collider collider)
       {
           if(collider.TryGetComponent(out PlayerMapAreas playerMapAreas))
           {
               if(!playerMapAreaList.Contains(playerMapAreas))
               {
                   playerMapAreaList.Add(playerMapAreas);
                   teamNumber = playerMapAreas.GetTeam();
               }
               
           }
       }

       private void OnTriggerExit(Collider collider)
       {
           if(collider.TryGetComponent(out PlayerMapAreas playerMapAreas))
           {
               playerMapAreaList.Remove(playerMapAreas);
           }
       }

       public List<PlayerMapAreas> GetPlayerMapAreaList()
       {
           return playerMapAreaList;
       }
    }
}
