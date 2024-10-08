namespace OllamaConnector.Responses;

public class StopResponse : IStopResponse
{
    public string Type {get;}

    public StopResponse(string type)
    {
        Type = type;
    }
}