using UnityEngine;

public class EnemyDecisionTree : MonoBehaviour
{
    private DecisionNode3 rootNode;

    private void Awake()
    {
        WeightedRandomActionNode lookingNode = new WeightedRandomActionNode(
            new (float, System.Action<EnemyController3>)[]
            {
                (60f, enemy => enemy.FleePlayer()),
                (30f, enemy => enemy.EvadePlayer()),
                (10f, enemy => enemy.Idle())
            }
        );

        WeightedRandomActionNode notLookingNode = new WeightedRandomActionNode(
            new (float, System.Action<EnemyController3>)[]
            {
                (70f, enemy => enemy.ArriveToPlayer()),
                (20f, enemy => enemy.EvadePlayer()),
                (10f, enemy => enemy.Idle())
            }
        );

        rootNode = new QuestionNode3(
            context => context.enemy.IsPlayerLookingAtMe(),
            lookingNode,
            notLookingNode
        );
    }

    public void Evaluate(EnemyController3 enemy, EnemyContext context)
    {
        rootNode.Evaluate(enemy, context);
    }
}