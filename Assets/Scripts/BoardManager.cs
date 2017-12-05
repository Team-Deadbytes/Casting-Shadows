using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

    [Serializable]
    enum RoomType { GENERAL };
    RoomType m_roomType;

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

    public int columns = 8;
    public int rows = 8;
    public GameObject player;
    public GameObject exit;
    public GameObject[] wallTiles;
    public GameObject[] wallCornerTiles;
    public GameObject[] floorTiles;

    public Count brokenFloorCount = new Count(0, 3);
    public GameObject[] brokenFloorTiles;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    void InitialiseList()
    {
        gridPositions.Clear();
        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0));
            }
        }
    }

    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;
        for(int x = -1; x < columns + 1; x++)
        {
            for(int y = -1; y < rows + 1; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                Quaternion rotation = Quaternion.identity;
                if (x == -1 && y == -1)             // Corner, Bottom Left
                {
                    toInstantiate = wallCornerTiles[Random.Range(0, wallCornerTiles.Length)];
                    //rotation = Quaternion.Euler(90, 0, 0);
                }
                else if (x == -1 && y == rows)      // Corner, Bottom Right
                {
                    toInstantiate = wallCornerTiles[Random.Range(0, wallCornerTiles.Length)];
                }
                else if (x == columns && y == -1)   // Corner, Top Left
                {
                    toInstantiate = wallCornerTiles[Random.Range(0, wallCornerTiles.Length)];
                }
                else if(x == columns && y == rows)  // Corner, Top Right
                {
                    toInstantiate = wallCornerTiles[Random.Range(0, wallCornerTiles.Length)];
                }
                else if (x == -1 || x == columns)   // Straight Wall, Vertical
                {
                    toInstantiate = wallTiles[Random.Range(0, wallTiles.Length)];
                    rotation = Quaternion.Euler(90.0f, 0, 0);
                }
                else if (y == -1 || y == rows)      // Straight Wall, Horizontal
                {
                    toInstantiate = wallTiles[Random.Range(0, wallTiles.Length)];
                }
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0), rotation) as GameObject;
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1);
        for(int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    public void SetupScene(int level)
    {
        BoardSetup();
        InitialiseList();
        LayoutObjectAtRandom(brokenFloorTiles, brokenFloorCount.minimum, brokenFloorCount.maximum);
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0), Quaternion.identity);
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
	void Update () {
		
	}
}
