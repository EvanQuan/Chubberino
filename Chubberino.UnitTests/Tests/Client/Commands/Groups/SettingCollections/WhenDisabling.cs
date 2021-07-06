using Chubberino.UnitTests.Tests.Client.Commands.Groups.SettingCollections;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.Groups.EnableableCollections
{
    public sealed class WhenDisabling : UsingSettingCollection
    {
        [Fact]
        public void ShouldDisableExistingEnabledElement()
        {
            Sut.AddEnabled(Element1.Object);

            Sut.Disable(Element1.Object.Name);

            Assert.Single(Sut.Disabled);
            Assert.Contains(Element1.Object, Sut.Disabled);
            Assert.DoesNotContain(Element1.Object, Sut.Enabled);
        }

        [Fact]
        public void ShouldDoNothingOnNonExistingElement()
        {
            Sut.Disable(Element1.Object.Name);

            Assert.DoesNotContain(Element1.Object, Sut.Enabled);
            Assert.DoesNotContain(Element1.Object, Sut.Disabled);
        }

        [Fact]
        public void ShouldDoNothingOnEnabledElement()
        {
            Sut.AddDisabled(Element1.Object);

            Sut.Disable(Element1.Object.Name);

            Assert.Single(Sut.Disabled);
            Assert.Contains(Element1.Object, Sut.Disabled);
            Assert.DoesNotContain(Element1.Object, Sut.Enabled);
        }
    }
}
