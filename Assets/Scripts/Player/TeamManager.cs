using System.Collections.Generic;
using UnityEngine;

namespace RTS.Player
{
    public class TeamManager : MonoBehaviour
    {
        public static TeamManager instance;

        public List<List<Transform>> playerTeams = new List<List<Transform>>();
        [SerializeField]private  List<Transform> team1 = new List<Transform>();
        [SerializeField]private  List<Transform> team2 = new List<Transform>();
        [SerializeField]private  List<Transform> team3 = new List<Transform>();
        [SerializeField]private  List<Transform> team4 = new List<Transform>();

        private void Awake()
        {
            instance = this;

            playerTeams.Add(team1);
            playerTeams.Add(team2);
            playerTeams.Add(team3);
            playerTeams.Add(team4);
        }

    }
}