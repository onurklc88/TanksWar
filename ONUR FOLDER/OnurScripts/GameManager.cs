using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class GameManager : MonoBehaviourPun
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject GameCanvas;
    [SerializeField] private GameObject SceneCamera;
    private GameObject player;
    public GameObject[] spawnPoints;
    [SerializeField]private PhotonView PV;
    static List<int> uniqueColors = new List<int>();
    public static GameManager instance;
    int randomValue;
    int asd;
    int playerProp;

    private void Awake()
    {
        instance = this;
        GameCanvas.SetActive(true);
    }

    private void Start()
    {
        PV = GetComponent<PhotonView>();
    }
    
    public void SpawnPlayer()
    {
        asd = UnityEngine.Random.Range(0, 5);
        player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoints[asd].gameObject.transform.position, Quaternion.identity, 0);
        while (GetRandomValue())
        {
            GetRandomValue();
        }
        player.GetComponent<PhotonView>().Owner.CustomProperties["Color"] = randomValue;
        player.GetComponent<PhotonView>().RPC("SetPlayerColor", RpcTarget.AllBuffered, randomValue);
        GameCanvas.SetActive(false);
        SceneCamera.SetActive(false);
        
    }
    public static class MultiplayerHelper
    {
        public static int GetPlayerCount()
        {
            if (PhotonNetwork.CurrentRoom != null)
            {
             return PhotonNetwork.CurrentRoom.PlayerCount;
            }
            return 0;
        }
    }
   
  bool GetRandomValue()
    {
        randomValue = UnityEngine.Random.Range(0, 6);
        bool isNumberExist = false;
        foreach (Player p in PhotonNetwork.PlayerList)
        {

            if (p.CustomProperties["Color"] != null)
            {
               int color = (int)p.CustomProperties["Color"];

               
                if (randomValue == color)
                {
                    isNumberExist = true;
                    break;
                }
                
            }
            
        }
        return isNumberExist;
    }
  
}