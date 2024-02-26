using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public struct TileInfo
{
    public string name;
    public int tile;
    public List<int> potentialNeighbours;
}

public class Generate2DDungeon : MonoBehaviour
{
    public int dungeonWidth, dungeonHeight;

    public int[] grid;

    public List<TileInfo> tileInfo = new List<TileInfo>();

    public GameObject Floor, Wall;

    // Start is called before the first frame update
    void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        grid = new int[dungeonHeight * dungeonWidth];

        //int randomPoint = 4;
        int randomPoint = UnityEngine.Random.Range(0, dungeonHeight * dungeonWidth);

        grid[randomPoint] = 1;

        int[] neighbours = new int[4];
        int neighbourIndex = 0;

        //check up, right, down, left
        //add all directions to a temp array
        neighbours[neighbourIndex++] = (randomPoint - dungeonWidth) < 0 ? -1 : randomPoint - dungeonWidth;//up 
        neighbours[neighbourIndex++] = randomPoint % dungeonWidth != 0 ? -1 : randomPoint + 1;//right
        neighbours[neighbourIndex++] = randomPoint % (dungeonWidth - 1) != 0 ? -1 : randomPoint - 1;//left
        neighbours[neighbourIndex++] = (randomPoint + dungeonWidth) > (dungeonWidth * dungeonHeight) ? -1 : randomPoint + dungeonWidth;//down

        print($"{neighbours[0]} up");
        print($"{neighbours[1]} right");
        print($"{neighbours[2]} left");
        print($"{neighbours[3]} down");

        int index = 0;
        for (int i = 0; i < dungeonHeight; i++)
        {
            for (int j = 0; j < dungeonWidth; j++)
            {
                if (grid[index++] == 1)
                {
                    Instantiate(Wall, new Vector3(j + 0.5f, 0, i + 0.5f), Quaternion.identity);
                }
                else
                {
                    Instantiate(Floor, new Vector3(j + 0.5f, 0, i + 0.5f), Quaternion.identity);
                }
            }
        }
    }
}
