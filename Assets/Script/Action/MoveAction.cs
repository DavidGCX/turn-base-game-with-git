using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    //public static MoveAction Instance;
    
    private Vector3 targetPosition;
    private const float stopDistance = 0.3f;
    private const float turnspeed = 4f;
    private const float stopRotate = 1f;
    [SerializeField] private int maxMoveDistance = 4;
    [SerializeField] private float moveSpeed;

    protected override void Awake()
    {
        base.Awake();
        targetPosition = transform.position;
        //Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        IsActive = false;
    }

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
        }
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        if(Vector3.Distance(transform.position, targetPosition) > stopDistance) {
            Quaternion quaDir = Quaternion.LookRotation(moveDirection,Vector3.up);
            //Debug.Log(quaDir); 
            float angle = Quaternion.Angle(transform.rotation, quaDir);
            if (angle > stopRotate) {
                animator.SetFloat("IdleToRun", 0, 0.1f, Time.deltaTime);
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
            animator.SetFloat("IdleToRun", -1, 0.1f, Time.deltaTime);
            if (animator.GetFloat("IdleToRun") <=0 ) {
                IsActive = false;
                OnActionComplete();
            }
            
        }
        
        
    }
    public void Move(GridPosition targetPosition, Action OnActionComplete) {
        IsActive = true;
        this.OnActionComplete = OnActionComplete;
        this.targetPosition  = LevelGrid.instance.GetWorldPosition(targetPosition);
    }

    public List<GridPosition> GetValidGridPositionList() {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        
        for (int i = -maxMoveDistance; i <=maxMoveDistance; i++) {
            for (int j = -maxMoveDistance; j <=+maxMoveDistance; j++)
            {
                GridPosition offsetGridpos = new GridPosition(i,j);
                GridPosition resultGridpos = unit.GetGridPosition() + offsetGridpos;
                if (!LevelGrid.instance.IsAValidGridPosition(resultGridpos)) {
                    continue;
                }
                if (LevelGrid.instance.HasAnyUnitOnGridPosition(resultGridpos)) {
                    continue;
                }
                validGridPositionList.Add(resultGridpos);
            }
        }
        /* foreach (var item in validGridPositionList)
        {
            Debug.Log(item);
        } */
        
        return validGridPositionList;
    }

    public bool IsValidMoveGridPosition(GridPosition gridPosition) => GetValidGridPositionList().Contains(gridPosition);
}
