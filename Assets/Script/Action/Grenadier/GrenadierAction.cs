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

    protected override IEnumerator SpecificAttack()
    {
        animator.Play("Throw");
        yield return new WaitForSeconds(.2f);
        //ScreenShake.Instance.Shake();
        Transform bomb = Instantiate(bombPrefab, rightHand.position, Quaternion.identity);
        bomb.GetComponent<Bomb>().SetUp(this, (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized, targetUnit.GetWorldPosition());
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
