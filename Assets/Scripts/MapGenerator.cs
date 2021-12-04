using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    enum MapTile { Empty, Floor, Wall, BottomWall };
    MapTile[,] map;

    struct RandomWalker
    {
        public Vector2 position;
        public Vector2 direction;
    }
    List<RandomWalker> walkers;
    public int maxWalkers = 10;
    public int iterations = 100;
    public float chanceWalkerSpawn = 0.05f;
    public float chanceWalkerDestroy = 0.05f;
    public float chanceWalkerNewDir= 0.5f;

    public GameObject tileContainer;
    public GameObject wallPrefab;
    public GameObject bottomWallPrefab;
    public GameObject floorPrefab;

    private List<Vector2> floorCoordinates;
    private Vector2 exitPosition;

    public int width = 15;
    public int height = 15;

    public Vector2 middleMap;


    // Start is called before the first frame update
    public void Initialize()
    {
        map = new MapTile[width, height];
        floorCoordinates = new List<Vector2>();
        middleMap = new Vector2(Mathf.RoundToInt(width / 2.0f), Mathf.RoundToInt(height / 2.0f));
        exitPosition = middleMap;
        GenerateMap();
    }

    public void ReInitialize()
    {
        // Destroy tiles
        foreach (Transform child in tileContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        floorCoordinates.Clear();
        exitPosition = middleMap;
        GenerateMap();
    }

    public void GenerateMap()
    {
        //Loop through the width of the map
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            //Loop through the height of the map
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                map[x, y] = MapTile.Empty;
            }
        }

        // Walker Simulation
        CreateFloor();
        
        CreateWalls();
        CreateBottomWalls();
        InstanciateTiles();
    }

    public Vector2 GetRandomFloorPosition()
    {
        return floorCoordinates[Random.Range(0, floorCoordinates.Count - 1)];
    }

    public Vector2 GetExitPosition()
    {
        return exitPosition;
    }

    private void CreateFloor()
    {
        // Generate First Walker
        walkers = new List<RandomWalker>();
        var firstWalker = new RandomWalker();
        firstWalker.position = new Vector2(Mathf.RoundToInt(width / 2.0f), Mathf.RoundToInt(height / 2.0f));
        firstWalker.direction = RandomDirection();
        walkers.Add(firstWalker);

        for (int i = 0; i < iterations; i++)
        {
            // Create a floor tile at position of every walker
            for (int j = 0; j < walkers.Count; j++)
            {
                map[(int)walkers[j].position.x, (int)walkers[j].position.y] = MapTile.Floor;
            }

            // Chance to destroy a walker
            for (int j = 0; j < walkers.Count; j++)
            {
                if (Random.value < chanceWalkerDestroy && walkers.Count > 1)
                {
                    walkers.RemoveAt(j);
                }
            }

            // Chance to create a new walker
            for (int j = 0; j < walkers.Count; j++)
            {
                if (walkers.Count < maxWalkers && Random.value < chanceWalkerSpawn)
                {
                    RandomWalker walker = new RandomWalker();
                    walker.position = walkers[j].position;
                    walker.direction = RandomDirection();
                    walkers.Add(walker);
                }
            }

            // Chance to pick a new direction
            for (int j = 0; j < walkers.Count; j++)
            {
                if (Random.value < chanceWalkerNewDir)
                {
                    RandomWalker walker = new RandomWalker();
                    walker.position = walkers[j].position;
                    walker.direction = RandomDirection();
                    walkers[j] = walker;
                }
            }

            // Move walkers
            for (int j = 0; j < walkers.Count; j++)
            {
                RandomWalker walker = walkers[j];
                
                walker.position += walker.direction;
                // Avoid borders
                walker.position.x = Mathf.Clamp(walker.position.x, 5, width - 5);
                walker.position.y = Mathf.Clamp(walker.position.y, 5, height - 5);
                
                walkers[j] = walker;
            }
        }
    }

    private Vector2 RandomDirection()
    {
        switch (Random.Range(0, 4))
        {
            case 0:
                return Vector2.down;
            case 1:
                return Vector2.left;
            case 2:
                return Vector2.up;
            default:
                return Vector2.right;
        }
    }

    private void CreateWalls()
    {
        for (int x = 0; x < map.GetUpperBound(0) - 1; x++)
        {
            for (int y = 0; y < map.GetUpperBound(1) - 1; y++)
            {
                if (map[x,y] == MapTile.Floor)
                {
                    if (map[x - 1, y] == MapTile.Empty)
                        map[x - 1, y] = MapTile.Wall;
                    if (map[x + 1, y] == MapTile.Empty)
                        map[x + 1, y] = MapTile.Wall;
                    if (map[x, y - 1] == MapTile.Empty)
                        map[x, y - 1] = MapTile.Wall;
                    if (map[x, y + 1] == MapTile.Empty)
                        map[x, y + 1] = MapTile.Wall;
                    if (map[x + 1, y + 1] == MapTile.Empty)
                        map[x + 1, y + 1] = MapTile.Wall;
                    if (map[x - 1, y + 1] == MapTile.Empty)
                        map[x - 1, y + 1] = MapTile.Wall;
                    if (map[x + 1, y - 1] == MapTile.Empty)
                        map[x + 1, y - 1] = MapTile.Wall;
                    if (map[x - 1, y - 1] == MapTile.Empty)
                        map[x - 1, y - 1] = MapTile.Wall;
                }
            }
        }
    }

    private void CreateBottomWalls()
    {
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 1; y < map.GetUpperBound(1); y++)
            {
                if (map[x, y] == MapTile.Wall && map[x, y - 1] == MapTile.Floor)
                    map[x, y] = MapTile.BottomWall;
            }
        }
    }

    private void InstanciateTiles()
    {
        GameObject tile;
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                switch (map[x,y])
                {
                    case MapTile.Empty:
                        tile = Instantiate(wallPrefab, new Vector3Int(x, y, 0), Quaternion.identity);
                        tile.transform.parent = tileContainer.transform;
                        break;
                    
                    case MapTile.Floor:
                        tile = Instantiate(floorPrefab, new Vector3Int(x, y, 0), Quaternion.identity);
                        var coord = new Vector2(x, y);
                        floorCoordinates.Add(coord);
                        tile.transform.parent = tileContainer.transform;
                        if (Vector2.Distance(coord, middleMap) > Vector2.Distance(exitPosition, middleMap))
                        {
                            exitPosition = coord;
                        }
                        break;
                    
                    case MapTile.Wall:
                        tile = Instantiate(wallPrefab, new Vector3Int(x, y, 0), Quaternion.identity);
                        tile.transform.parent = tileContainer.transform;
                    
                        break;
                    case MapTile.BottomWall:
                        tile = Instantiate(bottomWallPrefab, new Vector3Int(x, y, 0), Quaternion.identity);
                        tile.transform.parent = tileContainer.transform;
                        break;
                }
            }
        }
    }

}
