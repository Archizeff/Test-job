using UnityEngine;
using System.Collections;

public class BadGuy : MonoBehaviour {
    static float speed = 0.3f;

    public Vector4 actualArea;
    private Vector2 target;
    private bool state = true;
    private Rigidbody2D body;

    void Start () {
        body = GetComponent<Rigidbody2D>();
    }

    void Update() {

        if (state)
        {
            target = new Vector2(Random.Range(actualArea[0], actualArea[1]), Random.Range(actualArea[2], actualArea[3]));
            body.velocity = (target - body.position) * speed;
            state = false;
        }

        if (Vector3.Distance(transform.position, target) < 1.2f)
        {
            body.velocity = new Vector2(0, 0);
            state = true;
        }

    }
}
