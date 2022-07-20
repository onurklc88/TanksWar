using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using TMPro;
using Photon.Pun;
using MoreMountains.Feedbacks;
using UnityEngine.Events;
using ExitGames.Client.Photon;


public class TankPlayer : MonoBehaviour
{
   
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform PlayerCamera;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject PlayerCamera1;
    [SerializeField] private GameObject PlayerCamera2;
    [SerializeField] private GameObject playerList;
    [SerializeField] private TextMeshProUGUI nameList;
    [SerializeField] private GameObject turret;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private float speed;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private PhotonView photonView;
    [SerializeField] private PhotonView turretView;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private ParticleSystem fire;
    [SerializeField] private MMFeedbacks jumpFeedBack;
    private Component[] meshRenderer;
    [SerializeField] private Material[] playerColors;
    RaycastHit hit;
    Vector3 moveVeloticy;
    Vector3 jumpVelocity;
    Vector3 yPos;
    public float gravity = -5000;
    //public float jumpSpeed = -10;
    //public float jumpHeight = 2f;
    public static Action<GameObject> PVOwner;
    string allPlayerNames;
    float turnSmoothVelocity;
    float smoothTime = 0.1f;
    private float horizontal;
    private float vertical;
    bool canShoot = true;
    bool isGrounded;
    int health = 100;
    int currentHealth;
    
    private void Awake()
    {

        if (photonView.IsMine)
        {
            PlayerCamera1.SetActive(true);
            PlayerCamera2.SetActive(true);
        }
        
    }
    void Start()
    {
        currentHealth = health;
        //Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        controller = GetComponent<CharacterController>();
        playerNameText.text = photonView.Owner.NickName;
        playerNameText.color = Color.green;
        turretView = GetComponentInChildren<PhotonView>();
        //GetComponent<BoxCollider>().enabled = false;


    }
    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, 0.1f);
        yPos = transform.position;
       // GetComponent<BoxCollider>().enabled = true;
        if (photonView.IsMine)
        {
            
            //GetComponent<Collider>().gameObject.SetActive(true);
            if (yPos.y < -0.01f)
            {
                transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
            }
            MovePlayer();
            InputManager();
            if (!isGrounded)
            {
                Debug.Log("is not grounded");
                moveVeloticy.y += gravity * Time.deltaTime;
                controller.Move(moveVeloticy * Time.deltaTime);
            }
         
        }
        if (turretView.IsMine)
        {
            TurretRotation();
        }
     }
    void MovePlayer()
    {
       horizontal = Input.GetAxisRaw("Horizontal1");
       vertical = Input.GetAxisRaw("Vertical1");
       Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
       if (direction.magnitude >= 0.1f)
            {
                 GetComponent<BoxCollider>().enabled = true;
                float targetAngel = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + PlayerCamera.transform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngel, ref turnSmoothVelocity, smoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 moveDir = Quaternion.Euler(0f, targetAngel, 0f) * Vector3.forward;
                direction.y += gravity * Time.deltaTime;
                controller.Move(moveDir.normalized * speed * Time.deltaTime);
               
       }
       if (Input.GetKeyDown(KeyCode.Space))
       {
                Fire();
       }
        
            
    }
    
   
    void InputManager()
    {
        
        
        if (Input.GetKey(KeyCode.Tab))
        {
            playerList.SetActive(true);
            playerList.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "";

            
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                playerList.gameObject.GetComponentInChildren<TextMeshProUGUI>().text += p.NickName + " " +  p.CustomProperties["Kills"] + " " + p.CustomProperties["Dead"] + " \n";
               
            }
        }
        else
        {
            playerList.SetActive(false);
        }
        if (Input.GetKey(KeyCode.E) && isGrounded)
        {
            //Jump();
        }
     
        
     }
    /**
    private void Jump()
    {
        //Jump Function
        
       moveVeloticy.y = Mathf.Sqrt(jumpHeight * -1f * gravity);
       controller.Move(moveVeloticy * Time.deltaTime);
    }
    */
    void TurretRotation()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            turret.transform.Rotate(new Vector3(0, 80f, 0f) * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            turret.transform.Rotate(new Vector3(0, -80f, 0f) * Time.deltaTime);
        }
    }
    void Fire()
    {
        if (canShoot)
        {
            StartCoroutine(TimeBetweenShots());
            fire.Play();
            jumpFeedBack?.PlayFeedbacks();
            bullet = PhotonNetwork.Instantiate("Bullet", firePoint.transform.position, turret.transform.rotation, 0);
            PVOwner?.Invoke(gameObject);
            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
            bulletRigidbody.AddForce(bullet.transform.forward * 1800f);
        }
       

    }
    IEnumerator TimeBetweenShots()
    {
        canShoot = false;
        yield return new WaitForSeconds(1.5f);
        canShoot = true;
    }
    [PunRPC]
    public void SetPlayerColor(int color)
    {

       meshRenderer = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer renderer in meshRenderer)
         {
                renderer.material = playerColors[color];
                GetComponent<PhotonView>().Owner.CustomProperties["Color"] = color;
                
        }

    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(currentHealth);
        if (currentHealth <= 0)
        {
           Debug.Log("Dead");
        }
    }

    private void OnDrawGizmos()
    {
        Color color = Color.red;
        Gizmos.DrawRay(transform.position, -transform.up);
    }
}
    


