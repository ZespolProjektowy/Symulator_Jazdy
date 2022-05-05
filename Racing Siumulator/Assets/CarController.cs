using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public PrometeoCarController Prometeo;

    private Vector3 startPosition, startRotation;

    public float timeSinceStart = 0f;

    [Header("Fitness")]
    public float overallFitness;
    // value distance the most
    public float distanceMultiplier = 1.4f;
    // speed has a small impact on fitness
    public float avgSpeedMultiplier = 0.2f;

    private Vector3 lastPosition;
    private float totalDistanceTravelled;
    private float avgSpeed;

    private float[] sensors = new float[5];

    private void Awake(){
        startPosition = Prometeo.transform.position;
        startRotation = Prometeo.transform.eulerAngles;
    }

    public void Reset(){
        timeSinceStart = 0f;
        totalDistanceTravelled = 0f;
        avgSpeed = 0f;
        lastPosition = startPosition;
        overallFitness = 0f;
        Prometeo.transform.position = startPosition;
        Prometeo.transform.eulerAngles = startRotation;
    }

    private void OnCollisionEnter (Collision collision){
        Reset();
    }
    private int[] inputs = new int[4];
    private void MoveCar(){
        if(inputs[0] == 1){
          CancelInvoke("DecelerateCar");
          Prometeo.deceleratingCar = false;
          Prometeo.GoForward();
        }
        if(inputs[1] == 1){
          CancelInvoke("DecelerateCar");
          Prometeo.deceleratingCar = false;
          Prometeo.GoReverse();
        }

        if(inputs[2] == 1){
          Prometeo.TurnLeft();
        }
        if(inputs[3] == 1){
          Prometeo.TurnRight();
        }
        if((inputs[0] == 0) && (inputs[1] == 0)){
          Prometeo.ThrottleOff();
        }
        if((inputs[0] == 0) && (inputs[1] == 0) && !Prometeo.deceleratingCar){
          InvokeRepeating("DecelerateCar", 0f, 0.1f);
          Prometeo.deceleratingCar = true;
        }
        if((inputs[2] == 0) && (inputs[3] == 0) && Prometeo.steeringAxis != 0f){
          Prometeo.ResetSteeringAngle();
        }
    }

}
