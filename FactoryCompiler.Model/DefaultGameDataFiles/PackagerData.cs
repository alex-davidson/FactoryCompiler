using System.Collections.Immutable;

namespace FactoryCompiler.Model.DefaultGameDataFiles;

internal class PackagerData : DefaultGameDataBase, IFactoryData
{
    public static Factory Packager { get; } = new Factory("Packager");

    public Factory Factory => Packager;

    public Recipe[] Recipes { get; } =
    {
        PackInCanister(new Item("Alumina Solution").Volume(2), new Duration(1), 2),
        PackInCanister(new Item("Fuel").Volume(2), new Duration(3), 2),
        PackInCanister(new Item("Heavy Oil Residue").Volume(2), new Duration(4), 2),
        PackInCanister(new Item("Liquid Biofuel").Volume(2), new Duration(3), 2),
        PackInFluidTank(new Item("Nitric Acid").Volume(1), new Duration(2), 1),
        PackInFluidTank(new Item("Nitrogen Gas").Volume(4), new Duration(1), 1),
        new Recipe("Packaged Oil", new Duration(4),
            Packager,
            Item.List(
                new Item("Crude Oil").Volume(2),
                new Item("Empty Canister").Volume(2)),
            Item.List(new Item("Packaged Oil").Volume(2))),
        PackInCanister(new Item("Sulfuric Acid").Volume(2), new Duration(3), 2),
        PackInCanister(new Item("Turbofuel").Volume(2), new Duration(6), 2),
        PackInCanister(new Item("Water").Volume(2), new Duration(2), 2),

        UnpackFromCanister(new Item("Alumina Solution").Volume(2), new Duration(1), 2),
        UnpackFromCanister(new Item("Fuel").Volume(2), new Duration(2), 2),
        UnpackFromCanister(new Item("Heavy Oil Residue").Volume(2), new Duration(6), 2),
        UnpackFromCanister(new Item("Liquid Biofuel").Volume(2), new Duration(2), 2),
        UnpackFromFluidTank(new Item("Nitric Acid").Volume(1), new Duration(3), 1),
        UnpackFromFluidTank(new Item("Nitrogen Gas").Volume(4), new Duration(1), 1),
        new Recipe("Unpackage Oil", new Duration(2),
            Packager,
            Item.List(new Item("Packaged Oil").Volume(2)),
            Item.List(
                new Item("Crude Oil").Volume(2),
                new Item("Empty Canister").Volume(2))),
        UnpackFromCanister(new Item("Sulfuric Acid").Volume(2), new Duration(1), 2),
        UnpackFromCanister(new Item("Turbofuel").Volume(2), new Duration(6), 2),
        UnpackFromCanister(new Item("Water").Volume(2), new Duration(1), 2),
    };

    private static Recipe PackInCanister(ItemVolume input, Duration duration, int packageCount)
    {
        var output = $"Packaged {input.Item.Identifier}";
        return new Recipe(output, duration,
            Packager,
            Item.List(
                input,
                new Item("Empty Canister").Volume(packageCount)),
            Item.List(new Item(output).Volume(packageCount)));
    }

    private static Recipe UnpackFromCanister(ItemVolume output, Duration duration, int packageCount)
    {
        return new Recipe($"Unpackage {output.Item.Identifier}", duration,
            Packager,
            Item.List(new Item($"Packaged {output.Item.Identifier}").Volume(packageCount)),
            Item.List(
                output,
                new Item("Empty Canister").Volume(packageCount)));
    }

    private static Recipe PackInFluidTank(ItemVolume input, Duration duration, int packageCount)
    {
        var output = $"Packaged {input.Item.Identifier}";
        return new Recipe(output, duration,
            Packager,
            Item.List(
                input,
                new Item("Empty Fluid Tank").Volume(packageCount)),
            Item.List(new Item(output).Volume(packageCount)));
    }

    private static Recipe UnpackFromFluidTank(ItemVolume output, Duration duration, int packageCount)
    {
        return new Recipe($"Unpackage {output.Item.Identifier}", duration,
            Packager,
            Item.List(new Item($"Packaged {output.Item.Identifier}").Volume(packageCount)),
            Item.List(
                output,
                new Item("Empty Fluid Tank").Volume(packageCount)));
    }

    public Recipe[] AlternateRecipes { get; } =
    {
    };

    public Recipe[] ProjectAssemblyRecipes { get; } =
    {
    };
}
