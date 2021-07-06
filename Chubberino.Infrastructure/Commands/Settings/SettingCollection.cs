using System;
using System.Collections.Generic;

namespace Chubberino.Infrastructure.Commands.Settings
{
    public sealed class SettingCollection<TElement>
        where TElement : ISetting
    {
        public IReadOnlyCollection<TElement> Enabled => EnabledEntries.Values;

        public IReadOnlyCollection<TElement> Disabled => DisabledEntries.Values;

        private Dictionary<String, TElement> EnabledEntries { get; }

        private Dictionary<String, TElement> DisabledEntries { get; }

        public SettingCollection()
        {
            EnabledEntries = new();
            DisabledEntries = new();
        }

        public SettingCollection<TElement> AddEnabled(TElement command)
        {
            EnabledEntries.TryAdd(command.Name, command);

            return this;
        }

        public SettingCollection<TElement> AddDisabled(TElement command)
        {
            DisabledEntries.TryAdd(command.Name, command);

            return this;
        }

        public void Enable(String settingName)
        {
            if (DisabledEntries.Remove(settingName, out var setting))
            {
                setting.IsEnabled = true;
                EnabledEntries.TryAdd(setting.Name, setting);
            }
        }

        public void Disable(String settingName)
        {
            if (EnabledEntries.Remove(settingName, out var setting))
            {
                setting.IsEnabled = false;
                DisabledEntries.TryAdd(setting.Name, setting);
            }
        }

        public TElement GetValueOrDefault(String name)
        {
            return EnabledEntries.GetValueOrDefault(name)
                ?? DisabledEntries.GetValueOrDefault(name);
        }
    }
}
