using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WaypointScript : MonoBehaviour
{
    [SerializeField] private float yOffset;
    [SerializeField] private float lifetime, moveTime;
    private Vector3 upPos, originalPos;

    private void Start()
    {
        upPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + yOffset, gameObject.transform.position.z);
        originalPos = gameObject.transform.position;
        StartCoroutine(DestroyAfterTime());
    }

    private void Update()
    {
        if (upPos != null && originalPos != null)
        {
            if (gameObject.transform.position == upPos)
            {
                LeanTween.move(gameObject, originalPos, moveTime).setEaseInOutSine();
            }
            else if (gameObject.transform.position == originalPos)
            {
                LeanTween.move(gameObject, upPos, moveTime).setEaseInOutSine();
            }
        }
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
