using UnityEngine;

public class Hoover : MonoBehaviour
{
    [SerializeField] float _speed = 1f;
    [SerializeField] float _distance = 3f;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y + _distance * Mathf.Sin(Time.time * _speed) * Time.deltaTime);
    }
}
