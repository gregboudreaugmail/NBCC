using System.Text.Json;

namespace NBCC.Logging.Models;

public sealed class Interaction
{
    public string AssemblyName { get; } = string.Empty;
    public string Command { get; } = string.Empty;
    public string? Parameters { get; }

    public Interaction() { }

    public Interaction(string assemblyName, string command, string? parameters)
    {
        AssemblyName = assemblyName;
        Command = command;
        Parameters = parameters;
    }

    public Interaction(string assemblyName, string command)
    {
        AssemblyName = assemblyName;
        Command = command;
        Parameters = null;
    }

    public static implicit operator string(Interaction interaction) => JsonSerializer.Serialize(interaction);
}