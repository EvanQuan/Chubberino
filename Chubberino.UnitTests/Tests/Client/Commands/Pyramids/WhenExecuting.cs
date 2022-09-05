using System;
using Chubberino.Infrastructure.Client;
using Moq;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.Pyramids;

public sealed class WhenExecuting : UsingPyramid
{
    [Theory]
    [InlineData("a")]
    [InlineData("1.0")]
    public void ShouldOutputInvalidHeightMessage(String height)
    {
        Sut.Execute(new String[] { height });

        MockedWriter.Verify(x => x.WriteLine($"Pyramid height of \"{height}\" must be an integer"));
        MockedTwitchClientManager.Verify(x => x.SpoolMessage(PrimaryChannelName, It.IsAny<String>(), It.IsAny<Priority>()), Times.Never());
    }

    [Theory]
    [InlineData("-1")]
    [InlineData("0")]
    [InlineData("1")]
    public void ShouldOutputBlockNotSuppliedMessage(String height)
    {
        Sut.Execute(new String[] { height });

        MockedWriter.Verify(x => x.WriteLine($"Pyramid block not supplied."));
        MockedTwitchClientManager.Verify(x => x.SpoolMessage(PrimaryChannelName, It.IsAny<String>(), It.IsAny<Priority>()), Times.Never());
    }
}
