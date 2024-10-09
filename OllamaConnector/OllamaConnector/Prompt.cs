﻿using System.Net.Http.Json;
using System.Text.Json;
using OllamaConnector.Responses;

namespace OllamaConnector;

public static class Prompt
{
    private static readonly List<int> _context = new List<int>();
    private static HttpClient _httpClient = new();
    private const string URL = "http://localhost:11434/api/generate";
    public static readonly JsonSerializerOptions Options = new JsonSerializerOptions(){
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private static async Task<(RawResponse? Reponse, string json)> SendAsync(string prompt)
    {
        System.Console.WriteLine($"Sending: {prompt}.");
        var request = new Request(prompt, _context);
        var httpContent = JsonContent.Create(request, options: Options);
        var response = await _httpClient.PostAsync(URL, httpContent);

        var json = await response.Content.ReadAsStringAsync();
        System.Console.WriteLine($"Response json: {json}");

        var rawResponse =  JsonSerializer.Deserialize<RawResponse>(json: json, Options);

        if(rawResponse != null)
            BuildContext(rawResponse);

        System.Console.WriteLine($"Response obj: {rawResponse}");

        return (rawResponse, json);
    }

    /// <summary>
    /// Sends initial prompt.
    /// </summary>
    /// <returns></returns>
    public static async Task Initialize()
    {
        var (response, _) = await SendAsync(INITIAL_PROMPT);
    }

    private static void BuildContext(RawResponse response)
    {
        _context.AddRange(response.Context);
    }

    /// <summary>
    /// Sends a prompt and waits for a message.
    /// </summary>
    /// <param name="prompt"></param>
    /// <returns></returns>
    public static async Task<IResponse?> Send(string prompt)
    {
        var (response, json) = await SendAsync(prompt);

        var baseObj = JsonSerializer.Deserialize<ResponseBase>(response.Response, Options);


        if(baseObj != null)
            baseObj._internalJson = json;

        Console.WriteLine(baseObj?.Type);
        return baseObj;
    }

    /// <summary>
    /// Sends a prompt and waits for a message of type <typeparamref name="TResponse"/>
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="prompt"></param>
    /// <returns></returns>
    public static async Task<IResponse?> Send<TResponse>(string prompt) where TResponse: class, IResponse
    {
        var response = (ResponseBase?)await Send(prompt) ;
        return response?.As<TResponse>();
    }


    private const string INITIAL_PROMPT = @"You are an expert in Thermal Analysis, specializing in Differential Scanning Calorimetry (DSC) measurements.

You will receive queries regarding DSC measurements, and you should respond **only** using one of the following JSON formats:

---

**1. Start a Measurement**

*When the user instructs you to start a measurement:*

{
    ""type"": ""start"",
    ""params"": [
        // The previously provided list of steps.
    ],
    ""context"": ""// Additional information if no 'define' command was sent before.""
}


---

**2. Stop a Measurement**

*When the user instructs you to stop a measurement:*

{
    ""type"": ""stop""
}

---

**3. Define Measurement Steps**

*When the user instructs you to create a list of steps to measure a sample:*

{
    ""type"": ""define"",
    ""params"": [
        {
            ""type"": ""dynamic"" | ""isothermal"",
            ""targetTemperature"": number,
            ""heatingRate"": number,
            ""durationInSeconds"": number
        }
        // Additional steps as necessary...
    ]
}

---

**4. Request Clarification**

*When the user's instruction is unclear or you need more information:*

{
    ""type"": ""clarification"",
    ""params"": ""Your question to the user.""
}

---

**5. Provide Information**

*When the user asks a question or requests information:*

{
    ""type"": ""information"",
    ""params"": ""Your explanation or answer to the user's question.""
}

---

**Important Guidelines:**

- **Respond exclusively in JSON format** as specified above.
- **Do not include any free text** outside the JSON structures.
- Ensure all numerical values are accurate and units are consistent.
- If unsure how to proceed, use the **clarification** type to ask for more details.
- Keep your responses clear, concise, and relevant to DSC measurements.";
}
