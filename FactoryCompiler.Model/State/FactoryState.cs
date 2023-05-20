using FactoryCompiler.Model.Diagnostics;
using System.Collections.Immutable;

namespace FactoryCompiler.Model.State
{
    public class FactoryState
    {
        public ImmutableArray<RegionState> Regions { get; }
        public ImmutableArray<TransportLink> TransportLinks { get; }
        public ImmutableArray<Diagnostic> Diagnostics { get; }
        public ItemVolumesState ItemVolumes { get; set; }

        public FactoryState(ImmutableArray<RegionState> regions, ImmutableArray<TransportLink> transportLinks, ImmutableArray<Diagnostic> diagnostics)
        {
            Regions = regions;
            TransportLinks = transportLinks;
            Diagnostics = diagnostics;
            ItemVolumes = new ItemVolumesState();
        }
    }
}
