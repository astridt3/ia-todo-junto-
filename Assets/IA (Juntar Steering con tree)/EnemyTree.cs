using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyTree : MonoBehaviour
{
    private DecisionNode rootNode;
    private DecisionNode questionAttackNode;

    private void Awake()
    {
        //ActionNode patrolNode = new ActionNode(EnemyModel3 => EnemyModel3.Patrol());
        //ActionNode PursueNode = new ActionNode(EnemyModel3 => EnemyModel3.Pursue());
        ActionNode attackNode = new ActionNode(enemy => enemy.Attack());
        ActionNode seekNode = new ActionNode(enemy => enemy.Seek());
        ActionNode pathNode = new ActionNode(enemy => enemy.ChaseWithPath());
        ActionNode patrolNode = new ActionNode(enemy => enemy.PatrolNodes());

        QuestionNode obstacleNode = new QuestionNode(
            context => context.los.IsObstacle(context.self, context.player),
            pathNode,
            seekNode
        );

        QuestionNode chaseNode = new QuestionNode(
            context => context.los.IsRange(context.self, context.player),
            obstacleNode,
            patrolNode
        );

        rootNode = new QuestionNode(
            context => context.los.IsRangeAttack(context.self, context.player),
            attackNode,
            chaseNode
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

