namespace OllamaConnector.Responses;

public class RawResponse
{
    public string Response { get; }

    public IEnumerable<int> Context {get;}

    public RawResponse(string response, IEnumerable<int> context)
    {
        Response = response;
        Context = context;
    }
}