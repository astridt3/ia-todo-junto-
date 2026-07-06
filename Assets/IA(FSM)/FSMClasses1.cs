using UnityEngine;

public class FSMClasses1 : MonoBehaviour
{
    State1 currentState { get; set; }

    private PatrolState1 patrolState;
    private PursueState1 pursueState;
    public EnemyControllerFSM1 enemy;
    private FreezeState1 freezeState;
    private SearchState1 searchState;

    public State1 _currentState { get { return currentState; } set { currentState = value; } }

    private void Awake()
    {
        enemy = GetComponent<EnemyControllerFSM1>();
        freezeState = new FreezeState1(this);
        patrolState = new PatrolState1(this);
        pursueState = new PursueState1(this);
        searchState = new SearchState1(this);

        currentState = patrolState;
    }
    public void ChangeToFreeze()
    {
        ChangeState(freezeState);
    }
    public void ChangeToPatrol()
    {
        ChangeState(patrolState);
    }

    public void ChangeToPursue()
    {
        ChangeState(pursueState);
    }

    public void ChangeState(State1 newState)
    {
        if (currentState == newState)
        {
            return;
        }

        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void UpdateState(bool canSeePlayer)
    {
        currentState.Update(canSeePlayer);
    }
    public void ChangeToSearch()
    {
        ChangeState(searchState);
    }
    public void ToSearch() => ChangeState(searchState);
    public void ToPatrol() => ChangeState(patrolState);
    public void ToPursue() => ChangeState(pursueState);
    public void ToFreeze() => ChangeState(freezeState);
}

public abstract class State1
{
    protected FSMClasses1 fsm1;
    public State1(FSMClasses1 fsm)
    {
        this.fsm1 = fsm;
    }

    public virtual void Enter() { }

    public virtual void Exit() { }

    public abstract void Update(bool canSeePlayer);
}

public class PatrolState1 : State1
{
    public PatrolState1(FSMClasses1 fsm1) : base(fsm1) { }

    public override void Update(bool canSeePlayer)
    {
        fsm1.enemy.Wander();

        if (canSeePlayer)
        {
            fsm1.ToPursue();
        }
    }
}

public class PursueState1 : State1
{
    public PursueState1(FSMClasses1 fsm) : base(fsm) { }

    public override void Update(bool canSeePlayer)
    {
        fsm1.enemy.PursueStar();

        float distance = Vector3.Distance(
            fsm1.enemy.transform.position,
            fsm1.enemy.player.position
        );

        if (!canSeePlayer)
        {
            fsm1.ToPatrol();
        }
        else if (distance <= 4f)
        {
            fsm1.ToFreeze();
        }
    }
}
public class FreezeState1 : State1
{
    private float freezeTime1 = 2f;
    private float timer1;

    public FreezeState1(FSMClasses1 fsm) : base(fsm) { }

    public override void Enter()
    {
        timer1 = freezeTime1;
        fsm1.enemy.FreezePlayer(freezeTime1);
    }

    public override void Update(bool canSeePlayer)
    {
        timer1 -= Time.deltaTime;

        if (timer1 <= 0f)
        {
            fsm1.ToSearch();
        }
    }
}
public class SearchState1 : State1
{
    private float totalSearchTime = 15f;
    private float ignorePlayerTime = 7f;
    private float timer;
    public SearchState1(FSMClasses1 fsm1) : base(fsm1) { }
    public override void Enter()
    {
        timer = totalSearchTime;
    }
    public override void Update(bool canSeePlayer)
    {
        timer -= Time.deltaTime;
        if (timer > totalSearchTime - ignorePlayerTime)
        {
            fsm1.enemy.Wander();
            return;
        }
        if (canSeePlayer)
        {
            fsm1.ToPursue();
            return;
        }

        if (timer <= 0f)
        {
            fsm1.ToPatrol();
            return;
        }
        fsm1.enemy.Wander();
    }
}

