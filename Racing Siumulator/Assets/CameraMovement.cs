using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //TO DO
    //MAYBE JAKIS WIEKSZY OFFSET KAMERY OD UZYTKOWNIKA

    public GameObject Player;
    public float speed;
    public GameObject PointToLookAt;
    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PointToLookAt = Player.transform.Find("PointToLookAt").gameObject; 
        
    }

    
    private void FixedUpdate()
    {
        FollowPlayer();
    }
    private void FollowPlayer() {
        
        gameObject.transform.position = Vector3.Lerp(transform.position, PointToLookAt.transform.position, Time.deltaTime * speed);
        gameObject.transform.LookAt(Player.gameObject.transform.position);
    
    }
}
