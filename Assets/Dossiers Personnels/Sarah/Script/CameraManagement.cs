using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraManagement : MonoBehaviour
{
    [SerializeField]
    float transitionDuration = 0.5f;
    Vector3 roomCenter;
    Vector2 velocity;
    bool isTransitionning;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        roomCenter = GameObject.Find("DungeonRoom_Entrance").transform.Find("CameraPoint").transform.position;
        isTransitionning = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (isTransitionning) // only moves when changing rooms
        {
            //move camera towards center of room
            Vector2 newPos = Vector2.SmoothDamp(transform.position, roomCenter, ref velocity, transitionDuration);
            transform.position = new Vector3(newPos.x, newPos.y, -10f); // z always stays the same (tried not using with a vector2 and would set it to 0 and we would get a black screen)
            if (Vector2.Distance(transform.position, roomCenter) < 0.05f)
            {
                isTransitionning = false;
            }
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
        roomCenter = new Vector3(newRoomPosition.x, newRoomPosition.y);
        isTransitionning=true;

    }
}
