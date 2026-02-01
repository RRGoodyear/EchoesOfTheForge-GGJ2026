using UnityEngine;
using UnityEngine.SceneManagement;
public class NextLevelTrigger : MonoBehaviour
{
    public string levelSceneName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GoToLevel(string name)
    {
        SceneManager.LoadScene(name);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GoToLevel(levelSceneName);
        }
    }
}
