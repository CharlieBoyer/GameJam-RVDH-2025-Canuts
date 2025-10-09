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
        /// Helper function to request a single asset object from the <c>Addressables</c> system.
        /// </summary>
        /// <param name="address">The asset's address defined by the <c>Addressable</c> system.</param>
        /// <param name="runner">The <c>MonoBehaviour</c> to run the Coroutine from.</param>
        /// <param name="onAssetLoaded">Callback fired upon loading completion, to which the loaded asset will be passed.</param>
        /// <typeparam name="T">Type of the requested asset.</typeparam>
        /// <returns>A Coroutine object for checking the routine's lifetime</returns>
        public static Coroutine LoadSingleAsset<T>(string address, MonoBehaviour runner, Action<T> onAssetLoaded)
        {
            return runner.StartCoroutine(LoadSingleAssetRoutine<T>(address, onAssetLoaded));
        }

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
        /// Request a single asset object from the <c>Addressables</c> system.
        /// </summary>
        /// <param name="addressKey">The asset's address defined by the <c>Addressable</c> system.</param>
        /// <param name="onAssetLoaded">Callback fired upon loading completion, to which the loaded asset will be passed.</param>
        /// <typeparam name="T">Type of the requested asset.</typeparam>
        /// <returns></returns>
        /// <exception cref="OperationException">Throws if the <c>AsyncOperationHandle</c> fails or if no object had been found.</exception>
        private static IEnumerator LoadSingleAssetRoutine<T>(string addressKey, Action<T> onAssetLoaded)
        {
            T loadedAsset;

            AsyncOperationHandle handle = Addressables.LoadAssetAsync<T>(addressKey);

            yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                loadedAsset = (T) handle.Result ?? throw new OperationException($"No {typeof(T).Name} assets found from address: '{addressKey}'.");
                onAssetLoaded?.Invoke(loadedAsset);
            }
            else
            {
                throw new OperationException($"{typeof(T).Name} assets load operation failed.");
            }
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
