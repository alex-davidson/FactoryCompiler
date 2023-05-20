namespace FactoryCompiler.Model.State;

public enum TransportLinkDirection
{
    Invalid = 0,
    FromRegion = 1,
    ToRegion = 2,
}

public record TransportLink(Transport Definition, TransportLinkDirection Direction, Region Region, Identifier NetworkName, Item Item);
