using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Breakable : MonoBehaviour
{
    public GameObject BrokenObjectRef;

    public void Break()
    {
        var instantiated = Instantiate(BrokenObjectRef, transform.position, transform.rotation);
        instantiated.SetActive(true);
        instantiated.transform.localScale = transform.localScale;
        foreach (var rb in instantiated.GetComponentsInChildren<Rigidbody>())
        {
            rb.AddExplosionForce(1000f, transform.position, 10f);
            rb.transform.DOScale(Vector3.zero, 5f);
        }
        Destroy(gameObject);
        Destroy(instantiated, 5f);
    }
}
