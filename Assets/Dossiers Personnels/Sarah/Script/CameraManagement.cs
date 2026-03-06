using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraManagement : MonoBehaviour
{
    [SerializeField]
    float transitionDuration = 0.5f;
    Vector3 roomCenter;
    Vector3 velocity = Vector3.zero;
    bool isTransitionning;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        roomCenter = GameObject.Find("Room").transform.Find("CameraPoint").transform.position;
        GetComponent<Camera>().aspect = 1.0f;
        isTransitionning = true;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //move camera towards center of room
        transform.position = Vector3.SmoothDamp(transform.position, roomCenter, ref velocity, transitionDuration);
        //if camera is not in place pause objects if in place unpause objects
        if (Vector2.Distance(transform.position, roomCenter) < 0.05f)
        {
            isTransitionning = false;
        }
        else
        {
            isTransitionning = true;
        }
    }

    //Gives if camera is moving
    public bool GetTransitionning()
    {
        return isTransitionning;
    }

    public void MoveToNewRoom(Vector3 newRoomPosition)
    {
        //Put next room center point has new center point
        roomCenter = new Vector3(newRoomPosition.x, newRoomPosition.y, transform.position.z);

    }
}
