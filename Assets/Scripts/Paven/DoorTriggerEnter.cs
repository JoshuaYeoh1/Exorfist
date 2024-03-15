using Unity.VisualScripting;
using UnityEngine;

public class DoorTriggerEnter : MonoBehaviour
{
    [SerializeField] GameObject popUpPrefab;
    GameObject popUpPrefabRef;
    public Transform currentRoomTransform;

    bool isPopupPoint, destroyOnContact;
    public int roomID { get; set; }

    void OnEnable()
    {
        GameEventSystem.Current.RoomEnterEvent += OnRoomEnter;
    }
    void OnDisable()
    {
        GameEventSystem.Current.RoomEnterEvent -= OnRoomEnter;
    }

    GameObject player;

    void OnTriggerEnter(Collider other)
    {
        ReliableOnTriggerExit.NotifyTriggerEnter(other, gameObject, OnTriggerExit);

        player = other.attachedRigidbody.gameObject;

        if(player)
        {
            if(!popUpPrefabRef)
            {
                popUpPrefabRef = Instantiate(popUpPrefab);
            }

            GameEventSystem.Current.OnDoorTriggerEnter(currentRoomTransform);
        }
    }

    void OnTriggerExit(Collider other)
    {
        ReliableOnTriggerExit.NotifyTriggerExit(other, gameObject);

        if(player)
        {
            if(popUpPrefabRef) Destroy(popUpPrefabRef);
        }
    }

    void OnRoomEnter()
    {
        if(popUpPrefabRef) Destroy(popUpPrefabRef);

        player.transform.position = currentRoomTransform.position;
    }
}
