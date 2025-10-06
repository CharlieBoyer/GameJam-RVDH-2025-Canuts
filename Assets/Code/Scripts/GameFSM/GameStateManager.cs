using Code.Scripts.Utils;
using UnityEngine;

namespace Code.Scripts.GameFSM
{
    public class GameStateManager: MonoBehaviour
    {
        public GameStateInstances States { get; } = new();

        private GameBaseState _currentState;

        private void Start()
        {
            _currentState = States.Intro;
            _currentState.EnterState(this);
        }

        private void Update()
        {
            _currentState.UpdateState();
        }

        /// <summary>
        /// Exits the current state by running post-exit logic, then enters the new state.
        /// </summary>
        /// <param name="newState"><c>GameBaseState</c> instance to transition to.</param>
        public void SwitchState(GameBaseState newState)
        {
            _currentState.ExitState();
            _currentState = newState;
            _currentState.EnterState(this);
        }

        /// <summary>
        /// Overload that delays the state switch after exiting the current one.
        /// </summary>
        /// <param name="newState"><c>GameBaseState</c> instance to transition to.</param>
        /// <param name="delay">Time (in seconds) before starting the new state.</param>
        public void SwitchState(GameBaseState newState, float delay)
        {
            _currentState.ExitState();
            StartCoroutine(CoroutineUtils.DelaySeconds(() => { SwitchState(newState); }, delay));
        }
    }
}
