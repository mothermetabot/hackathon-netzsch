
namespace OllamaConnector.Responses;

public class StartResponse : IStartResponse
{
    public StartResponse(string type, IEnumerable<Step> @params)
    {
        Type = type;
        Params = @params;
    }

    public IEnumerable<Step> Params {get;}

    public string Type {get;}
}