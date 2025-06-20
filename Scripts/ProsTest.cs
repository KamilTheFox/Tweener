using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProsTest : MonoBehaviour
{
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vector3 direction;
    [SerializeField] private float distance;
    [SerializeField] private float duration;
    [SerializeField] private float currentTime;
    [SerializeField] private bool isMoving;
    [SerializeField] private Vector3 axisMask; // Например (0,1,0) для движения только по Y

    public void StartProjectedMovement(Vector3 moveDirection, float moveDistance, float moveDuration, Vector3 mask)
    {
        startPosition = transform.position;
        direction = moveDirection.normalized;
        distance = moveDistance;
        duration = moveDuration;
        currentTime = 0f;
        axisMask = mask;
        isMoving = true;
    }

    private void Update()
    {
        if (!isMoving) return;

        currentTime += Time.deltaTime;
        float progress = currentTime / duration;

        // Можно использовать разные функции плавности
        float smoothProgress = SmoothStep(progress); // или другие варианты

        if (progress >= 1f)
        {
            isMoving = false;
            progress = 1f;
            smoothProgress = 1f;
        }

        // Вычисляем новую позицию
        Vector3 movement = direction * (distance * smoothProgress);
        // Применяем маску осей
        //Vector3 maskedMovement = Vector3.Scale(movement, axisMask);
        transform.position = GetProjectedPosition(direction, axisMask);
    }
    public Vector3 GetProjectedPosition(Vector3 direction, Vector3 axisMask)
    {
        Vector3 normalizedDir = direction.normalized;
        Vector3 result = Vector3.zero;

        if (axisMask.x != 0)
        {
            float angleX = Vector3.Angle(normalizedDir, Vector3.right);
            float angleRadX = angleX * Mathf.Deg2Rad;
            float projectedDistanceX = axisMask.x / Mathf.Cos(angleRadX);
            result += normalizedDir * projectedDistanceX;
        }

        if (axisMask.y != 0)
        {
            float angleY = Vector3.Angle(normalizedDir, Vector3.up);
            float angleRadY = angleY * Mathf.Deg2Rad;
            float projectedDistanceY = axisMask.y / Mathf.Cos(angleRadY);
            result += normalizedDir * projectedDistanceY;
        }

        if (axisMask.z != 0)
        {
            float angleZ = Vector3.Angle(normalizedDir, Vector3.forward);
            float angleRadZ = angleZ * Mathf.Deg2Rad;
            float projectedDistanceZ = axisMask.z / Mathf.Cos(angleRadZ);
            result += normalizedDir * projectedDistanceZ;
        }

        return result;
    }
    Vector3 GetProjectedPosition2(Vector3 direction, float heightY)
    {
        Vector3 upVector = Vector3.up * heightY;
        // Тут важно: мы берем длину проекции и умножаем на direction
        float projectedLength = Vector3.Dot(upVector, direction.normalized);
        return direction.normalized * projectedLength;
    }
    // Простая функция плавности
    private float SmoothStep(float x)
    {
        // Можно использовать разные варианты
        return x * x * (3f - 2f * x); // Плавное ускорение и замедление
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(startPosition, direction);

        Gizmos.color = Color.red;

        Gizmos.DrawSphere(GetProjectedPosition(direction, axisMask), 0.2F);

        Gizmos.color = Color.cyan;

        Gizmos.DrawSphere(GetProjectedPosition2(direction, axisMask.y), 0.21F);

        Gizmos.color = Color.green;

        Gizmos.DrawSphere(Vector3.Project(axisMask, direction),0.2F);

        Gizmos.color = Color.blue;

        Gizmos.DrawSphere(Vector3.Project(direction, axisMask), 0.2F);

        Gizmos.color = Color.magenta;

        Gizmos.DrawLine(startPosition, startPosition + axisMask);

    }

    // Дополнительные функции плавности на выбор
    private float EaseInOut(float x)
    {
        return -(Mathf.Cos(Mathf.PI * x) - 1f) / 2f;
    }

    private float Linear(float x)
    {
        return x;
    }
}
