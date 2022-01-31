using Server.API;
using System;
using System.Threading.Tasks;

public class ServerService : IServerService
{
    private readonly GameplayApi api;

    //IMPORTANT: I personally don't really like using Promises in C#, so I wrapped it into Tasks with kind of a REST api.
    //But in a real application I can work with any async concept you want :) 
    //also, a small drawback of native C# Tasks is that breakpoints can't really tell you what's happening, but this is solved by using the UniTask framework
    public ServerService(GameplayApi api)
    {
        this.api = api;
        api.Initialise();
    }

    public async Task<HttpResponse<int>> GetInitialWin()
    {
        int? result = null;
        Exception exception = null;
        api.GetInitialWin().Done(
            (value) => result = value, 
            (e) => exception = e);

        while (result == null && exception == null)
            await Task.Delay(100);

        return exception != null ? 
            new HttpResponse<int>(HttpStatus.BadRequest, exception.Message, 0) : 
            new HttpResponse<int>(HttpStatus.Ok, "Success", (int) result);
    }

    public async Task<HttpResponse<int>> GetMultiplier()
    {
        int? result = null;
        Exception exception = null;
        api.GetMultiplier().Done(
            (value) => result = value,
            (e) => exception = e);

        while (result == null && exception == null)
            await Task.Delay(100);

        return exception != null ? 
            new HttpResponse<int>(HttpStatus.BadRequest, exception.Message, 0) : 
            new HttpResponse<int>(HttpStatus.Ok, "Success", (int) result);
    }

    public async Task<HttpResponse<long>> GetPlayerBalance()
    {
        long? result = null;
        Exception exception = null;
        api.GetPlayerBalance().Done(
            (value) => result = value,
            (e) => exception = e);

        while (result == null && exception == null)
            await Task.Delay(100);

        return exception != null ? 
            new HttpResponse<long>(HttpStatus.BadRequest, exception.Message, 0) : 
            new HttpResponse<long>(HttpStatus.Ok, "Success", (long) result);
    }

    public async Task<HttpResponse> SetPlayerBalance(long value)
    {
        bool ready = false;
        Exception exception = null;
        api.SetPlayerBalance(value).Done(() => ready = true);
        while (!ready && exception == null)
            await Task.Delay(100);

        return exception != null ? 
            new HttpResponse(HttpStatus.BadRequest, exception.Message) : 
            new HttpResponse(HttpStatus.Ok, "Success");
    }
}
