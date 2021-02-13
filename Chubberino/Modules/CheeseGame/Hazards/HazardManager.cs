using Chubberino.Client.Abstractions;
using Chubberino.Modules.CheeseGame.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Models;
using System;

namespace Chubberino.Modules.CheeseGame.Hazards
{
    public sealed class HazardManager : AbstractCommandStrategy, IHazardManager
    {
        public HazardManager(ApplicationContext context, IMessageSpooler spooler, Random random, IEmoteManager emoteManager) : base(context, spooler, random, emoteManager)
        {
        }

        public Int32 GetMouseInfestationPointLoss(Int32 points)
        {
            return (Int32)(points * 0.8);
        }

        public Boolean ResolveStartMouseInfestation(Player player)
        {
            if (player.MouseTrapCount > 0)
            {
                player.MouseTrapCount--;
                Context.SaveChanges();
                return false;
            }
            return true;
        }
    }
}
