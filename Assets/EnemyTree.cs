using System;
using UnityEngine;

public class EnemyTree : MonoBehaviour
{
    private DecisionNode rootNode;
    private DecisionNode questionAttackNode;

    private void Awake()
    {
        ActionNode patrolNode = new ActionNode(EnemyModel3 => EnemyModel3.Patrol());//llamar funcion sin un nombre,arrow function
        ActionNode PursuitNode = new ActionNode(EnemyModel3 => EnemyModel3.Pursuit());
        ActionNode WanderNode = new ActionNode(EnemyModel3 => EnemyModel3.Wander());
        ActionNode SeekNode = new ActionNode(EnemyModel3 => EnemyModel3.Seek());
        rootNode = new QuestionNode(context => context.los.IsRange(context.self, context.player)
        && context.los.IsAngle(context.self, context.player) &&
        context.los.IsObstacle(context.self, context.player),
        SeekNode, WanderNode);

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

