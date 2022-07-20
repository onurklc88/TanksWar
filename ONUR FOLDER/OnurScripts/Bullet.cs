using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using ExitGames.Client.Photon;
using System.Collections;

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
    public bool isTouch = false;
    GameObject player;
    
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
        Shoot();
      
   }
    public void SetOwner(GameObject ownerOfBullet)
    {

        playerNickName = ownerOfBullet.GetComponent<PhotonView>().Owner.NickName;

    }

    void Shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, 1f))
        {
           
            if(hit.transform.gameObject.layer == 9 && !isTouch)
            {
                isTouch = true;
                hit.transform.gameObject.GetComponent<PhotonView>().RPC("DecreaseHealth", RpcTarget.All, 20);
            
            }
          
            
        }
    }
    
    
    private void OnTriggerEnter(Collider other)
    {
       
         
            if (other.transform.gameObject.CompareTag("Player"))
            {
                if (pv.IsMine)
                {
                    PhotonNetwork.Destroy(gameObject);
                }
              
            }
          
        
      
        
    }
    
    private void ApplyDamageToPlayer(GameObject player)
    {
        if (isTouch)
        {
            Debug.Log("girdi");
            player.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, 50);
            PhotonNetwork.Destroy(this.gameObject);
           
        }
        isTouch = false;
    }

 
    private void ApplyKillToOwner()
    {
        /*
        if (pv.IsMine)
        {
            Hashtable hash = new Hashtable();
            killsOfParent = Convert.ToInt32(GetComponent<PhotonView>().Owner.CustomProperties["Kills"]);
            hash.Add("Kills", killsOfParent + 1);
            pv.Owner.SetCustomProperties(hash);
            Debug.Log("KÝLL : " + pv.Owner.NickName + " Has " + killsOfParent);

        }
        */
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 direction = transform.TransformDirection(Vector3.forward) * 1f;
        Gizmos.DrawRay(transform.position, direction);
    }

}
