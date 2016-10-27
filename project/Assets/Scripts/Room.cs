using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour {

    public GameObject badGuy, world;

    void Start () {
        int MAX_COUNT_BadGuys = (int) (transform.localScale.x * transform.localScale.y / 3.24) + 1;
        int MIN_COUNT_BadGuys = (int)(transform.localScale.x * transform.localScale.y / 9.72) + 1;
        int countBadGuys = Random.Range(MIN_COUNT_BadGuys, MAX_COUNT_BadGuys);
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
            GameObject obj = (GameObject) Instantiate(badGuy, position, Quaternion.identity);
            obj.GetComponent<BadGuy>().actualArea = area;
        }
	}
}
