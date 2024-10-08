
namespace OllamaConnector.Responses;

public sealed class DefineResponse : IDefineResponse
{
    public DefineResponse(string type, IEnumerable<Step> steps)
    {
        Type = type;
        Steps = steps;
    }

    public IEnumerable<Step> Steps { get; }

    public string Type {get;}
}