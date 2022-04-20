using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveBall : MonoBehaviour
{

    public Rigidbody BallBody;
    private bool onTable = true;

    // Update is called once per frame
    void Update()
    {
        if (BallBody.position.y > BallBody.transform.localScale.y && onTable)
        {
            BallBody.velocity = new Vector3(BallBody.velocity.x, 0f, BallBody.velocity.z);
            BallBody.position = new Vector3(BallBody.position.x, BallBody.transform.localScale.x / 2, BallBody.position.z);
        }
        if (BallBody.position.y < -0.3 && onTable)
        {
            onTable = false;
            ScoreBoard.Score++;
        }
        if (!onTable)
        {
            commenceSUCC();
        }
    }
    void commenceSUCC()
    {
        BallBody.velocity = new Vector3(0, -1, 0) - transform.position  * 2;

        if (Mathf.Abs(transform.position.x) < 0.3 && Mathf.Abs(transform.position.z) < 0.3)
        {
            Destroy(this.gameObject);
        }
    }
}
