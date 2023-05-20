using System.Collections.Immutable;
using Rationals;

namespace FactoryCompiler.Model;

public static class Examples
{
    public static FactoryDescription SimpleFactory =>
        new FactoryDescription(
            ImmutableArray.Create(
                new Region(
                    "Smelting",
                    Groups: ImmutableArray.Create(
                        new Group("Iron",
                            new Production("Smelter", "Iron Ingot", 4)),
                        new Group("Copper",
                            new Production("Smelter", "Copper Ingot", 2))),
                    Inbound: ImmutableArray.Create(
                        new Transport("Iron Ore", "Mines"),
                        new Transport("Copper Ore", "Mines")),
                    Outbound: ImmutableArray.Create(
                        new Transport("Iron Ingot", "Ingots"),
                        new Transport("Copper Ingot", "Ingots"))),
                new Region(
                    "Factories",
                    Groups: ImmutableArray.Create(
                        new Group("Iron",
                            ImmutableArray.Create(
                                new Group(null,
                                    new Production("Constructor", "Iron Plate", 3)),
                                new Group(null,
                                    new Production("Constructor", "Iron Rod", 2)))),
                        new Group("Copper",
                            new Production("Constructor", "Wire", 2))),
                    Inbound: ImmutableArray.Create(
                        new Transport("Iron Ingot", "Ingots"),
                        new Transport("Copper Ingot", "Ingots")),
                    Outbound: ImmutableArray.Create(
                        new Transport("Iron Rod", "Ammo Factory")))));

    public static FactoryDescription PolymerLoopFactory =>
        SingleRegion(
            new Region("PolymerLoop",
                Groups: ImmutableArray.Create(
                    new Group(null,
                        new Production(null, "Heavy Oil Residue", 10)),
                    new Group(null,
                        new Production(null, "Residual Rubber", 5)),
                    new Group(null,
                        new Production(null, "Diluted Fuel", 8)),
                    new Group(null,
                        new Production(null, "Recycled Plastic", Rational.Approximate(14.44444, 0.0001))),
                    new Group(null,
                        new Production(null, "Recycled Rubber", Rational.Approximate(12.22222, 0.0001)))),
                Inbound: ImmutableArray<Transport>.Empty,
                Outbound: ImmutableArray<Transport>.Empty));

    public static FactoryDescription AluminumIngotFactory =>
        SingleRegion(
            new Region("AluminiumIngot",
                Groups: ImmutableArray.Create(
                    new Group(null,
                        new Production(null, "Heavy Oil Residue", 2)),
                    new Group(null,
                        new Production("Refinery", "Petroleum Coke", 2)),
                    new Group(null,
                        new Production(null, "Electrode Aluminum Scrap", 4)),
                    new Group(null,
                        new Production(null, "Pure Aluminum Ingot", 20)),
                    new Group(null,
                        new Production(null, "Sloppy Alumina", 3))),
                Inbound: ImmutableArray<Transport>.Empty,
                Outbound: ImmutableArray<Transport>.Empty));

    public static FactoryDescription RadioControlUnitFactory =>
        SingleRegion(
            new Region("RadioControlUnit",
                Groups: ImmutableArray.Create(
                    new Group("Copper",
                        ImmutableArray.Create(
                            new Group(null,
                                new Production(null, "Copper Ingot", 2)),
                            new Group(null,
                                new Production(null, "Copper Sheet", 3)))),
                    new Group("Caterium",
                        ImmutableArray.Create(
                            new Group(null,
                                new Production(null, "Caterium Ingot", Rational.Approximate(5.6, 0.01))),
                            new Group(null,
                                new Production(null, "Quickwire", 7)))),
                    new Group("Quartz",
                        ImmutableArray.Create(
                            new Group(null,
                                new Production(null, "Quartz Crystal", Rational.Approximate(2.67, 0.01))),
                            new Group(null,
                                new Production(null, "Silica", 12)))),
                    new Group("Aluminum",
                        ImmutableArray.Create(
                            new Group(null,
                                new Production(null, "Alumina Solution", Rational.Approximate(4.5))),
                            new Group(null,
                                new Production(null, "Aluminum Scrap", Rational.Approximate(2.25))),
                            new Group(null,
                                new Production(null, "Aluminum Ingot", 9)),
                            new Group(null,
                                new Production(null, "Aluminum Casing", 6)))),
                    new Group("Polymers",
                        ImmutableArray.Create(
                            new Group(null,
                                new Production(null, "Rubber", Rational.Approximate(11.10, 0.01))),
                            new Group(null,
                                new Production(null, "Plastic", Rational.Approximate(4.29, 0.01))))),
                    new Group(null,
                        new Production(null, "AI Limiter", Rational.Approximate(1.2))),
                    new Group(null,
                        new Production(null, "Insulated Crystal Oscillator", Rational.Approximate(3.20, 0.01))),
                    new Group(null,
                        new Production(null, "Caterium Circuit Board", Rational.Approximate(6.86, 0.01))),
                    new Group(null,
                        new Production(null, "Radio Control System", 4))),
                Inbound: ImmutableArray<Transport>.Empty,
                Outbound: ImmutableArray<Transport>.Empty));

    private static FactoryDescription SingleRegion(Region region) =>
        new FactoryDescription(ImmutableArray.Create(region));
}
