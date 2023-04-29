using System.Text.Json;

namespace NBCC.Logging;

public sealed class Interaction
{
    public int InteractionId { get; }
    public string AssemblyName { get; } = string.Empty;
    public string Command { get; } = string.Empty;
    public string? Parameters { get; }

    public Interaction() { }

    public Interaction(int interactionId, string assemblyName, string command, string? parameters)
    {
        InteractionId = interactionId;
        AssemblyName = assemblyName;
        Command = command;
        Parameters = parameters;
    }

    public Interaction(int interactionId, string assemblyName, string command)
    {
        InteractionId = interactionId;
        AssemblyName = assemblyName;
        Command = command;
        Parameters = null;
    }

    public static implicit operator string(Interaction interaction) => JsonSerializer.Serialize(interaction);
}