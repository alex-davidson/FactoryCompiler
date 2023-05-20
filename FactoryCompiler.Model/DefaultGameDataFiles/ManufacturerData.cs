using System.Collections.Immutable;

namespace FactoryCompiler.Model.DefaultGameDataFiles;

internal class ManufacturerData : DefaultGameDataBase, IFactoryData
{
    public static Factory Manufacturer { get; } = new Factory("Manufacturer");

    public Factory Factory => Manufacturer;

    public Recipe[] Recipes { get; } =
    {
        new Recipe("Beacon", new Duration(8),
            Manufacturer,
            Item.List(
                new Item("Iron Plate").Volume(3),
                new Item("Iron Rod").Volume(1),
                new Item("Wire").Volume(15),
                new Item("Cable").Volume(2)),
            Item.List(new Item("Beacon").Volume(1))),

        new Recipe("Computer", new Duration(24),
            Manufacturer,
            Item.List(
                new Item("Circuit Board").Volume(10),
                new Item("Cable").Volume(9),
                new Item("Plastic").Volume(18),
                new Item("Screw").Volume(52)),
            Item.List(new Item("Computer").Volume(1))),
        new Recipe("Crystal Oscillator", new Duration(120),
            Manufacturer,
            Item.List(
                new Item("Quartz Crystal").Volume(36),
                new Item("Cable").Volume(28),
                new Item("Reinforced Iron Plate").Volume(5)),
            Item.List(new Item("Crystal Oscillator").Volume(2))),

        new Recipe("Explosive Rebar", new Duration(12),
            Manufacturer,
            Item.List(
                new Item("Iron Rebar").Volume(2),
                new Item("Smokeless Powder").Volume(2),
                new Item("Steel Pipe").Volume(2)),
            Item.List(new Item("Explosive Rebar").Volume(1))),

        new Recipe("Gas Filter", new Duration(8),
            Manufacturer,
            Item.List(
                new Item("Coal").Volume(5),
                new Item("Rubber").Volume(2),
                new Item("Fabric").Volume(2)),
            Item.List(new Item("Gas Filter").Volume(1))),

        new Recipe("Heavy Modular Frame", new Duration(30),
            Manufacturer,
            Item.List(
                new Item("Modular Frame").Volume(5),
                new Item("Steel Pipe").Volume(15),
                new Item("Encased Industrial Beam").Volume(5),
                new Item("Screw").Volume(100)),
            Item.List(new Item("Heavy Modular Frame").Volume(1))),
        new Recipe("High-Speed Connector", new Duration(16),
            Manufacturer,
            Item.List(
                new Item("Quickwire").Volume(56),
                new Item("Cable").Volume(10),
                new Item("Circuit Board").Volume(1)),
            Item.List(new Item("High-Speed Connector").Volume(1))),

        new Recipe("Iodine Infused Filter", new Duration(16),
            Manufacturer,
            Item.List(
                new Item("Gas Filter").Volume(1),
                new Item("Quickwire").Volume(8),
                new Item("Aluminum Casing").Volume(1)),
            Item.List(new Item("Iodine Infused Filter").Volume(1))),

        new Recipe("Nuke Nobelisk", new Duration(120),
            Manufacturer,
            Item.List(
                new Item("Nobelisk").Volume(5),
                new Item("Encased Uranium Cell").Volume(20),
                new Item("Smokeless Powder").Volume(10),
                new Item("AI Limiter").Volume(6)),
            Item.List(new Item("Nuke Nobelisk").Volume(1))),

        new Recipe("Plutonium Fuel Rod", new Duration(240),
            Manufacturer,
            Item.List(
                new Item("Encased Plutonium Cell").Volume(30),
                new Item("Steel Beam").Volume(18),
                new Item("Electromagnetic Control Rod").Volume(6),
                new Item("Heat Sink").Volume(10)),
            Item.List(new Item("Plutonium Fuel Rod").Volume(1))),

        new Recipe("Radio Control Unit", new Duration(48),
            Manufacturer,
            Item.List(
                new Item("Aluminum Casing").Volume(32),
                new Item("Crystal Oscillator").Volume(1),
                new Item("Computer").Volume(1)),
            Item.List(new Item("Radio Control Unit").Volume(2))),

        new Recipe("Supercomputer", new Duration(50),
            Manufacturer,
            Item.List(
                new Item("Computer").Volume(2),
                new Item("AI Limiter").Volume(2),
                new Item("High-Speed Connector").Volume(3),
                new Item("Plastic").Volume(28)),
            Item.List(new Item("Supercomputer").Volume(1))),

        new Recipe("Turbo Motor", new Duration(32),
            Manufacturer,
            Item.List(
                new Item("Cooling System").Volume(4),
                new Item("Radio Control Unit").Volume(2),
                new Item("Motor").Volume(4),
                new Item("Rubber").Volume(24)),
            Item.List(new Item("Turbo Motor").Volume(1))),
        new Recipe("Turbo Rifle Ammo", new Duration(12),
            Manufacturer,
            Item.List(
                new Item("Rifle Ammo").Volume(25),
                new Item("Aluminum Casing").Volume(3),
                new Item("Packaged Turbofuel").Volume(3)),
            Item.List(new Item("Turbo Rifle Ammo").Volume(50))),

        new Recipe("Uranium Fuel Rod", new Duration(150),
            Manufacturer,
            Item.List(
                new Item("Encased Uranium Cell").Volume(50),
                new Item("Encased Industrial Beam").Volume(3),
                new Item("Electromagnetic Control Rod").Volume(5)),
            Item.List(new Item("Uranium Fuel Rod").Volume(1))),
    };

