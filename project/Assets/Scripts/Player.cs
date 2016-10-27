using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
    static float maxSpeed = 2f;
    public Vector4 actualArea;
    public bool god = true;

    private Rigidbody2D body;
    private List<Transform> _areas = new List<Transform>();

	void Start () {
        body = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (god && movement.x != 0 && movement.y != 0) {
            god = false;
            gameObject.layer = 0;
        }

        body.velocity = movement * maxSpeed;

        body.position = new Vector2
            (
                Mathf.Clamp(body.position.x, actualArea[0], actualArea[1]),
                Mathf.Clamp(body.position.y, actualArea[2], actualArea[3])
            );
    }

    void OnTriggerExit2D(Collider2D col) {
        _areas.Remove(col.GetComponent<Transform>());
        updateArea();
    }

    void OnTriggerEnter2D(Collider2D col) {
        _areas.Add(col.GetComponent<Transform>());
        updateArea();
    }

    void OnCollisionEnter2D() {
        Debug.Log("Bah!");
    }

    private float[] toArray(int axe)
    {
        float[] temp = new float[_areas.Count];
        for (int i = 0; i < temp.Length; i++)
        {
            float position = _areas[i].position[axe == 0 || axe == 1 ? 0 : 1];
            float scale = _areas[i].localScale[axe == 0 || axe == 1 ? 0 : 1] / 2 - 0.1f;
            temp[i] = position + scale * (axe == 1 || axe == 3 ? 1 : -1);
        }
        return temp;
    }

    private void updateArea()
    {
        actualArea[0] = Mathf.Min(toArray(0));
        actualArea[1] = Mathf.Max(toArray(1));
        actualArea[2] = Mathf.Min(toArray(2));
        actualArea[3] = Mathf.Max(toArray(3));
    }
}
