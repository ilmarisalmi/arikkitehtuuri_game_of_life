using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOfLife : MonoBehaviour
{

    public GameObject TileObject;
    private static readonly int Width = 10;
    private static readonly int Height = 10;
    bool[,] grid = new bool[Width, Height];
    GameObject[,] tiles = new GameObject[Width, Height];

    private float TimeAccu = 0.0f;
    void Start()
    {
        //generate tiles
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {

                //set all the tiles to true initially
                grid[x, y] = true;
                tiles[x, y] = Instantiate(TileObject, new Vector3(x-12.0f, 0f, y), Quaternion.identity);
                //tiles[x, y].SetActive(false);

                //set the tile color to white
                tiles[x, y].GetComponent<Renderer>().material.color = Color.white;
            }
        }

    }

    //check for live neighbours
    private int GetLiveNeighbours(int x, int y)
    {
        int count = 0;
        //loop through the 8 adjacent cells
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                
                if (i == 0 && j == 0) continue;
                
                int nx = (x + i + Width) % Width;
                int ny = (y + j + Height) % Height;
                //count the live cells
                if (grid[nx, ny]) count++;
            }
        }
        return count;
    }


    void Update()
    {

        TimeAccu += Time.deltaTime;
        if (TimeAccu > 0.5f)
        {
            //Game of life rules:
            //Any live cell with fewer than two live neighbours dies, as if by underpopulation.
            //Any live cell with two or three live neighbours lives on to the next generation.
            //Any live cell with more than three live neighbours dies, as if by overpopulation.
            //Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.


            int[,] liveNeighbours = new int[Width, Height];

            //calculate the live neighbours for each cell
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    liveNeighbours[x, y] = GetLiveNeighbours(x, y);
                }
            }


            //update tiles based on the rules
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    //get the current state and the number of live neighbours
                    bool state = grid[x, y];
                    int neighbours = liveNeighbours[x, y];
                    //and apply the rules
                    if (state && (neighbours < 2 || neighbours > 3))
                    {
                        //die by underpopulation or overpopulation
                        grid[x, y] = false;
                    }
                    else if (!state && neighbours == 3)
                    {
                        //become alive by reproduction
                        grid[x, y] = true;
                    }
                    //update the tile color
                    tiles[x, y].GetComponent<Renderer>().material.color = grid[x, y] ? Color.white : Color.black;
                }
            }

            //reset the timer
            TimeAccu = 0.0f;
        }

        //check if space is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //set a random number of tiles to alive
            int n = Random.Range(1, Width * Height);
            for (int i = 0; i < n; i++)
            {
                //pick a random position
                int x = Random.Range(0, Width);
                int y = Random.Range(0, Height);
                //set the tile to true
                grid[x, y] = true;
                //update the tile color
                tiles[x, y].GetComponent<Renderer>().material.color = Color.white;
            }
        }
    }
}
