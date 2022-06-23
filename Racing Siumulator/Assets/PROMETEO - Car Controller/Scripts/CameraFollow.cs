using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform carTransform;
    [Range(1, 10)]
    public float followSpeed = 2;
    [Range(1, 10)]
    public float lookSpeed = 5;
    Vector3 initialCameraPosition;

    Vector3 SmoothPosVelocity; // Velocity of Position Smoothing
    Vector3 SmoothRotVelocity; // Velocity of Rotation  Smoothing
    private GlobalSettings globalSettings;

    void Start()
    {
        //init global 
        globalSettings = GameObject.Find("GlobalSettings").GetComponent<GlobalSettings>();
        initialCameraPosition = gameObject.transform.position;
    }

    void FixedUpdate()
    {
        if (!globalSettings.useAiControls)
        {
            //Look at car
            Vector3 _lookDirection = (new Vector3(carTransform.position.x, carTransform.position.y, carTransform.position.z)) - transform.position;
            Quaternion _rot = Quaternion.LookRotation(_lookDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, _rot, lookSpeed * Time.deltaTime);

            //Move to car
            Vector3 _targetPos = initialCameraPosition + carTransform.transform.position;
            transform.position = Vector3.Lerp(transform.position, _targetPos, followSpeed * Time.deltaTime);
        }
        else
        {
            CarController BestCar = transform.GetChild(0).GetComponent<CarController>(); // The best car in the bunch is the first one

            for (int i = 1; i < transform.childCount; i++) // Loop over all the cars
            {
                CarController CurrentCar = transform.GetChild(i).GetComponent<CarController>(); // Get the component of the current car

                if (CurrentCar.Fitness > BestCar.Fitness) // If the current car is better than the best car
                {
                    BestCar = CurrentCar; // Then, the best car is the current car
                }
            }

            carTransform = BestCar.transform.GetChild(0); // The target position of the camera relative to the best car

            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, carTransform.position, ref SmoothPosVelocity, 0.7f); // Smoothly set the position

            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation,
                                                             Quaternion.LookRotation(BestCar.transform.position - Camera.main.transform.position),
                                                             0.1f); // Smoothly set the rotation
        }
    }

}
