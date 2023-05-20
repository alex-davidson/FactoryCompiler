using System.Collections.Immutable;

namespace FactoryCompiler.Model.DefaultGameDataFiles;

internal class AssemblerData : DefaultGameDataBase, IFactoryData
{
    public static Factory Assembler { get; } = new Factory("Assembler");

    public Factory Factory => Assembler;

    public Recipe[] Recipes { get; } =
    {
        new Recipe("AI Limiter", new Duration(12),
            Assembler,
            Item.List(
                new Item("Copper Sheet").Volume(5),
                new Item("Quickwire").Volume(20)),
            Item.List(new Item("AI Limiter").Volume(1))),
        new Recipe("Alclad Aluminum Sheet", new Duration(6),
            Assembler,
            Item.List(
                new Item("Aluminum Ingot").Volume(3),
                new Item("Copper Ingot").Volume(1)),
            Item.List(new Item("Alclad Aluminum Sheet").Volume(3))),
        new Recipe("Black Powder", new Duration(4),
            Assembler,
            Item.List(
                new Item("Coal").Volume(1),
                new Item("Sulfur").Volume(1)),
            Item.List(new Item("Black Powder").Volume(2))),
        new Recipe("Circuit Board", new Duration(8),
            Assembler,
            Item.List(
                new Item("Copper Sheet").Volume(2),
                new Item("Plastic").Volume(4)),
            Item.List(new Item("Circuit Board").Volume(1))),
        new Recipe("Cluster Nobelisk", new Duration(24),
            Assembler,
            Item.List(
                new Item("Nobelisk").Volume(3),
                new Item("Smokeless Powder").Volume(4)),
            Item.List(new Item("Cluster Nobelisk").Volume(1))),
        new Recipe("Compacted Coal", new Duration(12),
            Assembler,
            Item.List(
                new Item("Coal").Volume(5),
                new Item("Sulfur").Volume(5)),
            Item.List(new Item("Compacted Coal").Volume(5))),

        new Recipe("Electromagnetic Control Rod", new Duration(30),
            Assembler,
            Item.List(
                new Item("Stator").Volume(3),
                new Item("AI Limiter").Volume(2)),
            Item.List(new Item("Electromagnetic Control Rod").Volume(2))),
        new Recipe("Encased Industrial Beam", new Duration(10),
            Assembler,
            Item.List(
                new Item("Steel Beam").Volume(4),
                new Item("Concrete").Volume(5)),
            Item.List(new Item("Encased Industrial Beam").Volume(1))),
        new Recipe("Encased Plutonium Cell", new Duration(12),
            Assembler,
            Item.List(
                new Item("Plutonium Pellet").Volume(2),
                new Item("Concrete").Volume(4)),
            Item.List(new Item("Encased Plutonium Cell").Volume(1))),

        new Recipe("Fabric", new Duration(4),
            Assembler,
            Item.List(
                new Item("Mycelia").Volume(1),
                new Item("Biomass").Volume(5)),
            Item.List(new Item("Fabric").Volume(1))),

        new Recipe("Gas Nobelisk", new Duration(12),
            Assembler,
            Item.List(
                new Item("Nobelisk").Volume(1),
                new Item("Biomass").Volume(10)),
            Item.List(new Item("Gas Nobelisk").Volume(1))),

        new Recipe("Heat Sink", new Duration(8),
            Assembler,
            Item.List(
                new Item("Alclad Aluminum Sheet").Volume(5),
                new Item("Copper Sheet").Volume(3)),
            Item.List(new Item("Heat Sink").Volume(1))),
        new Recipe("Homing Rifle Ammo", new Duration(24),
            Assembler,
            Item.List(
                new Item("Rifle Ammo").Volume(20),
                new Item("High-Speed Connector").Volume(1)),
            Item.List(new Item("Homing Rifle Ammo").Volume(10))),

        new Recipe("Modular Frame", new Duration(60),
            Assembler,
            Item.List(
                new Item("Reinforced Iron Plate").Volume(3),
                new Item("Iron Rod").Volume(12)),
            Item.List(new Item("Modular Frame").Volume(2))),
        new Recipe("Motor", new Duration(12),
            Assembler,
            Item.List(
                new Item("Rotor").Volume(2),
                new Item("Stator").Volume(2)),
            Item.List(new Item("Motor").Volume(1))),
        new Recipe("Nobelisk", new Duration(6),
            Assembler,
            Item.List(
                new Item("Steel Pipe").Volume(2),
                new Item("Black Powder").Volume(2)),
            Item.List(new Item("Nobelisk").Volume(1))),

        new Recipe("Pressure Conversion Cube", new Duration(60),
            Assembler,
            Item.List(
                new Item("Fused Modular Frame").Volume(1),
                new Item("Radio Control Unit").Volume(2)),
            Item.List(new Item("Pressure Conversion Cube").Volume(1))),
        new Recipe("Pulse Nobelisk", new Duration(60),
            Assembler,
            Item.List(
                new Item("Nobelisk").Volume(5),
                new Item("Crystal Oscillator").Volume(1)),
            Item.List(new Item("Pulse Nobelisk").Volume(5))),

        new Recipe("Reinforced Iron Plate", new Duration(12),
            Assembler,
            Item.List(
                new Item("Iron Plate").Volume(6),
                new Item("Screw").Volume(12)),
            Item.List(new Item("Reinforced Iron Plate").Volume(1))),
        new Recipe("Rifle Ammo", new Duration(12),
            Assembler,
            Item.List(
                new Item("Copper Sheet").Volume(3),
                new Item("Smokeless Powder").Volume(2)),
            Item.List(new Item("Rifle Ammo").Volume(15))),
        new Recipe("Rotor", new Duration(15),
            Assembler,
            Item.List(
                new Item("Iron Rod").Volume(5),
                new Item("Screw").Volume(25)),
            Item.List(new Item("Rotor").Volume(1))),

        new Recipe("Shatter Rebar", new Duration(12),
            Assembler,
            Item.List(
                new Item("Iron Rebar").Volume(2),
                new Item("Quartz Crystal").Volume(3)),
            Item.List(new Item("Shatter Rebar").Volume(1))),
        new Recipe("Stator", new Duration(12),
            Assembler,
            Item.List(
                new Item("Steel Pipe").Volume(3),
                new Item("Wire").Volume(8)),
            Item.List(new Item("Stator").Volume(1))),
        new Recipe("Stun Rebar", new Duration(6),
            Assembler,
            Item.List(
                new Item("Iron Rebar").Volume(1),
                new Item("Quickwire").Volume(5)),
            Item.List(new Item("Stun Rebar").Volume(1))),
    };

