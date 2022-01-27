using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class WheelView : MonoBehaviour
{
    [SerializeField] private WheelPieceView[] wheelPieces;
    [SerializeField] private PieceViewModel[] PieceViews;
    [SerializeField] private Transform pivot;
    [SerializeField] [Range(5f, 80f)] private float startSpeed;
    [SerializeField] [Range(0f, 3f)] private float decreaseStep;

    private float currentAngle;
    private float speed;
    private bool isSpinning;

    public void Start()
    {
        FixAngles();
        SetNumbers();
    }

    public void OnValidate()
    {
        FixAngles();
        SetNumbers();
    }

    public IEnumerator StartSpin()
    {
        speed = startSpeed;
        isSpinning = true;
        while(isSpinning)
        {
            IncreaseAngle();
            DecreaseSpeed();
            yield return null;
        }
    }

    public IEnumerator StopAtTarget(int targetResult)
    {
        isSpinning = false;
        StopCoroutine(StartSpin());

        var targetAngle = wheelPieces.First(wp => wp.Number == targetResult).Angle;
        Debug.Log($"Current: {NormalizeAngle(currentAngle)}, Target: {targetAngle}");
        while (NormalizeAngle(currentAngle) - targetAngle > 3f)
        {
            Debug.Log($"Current: {NormalizeAngle(currentAngle)}, Target: {targetAngle}");
            IncreaseAngle();
            DecreaseSpeed();
            yield return null;
        }
    }

    private double NormalizeAngle (float value)
    {
        return value % 360;
    }

    private void IncreaseAngle()
    {
        currentAngle += speed;
        pivot.transform.eulerAngles = new Vector3(0, 0, currentAngle);
    }

    private void DecreaseSpeed()
    {
        speed = Mathf.Clamp(speed - 0.05f, 0.5f, float.MaxValue);
    }

    public void SetNumbers()
    {
        for (var i = 0; i < wheelPieces.Length; i++)
            wheelPieces[i].SetNumber(PieceViews[i]);
    }

    private void FixAngles()
    {
        if (wheelPieces == null || wheelPieces.Length == 0)
            return;

        float angle = (float) (360) / (float) wheelPieces.Length;
        for (var i = 0; i < wheelPieces.Length; i++)
        {
            wheelPieces[i].SetAngle(angle * -i);
        }
    }
}
