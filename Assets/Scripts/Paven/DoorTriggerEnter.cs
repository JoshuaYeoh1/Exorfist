using Unity.VisualScripting;
using UnityEngine;

public class DoorTriggerEnter : MonoBehaviour
{
    [SerializeField] GameObject popUpPrefab;
    GameObject popUpPrefabRef;
    public Transform currentRoomTransform;

    public bool isPopupPoint, destroyOnContact;
    public int roomID { get; set; }

    void Start()
    {
        GameEventSystem.Current.RoomEnterEvent += OnRoomEnter;
    }
    void OnDestroy()
    {
        GameEventSystem.Current.RoomEnterEvent -= OnRoomEnter;
    }

    void OnTriggerEnter(Collider other)
    {
        ReliableOnTriggerExit.NotifyTriggerEnter(other, gameObject, OnTriggerExit);

        Rigidbody otherRb = other.attachedRigidbody;

        if(otherRb && otherRb.gameObject.tag=="Player")
        {
            OnDoorTriggerEnter();

            if (isPopupPoint)
            {
                if(!popUpPrefabRef)
                {
                    popUpPrefabRef = Instantiate(popUpPrefab);
                    if(destroyOnContact)
                    {
                        Destroy(gameObject);
                    }
                }
            }
            else
            {
                GameEventSystem.Current?.OnRoomEnter();
                if (destroyOnContact)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        ReliableOnTriggerExit.NotifyTriggerExit(other, gameObject);

        Rigidbody otherRb = other.attachedRigidbody;

        if(otherRb && otherRb.gameObject.tag=="Player")
        {
            if(isPopupPoint)
            {
                if(popUpPrefabRef) Destroy(popUpPrefabRef);
            }
        }
    }

    void OnRoomEnter()
    {
        if(popUpPrefabRef) Destroy(popUpPrefabRef);
    }

    void OnDoorTriggerEnter()
    {
        GameEventSystem.Current.OnDoorTriggerEnter(currentRoomTransform);
    }
}