    public Recipe[] AlternateRecipes { get; } =
    {
        new Recipe("Adhered Iron Plate", new Duration(16),
            Assembler,
            Item.List(
                new Item("Iron Plate").Volume(3),
                new Item("Rubber").Volume(1)),
            Item.List(new Item("Reinforced Iron Plate").Volume(1)),
            RecipeFlags.Alternate),
        new Recipe("Alclad Casing", new Duration(8),
            Assembler,
            Item.List(
                new Item("Aluminum Ingot").Volume(20),
                new Item("Copper Ingot").Volume(10)),
            Item.List(new Item("Aluminum Casing").Volume(15)),
            RecipeFlags.Alternate),
        new Recipe("Bolted Frame", new Duration(24),
            Assembler,
            Item.List(
                new Item("Reinforced Iron Plate").Volume(3),
                new Item("Screw").Volume(56)),
            Item.List(new Item("Modular Frame").Volume(2)),
            RecipeFlags.Alternate),
        new Recipe("Bolted Iron Plate", new Duration(12),
            Assembler,
            Item.List(
                new Item("Iron Plate").Volume(18),
                new Item("Screw").Volume(50)),
            Item.List(new Item("Reinforced Iron Plate").Volume(3)),
            RecipeFlags.Alternate),
        new Recipe("Caterium Circuit Board", new Duration(48),
            Assembler,
            Item.List(
                new Item("Plastic").Volume(10),
                new Item("Quickwire").Volume(30)),
            Item.List(new Item("Circuit Board").Volume(7)),
            RecipeFlags.Alternate),
        new Recipe("Cheap Silica", new Duration(16),
            Assembler,
            Item.List(
                new Item("Raw Quartz").Volume(3),
                new Item("Limestone").Volume(5)),
            Item.List(new Item("Silica").Volume(7)),
            RecipeFlags.Alternate),
        new Recipe("Coated Iron Canister", new Duration(4),
            Assembler,
            Item.List(
                new Item("Iron Plate").Volume(2),
                new Item("Copper Sheet").Volume(1)),
            Item.List(new Item("Empty Canister").Volume(4)),
            RecipeFlags.Alternate),
        new Recipe("Coated Iron Plate", new Duration(4),
            Assembler,
            Item.List(
                new Item("Iron Ingot").Volume(10),
                new Item("Plastic").Volume(2)),
            Item.List(new Item("Iron Plate").Volume(15)),
            RecipeFlags.Alternate),
        new Recipe("Copper Rotor", new Duration(4),
            Assembler,
            Item.List(
                new Item("Copper Sheet").Volume(6),
                new Item("Screw").Volume(52)),
            Item.List(new Item("Rotor").Volume(3)),
            RecipeFlags.Alternate),
        new Recipe("Crystal Computer", new Duration(64),
            Assembler,
            Item.List(
                new Item("Circuit Board").Volume(8),
                new Item("Crystal Oscillator").Volume(3)),
            Item.List(new Item("Computer").Volume(3)),
            RecipeFlags.Alternate),

        new Recipe("Electric Motor", new Duration(16),
            Assembler,
            Item.List(
                new Item("Electromagnetic Control Rod").Volume(1),
                new Item("Rotor").Volume(2)),
            Item.List(new Item("Motor").Volume(2)),
            RecipeFlags.Alternate),
        new Recipe("Electrode Circuit Board", new Duration(12),
            Assembler,
            Item.List(
                new Item("Rubber").Volume(6),
                new Item("Petroleum Coke").Volume(9)),
            Item.List(new Item("Circuit Board").Volume(1)),
            RecipeFlags.Alternate),
        new Recipe("Electromagnetic Connection Rod", new Duration(30),
            Assembler,
            Item.List(
                new Item("Stator").Volume(2),
                new Item("High-Speed Connector").Volume(1)),
            Item.List(new Item("Electromagnetic Control Rod").Volume(1)),
            RecipeFlags.Alternate),
        new Recipe("Encased Industrial Pipe", new Duration(15),
            Assembler,
            Item.List(
                new Item("Steel Pipe").Volume(7),
                new Item("Concrete").Volume(5)),
            Item.List(new Item("Encased Industrial Beam").Volume(1)),
            RecipeFlags.Alternate),

        new Recipe("Fine Black Powder", new Duration(16),
            Assembler,
            Item.List(
                new Item("Sulfur").Volume(2),
                new Item("Compacted Coal").Volume(1)),
            Item.List(new Item("Black Powder").Volume(4)),
            RecipeFlags.Alternate),
        new Recipe("Fine Concrete", new Duration(24),
            Assembler,
            Item.List(
                new Item("Silica").Volume(3),
                new Item("Limestone").Volume(12)),
            Item.List(new Item("Concrete").Volume(10)),
            RecipeFlags.Alternate),
        new Recipe("Fused Quickwire", new Duration(8),
            Assembler,
            Item.List(
                new Item("Caterium Ingot").Volume(1),
                new Item("Copper Ingot").Volume(5)),
            Item.List(new Item("Quickwire").Volume(12)),
            RecipeFlags.Alternate),
        new Recipe("Fused Wire", new Duration(20),
            Assembler,
            Item.List(
                new Item("Copper Ingot").Volume(4),
                new Item("Caterium Ingot").Volume(1)),
            Item.List(new Item("Wire").Volume(30)),
            RecipeFlags.Alternate),

        new Recipe("Heat Exchanger", new Duration(6),
            Assembler,
            Item.List(
                new Item("Aluminum Casing").Volume(3),
                new Item("Rubber").Volume(3)),
            Item.List(new Item("Heat Sink").Volume(1)),
            RecipeFlags.Alternate),

        new Recipe("Insulated Cable", new Duration(12),
            Assembler,
            Item.List(
                new Item("Wire").Volume(9),
                new Item("Rubber").Volume(6)),
            Item.List(new Item("Cable").Volume(20)),
            RecipeFlags.Alternate),

        new Recipe("OC Supercomputer", new Duration(20),
            Assembler,
            Item.List(
                new Item("Radio Control Unit").Volume(3),
                new Item("Cooling System").Volume(3)),
            Item.List(new Item("Supercomputer").Volume(1)),
            RecipeFlags.Alternate),
        new Recipe("Plutonium Fuel Unit", new Duration(120),
            Assembler,
            Item.List(
                new Item("Encased Plutonium Cell").Volume(20),
                new Item("Pressure Conversion Cube").Volume(1)),
            Item.List(new Item("Plutonium Fuel Rod").Volume(1)),
            RecipeFlags.Alternate),

        new Recipe("Quickwire Cable", new Duration(24),
            Assembler,
            Item.List(
                new Item("Quickwire").Volume(3),
                new Item("Rubber").Volume(2)),
            Item.List(new Item("Cable").Volume(11)),
            RecipeFlags.Alternate),
        new Recipe("Quickwire Stator", new Duration(15),
            Assembler,
            Item.List(
                new Item("Steel Pipe").Volume(4),
                new Item("Quickwire").Volume(15)),
            Item.List(new Item("Stator").Volume(2)),
            RecipeFlags.Alternate),

        new Recipe("Rubber Concrete", new Duration(12),
            Assembler,
            Item.List(
                new Item("Limestone").Volume(10),
                new Item("Rubber").Volume(2)),
            Item.List(new Item("Concrete").Volume(9)),
            RecipeFlags.Alternate),

        new Recipe("Silicon Circuit Board", new Duration(24),
            Assembler,
            Item.List(
                new Item("Copper Sheet").Volume(11),
                new Item("Silica").Volume(11)),
            Item.List(new Item("Circuit Board").Volume(5)),
            RecipeFlags.Alternate),
        new Recipe("Steel Coated Plate", new Duration(24),
            Assembler,
            Item.List(
                new Item("Steel Ingot").Volume(3),
                new Item("Plastic").Volume(2)),
            Item.List(new Item("Iron Plate").Volume(18)),
            RecipeFlags.Alternate),
        new Recipe("Steel Rotor", new Duration(12),
            Assembler,
            Item.List(
                new Item("Steel Pipe").Volume(2),
                new Item("Wire").Volume(6)),
            Item.List(new Item("Rotor").Volume(1)),
            RecipeFlags.Alternate),
        new Recipe("Steeled Frame", new Duration(60),
            Assembler,
            Item.List(
                new Item("Reinforced Iron Plate").Volume(2),
                new Item("Steel Pipe").Volume(10)),
            Item.List(new Item("Modular Frame").Volume(3)),
            RecipeFlags.Alternate),
        new Recipe("Stitched Iron Plate", new Duration(32),
            Assembler,
            Item.List(
                new Item("Iron Plate").Volume(10),
                new Item("Wire").Volume(20)),
            Item.List(new Item("Reinforced Iron Plate").Volume(3)),
            RecipeFlags.Alternate),
    };

