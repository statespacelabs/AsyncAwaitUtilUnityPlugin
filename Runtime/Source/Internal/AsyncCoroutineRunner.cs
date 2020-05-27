using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using Unity.EditorCoroutines.Editor;
#endif

namespace UnityAsyncAwaitUtil
{
    public class AsyncCoroutineRunner : MonoBehaviour, AsyncCoroutineRunner.ICoroutineRunner
    {
        public interface ICoroutineRunner
        {
            object StartCoroutine(IEnumerator routine);
        }

#if UNITY_EDITOR
        class EditorAsyncCoroutineRunner : ICoroutineRunner
        {
            object ICoroutineRunner.StartCoroutine(IEnumerator routine)
            {
                return EditorCoroutineUtility.StartCoroutine(routine, this);
            }
        }
#endif

        static ICoroutineRunner _instance;

        public static ICoroutineRunner Instance
        {
            get
            {
#if UNITY_EDITOR
                if (_instance == null && !Application.isPlaying)
                {
                    _instance = new EditorAsyncCoroutineRunner();
                }
#endif
                if (_instance == null)
                {
                    _instance = new GameObject("AsyncCoroutineRunner")
                        .AddComponent<AsyncCoroutineRunner>();
                }

                return _instance;
            }
        }

        void Awake()
        {
            // Don't show in scene hierarchy
            gameObject.hideFlags = HideFlags.HideAndDontSave;

            DontDestroyOnLoad(gameObject);
        }

        object ICoroutineRunner.StartCoroutine(IEnumerator routine)
        {
            return StartCoroutine(routine);
        }
    }
}