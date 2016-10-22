using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {
    
    public int width, height, count;

	void Start () {

        List<Leaf> _leafs = new List<Leaf>();

        Leaf root = new Leaf(-width / 2,  -height / 2, width, height);
        _leafs.Add(root);
        count--;

        bool didSplit = true;
        while (didSplit)
        {
            didSplit = false;
            for (int i = 0; i < _leafs.Count; i++) {
                Leaf l = _leafs[i];
                if (l.rightChild == null && l.leftChild == null && count > 0)
                {
                    if (l.Split())
                    {
                        _leafs.Add(l.leftChild);
                        _leafs.Add(l.rightChild);
                        count--;
                        didSplit = true;
                    }
                }
            }
        }

        root.CreateRooms();

        Vector2 pos = GameObject.FindGameObjectWithTag("Room").transform.position;
        Instantiate(Resources.Load("Player") as GameObject, new Vector3(pos.x, pos.y, -1), Quaternion.identity);
	}
	
}
