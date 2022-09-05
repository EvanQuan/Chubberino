using System;
using System.Collections.Generic;
using Chubberino.Common.ValueObjects;

namespace Chubberino.Infrastructure.Commands.Settings;

public sealed class SettingCollection<TElement>
    where TElement : ISetting
{
    public IReadOnlyCollection<TElement> Enabled => EnabledEntries.Values;

    public IReadOnlyCollection<TElement> Disabled => DisabledEntries.Values;

    private Dictionary<Name, TElement> EnabledEntries { get; }

    private Dictionary<Name, TElement> DisabledEntries { get; }

    public SettingCollection()
    {
        EnabledEntries = new();
        DisabledEntries = new();
    }

    public SettingCollection<TElement> AddEnabled(TElement command)
    {
        command.IsEnabled = true;
        EnabledEntries.TryAdd(command.Name, command);

        return this;
    }

    public SettingCollection<TElement> AddDisabled(TElement command)
    {
        command.IsEnabled = false;
        DisabledEntries.TryAdd(command.Name, command);

        return this;
    }

    public void Enable(Name settingName)
    {
        if (DisabledEntries.Remove(settingName, out var setting))
        {
            setting.IsEnabled = true;
            EnabledEntries.TryAdd(setting.Name, setting);
        }
    }

    public void Disable(Name settingName)
    {
        if (EnabledEntries.Remove(settingName, out var setting))
        {
            setting.IsEnabled = false;
            DisabledEntries.TryAdd(setting.Name, setting);
        }
    }

    public TElement GetValueOrDefault(Name name)
    {
        return EnabledEntries.GetValueOrDefault(name)
            ?? DisabledEntries.GetValueOrDefault(name);
    }
}
