using UnityEngine;

public static class MathFunctions
{
    public static Vector2 HooverY(Vector2 targetPosition, float amplitude, float speed)
    {
        return new Vector2(targetPosition.x, targetPosition.y + amplitude * Mathf.Sin(Time.time * speed) * Time.deltaTime);
    }

    public static Vector2 Orbit(Vector2 targetPosition, Vector2 orbitorPosition, float orbitalSpeed)
    {
        float orbitDistance = Vector2.Distance(orbitorPosition, targetPosition);
        float moveX = orbitDistance * Mathf.Cos(Time.time * orbitalSpeed);
        float moveY = orbitDistance * Mathf.Sin(Time.time * orbitalSpeed);
        return new Vector2(targetPosition.x + moveX, targetPosition.y + moveY);
    }

    public static float Flicker(float initialValue, float minValueChange, float maxValueChange)
    {
        float randChange = Random.Range(minValueChange, maxValueChange);
        float randSign = Random.Range(0, 2) == 0 ? -1 : 1; // Randomly choose to increase or decrease
        return initialValue + randChange * randSign;
    }
}
