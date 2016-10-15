using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour {

    public float width, height;
    private float cordX, cordY;
    public GameObject room;

    private void CreateRoom(int[] room)
    {
    }

    void Start()
    {
        GameObject obj = Instantiate(room);
        obj.transform.position = new Vector2(cordX, cordY);
        obj.transform.localScale = new Vector2(width, height);
    }

    void Update()
    {

    }
}
