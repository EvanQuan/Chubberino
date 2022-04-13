using System;
using Chubberino.Common.ValueObjects;
using TwitchLib.Client.Models;

namespace Chubberino.Infrastructure.Credentials;

public sealed record class LoginCredentials(ConnectionCredentials ConnectionCredentials, Boolean IsBot, Name PrimaryChannelName);
