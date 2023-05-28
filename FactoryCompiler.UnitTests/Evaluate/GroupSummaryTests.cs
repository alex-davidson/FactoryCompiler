using FactoryCompiler.Model;
using FactoryCompiler.Model.Evaluate;
using FactoryCompiler.Model.State;
using NUnit.Framework;

namespace FactoryCompiler.UnitTests.Evaluate
{
    [TestFixture]
    public class GroupSummaryTests
    {
        [Test]
        public void SummaryShortfallIsSpreadProportionally()
        {
            var parent = new ItemVolumesState();
            parent.Produce(new Item("Iron Ingot"), 500);
            parent.Consume(new Item("Iron Ingot"), 1000);

            var groupA = new ItemVolumesState();
            groupA.Consume(new Item("Iron Ingot"), 200);

            var groupB = new ItemVolumesState();
            groupB.Consume(new Item("Iron Ingot"), 800);

            var groupASummary = new GroupSummary(parent, groupA);
            var groupBSummary = new GroupSummary(parent, groupB);

            Assert.That(groupASummary.GetScaledSummary(), Is.EqualTo(new ItemVolumeSummary(200, 0, 100)));
            Assert.That(groupBSummary.GetScaledSummary(), Is.EqualTo(new ItemVolumeSummary(800, 0, 400)));
        }

        [Test]
        public void SummaryExcessIsSpreadProportionally()
        {
            var parent = new ItemVolumesState();
            parent.Consume(new Item("Iron Ingot"), 500);
            parent.Produce(new Item("Iron Ingot"), 1000);

            var groupA = new ItemVolumesState();
            groupA.Produce(new Item("Iron Ingot"), 200);

            var groupB = new ItemVolumesState();
            groupB.Produce(new Item("Iron Ingot"), 800);

            var groupASummary = new GroupSummary(parent, groupA);
            var groupBSummary = new GroupSummary(parent, groupB);

            Assert.That(groupASummary.GetScaledSummary(), Is.EqualTo(new ItemVolumeSummary(200, 100, 0)));
            Assert.That(groupBSummary.GetScaledSummary(), Is.EqualTo(new ItemVolumeSummary(800, 400, 0)));
        }
    }
}
