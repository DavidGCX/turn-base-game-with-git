using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GrenadierAction : AttackAction
{

    [Header("手雷的prefab")]
    [SerializeField] private Transform bombPrefab;
    [Header("手雷生成的位置")]
    [SerializeField] private Transform rightHand;

    [SerializeField] private Transform gunBack;

    //投掷速度
    private Vector3 speed = new Vector3(0,0,0);
    protected override IEnumerator SpecificAttack()
    {
        animator.Play("Throw");
        yield return new WaitForSeconds(.7f);
        //ScreenShake.Instance.Shake();
        Transform bomb = Instantiate(bombPrefab, rightHand.position, Quaternion.identity);

        //确定投掷手雷路线的三个点位，起始点位，终点位，firstPoint,point,lastPoint.
        //检查是否有墙的射线
        RaycastHit hit;
        //过墙点位
        Vector3 point;
        Vector3 resultWorldPosition = targetUnit.GetWorldPosition();
        Vector3 lastPoint = new Vector3(resultWorldPosition.x, rightHand.position.y, resultWorldPosition.z);
        Vector3 startPoint = unit.GetWorldPosition() + new Vector3(0, 1, 0);
        Vector3 direction = lastPoint - startPoint;
        float distance = Vector3.Distance(resultWorldPosition, unit.GetWorldPosition());
        //Debug.Log(distance);
        int ObstacleLayer = (1 << 8); // do bitwise operation to the obstacle layer
        if (Physics.Raycast(startPoint, direction,out hit , distance, ObstacleLayer))
        {
            //越墙抛物线代码
            //碰撞点向上3个单位即越过墙的点位
            point = hit.point + new Vector3(0, 2, 0);
            //测试：(8,2,0)(8,4,4)(8,2,8)
            Vector3 firstPoint = rightHand.position;
            //firstPoint = new Vector3(8, 2, 0);
            //point = new Vector3(8, 4, 4);
            //lastPoint = new Vector3(8, 2, 8);
            //计算终点与起点的距离以及，碰撞点与起点的x，z轴距离。
            Vector2 fp = new Vector2(firstPoint.x, firstPoint.z);
            Vector2 p = new Vector2(point.x, point.z);
            Vector2 lp = new Vector2(lastPoint.x, lastPoint.z);
            float disfl = Vector2.Distance(fp, lp);
            float disfp = Vector2.Distance(fp, p);
            //求出飞行时间
            float t = Mathf.Sqrt((float)((9.81 * disfl * disfl) / (9.81 * disfp * disfl - 8 * disfp * disfp)));
            float speedXZ = disfl / t;
            float speedY = (float)9.81 * t / 2;
            float speedX = (speedXZ*(lastPoint.x - firstPoint.x)/disfl);
            float speedZ = (speedXZ * (lastPoint.z - firstPoint.z) / disfl);
            speed = new Vector3(speedX, speedY, speedZ);
            //double speed = Mathf.Sqrt((float)(speedXZ * speedXZ + speedY * speedY));
            Debug.Log("speedXZ: " + speedXZ + "  speedY: " + speedY + "  speed: " + speed + "   t: "+t);
            bomb.GetComponent<Bomb>().SetUp(this, speed, targetUnit.GetWorldPosition());
        }
        else
        {
            bomb.GetComponent<Bomb>().SetUp(this,(lastPoint - rightHand.position + new Vector3(0,4.905f,0)), targetUnit.GetWorldPosition());
            Debug.Log(lastPoint - rightHand.position);
            
        }
        yield return new WaitForSeconds(1f);
    }

    protected override bool ShouldUseAttackCamera()
    {
        if (GetTotalDamage() >= targetUnit.GetUnitCurrentHealth())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected override int CalculateEnemyAIActionValue() => 100;
    /// <summary>
    /// 用网格高亮显示投掷手雷的攻击范围，不包含可攻击目标的网格
    /// </summary>
    /// <returns></returns>
    public override List<GridPosition> GetGridPositionListInRange()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        for (int i = -effectiveDistance; i <= effectiveDistance; i++)
        {
            for (int j = -effectiveDistance; j <= +effectiveDistance; j++)
            {
                GridPosition offsetGridpos = new GridPosition(i, j);
                GridPosition resultGridpos = unit.GetGridPosition() + offsetGridpos;
                //跳过关卡区域外格网
                if (!LevelGrid.Instance.IsAValidGridPosition(resultGridpos))
                {
                    continue;
                }
                //跳过自身单位格网
                if (InvalidDistance(resultGridpos) && effectiveDistance != 1)
                {
                    // Not in distance
                    continue;
                }
                //跳过任意unit单位格网
                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(resultGridpos))
                {
                    //has unit
                    continue;
                }
                //跳过含有墙的格网
                if (LevelGrid.Instance.HasAnyWallOnGridPostion(resultGridpos))
                {
                    continue;
                }
                validGridPositionList.Add(resultGridpos);
            }
        }
        return validGridPositionList;
    }

    // show grid that can be used to attack;
    public override List<GridPosition> GetValidGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        for (int i = -effectiveDistance; i <= effectiveDistance; i++)
        {
            for (int j = -effectiveDistance; j <= +effectiveDistance; j++)
            {
                GridPosition offsetGridpos = new GridPosition(i, j);
                GridPosition resultGridpos = unit.GetGridPosition() + offsetGridpos;
                if (!LevelGrid.Instance.IsAValidGridPosition(resultGridpos))
                {
                    continue;
                }
                if (InvalidDistance(resultGridpos) && effectiveDistance != 1)
                {
                    continue;
                }
                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(resultGridpos))
                {
                    continue;
                }
                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(resultGridpos);
                if (targetUnit.GetUnitType() == unit.GetUnitType())
                {
                    continue;
                }
                validGridPositionList.Add(resultGridpos);
            }
        }
        return validGridPositionList;
    }


   
    public override string IsWhatAction() => "GrenadierAction";

    
}
