internal record Payload{
    public string Content {get; }

    public Payload(string content)
    {
        Content = content;
    }
}