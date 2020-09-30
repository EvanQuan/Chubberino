using Autofac;
using Chubberino.Client;
using Chubberino.Client.Abstractions;
using Chubberino.Client.Commands;
using Chubberino.Client.Commands.Settings;
using Chubberino.Client.Commands.Settings.Replies;
using Chubberino.Client.Commands.Strategies;
using System;
using System.IO;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Interfaces;

namespace Chubberino
{
    public static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        public static void Main()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Bot>().As<IBot>().SingleInstance();
            builder.RegisterInstance(Console.Out).As<TextWriter>().SingleInstance();
            builder.RegisterType<CommandRepository>().As<ICommandRepository>().SingleInstance();
            builder.RegisterType<ExtendedClient>().As<IExtendedClient>();
            builder.RegisterType<WebSocketClient>().As<IClient>();
            builder.Register(c => new ConnectionCredentials(TwitchInfo.BotUsername, TwitchInfo.BotToken)).As<ConnectionCredentials>().SingleInstance();
            builder.RegisterType<StopSettingStrategy>().As<IStopSettingStrategy>().SingleInstance();
            builder.RegisterType<Repeater>().As<IRepeater>();
            builder.RegisterType<ContainsComparator>().As<IContainsComparator>().SingleInstance();
            builder.RegisterType<EqualsComparator>().As<IEqualsComparator>().SingleInstance();

            // Commands
            builder.RegisterType<AutoChat>().AsSelf().SingleInstance();
            builder.RegisterType<AutoPogO>().AsSelf().SingleInstance();
            builder.RegisterType<Color>().AsSelf().SingleInstance();
            builder.RegisterType<Copy>().AsSelf().SingleInstance();
            builder.RegisterType<Count>().AsSelf().SingleInstance();
            builder.RegisterType<DisableAll>().AsSelf().SingleInstance();
            builder.RegisterType<Greet>().AsSelf().SingleInstance();
            builder.RegisterType<Jimbox>().AsSelf().SingleInstance();
            builder.RegisterType<Join>().AsSelf().SingleInstance();
            builder.RegisterType<Log>().AsSelf().SingleInstance();
            builder.RegisterType<MockStreamElements>().AsSelf().SingleInstance();
            builder.RegisterType<ModCheck>().AsSelf().SingleInstance();
            builder.RegisterType<Mode>().AsSelf().SingleInstance();
            builder.RegisterType<Pyramid>().AsSelf().SingleInstance();
            builder.RegisterType<Repeat>().AsSelf().SingleInstance();
            builder.RegisterType<Reply>().AsSelf().SingleInstance();
            builder.RegisterType<Say>().AsSelf().SingleInstance();
            builder.RegisterType<TimeoutAlert>().AsSelf().SingleInstance();
            builder.RegisterType<TrackJimbox>().AsSelf().SingleInstance();
            builder.RegisterType<TrackPyramids>().AsSelf().SingleInstance();
            builder.RegisterType<YepKyle>().AsSelf().SingleInstance();

            IContainer container = builder.Build();

            using ILifetimeScope scope = container.BeginLifetimeScope();

            var commandRepository = scope.Resolve<ICommandRepository>();
            commandRepository
                .AddCommand(scope.Resolve<AutoChat>())
                .AddCommand(scope.Resolve<AutoPogO>())
                .AddCommand(scope.Resolve<Color>())
                .AddCommand(scope.Resolve<Copy>())
                .AddCommand(scope.Resolve<Count>())
                .AddCommand(scope.Resolve<DisableAll>())
                .AddCommand(scope.Resolve<Greet>())
                .AddCommand(scope.Resolve<Jimbox>())
                .AddCommand(scope.Resolve<Join>())
                .AddCommand(scope.Resolve<Log>())
                .AddCommand(scope.Resolve<MockStreamElements>())
                .AddCommand(scope.Resolve<ModCheck>())
                .AddCommand(scope.Resolve<Mode>())
                .AddCommand(scope.Resolve<Pyramid>())
                .AddCommand(scope.Resolve<Repeat>())
                .AddCommand(scope.Resolve<Reply>())
                .AddCommand(scope.Resolve<Say>())
                .AddCommand(scope.Resolve<TimeoutAlert>())
                .AddCommand(scope.Resolve<TrackJimbox>())
                .AddCommand(scope.Resolve<TrackPyramids>())
                .AddCommand(scope.Resolve<YepKyle>());


            var bot = scope.Resolve<IBot>();


            bot.Scope = scope;

            if (!bot.Start())
            {
                Console.WriteLine("Failed to join channel");
                return;
            }

            Boolean shouldContinue = true;
            do
            {
                Console.Write(bot.GetPrompt());
                bot.ReadCommand(Console.ReadLine());

                switch (bot.State)
                {
                    // Continue onto the next prompt
                    default:
                    case BotState.ShouldContinue:
                        break;
                    // End the program.
                    case BotState.ShouldStop:
                        shouldContinue = false;
                        break;
                    // case BotState.ShouldRestart:
                        // TODO
                        // break;
                }
            }
            while (shouldContinue);
        }
    }
}
