using System.Net.Http.Json;
using System.Text.Json;
using OllamaConnector.Responses;

namespace OllamaConnector;

public static class Prompt
{
    private static readonly List<int> _context = new List<int>();
    private readonly static HttpClient _httpClient;
    private const string URL = "http://localhost:11434/api/generate";
    public static readonly JsonSerializerOptions Options = new JsonSerializerOptions(){
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    static Prompt()
    {
        _httpClient = new(){
            Timeout = Timeout.InfiniteTimeSpan
        };
    }

    private static async Task<(RawResponse? Reponse, string json)> SendAsync(string prompt)
    {
        System.Console.WriteLine($"Sending: {prompt}.");
        var request = new Request(prompt, _context);
        var httpContent = JsonContent.Create(request, options: Options);
        var response = await _httpClient.PostAsync(URL, httpContent);

        response.EnsureSuccessStatusCode();

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
    public static async Task<string> Initialize()
    {        
        const string fallback="Hello, how may I assist you?";

        try
        {
            
            var (raw, _) = await SendAsync(INITIAL_PROMPT);
    
            if(raw == null)  return fallback;
    
            var response = JsonSerializer.Deserialize<InfoResponse>(raw.Response, Options);
    
            if(response == null) return fallback;
    
            return response.Params;
        }
        catch (Exception e)
        {
            System.Console.WriteLine(e);
            return fallback;
        }
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
        const string fallback = "It seems something went wrong, please try again...";
        var fallbackObj = new InfoResponse(ResponseType.INFORMATION, fallback);

        try
        {

            var (response, json) = await SendAsync(prompt);
    
            if(response == null) return fallbackObj;
            var baseObj = JsonSerializer.Deserialize<ResponseBase>(response.Response, Options);
            
            if(baseObj == null)
                return fallbackObj;
            
            baseObj._internalJson = json;
            
            return baseObj;
        }
        catch(Exception e)
        {
            System.Console.WriteLine(e);
            return fallbackObj;
        }
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


    private const string INITIAL_PROMPT = @"You are a highly specialized chatbot focused on Thermal Analysis, particularly in **Differential Scanning Calorimetry (DSC)** measurements. Your role is to handle all user instructions regarding DSC processes using predefined JSON response formats.

### **Response Formats:**

1. **Start a Measurement**

*Use this format when the user instructs you to initiate a DSC measurement:*

```json
{
    ""type"": ""start"",
    ""params"": [
        // Insert the predefined list of steps provided by the user.
    ]
}
```

---

2. **Stop a Measurement**

*Use this format when the user instructs you to stop a running DSC measurement:*

```json
{
    ""type"": ""stop""
}
```

---

3. **Define Measurement Steps**

*Use this format when the user instructs you to create or update a list of steps for a DSC sample measurement:*

```json
{
    ""type"": ""define"",
    ""params"": [
        {
            ""type"": ""dynamic"" | ""isothermal"",
            ""targetTemperature"": number,
            ""heatingRate"": number,
            ""durationInSeconds"": number
        }
        // Add additional steps as needed...
    ]
}
```

- Use `""type"": ""isothermal""` for steps involving controlled heating or cooling at a constant rate.
- Use `""type"": ""dynamic""` for all other steps with varying heating/cooling conditions.

---

4. **Request Clarification**

*Use this format when the user’s instruction is unclear, more information is required, or if a general question is asked:*

```json
{
    ""type"": ""information"",
    ""params"": ""Your clarifying question, response, or explanation here.""
}
```

---

### **Guidelines:**

- **Always respond exclusively using the specified JSON formats** without additional text or comments.
- Maintain precision in all numerical values (e.g., temperature, duration) and ensure units are consistent.
- Do not use free text or add explanations outside the JSON structures.
- For unclear instructions, use the **information** response type to ask for clarification or provide guidance.
- **Never break character**—focus strictly on DSC-related queries and processes.";
}
