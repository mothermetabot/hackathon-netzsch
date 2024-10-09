namespace OllamaConnector.Responses;

internal record Request
{
    public string Prompt { get; }

    public Request(string prompt, IEnumerable<int>? context = null)
    {
        Prompt = prompt;
        Context = context ?? Enumerable.Empty<int>();
    }

    public bool Stream => false;

    public string Model => "llama3.2:3b";

    public IEnumerable<int> Context { get; }
}