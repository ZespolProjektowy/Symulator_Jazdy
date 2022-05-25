using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public PrometeoCarController Prometeo;

    private NeuralNetwork network;

    private Vector3 startPosition;

    private Quaternion startRotation;

    public float timeSinceStart = 0f;

    [Header("Fitness")]
    public float overallFitness;

    // value distance the most
    public float distanceMultiplier = 1.4f;

    // speed has a small impact on fitness
    public float avgSpeedMultiplier = 0.2f;

    public float sensorMultiplier = 0.1f;

    public GeneticAlgorithm geneticAlgorithm;

    public TrackCheckpoint trackCheckpoint;

    [Header("Hyperparameters")]
    public int layers = 1;

    public int neurons = 24;

    public int epoch = 200;

    private Vector3 lastPosition;

    private float totalDistanceTravelled = 0f;

    private float avgSpeed;

    [Header("Results")]
    public int generationCount = 0;

    public int solutionCount = 0;

    [Range(-1f, 1f)]
    public float

            a,
            t;

    private List<int> generationList = new List<int>();

    private List<int> solutionList = new List<int>();

    private string filePath;

    private float[] outputs = new float[2];

    private float[] sensors = new float[7];

    public void Awake()
    {
     geneticAlgorithm = GameObject.Find("Genetic Algorithm").GetComponent<GeneticAlgorithm>();
        startPosition = Prometeo.transform.position;
        startRotation = Prometeo.transform.rotation;
        lastPosition = startPosition;

        network = GetComponent<NeuralNetwork>();
    }

    public void ResetNetwork(NeuralNetwork neural)
    {
        network = neural;
        Reset();
    }

    public void Reset()
    {
        //Prometeo.StopCar();
        timeSinceStart = 0f;
        totalDistanceTravelled = 0f;
        avgSpeed = 0f;
        lastPosition = startPosition;
        overallFitness = 0f;
        Prometeo.transform.position = startPosition;
        Prometeo.transform.rotation = startRotation;

        if (Prometeo.carRigidbody != null)
        {
            Prometeo.carRigidbody.velocity = Vector3.zero;
            Prometeo.carRigidbody.angularVelocity = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        InputSensors();

        lastPosition = Prometeo.transform.position;

         (outputs[0], outputs[1]) = network.StartNetwork(sensors[0], sensors[1], sensors[2], sensors[3],
         sensors[4], sensors[5], sensors[6]);
        Prometeo.setOutputs(outputs);
        MoveCarBot (outputs);

        //Prometeo.GoForward();
        //MoveCar(outputs);
        timeSinceStart += Time.deltaTime;

        CalculateFitness();
    }

    private Vector3 moveInput;

    public void MoveCarBot(float[] output)
    {
        moveInput =
            Vector3
                .Lerp(Vector3.zero,
                new Vector3(0, 0, output[0] * 11.4f),
                0.02f);
        moveInput = transform.TransformDirection(moveInput);
        transform.position += moveInput;
        transform.eulerAngles += new Vector3(0, (output[1] * 45) * 0.02f, 0);
    }

    private void CalculateFitness()
    {
        totalDistanceTravelled +=
            Vector3.Distance(Prometeo.transform.position, lastPosition);

        lastPosition = Prometeo.transform.position;

        avgSpeed = totalDistanceTravelled / timeSinceStart;

        overallFitness =
            (totalDistanceTravelled * distanceMultiplier) +
            (avgSpeed * avgSpeedMultiplier) +
            (
            (
            (sensors[0] + sensors[1] + sensors[2] + sensors[3] + sensors[4]) / 5
            ) *
            sensorMultiplier
            );

        if (
            (timeSinceStart > 20 && overallFitness < 50) ||
            overallFitness > 1350
        )
        {
           // GameObject
            //    .FindObjectOfType<GeneticAlgorithm>()
            //    .Death(overallFitness, network);
        }

        // if ((timeSinceStart > 20 && overallFitness < 50) || overallFitness > 1350)
        // {
        //     if(overallFitness > 1350)
        //     {
        //         generationCount = geneticAlgorithm.getGenCount() + 1;
        //         solutionCount++;
        //         generationList.Add(generationCount);
        //         solutionList.Add(solutionCount);
        //     }
        //     GameObject.FindObjectOfType<GeneticAlgorithm>().Death(overallFitness, network);
        // }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!(collision.gameObject.tag == "Car"))
        {
             GameObject.FindObjectOfType<GeneticAlgorithm>().Death(overallFitness, network);
            //Reset();
            trackCheckpoint=GameObject.Find("Road").GetComponent<TrackCheckpoint>();
            trackCheckpoint.Reset();
        }
    }

    private Color returnHitColor(float hit)
    {
        if (hit <= 9)
        {
            return Color.red;
        }
        else if (hit <= 15 && hit > 9)
        {
            return new Color(255f, 165f, 0f);
        }
        else
        {
            return Color.green;
        }
    }

    private void InputSensors()
    {
        Vector3 a = (Prometeo.transform.forward + Prometeo.transform.right);
        Vector3 b = (Prometeo.transform.forward);
        Vector3 c = (Prometeo.transform.forward - Prometeo.transform.right);
        Vector3 d = (Prometeo.transform.forward + Prometeo.transform.right / 2);
        Vector3 e = (Prometeo.transform.forward - Prometeo.transform.right / 2);
        Vector3 f = (Prometeo.transform.forward + Prometeo.transform.right / 5);
        Vector3 g = (Prometeo.transform.forward - Prometeo.transform.right / 5);

        Ray r = new Ray(Prometeo.transform.position, a);
        RaycastHit hit;

        if (Physics.Raycast(r, out hit))
        {
            sensors[0] = hit.distance / 20;

            //Debug.Log("A: " + hit.distance);
            Debug
                .DrawRay(Prometeo.transform.position,
                a * hit.distance,
                returnHitColor(hit.distance));
        }

        r.direction = b;

        if (Physics.Raycast(r, out hit))
        {
            sensors[1] = hit.distance / 20;

            //Debug.Log("B: " + hit.distance);
            Debug
                .DrawRay(Prometeo.transform.position,
                b * hit.distance,
                returnHitColor(hit.distance - 8));
        }

        r.direction = c;

        if (Physics.Raycast(r, out hit))
        {
            sensors[2] = hit.distance / 20;

            //Debug.Log("C: " + hit.distance);
            Debug
                .DrawRay(Prometeo.transform.position,
                c * hit.distance,
                returnHitColor(hit.distance));
        }

        r.direction = d;

        if (Physics.Raycast(r, out hit))
        {
            sensors[3] = hit.distance / 20;

            //Debug.Log("D: " + hit.distance);
            Debug
                .DrawRay(Prometeo.transform.position,
                d * hit.distance,
                returnHitColor(hit.distance));
        }

        r.direction = e;

        if (Physics.Raycast(r, out hit))
        {
            sensors[4] = hit.distance / 20;

            //Debug.Log("E: " + hit.distance);
            Debug
                .DrawRay(Prometeo.transform.position,
                e * hit.distance,
                returnHitColor(hit.distance));
        }

        r.direction = f;

        if (Physics.Raycast(r, out hit))
        {
            sensors[5] = hit.distance / 20;

            //Debug.Log("F: " + hit.distance);
            Debug
                .DrawRay(Prometeo.transform.position,
                f * hit.distance,
                returnHitColor(hit.distance - 2));
        }

        r.direction = g;

        if (Physics.Raycast(r, out hit))
        {
            sensors[6] = hit.distance / 20;

            //Debug.Log("G: " + hit.distance);
            Debug
                .DrawRay(Prometeo.transform.position,
                g * hit.distance,
                returnHitColor(hit.distance - 2));
        }
    }

    public void decreaseEpoch()
    {
        epoch--;
    }

    public void checkEpoch()
    {
        if (this.epoch == 0)
        {
            //writeToFile();
            //Time.timeScale = 0;
        }
    }

    private void writeToFile()
    {
        float ratio = 0;

        filePath = getPath();
        StreamWriter writer = new StreamWriter(filePath);
        writer.WriteLine("Solution Count,Generation Count,Ratio");

        for (
            int i = 0;
            i < Math.Max(solutionList.Count, generationList.Count);
            i++
        )
        {
            if (i < solutionList.Count)
            {
                writer.Write(solutionList[i]);
            }
            writer.Write(",");

            if (i < generationList.Count)
            {
                writer.Write(generationList[i]);
            }
            writer.Write(",");
            ratio = (float) solutionList[i] / (float) generationList[i];
            writer.Write (ratio);
            writer.Write(System.Environment.NewLine);
        }

        writer.Flush();
        writer.Close();
    }

    private string getPath()
    {
        return Application.dataPath + "sols.csv";
    }

    public void CheckpointHit()
    {
        Debug.Log("ale zajebisty fitness");
    }
}
