using System.Collections.Generic;
using UnityEngine;

public class TrackCheckpoint : MonoBehaviour
{
    [SerializeField] private List<Transform> carTransformList;
    private List<CheckpointSingle> checkpointSingleList;
    private List<int> nextCheckpointSingleIndexList;
    private GeneticAlgorithm geneticAlgorithm;

    private void Awake()
    {
        //get the genetic algorithm
        geneticAlgorithm = GameObject.Find("Genetic Algorithm").GetComponent<GeneticAlgorithm>();
        //init car transform list with the car transform from genetic algorithm
        carTransformList = geneticAlgorithm.Cars.ConvertAll(car => car.transform);


        Transform checkpointsTransform = transform.Find("Spline/Checkpoints");
        checkpointSingleList = new List<CheckpointSingle>();

        foreach (Transform checkpointSingleTransform in checkpointsTransform)
        {
            CheckpointSingle checkpointSingle = checkpointSingleTransform.GetComponent<CheckpointSingle>();
            checkpointSingle.SetTrackCheckpoints(this);
            checkpointSingleList.Add(checkpointSingle);
        }

        nextCheckpointSingleIndexList = new List<int>();
        foreach (Transform carTransform in carTransformList)
        {
            nextCheckpointSingleIndexList.Add(0);
        }
    }


    public void updateList()
    {
        carTransformList = geneticAlgorithm.Cars.ConvertAll(car => car.transform);
    }



    public void PlayerTroughCheckpoint(CheckpointSingle checkpointSingle, CarController carController, Transform carTransform)
    {
        int nextCheckpointSingleIndex = nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)];
        if (checkpointSingleList.IndexOf(checkpointSingle) == nextCheckpointSingleIndex)
        {

            //correct checkpoint
            Debug.Log("Correct");
            nextCheckpointSingleIndex = (nextCheckpointSingleIndex + 1) % checkpointSingleList.Count;
            carController.CheckpointHit();
        }
        else
        {
            //wrong checkpoint
            Debug.Log("Wrong");
        }

    }

    //add reset
    public void Reset()
    {
    }
}
