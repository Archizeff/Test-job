using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
    private float maxSpeed = 2f;
    private Rigidbody2D body;
    private List<Transform> _areas = new List<Transform>();
    private Vector4 actualArea;

	void Start () {
        body = GetComponent<Rigidbody2D>();
        actualArea[0] = -20;
        actualArea[1] = 20;
        actualArea[2] = -20;
        actualArea[3] = 20;
    }

    void FixedUpdate()
    {
        Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        body.velocity = movement * maxSpeed;

        body.position = new Vector2
            (
                Mathf.Clamp(body.position.x, actualArea[0], actualArea[1]),
                Mathf.Clamp(body.position.y, actualArea[2], actualArea[3])
            );
    }

    private float[] toArray(int axe) {
        float[] temp = new float[_areas.Count];
        for (int i = 0; i < temp.Length; i++)
        {
            float position = _areas[i].position[axe == 0 || axe == 1 ? 0 : 1];
            float scale = _areas[i].localScale[axe == 0 || axe == 1 ? 0 : 1] / 2 - 0.1f;
            temp[i] = position + scale * (axe == 1 || axe == 3 ? 1 : -1);
        }
        return temp;
    }

    void OnTriggerExit2D(Collider2D col) {

        _areas.Remove(col.GetComponent<Transform>());

        actualArea[0] = Mathf.Min(toArray(0));
        actualArea[1] = Mathf.Max(toArray(1));
        actualArea[2] = Mathf.Min(toArray(2));
        actualArea[3] = Mathf.Max(toArray(3));
    }

    void OnTriggerEnter2D(Collider2D col) {

        _areas.Add(col.GetComponent<Transform>());

        actualArea[0] = Mathf.Min(toArray(0));
        actualArea[1] = Mathf.Max(toArray(1));
        actualArea[2] = Mathf.Min(toArray(2));
        actualArea[3] = Mathf.Max(toArray(3));
    }
}
