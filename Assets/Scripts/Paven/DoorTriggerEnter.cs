using Unity.VisualScripting;
using UnityEngine;

public class DoorTriggerEnter : MonoBehaviour
{
    [SerializeField] GameObject popUpPrefab;
    GameObject popUpPrefabRef;
    public Transform currentRoomTransform;

    public bool isPopupPoint, destroyOnContact;
    public int roomID { get; set; }

    void OnEnable()
    {
        GameEventSystem.Current.RoomEnterEvent += OnRoomEnter;
    }
    void OnDisable()
    {
        GameEventSystem.Current.RoomEnterEvent -= OnRoomEnter;
    }

    void OnTriggerEnter(Collider other)
    {
        ReliableOnTriggerExit.NotifyTriggerEnter(other, gameObject, OnTriggerExit);

        Rigidbody otherRb = other.attachedRigidbody;

        if(otherRb)
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

        if(otherRb)
        {
            if (isPopupPoint)
            {
                if (!popUpPrefabRef)
                {
                    Destroy(popUpPrefabRef);
                }
            }
            else
            {

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
