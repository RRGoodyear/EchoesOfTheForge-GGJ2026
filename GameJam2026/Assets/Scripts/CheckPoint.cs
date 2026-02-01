using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheckPoint : MonoBehaviour
{
    public TMP_Text checkPointReachedText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //checkPointReachedText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //checkPointReachedText.text = "Checkpoint Reached!";
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //checkPointReachedText.text = "";
        }
    }
}
