using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterRoomPopup : MonoBehaviour
{
    private void Start()
    {
        if(GameEventSystem.Current != null)
        {
            GameEventSystem.Current.RoomEnterEvent += ClosePopUp;
        }
    }

    private void OnDestroy()
    {
        if (GameEventSystem.Current != null)
        {
            GameEventSystem.Current.RoomEnterEvent -= ClosePopUp;
        }
    }

    public void ClosePopUp()
    {
        Destroy(gameObject);
    }

    public void EnterRoom()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player"); //find player
        if(player != null)
        {
            GameEventSystem.Current.OnRoomEnter();
        }
        else
        {
            Debug.LogError("Player is null, cannot enter room!");
        }
    }
}
