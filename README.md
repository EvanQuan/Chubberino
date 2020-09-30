# Chubberino

## Setup

### Clone repository

![Clone](/Images/Clone.png)

### Copy TwitchInfo.resx file

Execute `move_resx.sh`

![FileExplorer](/Images/move_resx.sh.png)

### Open `Chubberino.sln` solution file

![FileExplorer](/Images/FileExplorer.png)

### Go to the Solution Explorer to Open TwitchInfo.resx

![SolutionExplorer](/Images/SolutionExplorer.png)

### Add Fields

![AddFields](/Images/AddFields.png)

`BotToken`

[Get the OAuth access token for the bot account.](https://dev.twitch.tv/docs/authentication/getting-tokens-oauth/#oauth-client-credentials-flow)

[The easy way to get it.](https://twitchtokengenerator.com/)

- Choose Bot Chat Token

`BotUsername`

- The Twitch username that is using the bot.

`ClientId`

[Register your application on the Twitch developer site.](https://dev.twitch.tv/docs/authentication#registration)

`InitialChannelName`

- The Twitch username whose channel to initially join.
