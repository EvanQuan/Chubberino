using System;
using System.Collections.Generic;
using System.IO;
using Autofac;
using Chubberino.Bots.Channel.Commands;
using Chubberino.Bots.Common.Commands;
using Chubberino.Bots.Common.Commands.Settings;
using Chubberino.Bots.Common.Commands.Settings.Pyramids;
using Chubberino.Bots.Common.Commands.Settings.Replies;
using Chubberino.Bots.Common.Commands.Settings.Strategies;
using Chubberino.Client.Commands;
using Chubberino.Client.Commands.Settings;
using Chubberino.Client.Commands.Settings.ColorSelectors;
using Chubberino.Client.Commands.Settings.UserCommands;
using Chubberino.Common.Services;
using Chubberino.Database.Contexts;
using Chubberino.Infrastructure.Client;
using Chubberino.Infrastructure.Client.TwitchClients;
using Chubberino.Infrastructure.Commands;
using Chubberino.Infrastructure.Commands.Settings.UserCommands;
using Chubberino.Infrastructure.Credentials;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Hazards;
using Chubberino.Modules.CheeseGame.Heists;
using Chubberino.Modules.CheeseGame.Helping;
using Chubberino.Modules.CheeseGame.Items;
using Chubberino.Modules.CheeseGame.Items.Recipes;
using Chubberino.Modules.CheeseGame.Items.Upgrades.RecipeModifiers;
using Chubberino.Modules.CheeseGame.Points;
using Chubberino.Modules.CheeseGame.Quests;
using Chubberino.Modules.CheeseGame.Ranks;
using Chubberino.Modules.CheeseGame.Shops;
using Jering.Javascript.NodeJS;
using Microsoft.Extensions.DependencyInjection;
using TwitchLib.Api;
using TwitchLib.Api.Interfaces;
using TwitchLib.Client.Enums;
using TwitchLib.Communication.Clients;
using WolframAlphaNet;

using ChannelCommand = Chubberino.Bots.Common.Commands.Settings.Channel;

namespace Chubberino.Bots.Channel
{
    public class Program
    {
        private static TextWriter Writer { get; set; }

        private static IBot Bot { get; set; }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        public static void Main()
        {
            var credentials = SetupIoC(null);

            Boolean shouldContinue = credentials != null;

            Boolean shouldGetPrompt = true;

            while (shouldContinue)
            {
                try
                {
                    if (shouldGetPrompt)
                    {
                        Writer.Write(Bot.GetPrompt());
                        Bot.ReadCommand(Console.ReadLine());
                    }

                    switch (Bot.State)
                    {
                        // Continue onto the next prompt
                        default:
                        case BotState.ShouldContinue:
                            break;
                        // End the program.
                        case BotState.ShouldStop:
                            shouldContinue = false;
                            break;
                        case BotState.ShouldRestart:
                            credentials = SetupIoC(credentials);
                            shouldContinue = credentials != null;
                            break;
                    }

                    shouldGetPrompt = true;
                }
                catch (Exception ex)
                {
                    Writer.WriteLine(ex.Message);
                    Bot.State = BotState.ShouldRestart;
                    shouldGetPrompt = false;
                }
            }
        }

