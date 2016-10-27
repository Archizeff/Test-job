using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Leaf  {

    static float MIN_LEAF_SIZE = 2.0f;
    static float MIN_ROOM_LENGHT = 1.8f;
    static float MAX_ROOM_LENGHT = 8f;

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
            Vector2 roomSize = new Vector2
                (
                    Mathf.Clamp(Random.Range(0.8f, width - 0.2f), MIN_ROOM_LENGHT, MAX_ROOM_LENGHT),
                    Mathf.Clamp(Random.Range(0.8f, height - 0.2f), MIN_ROOM_LENGHT, MAX_ROOM_LENGHT)
                );

            Vector2 roomPosition = new Vector2
                (
                    Random.Range(0.1f, width - roomSize.x - 0.1f), 
                    Random.Range(0.1f, height - roomSize.y - 0.1f)
                );

            roomPosition.x = roomPosition.x + roomSize.x / 2 + x;
            roomPosition.y = roomPosition.y + roomSize.y / 2 + y;

            room = Object.Instantiate(Resources.Load("Room") as GameObject);
            room.transform.position = roomPosition;
            room.transform.localScale = roomSize;
            room.transform.parent = GameObject.FindGameObjectWithTag("Rooms").transform;
        }
    }

    private Transform GetRoom() {
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

    static bool CompInters(Vector2 interOne, Vector2 interTwo, char method) {
        if (method == 'm' && interOne[0] >= interTwo[1])
            return true;
        else if (method == 'l' && interOne[1] <= interTwo[0])
            return true;
        else if (method == 'e' && ((interOne[0] < interTwo[0] && interOne[1] > interTwo[0]) || 
                                   (interOne[0] < interTwo[1] && interOne[1] > interTwo[1]) || 
                                   (interOne[0] > interTwo[0] && interOne[1] < interTwo[1])))
            return true;
        else
            return false;
    }

    static void BuildHall(params Vector2[] points) {
        for (int i = 0; i < points.Length - 1; i++)
        {
            GameObject hall = Object.Instantiate(Resources.Load("Hall") as GameObject);
            Vector2 pointOne = points[i];
            Vector2 pointTwo = points[i + 1];
            Vector2 hallSize, hallPosition;

            if (pointOne.x == pointTwo.x)
            {
                hallSize = new Vector2(0.3f, Mathf.Abs(pointOne.y - pointTwo.y) + 0.3f);
                hallPosition = new Vector2(pointOne.x, Mathf.Min(pointOne.y, pointTwo.y) + hallSize.y / 2 - 0.15f);
            }
            else
            {
                hallSize = new Vector2(Mathf.Abs(pointOne.x - pointTwo.x) + 0.3f, 0.3f);
                hallPosition = new Vector2(Mathf.Min(pointOne.x, pointTwo.x) + hallSize.x / 2 - 0.15f, pointOne.y);
            }

            hall.transform.position = hallPosition;
            hall.transform.localScale = hallSize;
            hall.transform.parent = GameObject.FindGameObjectWithTag("Halls").transform;
        }
    }

    static void CreateHall(Transform lRoom, Transform rRoom) {

        Vector2 oneDeltaX = new Vector2(lRoom.position.x - lRoom.localScale.x / 2 + 0.15f, lRoom.position.x + lRoom.localScale.x / 2 - 0.15f);
        Vector2 oneDeltaY = new Vector2(lRoom.position.y - lRoom.localScale.y / 2 + 0.15f, lRoom.position.y + lRoom.localScale.y / 2 - 0.15f);
        Vector2 twoDeltaX = new Vector2(rRoom.position.x - rRoom.localScale.x / 2 + 0.15f, rRoom.position.x + rRoom.localScale.x / 2 - 0.15f);
        Vector2 twoDeltaY = new Vector2(rRoom.position.y - rRoom.localScale.y / 2 + 0.15f, rRoom.position.y + rRoom.localScale.y / 2 - 0.15f);

        Vector2 one = new Vector2(Random.Range(oneDeltaX[0], oneDeltaX[1]), Random.Range(oneDeltaY[0], oneDeltaY[1]));
        Vector2 two = new Vector2(Random.Range(twoDeltaX[0], twoDeltaX[1]), Random.Range(twoDeltaY[0], twoDeltaY[1]));
        bool side = Random.value < 0.5;

        if (CompInters(oneDeltaX, twoDeltaX, 'm') && CompInters(oneDeltaY, twoDeltaY, 'l'))
        {
            BuildHall(side ? new Vector2(oneDeltaX[0], one.y) : new Vector2(one.x, oneDeltaY[1]),
                      side ? new Vector2(two.x, one.y) : new Vector2(one.x, two.y),
                      side ? new Vector2(two.x, twoDeltaY[0]) : new Vector2(twoDeltaX[1], two.y));
            return;
        }
        if (CompInters(oneDeltaX, twoDeltaX, 'e') && CompInters(oneDeltaY, twoDeltaY, 'l'))
        {
            float point = Random.Range(Mathf.Max(oneDeltaX[0], twoDeltaX[0]), Mathf.Min(oneDeltaX[1], twoDeltaX[1]));
            BuildHall(new Vector2(point, oneDeltaY[1]), new Vector2(point, twoDeltaY[0]));
            return;
        }
        if (CompInters(oneDeltaX, twoDeltaX, 'l') && CompInters(oneDeltaY, twoDeltaY, 'l'))
        {
            BuildHall(side ? new Vector2(oneDeltaX[1], one.y) : new Vector2(one.x, oneDeltaY[1]),
                      side ? new Vector2(two.x, one.y) : new Vector2(one.x, two.y),
                      side ? new Vector2(two.x, twoDeltaY[0]) : new Vector2(twoDeltaX[0], two.y));
            return;
        }
        if (CompInters(oneDeltaX, twoDeltaX, 'l') && CompInters(oneDeltaY, twoDeltaY, 'e'))
        {
            float point = Random.Range(Mathf.Max(oneDeltaY[0], twoDeltaY[0]), Mathf.Min(oneDeltaY[1], twoDeltaY[1]));
            BuildHall(new Vector2(oneDeltaX[1], point), new Vector2(twoDeltaX[0], point));
            return;
        }
        if (CompInters(oneDeltaX, twoDeltaX, 'l') && CompInters(oneDeltaY, twoDeltaY, 'm'))
        {
            BuildHall(side ? new Vector2(oneDeltaX[1], one.y) : new Vector2(one.x, oneDeltaY[0]),
                      side ? new Vector2(two.x, one.y) : new Vector2(one.x, two.y),
                      side ? new Vector2(two.x, twoDeltaY[1]) : new Vector2(twoDeltaX[0], two.y));
            return;
        }
        if (CompInters(oneDeltaX, twoDeltaX, 'e') && CompInters(oneDeltaY, twoDeltaY, 'm'))
        {
            float point = Random.Range(Mathf.Max(oneDeltaX[0], twoDeltaX[0]), Mathf.Min(oneDeltaX[1], twoDeltaX[1]));
            BuildHall(new Vector2(point, oneDeltaY[0]), new Vector2(point, twoDeltaY[1]));
            return;
        }
        if (CompInters(oneDeltaX, twoDeltaX, 'm') && CompInters(oneDeltaY, twoDeltaY, 'm'))
        {
            BuildHall(side ? new Vector2(oneDeltaX[0], one.y) : new Vector2(one.x, oneDeltaY[0]),
                      side ? new Vector2(two.x, one.y) : new Vector2(one.x, two.y),
                      side ? new Vector2(two.x, twoDeltaY[1]) : new Vector2(twoDeltaX[1], two.y));
            return;
        }
        if (CompInters(oneDeltaX, twoDeltaX, 'm') && CompInters(oneDeltaY, twoDeltaY, 'e'))
        {
            float point = Random.Range(Mathf.Max(oneDeltaY[0], twoDeltaY[0]), Mathf.Min(oneDeltaY[1], twoDeltaY[1]));
            BuildHall(new Vector2(oneDeltaX[0], point), new Vector2(twoDeltaX[1], point));
            return;
        }
    }
}
