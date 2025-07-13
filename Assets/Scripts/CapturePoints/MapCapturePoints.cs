using System;
using System.Collections.Generic;
using System.Linq;
using RTS.CapturePoints;
using UnityEngine;

namespace CapturePoints
{
    public class MapCapturePoints : MonoBehaviour
    {
        private enum CaptureState
        {
            Neutral,
            Captured,
            RedCapture,
            BlueCapture
        }

        private List<AreaCollider> areaColliderList;
        [SerializeField] private List<PlayerMapAreas> team1InsideList, team2InsideList;
        private List<List<PlayerMapAreas>> teamList = new List<List<PlayerMapAreas>>();

        private CaptureState captureState;

        [SerializeField] private double progressBlue;
        [SerializeField] private double progressRed;
        [SerializeField] private float progressSpeed = .5f;

        [SerializeField] private GameObject pointGameObject;

        private SpriteRenderer pointSprite;


        private void Awake()
        {
            team1InsideList = new List<PlayerMapAreas>();
            team2InsideList = new List<PlayerMapAreas>();

            teamList.Add(team1InsideList);
            teamList.Add(team2InsideList);


            areaColliderList = new List<AreaCollider>();

            foreach (Transform child in transform)
            {
                AreaCollider areaCollider = child.GetComponent<AreaCollider>();
                if (areaCollider != null)
                {
                    areaColliderList.Add(areaCollider);
                }
            }

            captureState = CaptureState.Neutral;

            pointSprite = pointGameObject.GetComponent<SpriteRenderer>();
            pointSprite.color = Color.white;

        }

        private void Start()
        {
        }

        private void Update()
        {
            foreach (var team in teamList)
            {
                foreach (var playerMapAreas in team.Where(playerMapAreas => !areaColliderList
                             .SelectMany(areaCollider => areaCollider.GetPlayerMapAreaList())
                             .Contains(playerMapAreas)).ToList())
                {
                    team.Remove(playerMapAreas);
                }
            }

            foreach (var playerMapAreas in areaColliderList.SelectMany(areaCollider =>
                         areaCollider.GetPlayerMapAreaList()))
            {
                switch (playerMapAreas.GetTeam())
                {
                    case 1:
                        if (!team1InsideList.Contains(playerMapAreas))
                        {
                            team1InsideList.Add(playerMapAreas);
                        }

                        break;

                    case 2:
                        if (!team2InsideList.Contains(playerMapAreas))
                        {
                            team2InsideList.Add(playerMapAreas);
                        }

                        break;
                }
            }

            Capture();

            switch (captureState)
            {
                //List<PlayerMapAreas> team3InsideList = new List<PlayerMapAreas>();
                //List<PlayerMapAreas> team4InsideList = new List<PlayerMapAreas>();

                case CaptureState.Neutral:

                    //When capturing also reduce progress bar by opposite team
                    if (progressBlue >= 1)
                    {
                        captureState = CaptureState.BlueCapture;
                    }

                    if (progressRed >= 1)
                    {
                        captureState = CaptureState.RedCapture;
                    }

                    break;

                case CaptureState.Captured: //remove later!!!
                    break;

                case CaptureState.RedCapture:
                    pointSprite.color = Color.red;
                    break;

                case CaptureState.BlueCapture:
                    pointSprite.color = Color.blue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        private void Capture()
        {

            if (progressBlue <= 1.0)
            {
                progressBlue += team1InsideList.Count * progressSpeed * Time.deltaTime;
                if (team2InsideList.Count == 0) return;
                progressBlue -= team2InsideList.Count * progressSpeed * Time.deltaTime;
            }

            if (progressRed <= 1.0)
            {
                progressRed += team2InsideList.Count * progressSpeed * Time.deltaTime;
                if (team1InsideList.Count == 0) return;
                progressRed -= team1InsideList.Count * progressSpeed * Time.deltaTime;
            }

        }
    }
}


