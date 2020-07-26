using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellScript : MonoBehaviour
{
    public GameObject instigator; //variable for the isntigator of the shot

    public float damage; //damage shell does, recieved from pawn

    private void OnTriggerEnter(Collider other)
    {
        //check if collider belongs to player or enemy
        if (other.CompareTag("Player") || (other.CompareTag("Enemy")))
        {
            //if it does get pawn component call take damage function
            other.GetComponent<Pawn>().TakeDamage(damage, instigator);
        }

        Destroy(this.gameObject);
    }
}
