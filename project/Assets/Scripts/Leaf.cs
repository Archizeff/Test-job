using UnityEngine;
using System.Collections;

public class Leaf  {

    private float MIN_LEAF_SIZE = 1.0f;

    public float x, y, width, height;

    public Leaf rightChild = null;
    public Leaf leftChild = null;
    public GameObject room;


    public Leaf(float cordX, float cordY, float sizeW, float sizeH) {
        x = cordX;
        y = cordY;
        width = sizeW;
        height = sizeH;
    }

    public bool Split() {

        if (rightChild != null || leftChild != null)
            return false;

        bool splitH = Random.value < 0.5;
        if (width > height && width / height >= 1.25)
            splitH = false;
        else if (height > width && height / width >= 1.25)
            splitH = true;

        float max = (splitH ? height : width) - MIN_LEAF_SIZE;
        if (max <= MIN_LEAF_SIZE)
            return false;
        float split = Random.Range(MIN_LEAF_SIZE, max);

        if (splitH)
        {
            rightChild = new Leaf(x, y, width, split);
            leftChild = new Leaf(x, y + split, width, height - split);
        }
        else
        {
            rightChild = new Leaf(x, y, split, height);
            leftChild = new Leaf(x + split, y, width - split, height);
        }

        return true;
    }

    public void CreateRooms() {
        if (leftChild != null || rightChild != null)
        {
            if (leftChild != null)
                leftChild.CreateRooms();
            if (rightChild != null)
                rightChild.CreateRooms();

            if (leftChild != null && rightChild != null)
                CreateHall(leftChild.GetRoom(), rightChild.GetRoom());
        }
        else
        {
            Vector2 roomSize = new Vector2(Random.Range(0.8f, width - 0.2f), Random.Range(0.8f, height - 0.2f));
            Vector2 roomPosition = new Vector2(Random.Range(0.1f, width - roomSize.x - 0.1f), Random.Range(0.1f, height - roomSize.y - 0.1f));
            roomPosition.x = roomPosition.x + roomSize.x / 2 + x;
            roomPosition.y = roomPosition.y + roomSize.y / 2 + y;

            room = Object.Instantiate(Resources.Load("Room") as GameObject);
            room.transform.position = roomPosition;
            room.transform.localScale = roomSize;
        }
    }

    public Transform GetRoom() {
        if (room)
        {
            return room.transform;
        }
        else
        {
            Transform lRoom = null;
            Transform rRoom = null;
            if (leftChild != null)
                lRoom = leftChild.GetRoom();
            if (rightChild != null)
                rRoom = rightChild.GetRoom();

            if (lRoom == null && rRoom == null)
                return null;
            else if (rRoom == null)
                return lRoom;
            else if (lRoom == null)
                return rRoom;
            else if (Random.value > 0.5)
                return lRoom;
            else
                return rRoom;
        }
    }


    private void BuildHall(Vector2 point1, Vector2 point2, int shiftX, int shiftY) {

            GameObject hall1 = Object.Instantiate(Resources.Load("Hall") as GameObject);
            hall1.transform.position = new Vector2((point1.x + point2.x) / 2, point1.y + shiftY * (point2.y - point1.y));
            hall1.transform.localScale = new Vector2(point2.x - point1.x + 0.3f, 0.3f);

            GameObject hall2 = Object.Instantiate(Resources.Load("Hall") as GameObject);
            hall2.transform.position = new Vector2(point2.x + shiftX * (point2.x - point1.x), (point1.y + point2.y) / 2);
            hall2.transform.localScale = new Vector2(0.3f, point2.y - point1.y + 0.3f);

    }

    public void CreateHall(Transform lRoom, Transform rRoom) {

        Vector2 point1, point2;

        {
            Vector2 point = lRoom.position;
            Vector2 delta = lRoom.localScale;
            point1 = new Vector2(Random.Range(point.x - delta.x / 2 + 0.15f, point.x + delta.x / 2 - 0.15f), 
                                 Random.Range(point.y - delta.y / 2 + 0.15f, point.y + delta.y / 2 - 0.15f));
        }

        {
            Vector2 point = rRoom.position;
            Vector2 delta = rRoom.localScale;
            point2 = new Vector2(Random.Range(point.x - delta.x / 2 + 0.15f, point.x + delta.x / 2 - 0.15f), 
                                 Random.Range(point.y - delta.y / 2 + 0.15f, point.y + delta.y / 2 - 0.15f));
        }

        float width = point2.x - point1.x;
        float height = point2.y - point1.y;
        bool cor = Random.value > 0.5;

        if (width < 0)
        {
            if (height < 0)
                BuildHall(point2, point1, cor ? 0 : -1, cor ? 0 : 1);
            else
            {
                point1.y = point1.y + point2.y;
                point2.y = point1.y - point2.y;
                point1.y = point1.y - point2.y;
                BuildHall(point2, point1, cor ? 0 : -1, cor ? 1 : 0);
            }
        }
        else
        {
            if (height < 0)
            {
                point1.y = point1.y + point2.y;
                point2.y = point1.y - point2.y;
                point1.y = point1.y - point2.y;
                BuildHall(point1, point2, cor ? -1 : 0, cor ? 0 : 1);
            }
            else
                BuildHall(point1, point2, cor ? 0 : -1, cor ? 0 : 1);

        }
    }
}
