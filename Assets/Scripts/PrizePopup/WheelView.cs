using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class WheelView : MonoBehaviour
{
    [SerializeField] private WheelPieceView[] wheelPieces;
    [SerializeField] private PieceViewModel[] PieceViews;
    [SerializeField][Range(0.1f, 10f)] private float minSpinTime;
    [SerializeField][Range(0.1f, 10f)] private float maxSpinTime;

    private float currentAngle;
    public void Start()
    {
        FixAngles();
    }

    public void OnValidate()
    {
        FixAngles();
    }

    public IEnumerator Spin(int targetResult)
    {
        var spinTime = UnityEngine.Random.Range(minSpinTime, maxSpinTime);
        var startTime = DateTime.Now;

        var targetAngle = wheelPieces.First(wp => wp.Number == targetResult).transform.eulerAngles.z;
        float speed = 2;
        while(DateTime.Now.Subtract(startTime).Seconds < spinTime)
        {
            IncreaseAngle();
            yield return null;
        }

        while ((Mathf.Abs(currentAngle % 360) - targetAngle) > 0.1f)
        {
            IncreaseAngle();
            yield return null;
        }

        void IncreaseAngle()
        {
            currentAngle += speed;
            transform.eulerAngles = new Vector3(0, 0, currentAngle);
            speed = Mathf.Clamp(speed -= 0.05f, 0.5f, float.MaxValue);
        }
    }

    public void SetNumbers(PieceViewModel[] numbers)
    {
        for (var i = 0; i < wheelPieces.Length; i++)
            wheelPieces[i].SetNumber(numbers[i]);
    }

    private void FixAngles()
    {
        if (wheelPieces == null || wheelPieces.Length == 0)
            return;

        float angle = (float) (360) / (float) wheelPieces.Length;
        for (var i = 0; i < wheelPieces.Length; i++)
        {
            wheelPieces[i].transform.eulerAngles = new Vector3(0, 0, angle * i);
        }
    }
}
