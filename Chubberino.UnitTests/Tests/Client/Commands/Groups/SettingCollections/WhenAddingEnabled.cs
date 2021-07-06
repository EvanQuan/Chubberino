using Chubberino.UnitTests.Tests.Client.Commands.Groups.SettingCollections;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.Groups.EnableableCollections
{
    public sealed class WhenAddingEnabled : UsingSettingCollection
    {
        [Fact]
        public void ShouldAddNewElement()
        {
            Sut.AddEnabled(Element1.Object);

            Assert.Single(Sut.Enabled);
            Assert.Contains(Element1.Object, Sut.Enabled);
            Assert.DoesNotContain(Element1.Object, Sut.Disabled);
        }

        /// <summary>
        /// Adding an element that has already been added does not add it a second time.
        /// </summary>
        [Fact]
        public void ShouldIgnoreDuplicates()
        {
            Sut.AddEnabled(Element1.Object);
            Sut.AddEnabled(Element1.Object);

            Assert.Single(Sut.Enabled);
            Assert.Contains(Element1.Object, Sut.Enabled);
            Assert.DoesNotContain(Element1.Object, Sut.Disabled);
        }
    }
}
