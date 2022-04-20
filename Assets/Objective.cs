using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
    public Material BallMaterial;
    public GameObject Ball;
    private GameObject BallOP;


    private Color BallColor;
    // Start is called before the first frame update
    void Start()
    {
        float avgScale = (Ball.transform.localScale.x + Ball.transform.localScale.y + Ball.transform.localScale.z) / 3;
        for (float x = 0; x < 5; x++)
        {
            for (float z = 0; z < x + 1; z++)
            {
                BallColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                BallOP = Instantiate(Ball);
                BallOP.GetComponent<Renderer>().material.SetColor("_Color",BallColor);
                BallOP.transform.position = transform.TransformDirection(
                    new Vector3(transform.position.x + (x - 2.5f) * avgScale * 0.9f,
                    transform.position.y + avgScale / 2,
                    transform.position.z + (z - (x) / 2) * avgScale));
            }
        }
    }

}
