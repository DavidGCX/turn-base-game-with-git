using System.Net;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class APathFind : MonoBehaviour
{
    public static APathFind Instance;

    private GridSystem<PathNode> gridSystem;

    private List<PathNode> closedNodes;
    private List<PathNode> openNodes;

    private List<GridPosition> UnBlockedPositions;

    private const int DIAGONAL_DISTANCE = 14;
    private const int DISTANCE = 10;

    private PathNode targetNode;

    private GridPosition targetGridPosition;

    private void Awake() {
        Instance = this;
        gridSystem = LevelGrid.Instance.CreateANewGridSystemPathNode();
        openNodes = new List<PathNode>();
        closedNodes = new List<PathNode>();
    }

    public List<GridPosition> FindPath(GridPosition startPosition, GridPosition targetPosition, List<GridPosition> UnBlockedGridPosition) {
        SetUnBlockedGridPosition(UnBlockedGridPosition);
        if (startPosition == targetPosition){
            return null;
        }
        targetNode = gridSystem.GetGridObject(targetPosition);
        this.targetGridPosition = targetPosition;
        GridPosition currentPositon = startPosition;
        PathNode startNode = (PathNode) gridSystem.GetGridObject(startPosition);
        closedNodes.Add(startNode);
        openNodes.Add(startNode);

        while(openNodes.Count > 0) {
            PathNode currentNode = gridSystem.GetGridObject(currentPositon);
            closedNodes.Add(currentNode);
            
            if(HandleNeighborNodes(GetNeighborNodes(GetBestNodeFromOpenListPathNode(openNodes)), currentNode)) {
                return GetPath(targetNode);
            }
            openNodes.Remove(currentNode);
        }
        return null;
    }

    private List<GridPosition> GetPath(PathNode targetNode)
    {
        PathNode currentNode = targetNode;
        List<GridPosition> pathList = new List<GridPosition>();
        pathList.Add(targetNode.GetGridPosition());
        while (currentNode.GetParentNode().GetGridPosition() != currentNode.GetGridPosition()) {
            currentNode = currentNode.GetParentNode();
            pathList.Add(currentNode.GetGridPosition());
        }
        pathList.Reverse();
        return pathList;
    }

    private bool HandleNeighborNodes(List<PathNode> pathNodes, PathNode currentNode)
    {
        foreach (var pathNode in pathNodes)
        {
            if (pathNode.GetGridPosition() == targetGridPosition) {
                targetNode.SetParentNode(pathNode);
                return true;
            }
            if (closedNodes.Contains(pathNode)) {
                continue;
            }
            int newG = CalculateDistance(pathNode, currentNode) + currentNode.GetGValue();
            int newH = CalculateDistance(pathNode, targetNode);
            int newF = newG + newH;
            if(!openNodes.Contains(pathNode) || pathNode.GetFValue() > newF) {
                openNodes.Add(pathNode);
                pathNode.SetGValue(newG);
                pathNode.SetHValue(newF);
                pathNode.UpdateFValue();
                pathNode.SetParentNode(currentNode);
            }
            
        }
        return false;
    }

    //Get all the valid and unblocked neighbor gridPositions
    private List<GridPosition> GetNeighborNodes(GridPosition gridPosition) {
        GridPosition up = new GridPosition(gridPosition.x, gridPosition.z+1);
        GridPosition down = new GridPosition(gridPosition.x, gridPosition.z-1);
        GridPosition left = new GridPosition(gridPosition.x-1, gridPosition.z);
        GridPosition right = new GridPosition(gridPosition.x+1, gridPosition.z);
        GridPosition upLeft = new GridPosition(gridPosition.x-1, gridPosition.z+1);
        GridPosition upRight = new GridPosition(gridPosition.x+1, gridPosition.z+1);
        GridPosition downLeft = new GridPosition(gridPosition.x-1, gridPosition.z-1);
        GridPosition downRight = new GridPosition(gridPosition.x+1, gridPosition.z-1);
        List<GridPosition> temp = new List<GridPosition>();
        temp.Add(up);
        temp.Add(down);
        temp.Add(left);
        temp.Add(right);
        temp.Add(upLeft);
        temp.Add(upRight);
        temp.Add(downLeft);
        temp.Add(downRight);
        foreach(GridPosition gridPos in temp) {
            if(isBlocked(gridPos) || isNotValidPosition(gridPos)) {
                temp.Remove(gridPos);
            }
        }
        return temp;
    }
    private List<PathNode> GetNeighborNodes(PathNode pathNode) {
        GridPosition gridPosition = pathNode.GetGridPosition();
        GridPosition up = new GridPosition(gridPosition.x, gridPosition.z+1);
        GridPosition down = new GridPosition(gridPosition.x, gridPosition.z-1);
        GridPosition left = new GridPosition(gridPosition.x-1, gridPosition.z);
        GridPosition right = new GridPosition(gridPosition.x+1, gridPosition.z);
        GridPosition upLeft = new GridPosition(gridPosition.x-1, gridPosition.z+1);
        GridPosition upRight = new GridPosition(gridPosition.x+1, gridPosition.z+1);
        GridPosition downLeft = new GridPosition(gridPosition.x-1, gridPosition.z-1);
        GridPosition downRight = new GridPosition(gridPosition.x+1, gridPosition.z-1);
        List<GridPosition> tempForGridPosition = new List<GridPosition>();
        List<PathNode> tempForPathNode = new List<PathNode>();
        tempForGridPosition.Add(up);
        tempForGridPosition.Add(down);
        tempForGridPosition.Add(left);
        tempForGridPosition.Add(right);
        tempForGridPosition.Add(upLeft);
        tempForGridPosition.Add(upRight);
        tempForGridPosition.Add(downLeft);
        tempForGridPosition.Add(downRight);
        foreach(GridPosition gridPos in tempForGridPosition) {
            if(isBlocked(gridPos) || isNotValidPosition(gridPos)) {
                continue;
            }
            tempForPathNode.Add(gridSystem.GetGridObject(gridPos));
        }
        return tempForPathNode;
    }

    private int CalculateDistance(PathNode one, PathNode two) {
        return CalculateDistance(one.GetGridPosition(), two.GetGridPosition());
    }
    private int CalculateDistance(GridPosition one, GridPosition two) {
        int hDistance = Mathf.Abs(one.x - two.x);
        int vDistance = Mathf.Abs(one.z - two.z);
        return Mathf.Min(hDistance, vDistance) * DIAGONAL_DISTANCE + Mathf.Abs(hDistance - vDistance) * DISTANCE;
    }

    private GridPosition GetBestNodeFromOpenListGridPosition(List<PathNode> pathNodes){
        pathNodes.Sort((PathNode a, PathNode b) => a.GetFValue() - b.GetFValue());
        return pathNodes[0].GetGridPosition();
    }
    private PathNode GetBestNodeFromOpenListPathNode(List<PathNode> pathNodes){
        pathNodes.Sort((PathNode a, PathNode b) => a.GetFValue() - b.GetFValue());
        if(pathNodes.Count != 0) {
            return pathNodes[0];
        } else {
            return null;
        }
       
    }

    private bool isNotValidPosition(GridPosition gridPosition) => !gridSystem.IsAValidGridPosition(gridPosition);
    private bool isBlocked(GridPosition gridPosition) => !UnBlockedPositions.Contains(gridPosition);
    public void SetUnBlockedGridPosition(List<GridPosition> gridPositions){
        UnBlockedPositions = gridPositions;
    }
}
