namespace OllamaConnector.Responses;

public interface IInfoResponse : IResponse
{
    string Params { get; }
}