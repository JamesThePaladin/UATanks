using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{
    //components
    public TankMotor motor;
    public Pawn pawn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        motor.Move(pawn.moveSpeed);
        motor.Turn(pawn.turnSpeed);

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            pawn.Shoot(pawn.shotForce);
        }
    }
}
