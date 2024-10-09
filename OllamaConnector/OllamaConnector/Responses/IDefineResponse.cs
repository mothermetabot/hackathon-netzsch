namespace OllamaConnector.Responses;

public interface IDefineResponse : IResponse
{
    IEnumerable<Step> Steps { get; }
}