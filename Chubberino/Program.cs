using Autofac;
using Chubberino.Client;
using Chubberino.Client.Commands;
using Chubberino.Client.Commands.Pyramids;
using Chubberino.Client.Commands.Settings;
using Chubberino.Client.Commands.Settings.ColorSelectors;
using Chubberino.Client.Commands.Settings.Replies;
using Chubberino.Client.Commands.Settings.UserCommands;
using Chubberino.Client.Commands.Strategies;
using Chubberino.Client.Credentials;
using Chubberino.Client.Services;
using Chubberino.Client.Threading;
using Chubberino.Database.Contexts;
using Chubberino.Database.Models;
using Chubberino.Modules.CheeseGame;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Hazards;
using Chubberino.Modules.CheeseGame.Heists;
using Chubberino.Modules.CheeseGame.Items;
using Chubberino.Modules.CheeseGame.Points;
using Chubberino.Modules.CheeseGame.Quests;
using Chubberino.Modules.CheeseGame.Rankings;
using Chubberino.Modules.CheeseGame.Shops;
using Chubberino.Modules.CheeseGame.Upgrades;
using Jering.Javascript.NodeJS;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using TwitchLib.Api;
using TwitchLib.Api.Interfaces;
using TwitchLib.Client.Enums;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Interfaces;
using WolframAlphaNet;

namespace Chubberino
{
    public class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        public static void Main()
        {
            var services = new ServiceCollection();

            // services.AddTransient<ISiteInterface, SiteRepo>

