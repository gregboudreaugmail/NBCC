﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NBCC.Logging.DataAccess;
using NBCC.Logging.Models;

public sealed class LoggerAsync : ILoggerAsync
{
    IInteractionLog InteractionLog { get; }
    IConfiguration Configuration { get; }
    IExceptionLog ExceptionLog { get; }

    public LoggerAsync(IInteractionLog interactionLog, IConfiguration configuration, IExceptionLog exceptionLog)
    {
        InteractionLog = interactionLog;
        Configuration = configuration;
        ExceptionLog = exceptionLog;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        if (Enum.TryParse<LogLevel>(Configuration["Logging:LogLevel:Default"], out var level))
            return logLevel >= level;
        return false;
    }

    public async Task Log(LogLevel logLevel, Interaction interaction)
    {
        if (!IsEnabled(logLevel)) return;
        try
        {
            await InteractionLog.Log(interaction);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public async Task Log(Interaction interaction) => await Log(LogLevel.Information, interaction);

    public async Task<int?> Log(Exception exception) => await Log(LogLevel.Error, exception);

    public async Task<int?> Log(LogLevel logLevel, Exception exception)
    {
        try
        {
            if (!IsEnabled(logLevel)) return null;
            return await ExceptionLog.Log(exception);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}