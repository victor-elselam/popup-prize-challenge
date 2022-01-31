using DG.Tweening;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.PrizePopup
{
    public class WheelView : MonoBehaviour
    {
        [SerializeField] private WheelPieceView[] wheelPieces;
        [SerializeField] private WheelPieceViewModel[] PieceViews;
        [SerializeField] private Transform pivot;
        [SerializeField] [Range(50, 300f)] private float startSpeed;
        [SerializeField] [Range(0f, 10)] private float decreaseStep;

        private float currentAngle;
        private float? targetAngle;
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
            targetAngle = null;
            while (isSpinning)
            {
                IncreaseAngle();
                DecreaseSpeed();
                yield return null;

                if (targetAngle != null)
                {
                    var currentAngle = NormalizeAngle(this.currentAngle);
                    var floatTargetAngle = NormalizeAngle((float) (targetAngle)) * -1;

                    if (Mathf.Abs(currentAngle - floatTargetAngle) < 2f)
                    {
                        isSpinning = false;
                        break;
                    }
                }
            }
        }

        public IEnumerator StopAtTarget(int targetResult)
        {
            targetAngle = wheelPieces.First(wp => wp.Number == targetResult).Angle;
            while (isSpinning)
                yield return null;
        }

        private float NormalizeAngle(float angle)
        {
            angle = angle %= 360;
            if (angle > 180)
                return angle - 360;

            return angle;
        }

        private void IncreaseAngle()
        {
            currentAngle += speed * Time.deltaTime;
            pivot.transform.eulerAngles = new Vector3(0, 0, NormalizeAngle(currentAngle));
        }

        private void DecreaseSpeed()
        {
            speed = Mathf.Clamp(speed - decreaseStep, 0.5f, float.MaxValue);
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

            var angle = 360 / (float)wheelPieces.Length;
            for (var i = 0; i < wheelPieces.Length; i++)
            {
                wheelPieces[i].SetAngle(angle * i);
            }
        }
    }
}