            var builder = new ContainerBuilder();
            builder.RegisterType<Bot>().As<IBot>().SingleInstance();
            builder.RegisterType<Client.Console>().As<IConsole>().SingleInstance();
            builder.RegisterType<TwitchClientManager>().As<ITwitchClientManager>().SingleInstance();
            builder.RegisterType<CrendentialsManager>().As<ICredentialsManager>().SingleInstance();
            builder.RegisterType<CommandRepository>().As<ICommandRepository>().SingleInstance();
            builder.Register(c => new TwitchClientFactory(
                (IClientOptions options) => new WebSocketClient(options),
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
            builder.RegisterType<SpinWait>().As<ISpinWait>().SingleInstance();
            builder.RegisterType<ModeratorClientOptions>().As<IModeratorClientOptions>().SingleInstance();
            builder.RegisterType<RegularClientOptions>().As<IRegularClientOptions>().SingleInstance();
            builder.RegisterType<PyramidBuilder>().AsSelf().SingleInstance();

            builder.Register(c =>
            {
                var api = new TwitchAPI();

                var applicationCredentials = c.Resolve<ApplicationCredentials>();

                api.Settings.ClientId = applicationCredentials.TwitchAPIClientID;
                // Doesn't matter which user credentials access token is used here.
                api.Settings.AccessToken = c.Resolve<IApplicationContext>().UserCredentials.First().AccessToken;

                return api;
            }).As<ITwitchAPI>().SingleInstance();

            builder.Register(c => new WolframAlpha(c.Resolve<ApplicationCredentials>().WolframAlphaAppID)).AsSelf().SingleInstance();

            builder.Register(c => c.Resolve<IApplicationContext>().ApplicationCredentials.First()).AsSelf().SingleInstance();

            // Commands
            builder.RegisterType<AtAll>().AsSelf().SingleInstance();
            builder.RegisterType<AutoChat>().AsSelf().SingleInstance();
            builder.RegisterType<AutoPogO>().AsSelf().SingleInstance();
            builder.RegisterType<Channel>().AsSelf().SingleInstance();
            builder.RegisterType<Cheese>().AsSelf().SingleInstance();
            builder.RegisterType<Color>().AsSelf().SingleInstance();
            builder.RegisterType<Cookie>().AsSelf().SingleInstance();
            builder.RegisterType<Copy>().AsSelf().SingleInstance();
            builder.RegisterType<Count>().AsSelf().SingleInstance();
            builder.RegisterType<DisableAll>().AsSelf().SingleInstance();
            builder.RegisterType<Emotes>().AsSelf().SingleInstance();
            builder.RegisterType<Greet>().AsSelf().SingleInstance();
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
            builder.RegisterType<PyramidBuild>().AsSelf().SingleInstance();
            builder.RegisterType<Refresh>().AsSelf().SingleInstance();
            builder.RegisterType<Repeat>().AsSelf().SingleInstance();
            builder.RegisterType<Reply>().AsSelf().SingleInstance();
            builder.RegisterType<Say>().AsSelf().SingleInstance();
            builder.RegisterType<Switch>().AsSelf().SingleInstance();
            builder.RegisterType<TimeoutAlert>().AsSelf().SingleInstance();
            builder.RegisterType<TrackJimbox>().AsSelf().SingleInstance();
            builder.RegisterType<TrackPyramids>().AsSelf().SingleInstance();
            builder.RegisterType<Translate>().AsSelf().SingleInstance();
            builder.RegisterType<YepKyle>().AsSelf().SingleInstance();
            builder.RegisterType<Wolfram>().AsSelf().SingleInstance();

            // Cheese game database context
            builder.RegisterType<ApplicationContext>().As<IApplicationContext>().InstancePerLifetimeScope();

            builder.RegisterType<Shop>().As<IShop>().SingleInstance();
            builder.RegisterType<PointManager>().As<IPointManager>().SingleInstance();
            builder.RegisterType<RankManager>().As<IRankManager>().SingleInstance();
            builder.RegisterType<QuestManager>().As<IQuestManager>().SingleInstance();
            builder.RegisterType<HeistManager>().As<IHeistManager>().SingleInstance();
            builder.RegisterType<EmoteManager>().As<IEmoteManager>().SingleInstance();
            builder.RegisterType<UpgradeManager>().As<IUpgradeManager>().SingleInstance();
            builder.RegisterType<CheeseRepository>().As<IRepository<CheeseType>>().SingleInstance();
            builder.RegisterType<CheeseModifierManager>().As<ICheeseModifierManager>().SingleInstance();
            builder.RegisterType<HazardManager>().As<IHazardManager>().SingleInstance();
            builder.RegisterType<ItemManager>().As<IItemManager>().SingleInstance();

            // Quests
            builder.RegisterType<QuestRepository>().As<IRepository<Modules.CheeseGame.Quests.Quest>>().SingleInstance();

            IContainer container = builder.Build();

            using ILifetimeScope scope = container.BeginLifetimeScope();

            var console = scope.Resolve<IConsole>();

            var bot = scope.Resolve<IBot>();

            var twitchClientManager = scope.Resolve<ITwitchClientManager>();

            if (!twitchClientManager.TryInitialize(bot))
            {
                console.WriteLine("Failed to start");
                return;
            }

            var commandRepository = scope.Resolve<ICommandRepository>();
            commandRepository
                .AddCommand(scope.Resolve<AtAll>())
                .AddCommand(scope.Resolve<AutoChat>())
                .AddCommand(scope.Resolve<AutoPogO>())
                .AddCommand(scope.Resolve<Channel>())
                .AddCommand(scope.Resolve<Cheese>())
                .AddCommand(scope.Resolve<Color>())
                .AddCommand(scope.Resolve<Cookie>())
                .AddCommand(scope.Resolve<Copy>())
                .AddCommand(scope.Resolve<Count>())
                .AddCommand(scope.Resolve<DisableAll>())
                .AddCommand(scope.Resolve<Emotes>())
                .AddCommand(scope.Resolve<Greet>())
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
                .AddCommand(scope.Resolve<PyramidBuild>())
                .AddCommand(scope.Resolve<Refresh>())
                .AddCommand(scope.Resolve<Repeat>())
                .AddCommand(scope.Resolve<Reply>())
                .AddCommand(scope.Resolve<Say>())
                .AddCommand(scope.Resolve<Switch>())
                .AddCommand(scope.Resolve<TimeoutAlert>())
                .AddCommand(scope.Resolve<TrackJimbox>())
                .AddCommand(scope.Resolve<TrackPyramids>())
                .AddCommand(scope.Resolve<Translate>())
                .AddCommand(scope.Resolve<Wolfram>())
                .AddCommand(scope.Resolve<YepKyle>());

            var color = scope.Resolve<Color>();

            color.AddColorSelector(scope.Resolve<RandomColorSelector>());
            color.AddColorSelector(scope.Resolve<PresetColorSelector>());
            color.AddColorSelector(scope.Resolve<RainbowColorSelector>());

            Boolean shouldContinue = twitchClientManager.TryJoinInitialChannels();

            while (shouldContinue)
            {
                console.Write(bot.GetPrompt());
                bot.ReadCommand(console.ReadLine());

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
        }
    }
}
