using System.Windows.Media;

namespace FactoryCompiler.Jobs.Visualise;

public readonly struct Portion
{
    public Portion(float value, Color fill)
    {
        Value = value;
        Fill = fill;
    }

    public float Value { get; }
    public Color Fill { get; }
}
