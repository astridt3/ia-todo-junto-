using UnityEngine;

public class FSMClasses : MonoBehaviour
{
    State currentState { get; set; }

    private PatrolState patrolState;
    private PursueState pursueState;
    public EnemyControllerFSM enemy;
    private FreezeState freezeState;
    private SearchState searchState;
    public State _currentState { get { return currentState; } set { currentState = value; } }

    private void Awake()
    {
        enemy = GetComponent<EnemyControllerFSM>();
        freezeState = new FreezeState(this);
        patrolState = new PatrolState(this);
        pursueState = new PursueState(this);
        searchState = new SearchState(this);

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

    public void ChangeState(State newState)
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

public abstract class State
{
    protected FSMClasses fsm;
    public State(FSMClasses fsm)
    {
        this.fsm = fsm;
    }

    public virtual void Enter() { }

    public virtual void Exit() { }

    public abstract void Update(bool canSeePlayer);
}

public class PatrolState : State
{
    public PatrolState(FSMClasses fsm) : base(fsm) { }

    public override void Update(bool canSeePlayer)
    {
        fsm.enemy.Wander();

        if (canSeePlayer)
        {
            fsm.ToPursue();
        }
    }
}

public class PursueState : State
{
    public PursueState(FSMClasses fsm) : base(fsm) { }

    public override void Update(bool canSeePlayer)
    {
        fsm.enemy.PursueAStar();

        float distance = Vector3.Distance(
            fsm.enemy.transform.position,
            fsm.enemy.player.position
        );

        if (!canSeePlayer)
        {
            fsm.ToPatrol();
        }
        else if (distance <= 4f)
        {
            fsm.ToFreeze();
        }
    }
}
public class FreezeState : State
{
    private float freezeTime = 2f;
    private float timer;

    public FreezeState(FSMClasses fsm) : base(fsm) { }

    public override void Enter()
    {
        timer = freezeTime;
        fsm.enemy.FreezePlayer(freezeTime);
    }

    public override void Update(bool canSeePlayer)
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            fsm.ToSearch();
        }
    }
}
public class SearchState : State
{
    private float totalSearchTime = 15f;
    private float ignorePlayerTime = 7f;
    private float timer;
    public SearchState(FSMClasses fsm) : base(fsm) { }
    public override void Enter()
    {
        timer = totalSearchTime;
    }
    public override void Update(bool canSeePlayer)
    {
        timer -= Time.deltaTime;
        if (timer > totalSearchTime - ignorePlayerTime)
        {
            fsm.enemy.Wander();
            return;
        }
        if (canSeePlayer)
        {
            fsm.ToPursue();
            return;
        }

        if (timer <= 0f)
        {
            fsm.ToPatrol();
            return;
        }
        fsm.enemy.Wander();
    }
}

