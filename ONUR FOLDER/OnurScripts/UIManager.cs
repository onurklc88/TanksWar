using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{

    [SerializeField] private GameObject playerList;
    [SerializeField] private TextMeshProUGUI nameList;

     private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
     
    }
    public void ShowPlayerList(string playerNames, bool tabInput)
    {
        nameList.text = "";
        if (tabInput)
        {
            playerList.SetActive(true);
            nameList.text = playerNames;
        }
        else
        {
            playerList.SetActive(false);
        }
       

    }
    
}
