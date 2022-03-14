using Chubberino.UnitTests.Tests.Client.Commands.Groups.SettingCollections;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.Groups.EnableableCollections
{
    public sealed class WhenAddingDisabled : UsingSettingCollection
    {
        [Fact]
        public void ShouldAddNewElement()
        {
            Sut.AddDisabled(Element1.Object);

            Assert.Single(Sut.Disabled);
            Assert.Contains(Element1.Object, Sut.Disabled);
            Assert.DoesNotContain(Element1.Object, Sut.Enabled);
        }

        /// <summary>
        /// Adding an element that has already been added does not add it a second time.
        /// </summary>
        [Fact]
        public void ShouldIgnoreDuplicates()
        {
            Sut.AddDisabled(Element1.Object);
            Sut.AddDisabled(Element1.Object);

            Assert.Single(Sut.Disabled);
            Assert.Contains(Element1.Object, Sut.Disabled);
            Assert.DoesNotContain(Element1.Object, Sut.Enabled);
        }
    }
}
