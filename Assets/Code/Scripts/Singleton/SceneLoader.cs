using Code.Scripts.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Scripts.Singleton
{
    public class SceneLoader : SingletonMonoBehaviour<SceneLoader>
    {
        public void Load(string sceneName)
        {
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
