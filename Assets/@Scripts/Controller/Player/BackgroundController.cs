using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] Transform player;

    Vector3 _offset = new Vector3(0f, 0f, 200f);

    void Update()
    {
        transform.position = player.position + _offset;
    }
}
