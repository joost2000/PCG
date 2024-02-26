using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Mathematics;
using Unity.Properties;
using UnityEngine;

[Serializable]
public struct RoomInfo
{
    public List<int> matrixIndices;
    public RoomInfo(List<int> indices)
    {
        matrixIndices = indices;
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

        StartCoroutine(GenerateRoomTiles());
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
        roomInfo.Add(new RoomInfo(indicesArray.ToList()));
    }

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

    IEnumerator GenerateRoomTiles()
    {
        foreach (var item in roomInfo)
        {
            foreach (var indices in item.matrixIndices)
            {
                Instantiate(floorPrefab, new Vector3(indices % dungeonWidth, 0, (int)(indices / dungeonWidth)), quaternion.identity);
                yield return new WaitForSeconds(.02f);
            }
        }
    }
}
