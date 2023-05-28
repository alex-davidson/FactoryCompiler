namespace FactoryCompiler.Model.DefaultGameDataFiles;

internal class MinerMk3Data : DefaultGameDataBase, IFactoryData
{
    public static Factory MinerMk3 { get; } = new Factory("Miner Mk3");

    public Factory Factory => MinerMk3;

    public Recipe[] Recipes { get; } = MiningDataHelper.GetMiningRecipes(MinerMk3, 4);

    public Recipe[] AlternateRecipes { get; } =
    {
    };

    public Recipe[] ProjectAssemblyRecipes { get; } =
    {
    };
}
