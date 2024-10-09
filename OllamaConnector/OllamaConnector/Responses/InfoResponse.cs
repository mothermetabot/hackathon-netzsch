namespace OllamaConnector.Responses;

public class InfoResponse : IInfoResponse
{
    public string Params { get; }

    public string Type => throw new NotImplementedException();

    public InfoResponse(string @params)
    {
        Params = @params;
    }
}
