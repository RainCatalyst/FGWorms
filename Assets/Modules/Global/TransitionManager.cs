using UnityEngine.SceneManagement;

namespace FGWorms.Global
{
    public class TransitionManager : MonoSingleton<TransitionManager>
    {
        public void OpenLevel() => SceneManager.LoadScene("Level");
        public void OpenSetup() => SceneManager.LoadScene("Setup");
    }
}