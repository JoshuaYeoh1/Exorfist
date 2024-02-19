using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterRoomPopup : MonoBehaviour
{
    private void Start()
    {
        if(GameEventSystem.current != null)
        {
            GameEventSystem.current.OnRoomEntered += ClosePopUp;
        }
    }

    private void OnDestroy()
    {
        if (GameEventSystem.current != null)
        {
            GameEventSystem.current.OnRoomEntered -= ClosePopUp;
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
            GameEventSystem.current.roomEntered();
        }
        else
        {
            Debug.LogError("Player is null, cannot enter room!");
        }
    }
}
