using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Pun.Demo.PunBasics;
using Photon.Pun.UtilityScripts;
using ExitGames.Client.Photon;
using MoreMountains.Feedbacks;


public class PlayerHealth : MonoBehaviour
{
    PhotonView pv;
    int health = 100;
    public int currentHealth;
    public int deadsOfParent;
    public TextMeshProUGUI killLog;
    public TextMeshProUGUI respawnText;
    public  MMFeedbacks camShake;
    float counter = 3f;
    [SerializeField] private ParticleSystem muzzle;
    public ParticleSystem explode;
    public ParticleSystem smoke;
    public GameObject respawnUI;
    public GameObject respawnButton;
   
    void Start()
    {
        pv = GetComponent<PhotonView>();
        currentHealth = health;
        respawnButton.SetActive(false);
    }
    


    [PunRPC]
    public void DamageToPlayer(bool half, bool dead, string player, Vector3 muzzlePos)
    {
       
        muzzle.transform.position = muzzlePos;
        muzzle.Play();
        //currentHealth -= damage;
        if (pv.IsMine)
        {
            camShake?.PlayFeedbacks();
        }
        if(half)
        {
            smoke.Play();
        }
        if (dead && half)
            {
          if (pv.IsMine)
            {
                killLog.text = "YOU KILLED BY " + player;
                 respawnUI.SetActive(true);
                respawnButton.SetActive(true);
                Hashtable hash = new Hashtable();
                deadsOfParent = Convert.ToInt32(GetComponent<PhotonView>().Owner.CustomProperties["Dead"]);
                hash.Add("Dead", deadsOfParent + 1);
                pv.Owner.SetCustomProperties(hash);
                GetComponentInParent<TankPlayer>().enabled = false;
                Cursor.visible = true;
            }
            
            explode.Play();
        }
        
       
    }

    public void Dead()
    {
        respawnText.text = "Respawn ýn" + counter;
        GetComponent<PhotonView>().RPC("Respawn", RpcTarget.All);
     
    }

    
   [PunRPC]
    public void Respawn()
    {
        currentHealth = health;
        Vector3 newSpawnPoint = GameManager.instance.spawnPoints[UnityEngine.Random.Range(0, 5)].transform.position;
        transform.position = newSpawnPoint;
        GetComponentInParent<TankPlayer>().enabled = true;
        explode.Stop();
        smoke.Stop();
        respawnButton.SetActive(false);
        respawnUI.SetActive(false);
        Cursor.visible = false;
    }

}
