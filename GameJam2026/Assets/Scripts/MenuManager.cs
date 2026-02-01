using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void BeginGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void Credits()
    {

    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
