using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deneme : MonoBehaviour
{
    List<int> validNumber = new List<int>();
    int randomValue;
    void Start()
    {
     
           
}

    // Update is called once per frame
    void Update()
    {
        randomValue = UnityEngine.Random.Range(0, 6);
        if (!validNumber.Contains(randomValue))
        {
            Debug.Log(randomValue);
            validNumber.Add(randomValue);
        }
        else
        {
            Debug.Log("asdasd");
            return;

        }
    }
}
