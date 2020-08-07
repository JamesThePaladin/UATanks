using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatController : MonoBehaviour
{
    public PowerUpManager powMan;
    public PowerUp cheatPow;
    // Start is called before the first frame update
    void Start()
    {
        if (powMan == null) 
        {
            powMan = gameObject.GetComponent<PowerUpManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)) 
        {
            //Add powerup to the tank
            powMan.Add(cheatPow);
        }
    }
}
