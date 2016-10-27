using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour {
    static int MIN_COUNT_BadGuys = 2;
    static int MAX_COUNT_BadGuys = 5;

    public GameObject BadGuy;

    void Start () {
        int countBadGuys = Random.Range(MIN_COUNT_BadGuys, MAX_COUNT_BadGuys + 1);
        Vector4 position = new Vector3(transform.position.x, transform.position.y, -1);
        Vector4 area = new Vector4
            (
                transform.position.x - transform.localScale.x / 2,
                transform.position.x + transform.localScale.x / 2,
                transform.position.y - transform.localScale.y / 2,
                transform.position.y + transform.localScale.y / 2
            );

        for (int i = 0; i < countBadGuys; i++)
        {
            GameObject obj = (GameObject) Instantiate(BadGuy, position, Quaternion.identity);
            obj.GetComponent<BadGuy>().actualArea = area;
        }
	}
}
