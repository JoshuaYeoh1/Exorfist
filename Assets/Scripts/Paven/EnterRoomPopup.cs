using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterRoomPopup : MonoBehaviour
{
    void Start()
    {
        GameEventSystem.Current.RoomEnterEvent += ClosePopUp;
    }
    void OnDestroy()
    {
        GameEventSystem.Current.RoomEnterEvent -= ClosePopUp;
    }
    
    public void ClosePopUp()
    {
        Destroy(gameObject);
    }

    public void EnterRoom()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player"); //find player

        if(player)
        {
            GameEventSystem.Current.OnRoomEnter();
        }
        else
        {
            Debug.LogError("Player is null, cannot enter room!");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
