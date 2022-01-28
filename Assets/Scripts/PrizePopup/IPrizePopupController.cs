using System;

namespace Assets.Scripts.PrizePopup
{
    public interface IPrizePopupController
    {
        event Action<string> OnError;
        event Action<int, int, int> OnPrizeFetch;
        event Action OnCloseWindow;

        void CloseWindow();
        void FetchPrizeInfo();
    }
}

//luscav2m