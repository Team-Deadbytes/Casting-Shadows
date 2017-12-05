using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class Level : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    private Transform boardHolder;

    public GameObject exit;
    public GameObject[] wallTiles;
    public GameObject wallConcaveCorner, wallConvexCorner;
    public GameObject[] floorTiles;
    public GameObject[] brokenFloorTiles;

    public Count brokenFloorCount = new Count(0, 3);

    private Vector3[] wallTilesLoc;

    bool setup;
    int offset;

    void AddEntry(int type, int x, int y)
    {
        if (type == 0)
        {
            if (!setup)
            {
                offset++;
            }
            else
            {
                wallTilesLoc[offset++] = new Vector3(x, y, 0);
            }
        }
    }

    void BoardSetup()
    {
        offset = 0;
        setup = false;
        for (int numDot = 0; numDot < 2; numDot++)
        {
            if (numDot == 1)
            {
                wallTilesLoc = new Vector3[offset];
                setup = true;
                offset = 0;
            }
            for (int x = -1; x < 2; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    AddEntry(0, x, y);
                }
            }
            for (int x = 3; x < 6; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    AddEntry(0, x, y);
                }
                for (int y = 5; y < 11; y++)
                {
                    AddEntry(0, x, y);
                }
            }
            for (int x = -6; x < -2; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    AddEntry(0, x, y);
                }
                for (int y = 8; y < 11; y++)
                {
                    AddEntry(0, x, y);
                }
            }
            for (int x = -2; x < 3; x++)
            {
                AddEntry(0, x, 10);
            }
            for (int x = -10; x < -7; x++)
            {
                for (int y = 5; y < 11; y++)
                {
                    AddEntry(0, x, y);
                }
            }
            for (int x = -10; x < -7; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    AddEntry(0, x, y);
                }
            }

            for (int x = 7; x < 15; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    AddEntry(0, x, y);
                }
            }
            for (int x = 7; x < 10; x++)
            {
                for (int y = 3; y < 11; y++)
                {
                    AddEntry(0, x, y);
                }
            }
            for (int x = 11; x < 15; x++)
            {
                for (int y = 8; y < 11; y++)
                {
                    AddEntry(0, x, y);
                }
                for (int y = 3; y < 7; y++)
                {
                    AddEntry(0, x, y);
                }
            }
            for (int x = -1; x < 3; x++)
            {
                for (int y = 12; y < 18; y++)
                {
                    AddEntry(0, x, y);
                }
            }
            for (int x = -5; x < -2; x++)
            {
                for (int y = 12; y < 15; y++)
                {
                    AddEntry(0, x, y);
                }
            }
            for (int x = -10; x < -6; x++)
            {
                for (int y = 12; y < 15; y++)
                {
                    AddEntry(0, x, y);
                }
            }
            for (int x = 3; x < 12; x++)
            {
                for (int y = 13; y < 14; y++)
                {
                    AddEntry(0, x, y);
                }
            }
            AddEntry(0, 2, 1);
            AddEntry(0, -2, 1);
            AddEntry(0, 4, 4);
            AddEntry(0, -5, 7);
            AddEntry(0, -7, 5);
            AddEntry(0, -10, 4);
            AddEntry(0, 13, 2);
            AddEntry(0, 8, 2);
            AddEntry(0, 10, 8);
            AddEntry(0, 6, 0);
            AddEntry(0, -6, 13);
            AddEntry(0, -2, 13);
            AddEntry(0, -9, 11);
            AddEntry(0, 11, 12);
            AddEntry(0, 11, 11);
        }

        for (int i = 0; i < wallTilesLoc.Length; i++)
        {
            GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
            GameObject instance = Instantiate(toInstantiate, wallTilesLoc[i], Quaternion.identity) as GameObject;
            instance.transform.SetParent(boardHolder);
        }
    }

    public void SetupScene(Transform board)
    {
        boardHolder = board;
        BoardSetup();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
