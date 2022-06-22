using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField] private string VersionName = "0.1";
    [SerializeField] private GameObject userNameMenu;
    [SerializeField] private GameObject ConnectPanel;
    [SerializeField] private GameObject startButton;
   

    [SerializeField] private TMP_InputField UserNameInput;
    [SerializeField] private TMP_InputField CreateGameInput;
    [SerializeField] private TMP_InputField JoinGameInput;
    [SerializeField] private TextMeshProUGUI ErrorMessageText;



    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    private void Start()
    {
        userNameMenu.SetActive(true);
        startButton.SetActive(false);

    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Server'e Baðlanýldý.");
        PhotonNetwork.JoinLobby();
    }
    public void ChangeUserNameInput()
    {
        if(UserNameInput.text.Length >= 3)
        {
            startButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(false);
        }
        
    }
   
    public void SetUserName()
    {
        userNameMenu.SetActive(false);
        PhotonNetwork.NickName = UserNameInput.text;
    }

    public void CreateGame()
    {
        PhotonNetwork.CreateRoom(CreateGameInput.text, new RoomOptions() { MaxPlayers = 6 }, null);

    }

    
    public void JoinGame()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 6;
        PhotonNetwork.JoinOrCreateRoom(JoinGameInput.text, roomOptions, TypedLobby.Default);
    }
    public override void OnJoinedLobby()
    {
        PhotonNetwork.NickName = UserNameInput.text;
       
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        ErrorMessageText.text = message;
    }
   public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("MainScene");
    }
}
