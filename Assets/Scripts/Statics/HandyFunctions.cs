using System.Collections;
using UnityEngine;

public class HandyFunctions : MonoBehaviour
{
    //Script for easy to access functions

    public static IEnumerator HitStop()
    {
        Debug.Log("Hit Stop");
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = 1f;
    }
}
