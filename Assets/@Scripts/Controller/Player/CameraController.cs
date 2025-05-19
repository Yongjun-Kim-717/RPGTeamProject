using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform player;

    Vector3 _offset = new Vector3(0f, 5f, -10f);

    void Update()
    {
        transform.position = player.position + _offset;
    }
}
