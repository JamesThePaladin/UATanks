using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellScript : MonoBehaviour
{
    //variable for the isntigator of the shot
    public GameObject instigator; 
    //for the sound our bullet makes when we hit our target
    public AudioSource hitSound;

    //damage shell does, recieved from pawn
    [HideInInspector]
    public float damage; 

    private void OnTriggerEnter(Collider other)
    {
        hitSound.Play();
        //check if collider belongs to player or enemy
        if (other.CompareTag("Player") || (other.CompareTag("Enemy")))
        {
            //if it does get pawn component call take damage function
            other.GetComponent<Pawn>().TakeDamage(damage, instigator);
        }

        Destroy(this.gameObject);
    }
}
