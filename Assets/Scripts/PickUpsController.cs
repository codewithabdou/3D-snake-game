using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpsController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(DestroyPickUp), Random.Range(3f, 6f));
        
    }

    void DestroyPickUp()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.FRUIT) || other.CompareTag(Tags.WALL)
            || other.CompareTag(Tags.BOMB) || other.CompareTag(Tags.TAIL)
            || other.CompareTag(Tags.HEAD) || other.CompareTag(Tags.NODE))
            Destroy(gameObject);
    }

}
