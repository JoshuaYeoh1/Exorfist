using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterRoomPopup : MonoBehaviour
{
    private void Start()
    {
        if(GameEventSystem.current != null)
        {
<<<<<<< HEAD
            GameEventSystem.current.OnRoomEntered += ClosePopUp;
=======
            GameEventSystem.current.RoomEnterEvent += ClosePopUp;
>>>>>>> main
        }
    }

    private void OnDestroy()
    {
        if (GameEventSystem.current != null)
        {
<<<<<<< HEAD
            GameEventSystem.current.OnRoomEntered -= ClosePopUp;
=======
            GameEventSystem.current.RoomEnterEvent -= ClosePopUp;
>>>>>>> main
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
<<<<<<< HEAD
            GameEventSystem.current.roomEntered();
=======
            GameEventSystem.current.OnRoomEnter();
>>>>>>> main
        }
        else
        {
            Debug.LogError("Player is null, cannot enter room!");
        }
    }
}