    public Recipe[] AlternateRecipes { get; } =
    {
        new Recipe("Automated Miner", new Duration(60),
            Manufacturer,
            Item.List(
                new Item("Motor").Volume(1),
                new Item("Steel Pipe").Volume(4),
                new Item("Iron Rod").Volume(4),
                new Item("Iron Plate").Volume(2)),
            Item.List(new Item("Portable Miner").Volume(1)),
            RecipeFlags.Alternate),

        new Recipe("Caterium Computer", new Duration(16),
            Manufacturer,
            Item.List(
                new Item("Circuit Board").Volume(7),
                new Item("Quickwire").Volume(28),
                new Item("Rubber").Volume(12)),
            Item.List(new Item("Computer").Volume(1)),
            RecipeFlags.Alternate),
        new Recipe("Classic Battery", new Duration(8),
            Manufacturer,
            Item.List(
                new Item("Sulfur").Volume(6),
                new Item("Alclad Aluminum Sheet").Volume(7),
                new Item("Plastic").Volume(8),
                new Item("Wire").Volume(12)),
            Item.List(new Item("Battery").Volume(4)),
            RecipeFlags.Alternate),
        new Recipe("Crystal Beacon", new Duration(120),
            Manufacturer,
            Item.List(
                new Item("Steel Beam").Volume(4),
                new Item("Steel Pipe").Volume(16),
                new Item("Crystal Oscillator").Volume(1)),
            Item.List(new Item("Beacon").Volume(20)),
            RecipeFlags.Alternate),

        new Recipe("Heavy Encased Frame", new Duration(64),
            Manufacturer,
            Item.List(
                new Item("Modular Frame").Volume(8),
                new Item("Encased Industrial Beam").Volume(10),
                new Item("Steel Pipe").Volume(36),
                new Item("Concrete").Volume(22)),
            Item.List(new Item("Heavy Modular Frame").Volume(3)),
            RecipeFlags.Alternate),
        new Recipe("Heavy Flexible Frame", new Duration(16),
            Manufacturer,
            Item.List(
                new Item("Modular Frame").Volume(5),
                new Item("Encased Industrial Beam").Volume(3),
                new Item("Rubber").Volume(20),
                new Item("Screw").Volume(104)),
            Item.List(new Item("Heavy Modular Frame").Volume(1)),
            RecipeFlags.Alternate),

        new Recipe("Infused Uranium Cell", new Duration(12),
            Manufacturer,
            Item.List(
                new Item("Uranium").Volume(5),
                new Item("Silica").Volume(3),
                new Item("Sulfur").Volume(5),
                new Item("Quickwire").Volume(15)),
            Item.List(new Item("Encased Uranium Cell").Volume(4)),
            RecipeFlags.Alternate),
        new Recipe("Insulated Crystal Oscillator", new Duration(32),
            Manufacturer,
            Item.List(
                new Item("Quartz Crystal").Volume(10),
                new Item("Rubber").Volume(7),
                new Item("AI Limiter").Volume(1)),
            Item.List(new Item("Crystal Oscillator").Volume(1)),
            RecipeFlags.Alternate),

        new Recipe("Radio Connection Unit", new Duration(16),
            Manufacturer,
            Item.List(
                new Item("Heat Sink").Volume(4),
                new Item("High-Speed Connector").Volume(2),
                new Item("Quartz Crystal").Volume(12)),
            Item.List(new Item("Radio Control Unit").Volume(1)),
            RecipeFlags.Alternate),
        new Recipe("Radio Control System", new Duration(40),
            Manufacturer,
            Item.List(
                new Item("Crystal Oscillator").Volume(1),
                new Item("Circuit Board").Volume(10),
                new Item("Aluminum Casing").Volume(60),
                new Item("Rubber").Volume(30)),
            Item.List(new Item("Radio Control Unit").Volume(3)),
            RecipeFlags.Alternate),
        new Recipe("Rigour Motor", new Duration(48),
            Manufacturer,
            Item.List(
                new Item("Rotor").Volume(3),
                new Item("Stator").Volume(3),
                new Item("Crystal Oscillator").Volume(1)),
            Item.List(new Item("Motor").Volume(6)),
            RecipeFlags.Alternate),

        new Recipe("Silicon High-Speed Connector", new Duration(40),
            Manufacturer,
            Item.List(
                new Item("Quickwire").Volume(60),
                new Item("Silica").Volume(25),
                new Item("Circuit Board").Volume(2)),
            Item.List(new Item("High-Speed Connector").Volume(2)),
            RecipeFlags.Alternate),
        new Recipe("Super-State Computer", new Duration(50),
            Manufacturer,
            Item.List(
                new Item("Computer").Volume(3),
                new Item("Electromagnetic Control Rod").Volume(2),
                new Item("Battery").Volume(20),
                new Item("Wire").Volume(45)),
            Item.List(new Item("Supercomputer").Volume(2)),
            RecipeFlags.Alternate),

        new Recipe("Turbo Electric Motor", new Duration(64),
            Manufacturer,
            Item.List(
                new Item("Motor").Volume(7),
                new Item("Radio Control Unit").Volume(9),
                new Item("Electromagnetic Control Rod").Volume(5),
                new Item("Rotor").Volume(7)),
            Item.List(new Item("Turbo Motor").Volume(3)),
            RecipeFlags.Alternate),
        new Recipe("Turbo Pressure Motor", new Duration(32),
            Manufacturer,
            Item.List(
                new Item("Motor").Volume(4),
                new Item("Pressure Conversion Cube").Volume(1),
                new Item("Packaged Nitrogen Gas").Volume(24),
                new Item("Stator").Volume(8)),
            Item.List(new Item("Turbo Motor").Volume(2)),
            RecipeFlags.Alternate),

        new Recipe("Uranium Fuel Unit", new Duration(300),
            Manufacturer,
            Item.List(
                new Item("Encased Uranium Cell").Volume(100),
                new Item("Electromagnetic Control Rod").Volume(10),
                new Item("Crystal Oscillator").Volume(3),
                new Item("Beacon").Volume(6)),
            Item.List(new Item("Uranium Fuel Rod").Volume(3)),
            RecipeFlags.Alternate),
    };

