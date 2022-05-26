using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.MemoryMappedFiles;
using System.IO;
using System.Text;
using System.Globalization;



public class BackPropagation: MonoBehaviour
{
    [Range(0.5f, 50.0f)]
    public float timescale = 1f;

    NeuralNetwork_BP net = new NeuralNetwork_BP(new int[] { 7, 25, 25, 2 });
    // Start is called before the first frame updat
    void Start()
    {
         Time.timeScale = timescale;
        Debug.Log("Start");

        List<string[]> data = new List<string[]>();

        using (var mappedFile1 = MemoryMappedFile.CreateFromFile("samples.txt"))
        {
            using (Stream mmStream = mappedFile1.CreateViewStream())
            {
                using (StreamReader sr = new StreamReader(mmStream, ASCIIEncoding.ASCII))
                {
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();
                        string[] dataLine = line.Split(' ');
                        data.Add(dataLine);
                        Debug.Log("wczytuje");
                    }
                }
            }
        }
        List<float[]> dataFloat = new List<float[]>();
        var ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        ci.NumberFormat.NumberDecimalSeparator = ",";
        int count=0;
        foreach (string[] line in data)
        {
            Debug.Log("konwertuje "+count.ToString());
            count++;
            float[] floatLine = new float[9];
            for (int i = 0; i < line.Length; i++)
            {
                floatLine[i] = float.Parse(line[i], ci);
            }
            
            dataFloat.Add(floatLine);
        }

        for(int k=0;k<3;k++)
        {
            foreach (float[] line in dataFloat)
            {
                Debug.Log("ucze sie");
                for (int i = 0; i < line.Length; i+=9)
                {
                    net.FeedForward(new float[] { line[i], line[i+1], line[i+2], line[i+3], line[i+4], line[i+5], line[i+6] });
                    net.BackProp(new float[] { line[i+7], line[i+8] });
                }
            }
        }


        //     for(int i=0;i<5000;i++)
        //     {
        //         net.FeedForward(new float[]{0,0,0});
        //         net.BackProp(new float[] {0});

        //         net.FeedForward(new float[]{0,0,1});
        //         net.BackProp(new float[] {1});

        //         net.FeedForward(new float[]{0,1,0});
        //         net.BackProp(new float[] {1});

        //         net.FeedForward(new float[]{0,1,1});
        //         net.BackProp(new float[] {0});

        //         net.FeedForward(new float[]{1,0,0});
        //         net.BackProp(new float[] {1});

        //         net.FeedForward(new float[]{1,0,1});
        //         net.BackProp(new float[] {0});

        //         net.FeedForward(new float[]{1,1,0});
        //         net.BackProp(new float[] {0});

        //         net.FeedForward(new float[]{1,1,1});
        //         net.BackProp(new float[] {1});
        //     }
        //     Debug.Log(net.FeedForward(new float[]{0,0,0})[0]);
        //     Debug.Log(net.FeedForward(new float[]{0,0,1})[0]);
        //     Debug.Log(net.FeedForward(new float[]{0,1,0})[0]);
        //     Debug.Log(net.FeedForward(new float[]{0,1,1})[0]);
        //     Debug.Log(net.FeedForward(new float[]{1,0,0})[0]);
        //     Debug.Log(net.FeedForward(new float[]{1,0,1})[0]);
        //     Debug.Log(net.FeedForward(new float[]{1,1,0})[0]);
        //     Debug.Log(net.FeedForward(new float[]{1,1,1})[0]);
    }
    public float Calculate(float[] inputs,int n)
    {
        return net.FeedForward(inputs)[n];
    }

    // Update is called once per frame
    void Update()
    {
 Time.timeScale = timescale;
    }
}
