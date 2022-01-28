using DG.Tweening;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.PrizePopup
{
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
            while (isSpinning)
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

            var distance = transform.localEulerAngles.z - targetAngle;
            var duration = distance / speed; //time = distance / speed
            yield return pivot.transform.DOLocalRotate(new Vector3(0, 0, targetAngle), 3f, RotateMode.FastBeyond360).WaitForCompletion();
        }

        private void IncreaseAngle()
        {
            currentAngle -= speed;
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

            var angle = 360 / (float)wheelPieces.Length;
            for (var i = 0; i < wheelPieces.Length; i++)
            {
                wheelPieces[i].SetAngle(angle * -i);
            }
        }
    }
}