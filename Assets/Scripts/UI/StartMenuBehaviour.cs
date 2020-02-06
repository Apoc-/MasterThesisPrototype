using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class StartMenuBehaviour : MonoBehaviour
    {
        public void OnClickStart()
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}
