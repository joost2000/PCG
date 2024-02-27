using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public struct RoomInfo
{
    public int width, height;
    public List<int> matrixIndices;
    public RoomInfo(List<int> indices, int width, int height)
    {
        matrixIndices = indices;
        this.width = width;
        this.height = height;
    }
}

public class Generate2DDungeon : MonoBehaviour
{
    public GameObject wallPrefab, floorPrefab;

    public int dungeonWidth, dungeonHeight;

    public int[] grid;

    public int maxRoomWidth, maxRoomHeight, amountOfRoomsToSpawn;

    public List<RoomInfo> roomInfo = new List<RoomInfo>();

    // Start is called before the first frame update
    void Start()
    {
        grid = new int[dungeonHeight * dungeonWidth];
        for (int i = 0; i < amountOfRoomsToSpawn; i++)
        {
            int randomWidth = UnityEngine.Random.Range(2, maxRoomWidth + 1);
            int randomHeight = UnityEngine.Random.Range(2, maxRoomHeight + 1);
            GenerateRoomMatrixCoords(randomWidth, randomHeight);
        }

        for (int i = 0; i < roomInfo.Count; i++)
        {
            CheckIfRoomOverlap(i);
        }

        GenerateRoomTiles();
    }

    void GenerateRoomMatrixCoords(int roomWidth, int roomHeight)
    {
        //spawn room within matrix bounds
        int centerCoordsX = UnityEngine.Random.Range(roomWidth / 2 + 1, dungeonWidth - (roomWidth / 2) - 1);
        int centerCoordsY = UnityEngine.Random.Range(roomHeight / 2 + 1, dungeonHeight - (roomHeight / 2) - 1);

        int index = 0;
        int[] indicesArray = new int[roomWidth * roomHeight];

        //top of room starts at this index
        int startIndex = centerCoordsY * dungeonWidth + centerCoordsX;

        for (int i = 0; i < roomHeight; i++)
        {
            for (int j = 0; j < roomWidth; j++)
            {
                indicesArray[index++] = startIndex + (i * dungeonWidth) + j;
            }
        }
        roomInfo.Add(new RoomInfo(indicesArray.ToList(), roomWidth, roomHeight));
    }

    /// <summary>
    /// check if the room overlaps with any other room, if so remove the overlapping room
    /// </summary>
    /// <param name="indexToCheck"></param>
    void CheckIfRoomOverlap(int indexToCheck)
    {
        List<int> indicesToCheck = roomInfo[indexToCheck].matrixIndices;

        for (int i = 0; i < roomInfo.Count; i++)
        {
            if (i == indexToCheck) continue;
            foreach (var item in roomInfo[i].matrixIndices)
            {
                if (indicesToCheck.Contains(item))
                {
                    print($"list {i} & list {indexToCheck} overlap! | Removing list {i}");
                    roomInfo.RemoveAt(i);
                    break;
                }
            }
        }
    }

    void GenerateRoomTiles()
    {
        foreach (var item in roomInfo)
        {
            foreach (var indices in item.matrixIndices)
            {
                Instantiate(floorPrefab, new Vector3(indices % dungeonWidth, 0, indices / dungeonWidth), quaternion.identity, transform);
                // grid[indices] = 1;
            }
        }
    }

    /// <summary>
    /// Check the edges of a room to see if the room has adjacent rooms
    /// </summary>
    bool CheckEdges(RoomInfo context)
    {
        //check for neighbour above
        for (int i = 0; i < context.width; i++)
        {
            if (grid[context.matrixIndices[i] - dungeonWidth] == 1) return true;
        }

        return false;
    }
}
