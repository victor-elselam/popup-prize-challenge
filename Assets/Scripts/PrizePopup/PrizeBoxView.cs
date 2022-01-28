using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.PrizePopup
{
    public class PrizeBoxView : MonoBehaviour
    {
        public Action OnSpin;

        [SerializeField] private Text initialPrize;
        [SerializeField] private Text multiplier;
        [SerializeField] private Text totalResult;

        [SerializeField] private Button spinButton;

        private void Start()
        {
            ClearFields();

            spinButton.onClick.AddListener(() =>
            {
                OnSpin?.Invoke();
                ClearFields();
            });
        }

        private void ClearFields()
        {
            initialPrize.text = string.Empty;
            multiplier.text = string.Empty;
            totalResult.text = string.Empty;
        }

        public IEnumerator SetInitialValue(int initialValue)
        {
            yield return initialPrize.DOTextDouble(0, initialValue, 2f, it => it.ToString("F2")).WaitForCompletion();
            yield return null;
        }

        public IEnumerator SetMultiplierValue(int initialValue)
        {
            multiplier.text = initialValue.ToString(); //todo: format to money method
            yield return multiplier.transform.DOScale(Vector3.one * 1.5f, 0.3f).SetLoops(5, LoopType.Yoyo).WaitForCompletion();
            multiplier.transform.DOScale(Vector3.one, 0.2f);
        }

        public IEnumerator SetTotalResultValue(int totalValue)
        {
            totalResult.DOTextDouble(0, totalValue, 2f, it => it.ToString("F2"));
            yield return totalResult.transform.DOScale(Vector3.one * 1.5f, 0.3f).SetLoops(5, LoopType.Yoyo).WaitForCompletion();
            totalResult.transform.DOScale(Vector3.one, 0.2f);
        }

        internal void DisableButton() => spinButton.enabled = false;
        internal void EnableButton() => spinButton.enabled = true;
    }
}