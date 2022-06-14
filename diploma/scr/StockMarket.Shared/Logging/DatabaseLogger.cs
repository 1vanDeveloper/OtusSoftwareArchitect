using System;
using System.Text.Json;
using System.Threading.Tasks;
using StockMarket.Shared.Models;
using Microsoft.Extensions.Logging;

namespace StockMarket.Shared.Logging;

public class DatabaseLogger : ILogger
{
    private readonly LogWriter _logWriter;

    public DatabaseLogger(LogWriter logWriter)
    {
        _logWriter = logWriter;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel) =>
        logLevel is LogLevel.Error or LogLevel.Critical;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception exception,
        Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        Task.Run(async () => await _logWriter.Write(JsonSerializer.Deserialize<LogMessage>(formatter(state, exception))));
    }
}
