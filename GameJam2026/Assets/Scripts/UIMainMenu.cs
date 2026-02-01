using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    public GameObject main;
    public GameObject creditsUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        creditsUI.SetActive(false);
}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void credits()
    {
        creditsUI.SetActive(true);
        main.SetActive(false);
    }
    public void back()
    {
        creditsUI.SetActive(false);
        main.SetActive(true);
    }
}
