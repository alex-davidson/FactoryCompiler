using System.Collections;
using System.Collections.Generic;

namespace FactoryCompiler.Jobs.Visualise;

public class DescriptionLines : IEnumerable<DescriptionLine>
{
    private readonly List<DescriptionLine> lines = new List<DescriptionLine>();

    public void Add(string label, string? text, string? tooltip = null)
    {
        if (string.IsNullOrWhiteSpace(text)) return;
        lines.Add(new DescriptionLine(label, text, tooltip));
    }

    public IEnumerator<DescriptionLine> GetEnumerator() => lines.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public readonly struct DescriptionLine
{
    public DescriptionLine(string label, string? text, string? tooltip = null)
    {
        Label = label;
        Text = text;
        Tooltip = tooltip;
    }

    public string Label { get; }
    public string? Text { get; }
    public string? Tooltip { get; }
}
