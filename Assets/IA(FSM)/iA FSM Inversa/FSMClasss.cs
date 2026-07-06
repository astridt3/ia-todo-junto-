//using UnityEngine;

//public class FSMClasss : MonoBehaviour
//{

//    //FSMClasses va con enemyCotroller + A star
//    State currentStatee { get; set; }

//    private PatrolState patrolState;
//    public Controller enemy;

//    private SeekState seekState;
//    private PathState pathState;
//    private AttackState attackState;
//    public State _currentStatee { get { return currentStatee; } set { currentStatee = value; } }

//    private void Awake()
//    {
//        enemy = GetComponent<Controller>();
//        patrolState = new PatrolState(this);
//        seekState = new SeekState(this);
//        pathState = new PathState(this);
//        attackState = new AttackState(this);


//        currentStatee = patrolState;
//    }


//    public void ChangeState(State newState)
//    {
//        if (currentStatee == newState)
//        {
//            return;
//        }

//        currentStatee.Exitt();
//        currentStatee = newState;
//        currentStatee.Enterr();
//    }

//    public void UpdateState(bool canSeePlayer)
//    {
//        currentStatee.Update(canSeePlayer);
//    }


//    public void ToPatrol() => ChangeState(patrolState);
//    public void ToSeek() => ChangeState(seekState);
//    public void ToPath() => ChangeState(pathState);
//    public void ToAttack() => ChangeState(attackState);
//}

//public abstract class State
//{
//    protected FSMClasss fsmm;
//    public State(FSMClasss fsmm)
//    {
//        this.fsmm = fsmm;
//    }

//    public virtual void Enterr() { }

//    public virtual void Exitt() { }

//    public abstract void Update(bool canSeePlayer);
//}

//public class PatrolStatee : State
//{
//    public PatrolState(FSMClasss fsmm) : base(fsmm) { }

//    public override void Update(bool canSeePlayer)
//    {
//        fsmm.enemy.PatrolNodes();

//        if (canSeePlayer)
//            fsmm.ToSeek();
//    }
//}
//public class SeekState : State
//{
//    public SeekState(FSMClasss fsmm) : base(fsmm) { }

//    public override void Update(bool canSeePlayer)
//    {
//        if (!canSeePlayer)
//        {
//            fsmm.ToPatrol();
//            return;
//        }

//        if (fsmm.enemy.los.IsRangeAttack(fsmm.enemy.transform, fsmm.enemy.player))
//        {
//            fsmm.ToAttack();
//            return;
//        }

//        if (fsmm.enemy.los.IsObstacle(fsmm.enemy.transform, fsmm.enemy.player))
//        {
//            fsmm.ToPath();
//            return;
//        }

//        fsmm.enemy.Seek();
//    }
//}

//public class PathStatee : State
//{
//    public PathState(FSMClasss fsmm) : base(fsmm) { }

//    public override void Update(bool canSeePlayer)
//    {
//        if (!canSeePlayer)
//        {
//            fsmm.ToPatrol();
//            return;
//        }

//        if (fsmm.enemy.los.IsRangeAttack(fsmm.enemy.transform, fsmm.enemy.player))
//        {
//            fsmm.ToAttack();
//            return;
//        }

//        if (!fsmm.enemy.los.IsObstacle(fsmm.enemy.transform, fsmm.enemy.player))
//        {
//            fsmm.ToSeek();
//            return;
//        }

//        fsmm.enemy.ChaseWithPath();
//    }
//}

//public class AttackState : State
//{
//    public AttackState(FSMClasss fsmm) : base(fsmm) { }

//    public override void Enter()
//    {
//        fsmm.enemy.Attack();
//    }

//    public override void Update(bool canSeePlayer)
//    {
//        if (!fsmm.enemy.los.IsRangeAttack(fsmm.enemy.transform, fsmm.enemy.player))
//            fsmm.ToSeek();
//    }
//}