using System;
using System.Collections;
using UnityEngine;

namespace Code.Scripts.Utils
{
    public static class CoroutineUtils
    {
        /// <summary>
        /// Chains multiple coroutines together, running them sequentially.
        /// This function requires a <c>MonoBehaviour</c> instance anchor to run the coroutines.
        /// </summary>
        /// <param name="runner">The <c>MonoBehaviour</c> instance on which the coroutine attaches on and run from.</param>
        /// <param name="actions">Any "yieldable" function to execute</param>
        /// <returns></returns>
        public static IEnumerator Chain(MonoBehaviour runner, IEnumerator[] actions)
        {
            foreach (IEnumerator action in actions) {
                yield return runner.StartCoroutine(action);
            }
        }

        /// <summary>
        /// Specialized coroutine to quickly start an <c>Action</c> after a delay in seconds.
        /// </summary>
        /// <remarks> The <c>Action</c> supports no parameters.</remarks>
        /// <param name="action">The parameterless <c>Action</c> or anonymous function to execute</param>
        /// <param name="delay">Time (in seconds) to wait before starting the 'action'.</param>
        /// <returns></returns>
        /// <example><code>StartCoroutine(CoroutineUtils.DelaySeconds(() => DebugUtils.Log("2 seconds past"), 2);</code></example>
        public static IEnumerator DelaySeconds(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action();
        }

        /// <summary>
        /// Handy coroutine, useful to suspend time inside a coroutine <c>Chain</c>.
        /// </summary>
        /// <param name="time">Amount of time to wait (in seconds).</param>
        /// <returns></returns>
        public static IEnumerator WaitForSeconds(float time) {
            yield return new WaitForSeconds(time);
        }

        /// <summary>
        /// Shortcut coroutine, used with <c>Chain()</c> to execute anonymous functions or parameterless <c>Actions</c>.
        /// </summary>
        /// <param name="action">Anonymous function or parameterless <c>Action</c> to execute.</param>
        /// <returns></returns>
        public static IEnumerator Do(Action action) {
            action();
            yield return 0;
        }
    }
}
