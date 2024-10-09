namespace OllamaConnector.Responses;

public class InfoResponse : IInfoResponse
{
    public string Params { get; }

    public InfoResponse(string @params)
    {
        Params = @params;
    }
}
