using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField]
    private MazeCell mazeCellPrefab;

    [SerializeField]
    private int mazeWidth,
                mazeDepth;

    [SerializeField]
    Camera mainCamera;

    [SerializeField]
    GameObject generateMazeUi;

    float camXpos;
    float camYpos;
    float camZpos;

    private MazeCell[,] mazeGrid;

    [SerializeField]
    float waitTime = 0.1f;

    [SerializeField]
    InputField widthInput, depthInput;


    public void StartMazeGeneration()
    {
        mazeGrid = new MazeCell[mazeWidth, mazeDepth];

        SetCameraPosition();

        // using x, y, and z in lieu of i
        for(int x = 0; x < mazeWidth; x++)
        {
            for(int z = 0; z < mazeDepth; z++)
            {
                mazeGrid[x, z] = Instantiate(mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity);
            }
        }

        GenerateMaze(null, mazeGrid[0, 0]);

        HideGenerateMazeUI();
    }

    private void GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        //yield return new WaitForSeconds(waitTime);

        MazeCell nextCell;

        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                GenerateMaze(currentCell, nextCell);
            }
        } 
        while (nextCell != null);
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell);

        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.position.x;
        int z = (int)currentCell.transform.position.z;

        // Checks for an unvisited cell to the right (x + 1)
        if(x + 1 < mazeWidth)
        {
            var cellToRight = mazeGrid[x + 1, z];

            if(cellToRight.hasBeenVisited == false)
            {
                yield return cellToRight;
            }
        }
        // Checks for an unvisited cell to the left (x - 1)
        if(x - 1 >= 0)
        {
            var cellToLeft = mazeGrid[x - 1, z];
            
            if(cellToLeft.hasBeenVisited == false)
            {
                yield return cellToLeft;
            }
        }

        // Checks for unvisited cells to the front
        if(z + 1 < mazeDepth)
        {
            var cellToFront = mazeGrid[x, z + 1];

            if(cellToFront.hasBeenVisited == false)
            {
                yield return cellToFront;
            }
        }
        // Checks for unvisited cells to the back
        if (z - 1 >= 0)
        {
            var cellToBack = mazeGrid[x, z - 1];

            if (cellToBack.hasBeenVisited == false)
            {
                yield return cellToBack;
            }
        }
    }

    void SetCameraPosition()
    {
        camXpos = mazeWidth / 2;
        camZpos = (mazeDepth / 2) + (mazeDepth / 4);
        camYpos = ((mazeDepth + mazeWidth) / 2) + ((mazeDepth + mazeWidth) / 4);

        mainCamera.transform.position = new Vector3(camXpos, camYpos, camZpos);
    }

    public void SetMazeWidth()
    {
        int width = int.Parse(widthInput.text);

        mazeWidth = width;
    }

    public void SetMazeDepth()
    {
        int depth = int.Parse(depthInput.text);

        mazeDepth = depth;
    }

    void HideGenerateMazeUI()
    {
        generateMazeUi.SetActive(false);
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if(previousCell == null)
        {
            return;
        }

        // Comparing new position to previous position on X coordinates
        if(previousCell.transform.position.x < currentCell.transform.position.x)
        {
            previousCell.ClearRightwall();
            currentCell.ClearLeftWall();
            return;
        }
        if(previousCell.transform.position.x > currentCell.transform.position.x)
        {
            currentCell.ClearRightwall();
            previousCell.ClearLeftWall();
            return;
        }

        // Comparing new position to previous position on Z coordinates
        if(previousCell.transform.position.z < currentCell.transform.position.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }
        if(previousCell.transform.position.z > currentCell.transform.position.z)
        {
            currentCell.ClearFrontWall();
            previousCell.ClearBackWall();
            return;
        }
    }
}
