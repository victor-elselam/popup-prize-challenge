
using UnityEngine;

public class Initialize : MonoBehaviour
{
    [SerializeField] private PrizePopupView popupView;
    private IServerService serverService; 
    
    private void Start()
    {
        serverService = new ServerService(new Server.API.GameplayApi());

        var popupController = new PrizePopupController(serverService);
        Instantiate(popupView).OnInitialize(popupController);
    }
}
