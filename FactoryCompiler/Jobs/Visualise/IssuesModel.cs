using System.Linq;
using FactoryCompiler.Model.Diagnostics;

namespace FactoryCompiler.Jobs.Visualise;

public class IssuesModel
{
    public Diagnostic[] Diagnostics { get; }
    public string CountMarker { get; }

    public IssuesModel(Diagnostic[] diagnostics)
    {
        Diagnostics = diagnostics;
        CountMarker = $" ({diagnostics.Count(x => x.Severity >= Severity.Warning)})";
    }
}
