namespace Code.Scripts.GameFSM
{
    public abstract class GameBaseState
    {
        public abstract void EnterState(GameStateManager context);

        public abstract void UpdateState();

        public abstract void ExitState();
    }
}
