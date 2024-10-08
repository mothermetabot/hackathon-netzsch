namespace OllamaConnector.Responses;

public interface IStartResponse : IResponse
{
    IEnumerable<Step> Params { get; }
}