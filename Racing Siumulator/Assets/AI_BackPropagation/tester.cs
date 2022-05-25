using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tester : MonoBehaviour
{
    // Start is called before the first frame updat
    void Start()
    {
        NeuralNetwork_BP net = new NeuralNetwork_BP(new int[] {3,25,25,1});

        for(int i=0;i<5000;i++)
        {
            net.FeedForward(new float[]{0,0,0});
            net.BackProp(new float[] {0});

            net.FeedForward(new float[]{0,0,1});
            net.BackProp(new float[] {1});

            net.FeedForward(new float[]{0,1,0});
            net.BackProp(new float[] {1});

            net.FeedForward(new float[]{0,1,1});
            net.BackProp(new float[] {0});

            net.FeedForward(new float[]{1,0,0});
            net.BackProp(new float[] {1});

            net.FeedForward(new float[]{1,0,1});
            net.BackProp(new float[] {0});

            net.FeedForward(new float[]{1,1,0});
            net.BackProp(new float[] {0});

            net.FeedForward(new float[]{1,1,1});
            net.BackProp(new float[] {1});
        }
        Debug.Log(net.FeedForward(new float[]{0,0,0})[0]);
        Debug.Log(net.FeedForward(new float[]{0,0,1})[0]);
        Debug.Log(net.FeedForward(new float[]{0,1,0})[0]);
        Debug.Log(net.FeedForward(new float[]{0,1,1})[0]);
        Debug.Log(net.FeedForward(new float[]{1,0,0})[0]);
        Debug.Log(net.FeedForward(new float[]{1,0,1})[0]);
        Debug.Log(net.FeedForward(new float[]{1,1,0})[0]);
        Debug.Log(net.FeedForward(new float[]{1,1,1})[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
