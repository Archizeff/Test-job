using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {

    public int width = 10;
    public int height = 10;
    public int rooms = 3;

    private float minLenghtRoom = Player.unit * 5;
    private List<float[]> regions = new List<float[]>();

    public GameObject room;

    void Start() {
        float[] firstRegion = {0, 0, width, height };
        regions.Add(firstRegion);
        rooms--;
        CalculateRegions();
    }

    private void CreateRegion(float cordX, float cordY, float width, float height) {;
        GameObject obj = Instantiate(room);
        obj.transform.position = new Vector2(cordX, cordY);
        obj.transform.localScale = new Vector2(width, height);
    }

    private void CalculateRegions() {
        while (rooms > 0)
        {
            int numRegion = Random.Range(0, regions.Count);
            float[] region = regions[numRegion];
            float deltaX = region[2] - region[0];
            float delteY = region[3] - region[1];
            float doubleMinLehght = minLenghtRoom * 2;
            int axe;

            if (deltaX < doubleMinLehght && delteY < doubleMinLehght)
            {
                regions.Remove(region);
                CreateRegion(deltaX / 2 + region[0], delteY / 2 + region[1], deltaX, delteY);
                continue;
            }

            if (deltaX < doubleMinLehght) { axe = 1;  }
            else if (delteY < doubleMinLehght) { axe = 0; }
            else { axe = Random.Range(0, 2); }

            float newCord = (region[2 + axe] - region[0 + axe]) / 2 + region[0 + axe];
            float[] newRegion = {region[0], region[1], region[2], region[3] };
            newRegion[0 + axe] = newCord;
            region[2 + axe] = newCord;
            regions.Add(newRegion);
            rooms--;
        }

        foreach (float[] region in regions) {
            float deltaX = region[2] - region[0];
            float delteY = region[3] - region[1];
            CreateRegion(deltaX / 2 + region[0], delteY / 2 + region[1], deltaX, delteY);
        }
    }
}
