using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.PrizePopup
{
    public class PrizePopupView : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private PrizeBoxView prizeBoxView;
        [SerializeField] private WheelView wheel;

        private IPrizePopupController popupController;

        public void OnInitialize(IPrizePopupController popupController)
        {
            this.popupController = popupController;

            prizeBoxView.OnSpin += Spin;
            popupController.OnPrizeFetch += UpdateView;
            popupController.OnError += ShowError;
            popupController.OnCloseWindow += CloseWindow;

            //this event we don't need to unsubscribe because it's a UnityEvent, and Unity deals with it's unsubscription for us.
            closeButton.onClick.AddListener(popupController.CloseWindow);
        }

        private void OnDestroy()
        {
            //remove events to avoid leaks, since we're not using UnityEvents here
            prizeBoxView.OnSpin -= Spin;
            popupController.OnPrizeFetch -= UpdateView;
            popupController.OnError -= ShowError;
            popupController.OnCloseWindow -= CloseWindow;
        }

        public void Spin()
        {
            prizeBoxView.DisableButton();
            popupController.FetchPrizeInfo();
            StartCoroutine(wheel.StartSpin());
        }

        public void UpdateView(int initialValue, int multiplierValue, int totalResultValue)
        {
            StartCoroutine(InternalUpdateView(initialValue, multiplierValue, totalResultValue));
        }

        private IEnumerator InternalUpdateView(int initialValue, int multiplierValue, int totalResultValue)
        {
            //view logics is a lot easier with coroutines

            yield return prizeBoxView.SetInitialValue(initialValue);
            yield return wheel.StopAtTarget(multiplierValue);
            yield return prizeBoxView.SetMultiplierValue(multiplierValue);
            yield return prizeBoxView.SetTotalResultValue(totalResultValue);

            prizeBoxView.EnableButton();
        }

        public void CloseWindow() => gameObject.SetActive(false);

        public void ShowError(string error)
        {
            //todo: show some error popup
            Debug.LogError($"[{gameObject.name}] - {error}");
        }
    }
}