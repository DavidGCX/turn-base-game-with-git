using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    public static MoveAction Instance;
    [SerializeField] private Animator animator; 
    private Unit unit;
    private Vector3 targetPosition;
    private const float stopDistance = 0.3f;
    private const float turnspeed = 5f;
    private const float stopRotate = 10f;
    [SerializeField] private int maxMoveDistance = 4;
    [SerializeField] private float moveSpeed;
    private void Awake()
    {
        unit = GetComponent<Unit>();
        targetPosition = transform.position;
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(GetComponentInChildren<CharacterController>().transform.position);
        transform.position = GetComponentInChildren<CharacterController>().transform.position;
        Vector3 position = new Vector3(transform.position.x, 0f, transform.position.z);
        transform.position = position;
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        //Debug.Log(moveDirection);
        if(Vector3.Distance(transform.position, targetPosition) > stopDistance) {
            Quaternion quaDir = Quaternion.LookRotation(moveDirection,Vector3.up);
            //Debug.Log(quaDir); 
            float angle = Quaternion.Angle(transform.rotation, quaDir);
            if (angle > stopRotate) {
                animator.SetFloat("IdleToRun", 8, 0.1f, Time.deltaTime);
                //transform.position += moveDirection * Time.deltaTime * moveSpeed*0.5f;
                transform.rotation = Quaternion.Lerp(transform.rotation,quaDir,Time.fixedDeltaTime*turnspeed);
            } else {
                transform.LookAt(targetPosition);
                //transform.position += moveDirection * Time.deltaTime * moveSpeed;
                animator.SetFloat("IdleToRun", 10, 0.1f, Time.deltaTime);
            }
        } else {
            animator.SetFloat("IdleToRun", 0, 0.1f, Time.deltaTime);
        }
        
        
    }
    public void Move(Vector3 targetPosition) {
        this.targetPosition  = targetPosition;
    }

    public List<GridPosition> GetValidGridPositionList() {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        
        for (int i = -maxMoveDistance; i <=maxMoveDistance; i++) {
            for (int j = -maxMoveDistance; j <=+maxMoveDistance; j++)
            {
                GridPosition offsetGridpos = new GridPosition(i,j);
                GridPosition resultGridpos = unit.GetGridPosition() + offsetGridpos;
                if (LevelGrid.instance.IsAValidGridPosition(resultGridpos)) {
                    validGridPositionList.Add(resultGridpos);
                }
            }
        }
        foreach (var item in validGridPositionList)
        {
            Debug.Log(item);
        }
        return validGridPositionList;
    }
}
