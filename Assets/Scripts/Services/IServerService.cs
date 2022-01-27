using System.Threading.Tasks;

public interface IServerService
{
    Task<HttpResponse<int>> GetInitialWin();
    Task<HttpResponse<int>> GetMultiplier();
    Task<HttpResponse<long>> GetPlayerBalance();
    Task<HttpResponse> SetPlayerBalance(long value);
}