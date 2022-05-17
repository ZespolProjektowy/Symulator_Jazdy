using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public PrometeoCarController Prometeo;

    private Vector3 startPosition;
    private Quaternion startRotation;

    public double timeSinceStart = 0f;

    [Header("Fitness")]
    public double overallFitness;
    // value distance the most
    public double distanceMultiplier = 1.4f;
    // speed has a small impact on fitness
    public double avgSpeedMultiplier = 0.2f;
    public double sensorMultiplier = 0.1f;

    private Vector3 lastPosition;
    private double totalDistanceTravelled = 0f;
    private double avgSpeed;

    

    private int[] inputs = new int[4];

    public void Awake(){
<<<<<<< Updated upstream
        startPosition = Prometeo.transform.position;
        startRotation = Prometeo.transform.rotation;
        lastPosition = startPosition;
=======
       // geneticAlgorithm = GameObject.Find("Genetic Algorithm").GetComponent<GeneticAlgorithm>();

        startPosition = Prometeo.transform.position;
        startRotation = Prometeo.transform.rotation;
        lastPosition = startPosition;

       // network = GetComponent<NeuralNetwork>();
    }

    public void ResetNetwork(NeuralNetwork neural)
    {
        network = neural;
        Reset();
>>>>>>> Stashed changes
    }

    public void Reset(){
        //Prometeo.StopCar();
        timeSinceStart = 0f;
        totalDistanceTravelled = 0f;
        avgSpeed = 0f;
        lastPosition = startPosition;
        overallFitness = 0f;
        Prometeo.transform.position = startPosition;
        Prometeo.transform.rotation = startRotation;

        if (Prometeo.carRigidbody != null) {
        Prometeo.carRigidbody.velocity = Vector3.zero;
        Prometeo.carRigidbody.angularVelocity = Vector3.zero;
      }
    }

    private void FixedUpdate(){
        InputSensors();

        //MoveCar(inputs);

        timeSinceStart += Time.deltaTime;

        CalculateFitness();
    }

    private void CalculateFitness() {

       Debug.Log("x: " + startPosition.x);
       Debug.Log("z: " + startPosition.z);

       Debug.Log("Lx: " + lastPosition.x);
       Debug.Log("Lz: " + lastPosition.z);


      totalDistanceTravelled += Vector3.Distance(Prometeo.transform.position,lastPosition);

      lastPosition = Prometeo.transform.position;
      
      avgSpeed = totalDistanceTravelled/timeSinceStart;

       overallFitness = 
       (totalDistanceTravelled*distanceMultiplier)
       +(avgSpeed*avgSpeedMultiplier)
       +(((sensors[0]+sensors[1]+sensors[2]+sensors[3]+sensors[4])/5)*sensorMultiplier);

       

       Debug.Log("total: " + totalDistanceTravelled);

        // if (timeSinceStart > 20 && overallFitness < 40) {
        //     Reset();
        // }

        // if (overallFitness >= 1000) {
        //     //Saves network to a JSON
        //     Reset();
        // }

    }


    

    private void OnCollisionEnter(Collision collision)
    {
      if(!(collision.gameObject.tag == "Car"))
      {
<<<<<<< Updated upstream
        Reset();
=======
          Reset();
        GameObject.FindObjectOfType<GeneticAlgorithm>().Death(overallFitness, network);
        
>>>>>>> Stashed changes
      }
        
    }

    private Color returnHitColor(float hit){
      if(hit <= 9){
          return Color.red;
        }
        else if(hit <= 15 && hit > 9){
          return new Color(255f, 165f, 0f);
        }
        else {
          return Color.green;
        }
    } 

    double[] sensors = new double[7];
    private void InputSensors() {

        Vector3 a = (Prometeo.transform.forward+Prometeo.transform.right);
        Vector3 b = (Prometeo.transform.forward);
        Vector3 c = (Prometeo.transform.forward-Prometeo.transform.right);
        Vector3 d = (Prometeo.transform.forward+Prometeo.transform.right/2);
        Vector3 e = (Prometeo.transform.forward-Prometeo.transform.right/2);
        Vector3 f = (Prometeo.transform.forward+Prometeo.transform.right/5);
        Vector3 g = (Prometeo.transform.forward-Prometeo.transform.right/5);

        Ray r = new Ray(Prometeo.transform.position,a);
        RaycastHit hit;

        if (Physics.Raycast(r, out hit)) {
            sensors[0] = hit.distance/20;
            //Debug.Log("A: " + hit.distance);
            Debug.DrawRay(Prometeo.transform.position, a*hit.distance, returnHitColor(hit.distance));
            
        }

        

        r.direction = b;

        if (Physics.Raycast(r, out hit)) {
            sensors[1] = hit.distance/20;
            //Debug.Log("B: " + hit.distance);
            Debug.DrawRay(Prometeo.transform.position, b*hit.distance, returnHitColor(hit.distance-8));
            
        }

        r.direction = c;

        if (Physics.Raycast(r, out hit)) {
            sensors[2] = hit.distance/20;
            //Debug.Log("C: " + hit.distance);
            Debug.DrawRay(Prometeo.transform.position, c*hit.distance, returnHitColor(hit.distance));
           
        }

        r.direction = d;

        if (Physics.Raycast(r, out hit)) {
            sensors[3] = hit.distance/20;
            //Debug.Log("D: " + hit.distance);
            Debug.DrawRay(Prometeo.transform.position, d*hit.distance, returnHitColor(hit.distance));
           
        }

        r.direction = e;

        if (Physics.Raycast(r, out hit)) {
            sensors[4] = hit.distance/20;
            //Debug.Log("E: " + hit.distance);
            Debug.DrawRay(Prometeo.transform.position, e*hit.distance, returnHitColor(hit.distance));
           
        }

        r.direction = f;

        if (Physics.Raycast(r, out hit)) {
            sensors[5] = hit.distance/20;
            //Debug.Log("F: " + hit.distance);
            Debug.DrawRay(Prometeo.transform.position, f*hit.distance, returnHitColor(hit.distance-2));
           
        }

        r.direction = g;

        if (Physics.Raycast(r, out hit)) {
            sensors[6] = hit.distance/20;
            //Debug.Log("G: " + hit.distance);
            Debug.DrawRay(Prometeo.transform.position, g*hit.distance, returnHitColor(hit.distance-2));
           
        }

    }
    private void MoveCar(int[] input){

        if(input[0] == 1){
          CancelInvoke("DecelerateCar");
          Prometeo.deceleratingCar = false;
          Prometeo.GoForward();
        }
        if(input[1] == 1){
          CancelInvoke("DecelerateCar");
          Prometeo.deceleratingCar = false;
          Prometeo.GoReverse();
        }

        if(input[2] == 1){
          Prometeo.TurnLeft();
        }
        if(input[3] == 1){
          Prometeo.TurnRight();
        }

        if((input[0] == 0) && (input[1] == 0)){
          Prometeo.ThrottleOff();
        }
        if((input[0] == 0) && (input[1] == 0) && !Prometeo.deceleratingCar){
          InvokeRepeating("DecelerateCar", 0f, 0.1f);
          Prometeo.deceleratingCar = true;
        }
        if((input[2] == 0) && (input[3] == 0) && Prometeo.steeringAxis != 0f){
          Prometeo.ResetSteeringAngle();
        }

    }

}
