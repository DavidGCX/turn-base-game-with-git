using System.IO;
using System.Security.Cryptography.X509Certificates;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveAction : BaseAction
{
    //public static MoveAction Instance;
    
    private Vector3 targetPosition;

    private List<GridPosition> path;
    private const float stopDistance = 0.3f;
    private const float turnspeed = 4f;
    private const float stopRotate = 1f;
    [SerializeField] private float moveSpeed;
    private struct postitionPathPair{
        public GridPosition gridPosition;
        public List<GridPosition> path;
    }
    private List<postitionPathPair> postitionPathPairList;
    protected override void Awake()
    {
        postitionPathPairList = new List<postitionPathPair>();
        base.Awake();
        targetPosition = transform.position;
        //Instance = this;
    }
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(GetComponentInChildren<CharacterController>().transform.position);
        //transform.position = GetComponentInChildren<CharacterController>().transform.position;
        //Vector3 position = new Vector3(transform.position.x, 0f, transform.position.z);
        //transform.position = position;
        
        //Debug.Log(moveDirection);
        if (!IsActive) {
            return;
        };

        //以下为转向移动目标和实际移动，使用unity自带功能，可以用DOTween插件加上协程大幅减少（见AttackAciton），此处仅作为展示使用
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        if(Vector3.Distance(transform.position, targetPosition) > stopDistance) {
            Quaternion quaDir = Quaternion.LookRotation(moveDirection,Vector3.up);
            //Debug.Log(quaDir); 
            float angle = Quaternion.Angle(transform.rotation, quaDir);
            if (angle > stopRotate) {
                //animator.SetFloat("IdleToRun", -1, 0.1f, Time.deltaTime);
                transform.position += moveDirection * Time.deltaTime * moveSpeed*0.5f;
                transform.rotation = Quaternion.Lerp(transform.rotation,quaDir,Time.fixedDeltaTime*turnspeed);
            } else {
                transform.rotation = quaDir;
                //transform.LookAt(targetPosition);
                transform.position += moveDirection * Time.deltaTime * moveSpeed;
                animator.SetFloat("IdleToRun", 10, 0.1f, Time.deltaTime);
            }
        } else {
            transform.position = targetPosition;
            if (path.Count != 0) {
                this.targetPosition  = GetWorldPosition(path[0]);
                path.RemoveAt(0);
            } else {
                animator.SetFloat("IdleToRun", -1, 0.1f, Time.deltaTime);
                if (animator.GetFloat("IdleToRun") <=0 ) {
                    EndAction();
                }
            }
            
            
        }
        
    }
    

    public override List<GridPosition> GetValidGridPositionList() {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        postitionPathPairList = new List<postitionPathPair>();
        foreach (var gridPosition in GetPotentialValidGridPositionList()) {
            List<GridPosition> tempPath = APathFind.Instance.FindPath(unit.GetGridPosition(), gridPosition, GetPotentialValidGridPositionList());
            if(tempPath.Count <= effectiveDistance) {
                postitionPathPairList.Add(new postitionPathPair{
                    gridPosition = gridPosition,
                    path = tempPath,
                });
                validGridPositionList.Add(gridPosition);
            }
        }
        return validGridPositionList;
    }

    private List<GridPosition> GetPotentialValidGridPositionList() {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        
        for (int i = -effectiveDistance; i <=effectiveDistance; i++) {
            for (int j = -effectiveDistance; j <=+effectiveDistance; j++)
            {
                GridPosition offsetGridpos = new GridPosition(i,j);
                GridPosition resultGridpos = unit.GetGridPosition() + offsetGridpos;
                if (!LevelGrid.Instance.IsAValidGridPosition(resultGridpos)) {
                    continue;
                }
                 if(InvalidDistance(resultGridpos) && effectiveDistance != 1) {
                    continue;
                }
                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(resultGridpos)) {
                    continue;
                }
                validGridPositionList.Add(resultGridpos);
            }
        }
        return validGridPositionList;
    }
    


    public override void TakeAction(GridPosition targetPosition, Action ActionComplete)
    {
        StartAction(ActionComplete);
        path = SearchForPath(targetPosition);
        /*foreach (var grid in path)
        {
            Debug.Log(grid);
        }*/
        this.targetPosition  = GetWorldPosition(path[0]);
        //Debug.Log(this.targetPosition);
        path.RemoveAt(0);
    }

    private List<GridPosition> SearchForPath(GridPosition gridPosition) {
        foreach (var pair in postitionPathPairList)
        {
            if(gridPosition == pair.gridPosition) {
                return pair.path;
            }
        }
        return null;
    }
    private Vector3 GetWorldPosition(GridPosition gridPosition) => LevelGrid.Instance.GetWorldPosition(gridPosition);

    protected override int CalculateEnemyAIActionValue() => 10;

    /*
    public IEnumerator testFindPath(GridPosition targetPosition) {
        Debug.Log("Before path");
        Debug.Log(APathFind.Instance);
        List<GridPosition> path = APathFind.Instance.FindPath(unit.GetGridPosition(), targetPosition, GetValidGridPositionList());
        Debug.Log("Before path");
        foreach (var grid in path)
        {
            Debug.Log(grid);
        }
        yield return new WaitForSeconds(0f);
    }
    */
}
