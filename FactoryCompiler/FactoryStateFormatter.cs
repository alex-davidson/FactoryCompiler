using System.IO;
using System.Linq;
using FactoryCompiler.Model;
using FactoryCompiler.Model.Evaluate;
using FactoryCompiler.Model.State;

namespace FactoryCompiler;

internal class FactoryStateFormatter
{
    private readonly TextWriter writer;

    public FactoryStateFormatter(TextWriter writer)
    {
        this.writer = writer;
    }

    public void Format(FactoryStateSummary summary)
    {
        foreach (var region in summary.Regions.OrderBy(x => x.Key.RegionName.Name))
        {
            WriteLine($"Region: {region.Key.RegionName}");
            indent++;
            Summarise(region.Value);
            indent--;

            WriteLine("");
        }

        foreach (var network in summary.Networks.OrderBy(x => x.Key.Name))
        {
            WriteLine($"Network: {network.Key}");
            indent++;
            Summarise(network.Value);
            indent--;

            WriteLine("");
        }
    }

    private void Summarise(IItemVolumesState state)
    {
        var excess = state.Where(x => x.Volume > 0).ToArray();
        if (excess.Any())
        {
            WriteLine("Excess:");
            indent++;
            foreach (var stack in excess) WriteStack(stack);
            indent--;
        }
        var shortfall = state.Where(x => x.Volume < 0).ToArray();
        if (shortfall.Any())
        {
            WriteLine("Shortfall:");
            indent++;
            foreach (var stack in shortfall) WriteStack(stack);
            indent--;
        }

        if (!excess.Any() && !shortfall.Any())
        {
            WriteLine("(balanced)");
        }
    }

    private void WriteStack(ItemVolume stack)
    {
        WriteLine($"{stack.Item.Identifier,24}: {(decimal)stack.Volume,10:0.##}");
    }

    private int indent = 0;
    private void WriteLine(string line)
    {
        writer.Write(new string(' ', indent * 2));
        writer.WriteLine(line);
    }
}