    public Recipe[] ProjectAssemblyRecipes { get; } =
    {
        new Recipe("Adaptive Control Unit", new Duration(120),
            Manufacturer,
            Item.List(
                new Item("Automated Wiring").Volume(15),
                new Item("Circuit Board").Volume(10),
                new Item("Heavy Modular Frame").Volume(2),
                new Item("Computer").Volume(2)),
            Item.List(new Item("Adaptive Control Unit").Volume(2)),
            RecipeFlags.ProjectAssembly),
        new Recipe("Automated Speed Wiring", new Duration(32),
            Manufacturer,
            Item.List(
                new Item("Stator").Volume(2),
                new Item("Wire").Volume(40),
                new Item("High-Speed Connector").Volume(1)),
            Item.List(new Item("Automated Wiring").Volume(4)),
            RecipeFlags.ProjectAssembly | RecipeFlags.Alternate),

        new Recipe("Flexible Framework", new Duration(16),
            Manufacturer,
            Item.List(
                new Item("Modular Frame").Volume(1),
                new Item("Steel Beam").Volume(6),
                new Item("Rubber").Volume(8)),
            Item.List(new Item("Versatile Framework").Volume(2)),
            RecipeFlags.ProjectAssembly | RecipeFlags.Alternate),

        new Recipe("Magnetic Field Generator", new Duration(120),
            Manufacturer,
            Item.List(
                new Item("Versatile Framework").Volume(5),
                new Item("Electromagnetic Control Rod").Volume(2),
                new Item("Battery").Volume(10)),
            Item.List(new Item("Magnetic Field Generator").Volume(2)),
            RecipeFlags.ProjectAssembly),
        new Recipe("Modular Engine", new Duration(60),
            Manufacturer,
            Item.List(
                new Item("Motor").Volume(2),
                new Item("Rubber").Volume(15),
                new Item("Smart Plating").Volume(2)),
            Item.List(new Item("Modular Engine").Volume(1)),
            RecipeFlags.ProjectAssembly),

        new Recipe("Plastic Smart Plating", new Duration(24),
            Manufacturer,
            Item.List(
                new Item("Reinforced Iron Plate").Volume(1),
                new Item("Rotor").Volume(1),
                new Item("Plastic").Volume(3)),
            Item.List(new Item("Smart Plating").Volume(2)),
            RecipeFlags.ProjectAssembly | RecipeFlags.Alternate),

        new Recipe("Thermal Propulsion Rocket", new Duration(120),
            Manufacturer,
            Item.List(
                new Item("Modular Engine").Volume(5),
                new Item("Turbo Motor").Volume(2),
                new Item("Cooling System").Volume(6),
                new Item("Fused Modular Frame").Volume(2)),
            Item.List(new Item("Thermal Propulsion Rocket").Volume(2)),
            RecipeFlags.ProjectAssembly),
    };
}
