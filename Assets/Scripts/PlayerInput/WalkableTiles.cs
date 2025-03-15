using UnityEngine;
using UnityEngine.Tilemaps;

public class WalkableTiles : MonoBehaviour
{
    private void OnTriggerExit2D(UnityEngine.Collider2D collision)
    {
        if (collision.gameObject.CompareTag("WalkableTilemap"))
        {
            Debug.Log("OFF PATH");
            
        }

    }
}
