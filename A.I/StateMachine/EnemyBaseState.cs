using UnityEngine;

public abstract class EnemyBaseState
{
    public abstract void EnterState(PatrolingEnemy enemyAI);
    public abstract void UpdateState(PatrolingEnemy enemyAI);
    public abstract void ExitState(PatrolingEnemy enemyAI);
}
