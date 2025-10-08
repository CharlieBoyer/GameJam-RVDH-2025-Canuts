using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.Exceptions;

namespace Code.Scripts.Utils
{
    public static class AddressablesUtils
    {
        /// <summary>
        /// Helper function to request assets from the <c>Addressables</c> system.
        /// </summary>
        /// <param name="assetLabel">The label of the assets to load, as set in the <em>Addressable Groups</em>.</param>
        /// <param name="runner">The <c>MonoBehaviour</c> to run the Coroutine from.</param>
        /// <param name="onResourcesLoaded">Callback fired upon loading completion, to which the loaded assets will be passed.</param>
        /// <typeparam name="T">Type of the requested assets.</typeparam>
        /// <returns></returns>
        public static Coroutine LoadResources<T>(string assetLabel, MonoBehaviour runner, Action<List<T>> onResourcesLoaded)
        {
            return runner.StartCoroutine(LoadResourceCoroutine<T>(assetLabel, onResourcesLoaded));
        }

        /// <summary>
        /// Use <c>Addressables</c> system to load multiple assets with the same label.<br/>
        /// Resume the game execution by firing the <c>onResourcesLoaded</c> callback.
        /// </summary>
        /// <param name="label">The <c>Addressables</c> label of the assets to load.</param>
        /// <param name="onResourcesLoaded">Callback to pass the <c>List</c> of requested assets. Invoked upon loading completion.</param>
        /// <typeparam name="T">Type of the assets to load.</typeparam>
        /// <returns></returns>
        /// <exception cref="OperationException">Throws if the <c>AsyncOperationHandle</c> fails or if no objects can be loaded.</exception>
        private static IEnumerator LoadResourceCoroutine<T>(string label, Action<List<T>> onResourcesLoaded)
        {
            List<T> resourcesAssets;

            AsyncOperationHandle<IList<T>> fileHandle = Addressables.LoadAssetsAsync<T>(label);

            yield return fileHandle;

            if (fileHandle.Status == AsyncOperationStatus.Succeeded)
            {
                if (fileHandle.Result == null || fileHandle.Result.Count == 0)
                    throw new OperationException($"No {typeof(T).Name} assets found with label '{label}'.");

                resourcesAssets = new List<T>(fileHandle.Result);
                onResourcesLoaded?.Invoke(resourcesAssets);
            }
            else
            {
                throw new OperationException($"{typeof(T).Name} assets load operation failed.");
            }

            Addressables.Release(fileHandle);
        }
    }
}
