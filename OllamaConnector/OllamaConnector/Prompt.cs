using System.Net.Http.Json;
using System.Text.Json;
using OllamaConnector.Responses;

namespace OllamaConnector;

public static class Prompt
{
    private static List<int> _context = new List<int>();
    private readonly static HttpClient _httpClient;
    private const string URL = "http://localhost:11434/api/generate";
    public static readonly JsonSerializerOptions Options = new JsonSerializerOptions(){
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    static Prompt()
    {
        _httpClient = new(){
            Timeout = TimeSpan.FromMinutes(10)
        };

_context = [128006,9125,128007,271,38766,1303,33025,2696,25,6790,220,2366,18,271,128009,128006,882,128007,271,2675,527,264,7701,28175,6369,6465,10968,389,66726,18825,11,8104,304,3146,35,69757,2522,6073,3400,269,318,15501,320,35,3624,33395,22323,13,4718,3560,374,311,3790,682,1217,11470,9002,423,3624,11618,1701,64162,4823,2077,20447,18304,14711,3146,2647,80448,68063,881,16,13,3146,3563,264,55340,334,881,9,10464,420,3645,994,279,1217,21745,82,499,311,39201,264,423,3624,19179,54486,881,74694,2285,319,1700,262,330,1337,794,330,2527,4828,262,330,3603,794,24558,286,443,17539,279,64162,1160,315,7504,3984,555,279,1217,3304,262,49164,1809,74694,881,4521,881,17,13,3146,10903,264,55340,334,881,9,10464,420,3645,994,279,1217,21745,82,499,311,3009,264,4401,423,3624,19179,54486,881,74694,2285,319,1700,262,330,1337,794,330,9684,5139,1809,74694,881,4521,881,18,13,3146,36438,55340,40961,334,881,9,10464,420,3645,994,279,1217,21745,82,499,311,1893,477,2713,264,1160,315,7504,369,264,423,3624,6205,19179,54486,881,74694,2285,319,1700,262,330,1337,794,330,1312,4828,262,330,3603,794,24558,286,987,310,330,1337,794,330,22269,1,765,330,285,91096,4828,310,330,5775,41790,794,1396,1909,310,330,383,1113,11825,794,1396,1909,310,330,17456,97336,794,1396,319,286,1720,286,443,2758,5217,7504,439,4460,35929,262,49164,1809,74694,881,12,5560,54405,1337,794,330,285,91096,41017,369,7504,16239,14400,24494,477,28015,520,264,6926,4478,3304,12,5560,54405,1337,794,330,22269,41017,369,682,1023,7504,449,29865,24494,14,43296,287,4787,18304,4521,881,19,13,3146,1939,31181,2461,334,881,9,10464,420,3645,994,279,1217,753,7754,374,25420,11,810,2038,374,2631,11,477,422,264,4689,3488,374,4691,54486,881,74694,2285,319,1700,262,330,1337,794,330,26125,4828,262,330,3603,794,330,7927,20064,7922,3488,11,2077,11,477,16540,1618,72734,1809,74694,881,4521,881,14711,3146,17100,11243,68063,881,12,3146,38195,6013,24121,1701,279,5300,4823,20447,334,2085,5217,1495,477,6170,3304,12,87477,16437,304,682,35876,2819,320,68,1326,2637,9499,11,8250,8,323,6106,8316,527,13263,3304,12,3234,539,1005,1949,1495,477,923,41941,4994,279,4823,14726,3304,12,1789,25420,11470,11,1005,279,3146,26125,334,2077,955,311,2610,369,64784,477,3493,19351,3304,12,3146,27247,1464,3752,334,2345,17870,26549,389,423,3624,14228,20126,323,11618,13,128009,128006,78191,128007,271,40,2846,5644,311,7945,449,423,3624,22323,13,3639,1053,499,1093,311,656,1980,2675,649,1212,555,1701,832,315,279,64162,4823,20447,1473,16,13,5256,264,55340,25,1595,5018,1337,794,330,2527,498,330,3603,794,16702,14316,4077,17,13,14549,264,55340,25,1595,5018,1337,794,330,9684,9388,4077,18,13,19127,55340,40961,25,1595,5018,1337,794,330,1312,498,330,3603,794,16702,14316,4077,19,13,6274,31181,2461,25,1595,5018,1337,794,330,26125,498,330,3603,794,39813,5658,271,5618,3493,279,12435,4823,3645,369,701,7754,11,323,358,3358,6013,28178,13,128006,9125,128007,271,38766,1303,33025,2696,25,6790,220,2366,18,271,128009,128006,882,128007,271,2675,527,264,7701,28175,6369,6465,10968,389,66726,18825,11,8104,304,3146,35,69757,2522,6073,3400,269,318,15501,320,35,3624,33395,22323,13,4718,3560,374,311,3790,682,1217,11470,9002,423,3624,11618,1701,64162,4823,2077,20447,18304,14711,3146,2647,80448,68063,881,16,13,3146,3563,264,55340,334,881,9,10464,420,3645,994,279,1217,21745,82,499,311,39201,264,423,3624,19179,54486,8731,1700,262,330,1337,794,330,2527,4828,262,330,3603,794,24558,286,443,17539,279,64162,1160,315,7504,3984,555,279,1217,3304,262,49164,17702,4521,881,17,13,3146,10903,264,55340,334,881,9,10464,420,3645,994,279,1217,21745,82,499,311,3009,264,4401,423,3624,19179,54486,8731,1700,262,330,1337,794,330,9684,5139,17702,4521,881,18,13,3146,36438,55340,40961,334,881,9,10464,420,3645,994,279,1217,21745,82,499,311,1893,477,2713,264,1160,315,7504,369,264,423,3624,6205,19179,54486,8731,1700,262,330,1337,794,330,1312,4828,262,330,3603,794,24558,286,987,310,330,1337,794,330,22269,1,765,330,285,91096,4828,310,330,5775,41790,794,1396,1909,310,330,383,1113,11825,794,1396,1909,310,330,17456,97336,794,1396,319,286,1720,286,443,2758,5217,7504,439,4460,35929,262,49164,17702,12,5560,54405,1337,794,330,285,91096,41017,369,7504,16239,14400,24494,477,28015,520,264,6926,4478,3304,12,5560,54405,1337,794,330,22269,41017,369,682,1023,7504,449,29865,24494,14,43296,287,4787,18304,4521,881,19,13,3146,1939,31181,2461,334,881,9,10464,420,3645,994,279,1217,753,7754,374,25420,11,810,2038,374,2631,11,477,422,264,4689,3488,374,4691,54486,8731,1700,262,330,1337,794,330,26125,4828,262,330,3603,794,330,7927,20064,7922,3488,11,2077,11,477,16540,1618,72734,17702,4521,881,14711,3146,17100,11243,68063,881,12,3146,38195,6013,24121,1701,279,5300,4823,20447,334,2085,5217,1495,477,6170,3304,12,87477,16437,304,682,35876,2819,320,68,1326,2637,9499,11,8250,8,323,6106,8316,527,13263,3304,12,3234,539,1005,1949,1495,477,923,41941,4994,279,4823,14726,3304,12,1789,25420,11470,11,1005,279,3146,26125,334,2077,955,311,2610,369,64784,477,3493,19351,3304,12,3146,27247,1464,3752,334,2345,17870,26549,389,423,3624,14228,20126,323,11618,13,128009,128006,78191,128007,271,3923,649,358,1520,499,449,9002,423,3624,22323,30,5321,6013,1701,832,315,279,64162,4823,20447,382,2675,649,1212,555,38938,1473,16,13,28757,264,502,19179,25,1595,5018,1337,794,330,2527,498,2564,32357,17,13,800,7153,459,14529,19179,25,1595,5018,1337,794,330,9684,9388,4077,18,13,3979,5859,477,21686,19179,7504,25,1595,5018,1337,794,330,1312,498,2564,32357,19,13,6274,287,64784,389,701,7754,25,1595,5018,1337,794,330,26125,498,79310,5658,271,5618,3493,279,12435,4823,3645,369,701,7754,11,323,358,3358,6013,28178,13];
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

return fallback;
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
        System.Console.WriteLine(response.Context);
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


{
    ""type"": ""start"",
    ""params"": [
        // Insert the predefined list of steps provided by the user.
    ]
}


---

2. **Stop a Measurement**

*Use this format when the user instructs you to stop a running DSC measurement:*


{
    ""type"": ""stop""
}


---

3. **Define Measurement Steps**

*Use this format when the user instructs you to create or update a list of steps for a DSC sample measurement:*


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


- Use `""type"": ""isothermal""` for steps involving controlled heating or cooling at a constant rate.
- Use `""type"": ""dynamic""` for all other steps with varying heating/cooling conditions.

---

4. **Request Clarification**

*Use this format when the user’s instruction is unclear, more information is required, or if a general question is asked:*


{
    ""type"": ""information"",
    ""params"": ""Your clarifying question, response, or explanation here.""
}


---

### **Guidelines:**

- **Always respond exclusively using the specified JSON formats** without additional text or comments.
- Maintain precision in all numerical values (e.g., temperature, duration) and ensure units are consistent.
- Do not use free text or add explanations outside the JSON structures.
- For unclear instructions, use the **information** response type to ask for clarification or provide guidance.
- **Never break character**—focus strictly on DSC-related queries and processes.";
}
