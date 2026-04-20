using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyTree : MonoBehaviour
{
    private DecisionNode rootNode;
    private DecisionNode questionAttackNode;

    private void Awake()
    {
        ActionNode patrolNode = new ActionNode(EnemyModel3 => EnemyModel3.Patrol());
        ActionNode PursueNode = new ActionNode(EnemyModel3 => EnemyModel3.Pursue());
        ActionNode WanderNode = new ActionNode(EnemyModel3 => EnemyModel3.Wander());
        ActionNode SeekNode = new ActionNode(EnemyModel3 => EnemyModel3.Seek());
        ActionNode AttackNode = new ActionNode(EnemyController => EnemyController.Attack());
        rootNode = new QuestionNode(
     context => context.los.IsRangeAttack(context.self, context.player),
     AttackNode,
     new QuestionNode(
         context => context.los.IsRange(context.self, context.player)
         && !context.los.IsObstacle(context.self, context.player),
         SeekNode,
         WanderNode
     )
 );

    }

    public void Evaluate(EnemyController enemy, EnemyContext context)
    {
        rootNode.Evaluate(enemy, context);
    }

}

public class ActionNodee : DecisionNode
{
    private (float weight, Action<EnemyController> action)[] options;

    public ActionNodee((float weight, Action<EnemyController> action)[] options)
    {
        this.options = options;
    }

    public override void Evaluate(EnemyController enemy, EnemyContext context)
    {
        float totalweight = 0;
        foreach (var option in options)
        {
            totalweight += option.weight;
        }

        float randomValue = UnityEngine.Random.Range(0, totalweight);
        float currentWeight = 0;
        foreach (var option in options)
        {
            currentWeight += option.weight;
            if (randomValue <= currentWeight)
            {
                option.action(enemy);
                return;
            }
        }
    }
}

