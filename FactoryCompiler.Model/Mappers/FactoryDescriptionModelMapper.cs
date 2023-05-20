using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace FactoryCompiler.Model.Mappers;

internal struct FactoryDescriptionModelMapper
{
    public Dto.FactoryDescription MapToDto(FactoryDescription factoryDescription) =>
        new Dto.FactoryDescription
        {
            Regions = factoryDescription.Regions.Select(default(RegionMapper).MapToDto).ToArray(),
        };

    public FactoryDescription MapFromDto(Dto.FactoryDescription dto) =>
        new FactoryDescription(
            MapListFrom(dto.Regions?.Select(default(RegionMapper).MapFromDto)));

    private static ImmutableArray<T> MapListFrom<T>(IEnumerable<T>? list)
    {
        if (list == null) return ImmutableArray<T>.Empty;
        return list.ToImmutableArray();
    }

    private static Identifier? MapMaybeNullIdentifier(string? name)
    {
        if (name == null) return null;
        return (Identifier)name;
    }

    private struct RegionMapper
    {
        public Dto.FactoryDescription.Region MapToDto(Region region) =>
            new Dto.FactoryDescription.Region
            {
                RegionName = region.RegionName.Name,
                Groups = region.Groups.Select(default(GroupMapper).MapToDto).ToArray(),
                Inbound = region.Inbound.Select(default(TransportMapper).MapToDto).ToArray(),
                Outbound = region.Outbound.Select(default(TransportMapper).MapToDto).ToArray(),
            };

        public Region MapFromDto(Dto.FactoryDescription.Region dto) =>
            new Region(
                RegionName: dto.RegionName!,
                Groups: MapListFrom(dto.Groups?.Select(default(GroupMapper).MapFromDto)),
                Inbound: MapListFrom(dto.Inbound?.Select(default(TransportMapper).MapFromDto)),
                Outbound: MapListFrom(dto.Outbound?.Select(default(TransportMapper).MapFromDto)));
    }

    private struct GroupMapper
    {
        public Dto.FactoryDescription.Group MapToDto(Group group) =>
            new Dto.FactoryDescription.Group
            {
                GroupName = group.GroupName?.Name,
                FactoryName = group.Production?.FactoryName?.Name,
                Recipe = group.Production?.RecipeName.Name,
                Count = group.Production == null ? null : default(NumberMapper).MapToDecimalDto(group.Production.Count),
                Groups = group.Groups.Any() ? group.Groups.Select(MapToDto).ToArray() : null,
                Repeat = group.Repeat == 1 ? null : group.Repeat.ToString(),
            };

        public Group MapFromDto(Dto.FactoryDescription.Group dto) =>
            dto.Groups == null
                ? new Group(MapMaybeNullIdentifier(dto.GroupName), default(ProductionMapper).MapFromDto(dto), default(NumberMapper).MapIntegerFromDto(dto.Repeat, 1))
                : new Group(MapMaybeNullIdentifier(dto.GroupName), MapListFrom(dto.Groups?.Select(MapFromDto)), default(NumberMapper).MapIntegerFromDto(dto.Repeat, 1));
    }

    private struct ProductionMapper
    {
        public Production MapFromDto(Dto.FactoryDescription.Production dto) =>
            new Production(MapMaybeNullIdentifier(dto.FactoryName), dto.Recipe!, default(NumberMapper).MapFromDto(dto.Count, 1));
    }

    private struct TransportMapper
    {
        public Dto.FactoryDescription.Transport MapToDto(Transport transport) =>
            new Dto.FactoryDescription.Transport
            {
                Item = transport.ItemName.Name,
                Network = transport.Network.Name,
            };

        public Transport MapFromDto(Dto.FactoryDescription.Transport dto) =>
            new Transport(dto.Item!, dto.Network!);
    }
}
