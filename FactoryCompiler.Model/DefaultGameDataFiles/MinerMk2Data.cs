namespace FactoryCompiler.Model.DefaultGameDataFiles;

internal class MinerMk2Data : DefaultGameDataBase, IFactoryData
{
    public static Factory MinerMk2 { get; } = new Factory("Miner Mk2");

    public Factory Factory => MinerMk2;

    public Recipe[] Recipes { get; } = MiningDataHelper.GetMiningRecipes(MinerMk2, 2);

    public Recipe[] AlternateRecipes { get; } =
    {
    };

    public Recipe[] ProjectAssemblyRecipes { get; } =
    {
    };
}
