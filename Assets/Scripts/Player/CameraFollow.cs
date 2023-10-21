using UnityEngine;
using UnityEngine.Serialization;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] 
    public Transform player;
    public float speed = 2.0f; 

    private void LateUpdate()
    {
        if (player != null)
        {
// Calculate the direction towards the player
            Vector2 direction = (player.position - transform.position).normalized;

            // Move the object towards the player
            transform.position += (Vector3)direction * speed * Time.deltaTime;
        }
    }
}