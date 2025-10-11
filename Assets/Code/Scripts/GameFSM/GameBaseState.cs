namespace Code.Scripts.GameFSM
{
    public abstract class GameBaseState
    {
        protected GameStateManager _manager;

        protected bool _isInit = false;

        public abstract void EnterState(GameStateManager context);

        public abstract void UpdateState();

        public abstract void ExitState();
    }
}
