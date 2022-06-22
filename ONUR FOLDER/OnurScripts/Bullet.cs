using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using ExitGames.Client.Photon;

public class Bullet : MonoBehaviour
{
    public PhotonView pv;
    static int currentHealth = 100;
    private int damage = 20;
    int killsOfParent = 1;
    int deadsOfParent = 1;
    string playerNickName;
    bool half;
    bool full;
   static bool isTouch = false;
    
    public static Action<string> OwnerName;
    Rigidbody rb;

    private void OnEnable()
    {
        TankPlayer.PVOwner += SetOwner;
    }
    private void OnDisable()
    {
        TankPlayer.PVOwner -= SetOwner;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
   void Update()
    {
       
    }
    public void SetOwner(GameObject ownerOfBullet)
    {

        playerNickName = ownerOfBullet.GetComponent<PhotonView>().Owner.NickName;

    }
    private void OnTriggerEnter(Collider other)
    {
       
        
        if (!isTouch)
        {
           
            if (other.transform.gameObject.CompareTag("Player"))
            {
                isTouch = true;
                Debug.Log(isTouch);
                //rb.isKinematic = true;
                other.transform.gameObject.GetComponent<PlayerHealth>().currentHealth -= 20;
                Debug.Log(other.transform.gameObject.GetComponent<PlayerHealth>().currentHealth);
                pv.GetComponent<Rigidbody>().isKinematic = true;
                Vector3 bulletPos = gameObject.transform.position;
                
                if (other.transform.gameObject.GetComponent<PlayerHealth>().currentHealth <= 40)
                {
                    half = true;
                }


                if (other.transform.gameObject.GetComponent<PlayerHealth>().currentHealth <= 1)
                {
                    full = true;
                    ApplyKillToOwner();
                }

                other.gameObject.GetComponent<PhotonView>().RPC("DamageToPlayer", RpcTarget.All, half, full, playerNickName, bulletPos);
                if (pv.IsMine)
                {
                    PhotonNetwork.Destroy(gameObject);

                }
                half = false;
                full = false;
                isTouch = false;


            }
          
        }
        if (other.gameObject.CompareTag("Map"))
        {
          if (pv.IsMine)
            {
               
                PhotonNetwork.Destroy(gameObject);
                Destroy(gameObject, 1f);
            }
        }
        
    }

    private void ApplyKillToOwner()
    {
        if (pv.IsMine)
        {
            Hashtable hash = new Hashtable();
            killsOfParent = Convert.ToInt32(GetComponent<PhotonView>().Owner.CustomProperties["Kills"]);
            hash.Add("Kills", killsOfParent + 1);
            pv.Owner.SetCustomProperties(hash);
            Debug.Log("KÝLL : " + pv.Owner.NickName + " Has " + killsOfParent);

        }
    }

 
}
