using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterRoomPopup : MonoBehaviour
{
    private void Start()
    {
        if(GameEventSystem.current != null)
        {
            GameEventSystem.current.RoomEnterEvent += ClosePopUp;
        }
    }

    private void OnDestroy()
    {
        if (GameEventSystem.current != null)
        {
            GameEventSystem.current.RoomEnterEvent -= ClosePopUp;
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
            GameEventSystem.current.OnRoomEnter();
        }
        else
        {
            Debug.LogError("Player is null, cannot enter room!");
        }
    }
}
