using System.Collections.Generic;
using System.IO;
using Autofac;
using Chubberino.Bots.Channel.Commands;
using Chubberino.Bots.Channel.Modules.CheeseGame.Emotes;
using Chubberino.Bots.Channel.Modules.CheeseGame.Hazards;
using Chubberino.Bots.Channel.Modules.CheeseGame.Heists;
using Chubberino.Bots.Channel.Modules.CheeseGame.Helping;
using Chubberino.Bots.Channel.Modules.CheeseGame.Items;
using Chubberino.Bots.Channel.Modules.CheeseGame.Items.Recipes;
using Chubberino.Bots.Channel.Modules.CheeseGame.Items.Upgrades.RecipeModifiers;
using Chubberino.Bots.Channel.Modules.CheeseGame.Points;
using Chubberino.Bots.Channel.Modules.CheeseGame.Quests;
using Chubberino.Bots.Channel.Modules.CheeseGame.Ranks;
using Chubberino.Bots.Channel.Modules.CheeseGame.Shops;
using Chubberino.Bots.Common.Commands;
using Chubberino.Bots.Common.Commands.Settings;
using Chubberino.Bots.Common.Commands.Settings.ColorSelectors;
using Chubberino.Bots.Common.Commands.Settings.Replies;
using Chubberino.Bots.Common.Commands.Settings.Strategies;
using Chubberino.Bots.Common.Commands.Settings.UserCommands;
using Chubberino.Client.Commands.Settings.UserCommands;
using Chubberino.Common.Services;
using Chubberino.Database.Contexts;
using Chubberino.Infrastructure.Client;
using Chubberino.Infrastructure.Client.TwitchClients;
using Chubberino.Infrastructure.Commands;
using Chubberino.Infrastructure.Commands.Settings.UserCommands;
using Chubberino.Infrastructure.Configurations;
using Chubberino.Infrastructure.Credentials;
using Jering.Javascript.NodeJS;
using Microsoft.Extensions.DependencyInjection;
using TwitchLib.Api;
using TwitchLib.Api.Interfaces;
using TwitchLib.Client.Enums;
using TwitchLib.Communication.Clients;
using WolframAlphaNet;

using ChannelCommand = Chubberino.Bots.Common.Commands.Settings.Channel;
using RandomCommand = Chubberino.Bots.Common.Commands.Settings.UserCommands.Random;
using RandomSource = System.Random;

namespace Chubberino.Bots.Channel.IoC;

internal sealed class DependencyManager
{
    /// <summary>
    /// Set up Inversion of Control.
    /// </summary>
    /// <param name="credentials">Previous <see cref="LoginCredentials"/> if they exist; otherwise <see langword="null"/>.</param>
    /// <returns>The current <see cref="LoginCredentials"/>; <see langword="null"/> if failed to set up.</returns>
    public static (IBot Bot, LoginCredentials Credentials) SetupIoC(LoginCredentials credentials)
    {
        IContainer container = RegisterTypes();

        using ILifetimeScope scope = container.BeginLifetimeScope();

        var bot = scope.Resolve<IBot>();

        var twitchClientManager = scope.Resolve<ITwitchClientManager>();

        var finalCredentials = twitchClientManager.TryInitializeTwitchClient(bot, credentials: credentials);

        return finalCredentials
            .Some(credentials =>
            {

                if (!twitchClientManager.TryJoinInitialChannels())
                {
                    return default;
                }

                ResolveCommandDependencies(scope);

                return (bot, credentials);
            })
            .None(() => default);
    }

