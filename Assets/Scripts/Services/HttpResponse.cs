public class HttpResponse<T> : HttpResponse
{
    public HttpResponse(HttpStatus status, string body, T result) : base(status, body)
    {
        Result = result;
    }

    public T Result;
}

public class HttpResponse
{
    public HttpResponse(HttpStatus status, string body)
    {
        Status = status;
        Body = body;
    }

    public HttpStatus Status;
    public string Body;
}

public enum HttpStatus
{
    Ok = 200,
    BadRequest = 400,
    Unauthorized = 401,
    NotFound = 404,
    InternalServerError = 500
}
