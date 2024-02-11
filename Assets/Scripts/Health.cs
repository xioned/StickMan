using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public UnityEvent onHealthZero;
    public int health = 2;
    public Rigidbody2D[] bodypartRigidbody;
    public void DecreaseHealth(int amount)
    {
        health-=amount;
        if(health > 0) { return; }
        DoRagdoll();
        onHealthZero?.Invoke();
    }

    private void DoRagdoll()
    {
        for (int i = 0; i < bodypartRigidbody.Length; i++)
        {
            bodypartRigidbody[i].bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
