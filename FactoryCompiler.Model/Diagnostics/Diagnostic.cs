using System;

namespace FactoryCompiler.Model.Diagnostics;

public record struct Diagnostic(Severity Severity, object? Location, string Message, Exception? Exception)
{
    public static Diagnostic Error(string message, Exception? exception = null, object? location = null) =>
        new Diagnostic(Severity.Error, location, message, exception);
    public static Diagnostic Warning(string message, Exception? exception = null, object? location = null) =>
        new Diagnostic(Severity.Warning, location, message, exception);
    public static Diagnostic Info(string message, object? location = null) =>
        new Diagnostic(Severity.Info, location, message, null);

    public override string ToString() => $"{Severity}: {Message}";
}
