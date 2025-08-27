using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1;
    }

    public void ChangeScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
