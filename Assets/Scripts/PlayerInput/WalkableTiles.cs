using UnityEngine;

public class WalkableTiles : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("hi2");
        if (collision.gameObject.CompareTag("WalkableTilemap"))
        {
            Debug.Log("OFF PATH");
        }
    }
}
