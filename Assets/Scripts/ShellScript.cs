using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellScript : MonoBehaviour
{
    public float damage; //damage shell does, recieved from pawn

    private void OnTriggerEnter(Collider other)
    {
            other.GetComponent<Pawn>().TakeDamage(damage);
            Destroy(this.gameObject);
    }
}