        public static LoginCredentials SetupIoC(LoginCredentials credentials)
        {
            var services = new ServiceCollection();

            var builder = new ContainerBuilder();
            builder.RegisterType<Bot>().As<IBot>().SingleInstance();
            builder.Register(x => Console.Out).As<TextWriter>().SingleInstance();
            builder.Register(x => Console.In).As<TextReader>().SingleInstance();
            builder.RegisterType<TwitchClientManager>().As<ITwitchClientManager>().SingleInstance();
            builder.RegisterType<CrendentialsManager>().As<ICredentialsManager>().SingleInstance();
            builder.RegisterType<UserCommandValidator>().As<IUserCommandValidator>().SingleInstance();
            builder.RegisterType<CommandRepository>().As<ICommandRepository>().SingleInstance();
            builder.Register(c => new TwitchClientFactory(
                (options) => new WebSocketClient(options),
                ClientProtocol.WebSocket,
                null)).As<ITwitchClientFactory>().SingleInstance();

            builder.RegisterType<DateTimeService>().As<IDateTimeService>().SingleInstance();

            builder.RegisterType<StopSettingStrategy>().As<IStopSettingStrategy>().SingleInstance();
            builder.RegisterType<Repeater>().As<IRepeater>();
            builder.RegisterType<ContainsComparator>().As<IContainsComparator>().SingleInstance();
            builder.RegisterType<EqualsComparator>().As<IEqualsComparator>().SingleInstance();
            builder.RegisterType<Random>().AsSelf().SingleInstance();
            builder.RegisterType<ComplimentGenerator>().As<IComplimentGenerator>().SingleInstance();
            builder.RegisterType<ComplimentGenerator>().As<IComplimentGenerator>().SingleInstance();
            builder.RegisterType<RainbowColorSelector>().AsSelf().SingleInstance();
            builder.RegisterType<RandomColorSelector>().AsSelf().SingleInstance();
            builder.RegisterType<PresetColorSelector>().AsSelf().SingleInstance();
            builder.Register(c =>
            {
                var services = new ServiceCollection().AddNodeJS();

                var serviceProvider = services.BuildServiceProvider();
                return serviceProvider.GetRequiredService<INodeJSService>();
            }).As<INodeJSService>().SingleInstance();
            builder.RegisterType<SpinWaitService>().As<ISpinWaitService>().SingleInstance();
            builder.RegisterType<ModeratorClientOptions>().As<IModeratorClientOptions>().SingleInstance();
            builder.RegisterType<RegularClientOptions>().As<IRegularClientOptions>().SingleInstance();
            builder.RegisterType<PyramidBuilder>().AsSelf().SingleInstance();

            builder.Register(c =>
            {
                var api = new TwitchAPI();

                var credentials = c.Resolve<ICredentialsManager>();

                api.Settings.ClientId = credentials.ApplicationCredentials.TwitchAPIClientID;
                // Doesn't matter which user credentials access token is used here.
                api.Settings.AccessToken = credentials.UserCredentials.AccessToken;

                return api;
            }).As<ITwitchAPI>().SingleInstance();

            builder.Register(c => new WolframAlpha(c.Resolve<ICredentialsManager>().ApplicationCredentials.WolframAlphaAppID)).AsSelf().SingleInstance();

            // Commands
            builder.RegisterType<AtAll>().AsSelf().SingleInstance();
            builder.RegisterType<AutoChat>().AsSelf().SingleInstance();
            builder.RegisterType<AutoPogO>().AsSelf().SingleInstance();
            builder.RegisterType<ChannelCommand>().AsSelf().SingleInstance();
            builder.RegisterType<Cheese>().AsSelf().SingleInstance();
            builder.RegisterType<Color>().AsSelf().SingleInstance();
            builder.RegisterType<Cookie>().AsSelf().SingleInstance();
            builder.RegisterType<Copy>().AsSelf().SingleInstance();
            builder.RegisterType<Count>().AsSelf().SingleInstance();
            builder.RegisterType<DisableAll>().AsSelf().SingleInstance();
            builder.RegisterType<Emotes>().AsSelf().SingleInstance();
            // builder.RegisterType<Greet>().AsSelf().SingleInstance();
            builder.RegisterType<Jimbox>().AsSelf().SingleInstance();
            builder.RegisterType<Join>().AsSelf().SingleInstance();
            builder.RegisterType<Leave>().AsSelf().SingleInstance();
            builder.RegisterType<Log>().AsSelf().SingleInstance();
            builder.RegisterType<MockStreamElements>().AsSelf().SingleInstance();
            builder.RegisterType<ModCheck>().AsSelf().SingleInstance();
            builder.RegisterType<Mode>().AsSelf().SingleInstance();
            builder.RegisterType<Moya>().AsSelf().SingleInstance();
            builder.RegisterType<Permutations>().AsSelf().SingleInstance();
            builder.RegisterType<Pyramid>().AsSelf().SingleInstance();
            builder.RegisterType<Refresh>().AsSelf().SingleInstance();
            builder.RegisterType<Repeat>().AsSelf().SingleInstance();
            builder.RegisterType<Reply>().AsSelf().SingleInstance();
            builder.RegisterType<Say>().AsSelf().SingleInstance();
            builder.RegisterType<Switch>().AsSelf().SingleInstance();
            builder.RegisterType<TimeoutAlert>().AsSelf().SingleInstance();
            builder.RegisterType<TrackJimbox>().AsSelf().SingleInstance();
            builder.RegisterType<TrackPyramids>().AsSelf().SingleInstance();
            builder.RegisterType<Translate>().AsSelf().SingleInstance();
            builder.RegisterType<Wolfram>().AsSelf().SingleInstance();

            builder.RegisterType<ApplicationContextFactory>().As<IApplicationContextFactory>().SingleInstance();

            // Cheese game
            builder.RegisterType<HelpManager>().As<IHelpManager>().SingleInstance();
            builder.RegisterType<Shop>().As<IShop>().SingleInstance();
            builder.RegisterType<PointManager>().As<IPointManager>().SingleInstance();
            builder.RegisterType<RankManager>().As<IRankManager>().SingleInstance();
            builder.RegisterType<QuestManager>().As<IQuestManager>().SingleInstance();
            builder.RegisterType<HeistManager>().As<IHeistManager>().SingleInstance();
            builder.RegisterType<EmoteManager>().As<IEmoteManager>().SingleInstance();
            builder.Register(x => RecipeRepository.Recipes).As<IReadOnlyList<RecipeInfo>>().SingleInstance();
            builder.Register(x => RecipeModifierRepository.Modifiers).As<IReadOnlyList<RecipeModifier>>().SingleInstance();
            builder.RegisterType<HazardManager>().As<IHazardManager>().SingleInstance();

            // Items
            builder.RegisterType<Gear>().AsSelf().SingleInstance();
            builder.RegisterType<Mousetrap>().AsSelf().SingleInstance();
            builder.RegisterType<Population>().AsSelf().SingleInstance();
            builder.RegisterType<QuestLocation>().AsSelf().SingleInstance();
            builder.RegisterType<Recipe>().AsSelf().SingleInstance();
            builder.RegisterType<Storage>().AsSelf().SingleInstance();
            builder.RegisterType<Upgrade>().AsSelf().SingleInstance();
            builder.RegisterType<Worker>().AsSelf().SingleInstance();

            // Quests
            builder.RegisterType<QuestRepository>().As<IQuestRepository>().SingleInstance();

            IContainer container = builder.Build();

            using ILifetimeScope scope = container.BeginLifetimeScope();

            Writer = scope.Resolve<TextWriter>();

            Bot = scope.Resolve<IBot>();

            var twitchClientManager = scope.Resolve<ITwitchClientManager>();

            credentials = twitchClientManager.TryInitializeTwitchClient(Bot, credentials: credentials);

            if (credentials == null)
            {
                Writer.WriteLine("Failed to start");
                return null;
            }

            if (!twitchClientManager.TryJoinInitialChannels())
            {
                return null;
            }

            scope
                .Resolve<ICommandRepository>()
                .AddCommand(scope.Resolve<AtAll>())
                .AddCommand(scope.Resolve<AutoChat>())
                .AddCommand(scope.Resolve<AutoPogO>())
                .AddCommand(scope.Resolve<ChannelCommand>())
                .AddCommand(scope.Resolve<Cheese>())
                .AddCommand(scope.Resolve<Color>())
                .AddCommand(scope.Resolve<Cookie>())
                .AddCommand(scope.Resolve<Copy>())
                .AddCommand(scope.Resolve<Count>())
                .AddCommand(scope.Resolve<DisableAll>())
                .AddCommand(scope.Resolve<Emotes>())
                // .AddCommand(scope.Resolve<Greet>())
                .AddCommand(scope.Resolve<Jimbox>())
                .AddCommand(scope.Resolve<Join>())
                .AddCommand(scope.Resolve<Leave>())
                .AddCommand(scope.Resolve<Log>())
                .AddCommand(scope.Resolve<MockStreamElements>())
                .AddCommand(scope.Resolve<ModCheck>())
                .AddCommand(scope.Resolve<Mode>())
                .AddCommand(scope.Resolve<Moya>())
                .AddCommand(scope.Resolve<Permutations>())
                .AddCommand(scope.Resolve<Pyramid>())
                .AddCommand(scope.Resolve<Refresh>())
                .AddCommand(scope.Resolve<Repeat>())
                .AddCommand(scope.Resolve<Reply>())
                .AddCommand(scope.Resolve<Say>())
                .AddCommand(scope.Resolve<Switch>())
                .AddCommand(scope.Resolve<TimeoutAlert>())
                .AddCommand(scope.Resolve<TrackJimbox>())
                .AddCommand(scope.Resolve<TrackPyramids>())
                .AddCommand(scope.Resolve<Translate>())
                .AddCommand(scope.Resolve<Wolfram>());

            scope
                .Resolve<Color>()
                .AddColorSelector(scope.Resolve<RandomColorSelector>())
                .AddColorSelector(scope.Resolve<PresetColorSelector>())
                .AddColorSelector(scope.Resolve<RainbowColorSelector>());


            scope
                .Resolve<IShop>()
                .AddItem(scope.Resolve<Recipe>())
                .AddItem(scope.Resolve<Storage>())
                .AddItem(scope.Resolve<QuestLocation>())
                .AddItem(scope.Resolve<Upgrade>())
                .AddItem(scope.Resolve<Worker>())
                .AddItem(scope.Resolve<Population>())
                .AddItem(scope.Resolve<Gear>())
                .AddItem(scope.Resolve<Mousetrap>());

            return credentials;
        }
    }
}
