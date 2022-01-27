using System;
using System.Linq;
using UnityEngine;

public class PrizePopupController : IPrizePopupController
{
    private IServerService serverService;
    public event Action<string> OnError;
    public event Action<int, int, int> OnPrizeFetch;
    public event Action OnCloseWindow;

    public PrizePopupController(IServerService serverService)
    {
        this.serverService = serverService;
    }

    public async void FetchPrizeInfo()
    {
        var initialPrizeResponse = await serverService.GetInitialWin();
        var multiplierResponse = await serverService.GetMultiplier();
        var currentPlayerBalanceResponse = await serverService.GetPlayerBalance();
        if (!OkStatus(out var error, initialPrizeResponse, multiplierResponse, currentPlayerBalanceResponse))
        {
            Debug.LogError(error);
            OnError?.Invoke(error);
            return;
        }

        var finalPrize = initialPrizeResponse.Result * multiplierResponse.Result;
        var finalPlayerBalance = currentPlayerBalanceResponse.Result + finalPrize;

        OnPrizeFetch?.Invoke(initialPrizeResponse.Result, multiplierResponse.Result, finalPrize);

        var setResult = await serverService.SetPlayerBalance(finalPlayerBalance);
        if (!OkStatus(out error, setResult))
        {
            Debug.LogError($"[Balance] - Error setting player balance: {error}");
            OnError?.Invoke(error);
            return;
        }

        Debug.Log($"[Balance] - Final player balance: {finalPlayerBalance}");
    }

    public void CloseWindow()
    {
        //I opted to use this flow of view -> controller -> event to view, so we could make some analytics logic or log something
        //and this kind of logic can't be in the view
        OnCloseWindow?.Invoke();
    }

    private bool OkStatus(out string errorMessage, params HttpResponse[] httpResponses)
    {
        errorMessage = null;
        var allOk = httpResponses.All(http => http.Status == HttpStatus.Ok);
        if (!allOk)
            errorMessage = httpResponses.First(http => http.Status != HttpStatus.Ok).Body;

        return allOk;
    }
}
