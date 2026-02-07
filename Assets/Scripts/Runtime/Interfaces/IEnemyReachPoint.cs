/* --------------------------------------------------------------------------------------------
 * Interface that handles when enemy reach a point of the path
 * -------------------------------------------------------------------------------------------- */
interface IEnemyReachPoint
{
    void OnEnemyReachPoint(EnemyMove enemyMove, int currentPointIndex);
}

/* --------------------------------------------------------------------------------------------
 * Interface that handles when enemy reach the end of the path
 * -------------------------------------------------------------------------------------------- */

interface IEnemyReachEnd
{
    void OnEnemyReachEnd(EnemyMove enemyMove);
}