    private static void ResolveCommandDependencies(ILifetimeScope scope)
    {
        scope
            .Resolve<ICommandRepository>()
            .AddCommand(scope.Resolve<AtAll>())
            .AddCommand(scope.Resolve<AutoChat>())
            .AddCommand(scope.Resolve<AutoPogO>())
            .AddCommand(scope.Resolve<ChannelCommand>(), true)
            .AddCommand(scope.Resolve<Cheese>(), true)
            .AddCommand(scope.Resolve<Cheese2>(), true)
            .AddCommand(scope.Resolve<Color>())
            .AddCommand(scope.Resolve<Cookie>())
            .AddCommand(scope.Resolve<Copy>())
            .AddCommand(scope.Resolve<Count>())
            .AddCommand(scope.Resolve<DisableAll>())
            .AddCommand(scope.Resolve<Emotes>(), true)
            // .AddCommand(scope.Resolve<Greet>())
            .AddCommand(scope.Resolve<Jimbox>())
            //.AddCommand(scope.Resolve<Join>())
            //.AddCommand(scope.Resolve<Leave>())
            .AddCommand(scope.Resolve<RandomCommand>())
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


        var shop = scope.Resolve<IShop>();

        shop.Items = new IItem[]
        {
            scope.Resolve<Recipe>(),
            scope.Resolve<Storage>(),
            scope.Resolve<QuestLocation>(),
            scope.Resolve<Upgrade>(),
            scope.Resolve<Worker>(),
            scope.Resolve<Population>(),
            scope.Resolve<Gear>(),
            scope.Resolve<Mousetrap>()
        };
    }

    private static IContainer RegisterTypes()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<Bot>().As<IBot>().SingleInstance();
        builder.RegisterInstance(Console.Out).As<TextWriter>().SingleInstance();
        builder.RegisterInstance(Console.In).As<TextReader>().SingleInstance();
        builder.RegisterType<Config>().As<IConfig>().SingleInstance();
        builder.RegisterType<TwitchClientManager>().As<ITwitchClientManager>().SingleInstance();
        builder.RegisterType<CrendentialsManager>().As<ICredentialsManager>().SingleInstance();
        builder.RegisterType<UserCommandValidator>().As<IUserCommandValidator>().SingleInstance();
        builder.RegisterType<CommandRepository>().As<ICommandRepository>().SingleInstance();
        builder.RegisterType<CommandConfigurationStrategy>().As<ICommandConfigurationStrategy>().SingleInstance();
        builder.Register(c => new TwitchClientFactory(
            (options) => new WebSocketClient(options),
            ClientProtocol.WebSocket,
            null,
            c.Resolve<ICredentialsManager>()))
            .As<ITwitchClientFactory>().SingleInstance();

        builder.RegisterType<DateTimeService>().As<IDateTimeService>().SingleInstance();
        builder.RegisterType<ThreadService>().As<IThreadService>().SingleInstance();

        builder.RegisterType<StopSettingStrategy>().As<IStopSettingStrategy>().SingleInstance();
        builder.RegisterType<Repeater>().As<IRepeater>();
        builder.RegisterType<ContainsComparator>().As<IContainsComparator>().SingleInstance();
        builder.RegisterType<EqualsComparator>().As<IEqualsComparator>().SingleInstance();
        builder.RegisterType<RandomSource>().AsSelf().SingleInstance();
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

        RegisterCommands(builder);

        builder.RegisterType<ApplicationContextFactory>().As<IApplicationContextFactory>().SingleInstance();

        RegisterCheeseGame(builder);

        IContainer container = builder.Build();
        return container;
    }

    private static void RegisterCommands(ContainerBuilder builder)
    {
        // Commands
        builder.RegisterType<AtAll>().AsSelf().SingleInstance();
        builder.RegisterType<AutoChat>().AsSelf().SingleInstance();
        builder.RegisterType<AutoPogO>().AsSelf().SingleInstance();
        builder.RegisterType<ChannelCommand>().AsSelf().SingleInstance();
        builder.RegisterType<Cheese>().AsSelf().SingleInstance();
        builder.RegisterType<Cheese2>().AsSelf().SingleInstance();
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
        builder.RegisterType<RandomCommand>().AsSelf().SingleInstance();
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
    }

    private static void RegisterCheeseGame(ContainerBuilder builder)
    {
        // Cheese game
        builder.RegisterType<HelpManager>().As<IHelpManager>().SingleInstance();
        builder.RegisterType<Shop>().As<IShop>().SingleInstance();
        builder.RegisterType<PointManager>().As<IPointManager>().SingleInstance();
        builder.RegisterType<RankManager>().As<IRankManager>().SingleInstance();
        builder.RegisterType<QuestManager>().As<IQuestManager>().SingleInstance();
        builder.RegisterType<HeistManager>().As<IHeistManager>().SingleInstance();
        builder.RegisterType<EmoteManager>().As<IEmoteManager>().SingleInstance();
        builder.Register(x => RecipeRepository.Recipes).As<IReadOnlyList<RecipeInfo>>().SingleInstance();
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
    }
}
