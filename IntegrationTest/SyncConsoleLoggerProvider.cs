using Microsoft.Extensions.Logging;

namespace IntegrationTest;

internal sealed class SyncConsoleLoggerProvider : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName) => new SyncConsoleLogger(categoryName);
    public void Dispose() { }

    private sealed class SyncConsoleLogger(string categoryName) : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
        public bool IsEnabled(LogLevel logLevel) => logLevel >= LogLevel.Information;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;
            Console.WriteLine($"{DateTime.UtcNow:HH:mm:ss.fff} [{logLevel}] {categoryName}");
            Console.WriteLine($"      {formatter(state, exception)}");
            if (exception is not null) Console.WriteLine(exception);
        }
    }
}
