using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace FGWorms.Universal
{
    [DefaultExecutionOrder(int.MinValue)]
    public class Startup : MonoBehaviour
    {
        public UnityEvent InitializeEvent;
        public static bool Initialized;
        
        private void Awake()
        {
            if (Initialized)
                return;
            SceneManager.LoadScene("Master", LoadSceneMode.Additive);
            Initialized = true;
            InitializeEvent?.Invoke();
            StartCoroutine(CoDestroy());
        }

        private IEnumerator CoDestroy()
        {
            yield return null;
            Destroy(gameObject);
        }
    }
}