    public Recipe[] ProjectAssemblyRecipes { get; } =
    {
        new Recipe("Assembly Director System", new Duration(80),
            Assembler,
            Item.List(
                new Item("Adaptive Control Unit").Volume(2),
                new Item("Supercomputer").Volume(1)),
            Item.List(new Item("Assembly Director System").Volume(1)),
            RecipeFlags.ProjectAssembly),
        new Recipe("Automated Wiring", new Duration(24),
            Assembler,
            Item.List(
                new Item("Stator").Volume(1),
                new Item("Cable").Volume(20)),
            Item.List(new Item("Automated Wiring").Volume(1)),
            RecipeFlags.ProjectAssembly),
        new Recipe("Smart Plating", new Duration(30),
            Assembler,
            Item.List(
                new Item("Reinforced Iron Plate").Volume(1),
                new Item("Rotor").Volume(1)),
            Item.List(new Item("Smart Plating").Volume(1)),
            RecipeFlags.ProjectAssembly),
        new Recipe("Versatile Framework", new Duration(24),
            Assembler,
            Item.List(
                new Item("Modular Frame").Volume(1),
                new Item("Steel Beam").Volume(12)),
            Item.List(new Item("Versatile Framework").Volume(2)),
            RecipeFlags.ProjectAssembly),
    };
}
