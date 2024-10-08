namespace OllamaConnector.Responses;

public interface IDefineResponse : IResponse
{
    IEnumerable<Step> Steps { get; }
}

public record Step(
    double TargetTemperature,
    double HeatingRate,
    int DurationInSeconds,
    string Type
);