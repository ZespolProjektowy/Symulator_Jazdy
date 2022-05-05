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

}
