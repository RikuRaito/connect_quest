using UnityEngine;
using UnityEngine.SceneManagement; // シーン切り替え用

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string sceneName;
    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}