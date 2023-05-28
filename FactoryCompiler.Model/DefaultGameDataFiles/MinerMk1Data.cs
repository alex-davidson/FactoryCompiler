namespace FactoryCompiler.Model.DefaultGameDataFiles;

internal class MinerMk1Data : DefaultGameDataBase, IFactoryData
{
    public static Factory MinerMk1 { get; } = new Factory("Miner Mk1");

    public Factory Factory => MinerMk1;

    public Recipe[] Recipes { get; } = MiningDataHelper.GetMiningRecipes(MinerMk1, 1);

    public Recipe[] AlternateRecipes { get; } =
    {
    };

    public Recipe[] ProjectAssemblyRecipes { get; } =
    {
    };
}
