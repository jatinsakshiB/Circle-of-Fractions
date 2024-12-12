using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{


    public void InfoButtonClick()
    {
        Application.OpenURL("https://www.jbdev.in");
    }

    public void NextLevelButtonClick()
    {
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level", 1)+1);
        PlayerPrefs.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}
