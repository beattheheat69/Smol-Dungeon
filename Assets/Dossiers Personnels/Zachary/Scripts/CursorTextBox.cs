using UnityEngine;
using UnityEngine.InputSystem;

public class CursorTextBox : MonoBehaviour
{
    [SerializeField] float offsetX = 10;
    [SerializeField] float offsetY = 10;

    public bool isOverButton = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isOverButton = false;
    }

    // Update is called once per frame
    void Update()
    {
        //If the cursor is not currently hovering above a button
        if (!isOverButton)
        {
            //Have its position follow the cursor
            UpdatePosition();
        }

    }

    private void UpdatePosition()
    {
        transform.position = Mouse.current.position.ReadValue() + new Vector2(offsetX, offsetY);
    }
}
