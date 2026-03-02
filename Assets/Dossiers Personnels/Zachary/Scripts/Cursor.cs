using UnityEngine;
using UnityEngine.InputSystem;

public class Cursor : MonoBehaviour
{
    public float offsetX = 10;
    public float offsetY = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Mouse.current.position.ReadValue() + new Vector2(offsetX, offsetY);
    }
}
