using UnityEngine;

public class CollapseBridge : MonoBehaviour
{
    public GameObject collapsedBridge;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collapsedBridge.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
