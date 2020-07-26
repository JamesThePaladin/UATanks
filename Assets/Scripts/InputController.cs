using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    //input schemes
    public enum InputScheme {WASD, arrowKeys};
    public InputScheme input = InputScheme.WASD;

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
        //switch statement for control schemes
        switch (input) 
        {
            case InputScheme.WASD:
                if (Input.GetKey(KeyCode.W)) 
                {
                    motor.Move(pawn.moveSpeed);
                }
                if (Input.GetKey(KeyCode.S)) 
                {
                    motor.Move(-pawn.moveSpeed);
                }
                if (Input.GetKey(KeyCode.A))
                {
                    motor.Turn(-pawn.turnSpeed);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    motor.Turn(pawn.turnSpeed);
                }
                if (Input.GetKey(KeyCode.Space)) 
                {
                    pawn.Shoot(pawn.shotForce);
                }
                break;
            case InputScheme.arrowKeys:
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    motor.Move(pawn.moveSpeed);
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    motor.Move(-pawn.moveSpeed);
                }
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    motor.Turn(-pawn.turnSpeed);
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    motor.Turn(pawn.turnSpeed);
                }
                if (Input.GetKey(KeyCode.RightAlt))
                {
                    pawn.Shoot(pawn.shotForce);
                }
                break;
        }
    }
}
