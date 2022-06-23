using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public PrometeoCarController Prometeo;

    private GlobalSettings globalSettings;

    private NeuralNetwork network;
    private NeuralNetwork_BP network2;

    private Vector3 startPosition;

    private Quaternion startRotation;

    public GeneticAlgorithm geneticAlgorithm;

    public BackPropagation backPropagation;

    public TrackCheckpoint trackCheckpoint;

    private Vector3 lastPosition;

    private List<int> generationList = new List<int>();

    private List<int> solutionList = new List<int>();

    private string filePath;

    private double[] outputs = new double[2];
    private float[] outputs2 = new float[2];

    private double[] sensors = new double[7];

    [SerializeField] float FitnessUnchangedDie = 10; // The number of seconds to wait before checking if the fitness didn't increase
    [SerializeField] public int Fitness = 0;

    public static NeuralNetwork NextNetwork = new NeuralNetwork(new uint[] { 7, 25, 25, 2 }, null); // public NeuralNetwork that refers to the next neural network to be set to the next instantiated car

    public string TheGuid { get; private set; } // The Unique ID of the current car

    public NeuralNetwork TheNetwork { get; private set; } // The NeuralNetwork of the current car

    public float timeSinceStart = 0f;
    // value distance the most
    public float distanceMultiplier = 1.4f;

    // speed has a small impact on fitness
    public float avgSpeedMultiplier = 0.2f;

    public float sensorMultiplier = 0.1f;

    private float totalDistanceTravelled = 0f;

    private float avgSpeed;

    public void Awake()
    {
        //init global settings
        globalSettings = GameObject.Find("GlobalSettings").GetComponent<GlobalSettings>();

        geneticAlgorithm = GameObject.Find("Genetic Algorithm").GetComponent<GeneticAlgorithm>();
        backPropagation = GameObject.Find("Back Propagation").GetComponent<BackPropagation>();
        startPosition = Prometeo.transform.position;
        startRotation = Prometeo.transform.rotation;
        lastPosition = startPosition;

        network2 = GetComponent<NeuralNetwork_BP>();
        using (StreamWriter writetext = new StreamWriter("write.txt"))
        {

        }

        TheGuid = Guid.NewGuid().ToString(); // Assigns a new Unique ID for the current car

        TheNetwork = NextNetwork; // Sets the current network to the Next Network
        NextNetwork = new NeuralNetwork(NextNetwork.Topology, null); // Make sure the Next Network is reassigned to avoid having another car use the same network
        StartCoroutine(IsNotImproving()); // Start checking if the score stayed the same for a lot of time

    }

    // Checks each few seconds if the car didn't make any improvement
    IEnumerator IsNotImproving()
    {
        while (true)
        {
            int OldFitness = Fitness; // Save the initial fitness
            yield return new WaitForSeconds(FitnessUnchangedDie); // Wait for some time
            if (OldFitness == Fitness)
            {
                GeneticAlgorithm.Singleton.CarDead(this, Fitness); // Tell the Evolution Manager that the car is dead
                gameObject.SetActive(false); // Make sure the car is inactive
            }
        }
    }

    public void Reset()
    {
        //Prometeo.StopCar();
        timeSinceStart = 0f;
        totalDistanceTravelled = 0f;
        avgSpeed = 0f;
        lastPosition = startPosition;
        Prometeo.transform.position = startPosition;
        Prometeo.transform.rotation = startRotation;

        if (Prometeo.carRigidbody != null)
        {
            Prometeo.carRigidbody.velocity = Vector3.zero;
            Prometeo.carRigidbody.angularVelocity = Vector3.zero;
        }
    }

    private int accKey;
    private int strKey;

    private void FixedUpdate()
    {
        InputSensors();

        lastPosition = Prometeo.transform.position;

        // Feed through the network
        outputs = TheNetwork.FeedForward(sensors);

        outputs2[0] = backPropagation.Calculate(sensors, 0);
        outputs2[1] = backPropagation.Calculate(sensors, 1);
        Prometeo.setOutputs2(outputs2);
        //Prometeo.setOutputs(outputs);
        if (globalSettings.useAiControls)
        {
            MoveCarBot(outputs);
        }
        else
        {
            accKey = -1;
            strKey = 0;
            if (Input.GetKey(KeyCode.W))
            {
                accKey = 1;
            }

            if (Input.GetKey(KeyCode.A))
            {
                strKey = -1;
            }
            if (Input.GetKey(KeyCode.D))
            {
                strKey = 1;
            }
            using (StreamWriter writetext = new StreamWriter("write.txt", append: true))
            {
                writetext.WriteLine(sensors[0].ToString() + " " + sensors[1].ToString() + " " + sensors[2].ToString() + " " + sensors[3].ToString() + " " + sensors[4].ToString() + " " + sensors[5].ToString() + " " + sensors[6].ToString() + " " + accKey.ToString() + ",0 " + strKey.ToString() + ",0");
            }
        }
        CalculateFitness();

    }

    private void CalculateFitness()
    {
        totalDistanceTravelled +=
            Vector3.Distance(Prometeo.transform.position, lastPosition);

        lastPosition = Prometeo.transform.position;

        avgSpeed = totalDistanceTravelled / timeSinceStart;

        //calculate fitness based on total distance travelled
        Fitness = (int)(totalDistanceTravelled * distanceMultiplier);
    }

    private Vector3 moveInput;

    public void MoveCarBot(double[] output)
    {
        moveInput =
            Vector3
                .Lerp(Vector3.zero,
                new Vector3(0, 0, (float)(output[0] * 11.4f)),
                0.02f);
        moveInput = transform.TransformDirection(moveInput);
        transform.position += moveInput;
        transform.eulerAngles += new Vector3(0, (float)((output[1] * 45) * 0.02f), 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!(collision.gameObject.tag == "Car"))
        {
            if (globalSettings.useAiControls)
            {
                GeneticAlgorithm.Singleton.CarDead(this, Fitness); // Tell the Evolution Manager that the car is dead
                gameObject.SetActive(false); // Make sure the car is inactive
            }
            else
            {
                Reset();
            }
            //trackCheckpoint = GameObject.Find("Road").GetComponent<TrackCheckpoint>();
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
            // Debug
            //     .DrawRay(Prometeo.transform.position,
            //     a * hit.distance,
            //     returnHitColor(hit.distance));
        }

        r.direction = b;

        if (Physics.Raycast(r, out hit))
        {
            sensors[1] = hit.distance / 20;

            //Debug.Log("B: " + hit.distance);
            Debug
                .DrawRay(Prometeo.transform.position,
                b * hit.distance,
                returnHitColor(hit.distance));
        }

        r.direction = c;

        if (Physics.Raycast(r, out hit))
        {
            sensors[2] = hit.distance / 20;

            //Debug.Log("C: " + hit.distance);
            // Debug
            //     .DrawRay(Prometeo.transform.position,
            //     c * hit.distance,
            //     returnHitColor(hit.distance));
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
                returnHitColor(hit.distance));
        }

        r.direction = g;

        if (Physics.Raycast(r, out hit))
        {
            sensors[6] = hit.distance / 20;

            //Debug.Log("G: " + hit.distance);
            Debug
                .DrawRay(Prometeo.transform.position,
                g * hit.distance,
                returnHitColor(hit.distance));
        }
    }

    public void CheckpointHit()
    {
        //Fitness++;
    }
}
