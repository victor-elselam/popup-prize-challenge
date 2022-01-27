using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PrizePopupView : MonoBehaviour
{
    public Action OnClose;

    
    [SerializeField] private Button closeButton;
    [SerializeField] private PrizeBoxView prizeBoxView;

    private IServerService serverService;

    public void Start()
    {
        prizeBoxView.OnSpin += Spin;
        closeButton.onClick.AddListener(() => OnClose?.Invoke());
    }

    public void OnDestroy()
    {
        prizeBoxView.OnSpin -= Spin;
    }
    
    public void OnInitialize(IServerService serverService)
    {
        this.serverService = serverService;
    }

    //todo: split this business logics to a controller
    public async void Spin()
    {
        var initialPrizeResponse = await serverService.GetInitialWin();
        var multiplierResponse = await serverService.GetMultiplier();
        var currentPlayerBalanceResponse = await serverService.GetPlayerBalance();
        if (!OkStatus(out var error, initialPrizeResponse, multiplierResponse, currentPlayerBalanceResponse))
        {
            Debug.LogError(error);
            return;
        }

        var finalPrize = initialPrizeResponse.Result * multiplierResponse.Result;
        var finalPlayerBalance = currentPlayerBalanceResponse.Result + finalPrize;

        UpdateView(initialPrizeResponse.Result, multiplierResponse.Result, finalPrize);

        var setResult = await serverService.SetPlayerBalance(finalPlayerBalance);
        if (!OkStatus(out error, setResult))
        {
            Debug.LogError($"[Balance] - Error setting player balance: {error}");
            return;
        }

        Debug.Log($"[Balance] - Final player balance: {finalPlayerBalance}");

        bool OkStatus(out string errorMessage, params HttpResponse[] httpResponses)
        {
            errorMessage = null;
            var allOk = httpResponses.All(http => http.Status == HttpStatus.Ok);
            if (!allOk)
                errorMessage = httpResponses.First(http => http.Status != HttpStatus.Ok).Body;

            return allOk;
        }
    }

    public void CloseWindow() => gameObject.SetActive(false);

    public void UpdateView(int initialValue, int multiplierValue, int totalResultValue)
    {
        prizeBoxView.SetValues(initialValue, multiplierValue, totalResultValue);
    }

    public void ShowError(string error)
    {
        //todo: show some error popup
        Debug.LogError($"[{gameObject.name}] - {error}");
    }
}