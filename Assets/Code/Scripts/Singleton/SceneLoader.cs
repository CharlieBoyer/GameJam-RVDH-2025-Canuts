using System;
using System.Collections;
using Code.Scripts.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Scripts.Singleton
{
    public class SceneLoader : SingletonMonoBehaviour<SceneLoader>
    {
        [SerializeField] private Animator _transitionAnimator;
        [SerializeField] private float _transitionDuration;

        private static readonly int Start = Animator.StringToHash("Start");

        public void Load(string sceneName)
        {
            StartCoroutine(LoadCoroutine(sceneName));
        }

        private IEnumerator LoadCoroutine(string sceneName)
        {
            _transitionAnimator.SetTrigger(Start);

            yield return new WaitForSeconds(3f);

            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }

        public void Exit()
        {
            #if UNITY_EDITOR
                EditorApplication.ExitPlaymode();
            #endif

            Application.Quit();
        }
    }
}
