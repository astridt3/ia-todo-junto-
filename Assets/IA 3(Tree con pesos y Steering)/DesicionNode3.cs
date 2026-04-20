using System;
using UnityEngine;

public abstract class DecisionNode3
{
    public abstract void Evaluate(EnemyController3 enemy, EnemyContext context);
}

public class QuestionNode3 : DecisionNode3
{
    private Func<EnemyContext, bool> question;
    private DecisionNode3 trueNode;
    private DecisionNode3 falseNode;

    public QuestionNode3(Func<EnemyContext, bool> question, DecisionNode3 trueNode, DecisionNode3 falseNode)
    {
        this.question = question;
        this.trueNode = trueNode;
        this.falseNode = falseNode;
    }

    public override void Evaluate(EnemyController3 enemy, EnemyContext context)
    {
        if (question(context))
            trueNode.Evaluate(enemy, context);
        else
            falseNode.Evaluate(enemy, context);
    }
}

public class ActionNode3 : DecisionNode3
{
    private Action<EnemyController3> action;
    public ActionNode3(Action<EnemyController3> action)
    {
        this.action = action;
    }
    public override void Evaluate(EnemyController3 enemy, EnemyContext context)
    {
        action(enemy);
    }
}