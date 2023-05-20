using FactoryCompiler;
using FactoryCompiler.Jobs;
using FactoryCompiler.Model;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FactoryCompiler.Jobs;

internal class CreateExampleFactoryJob
{
    public Stream Output { get; set; } = Stream.Null;

    public async Task<int> Run(CancellationToken token)
    {
        new FactoryDescriptionSerialiser().Serialise(Output, Examples.SimpleFactory, true);
        return 0;
    }
}
