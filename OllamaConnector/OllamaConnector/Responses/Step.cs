namespace OllamaConnector.Responses;

""type"": ""dynamic"" | ""isothermal"",
            ""targetTemperature"": number,
            ""heatingRate"": number,
            ""durationInSeconds"": number

            /// <summary>
            /// An object representing a temperature segment.
            /// </summary>
            /// <param name="Type">The type of segment, either isothermal or dynamic. Can be of any value in <see cref="StepType"/></param>.
            /// <param name="TargetTemperature">The target temeperature of the segment.</param>
            /// <param name="HeatingRate">The heating rate.</param>
            /// <param name="DurationInSeconds">The duration of the segment in seconds.</param>
public record Step(
    
    string Type,
    double TargetTemperature,
    double HeatingRate,
    int DurationInSeconds 
)