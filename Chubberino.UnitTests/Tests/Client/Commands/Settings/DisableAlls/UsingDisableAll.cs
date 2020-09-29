using Chubberino.Client.Abstractions;
using Chubberino.Client.Commands;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.DisableAlls
{
    public abstract class UsingDisableAll
    {
        /// <summary>
        /// System under test.
        /// </summary>
        protected DisableAll Sut { get; }

        protected Mock<IExtendedClient> MockedExtendedClient { get; }

        protected Mock<ICommandRepository> MockedCommandRepository { get; }

        public UsingDisableAll()
        {
            MockedExtendedClient = new Mock<IExtendedClient>();
            MockedCommandRepository = new Mock<ICommandRepository>();

            Sut = new DisableAll(MockedExtendedClient.Object, MockedCommandRepository.Object, new Mock<TextWriter>().Object);
        }
    }
}
