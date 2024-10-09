namespace OllamaConnector.Responses;

public interface IResponse
{
    /// <summary>
    /// A string representing the type of response received. <see cref="ResponseType"/> for available types.
    /// </summary>
    string Type {get;}
}