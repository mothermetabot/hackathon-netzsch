using System.Text.Json;

namespace OllamaConnector.Responses;

public class ResponseBase : IResponse
{
    public string Type {get;}

    internal string _internalJson;

    public ResponseBase(string type)
    {
        Type = type;
    }


    public TResponse? As<TResponse>() where TResponse : class, IResponse
    {
        var response = JsonSerializer.Deserialize<TResponse>(_internalJson, Prompt.Options);
        return response;
    }
}
