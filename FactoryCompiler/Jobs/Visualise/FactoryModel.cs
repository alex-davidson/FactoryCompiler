using System.Linq;
using FactoryCompiler.Model.Diagnostics;
using FactoryCompiler.Model.Evaluate;
using FactoryCompiler.Model.State;

namespace FactoryCompiler.Jobs.Visualise;

public class FactoryModel
{
    public FactoryModel(FactoryState state, FactoryStateSummary? summary, bool ignoreErrors)
    {
        State = state;
        Summary = summary;
        IgnoreErrors = ignoreErrors;
    }

    public bool IgnoreErrors { get; }

    public FactoryState State { get; }
    public bool IsFailed => !IgnoreErrors && State?.Diagnostics.Any(x => x.Severity >= Severity.Error) == true;

    public FactoryStateSummary? Summary { get; }
}
