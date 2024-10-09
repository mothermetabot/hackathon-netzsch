namespace OllamaConnector.Responses;

public class InfoResponse : IInfoResponse
{
    public string Params { get; }

    public string Type { get; }

    public InfoResponse(string type, string @params)
    {
        Type = type;
        Params = @params;
    }
}
