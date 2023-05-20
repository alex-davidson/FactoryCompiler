using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FactoryCompiler.Model.Diagnostics;

public class DiagnosticFormatter
{
    public void Format(TextWriter writer, Diagnostic diagnostic)
    {
        var severity = "[" + diagnostic.Severity + "]";
        writer.WriteLine($"{severity,-9} {diagnostic.Message}");
    }

    public void FormatSummary(TextWriter writer, ICollection<Diagnostic> diagnostics)
    {
        var errorCount = diagnostics.Count(x => x.Severity >= Severity.Error);
        var warningCount = diagnostics.Count(x => x.Severity == Severity.Warning);
        if (errorCount == 0 && warningCount == 0) return;
        writer.WriteLine($"{warningCount} warnings and {errorCount} errors.");
    }
}
