using System;
using UnityEngine;

public class Treeeee : MonoBehaviour
{
    //EnemyTree va con EnemyControllerFSM + theta star
    private DecisionNode rootNode;
    private DecisionNode questionAttackNode;

    private void Awake()
    {
        //ActionNode patrolNode = new ActionNode(EnemyModel3 => EnemyModel3.Patrol());
        //ActionNode PursueNode = new ActionNode(EnemyModel3 => EnemyModel3.Pursue());
        ActionNode freezeNode = new ActionNode(enemy => enemy.Freeze());
        ActionNode searchNode = new ActionNode(enemy => enemy.Search());
        ActionNode pursueNode = new ActionNode(enemy => enemy.PursueAStar());
        ActionNode patrolNode = new ActionNode(enemy => enemy.Patrol());

        QuestionNode obstacleNode = new QuestionNode(
    context => context.los.IsObstacle(context.self, context.player),
    searchNode,
    pursueNode
);

        QuestionNode chaseNode = new QuestionNode(
            context => context.los.IsRange(context.self, context.player),
            obstacleNode,
            patrolNode
        );

        QuestionNode attackNode = new QuestionNode(
            context => context.los.IsRangeAttack(context.self, context.player),
            freezeNode,
            chaseNode
        );
        rootNode = new QuestionNode(
    context => context.ignorePlayer,
    searchNode,
    attackNode
);

    }

    public void Evaluate(EnemyController enemy, EnemyContext context)
    {
        rootNode.Evaluate(enemy, context);
    }

}



