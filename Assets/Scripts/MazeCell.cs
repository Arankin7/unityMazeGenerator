using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [SerializeField]
    private GameObject leftWall,
                       rightWall,
                       frontWall,
                       backwall,
                       unvisitedBlock;

    public bool hasBeenVisited { get; private set; }

    public void Visit()
    {
        hasBeenVisited = true;

        unvisitedBlock.SetActive(false);

    }

    public void ClearLeftWall()
    {
        leftWall.SetActive(false);
    }
    public void ClearRightwall()
    {
        rightWall.SetActive(false);
    }
    public void ClearFrontWall()
    {
        frontWall.SetActive(false);
    }
    public void ClearBackWall()
    {
        backwall.SetActive(false);
    }
}
