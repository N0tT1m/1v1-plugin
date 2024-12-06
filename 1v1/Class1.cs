using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Utils;

using Microsoft.Extensions.Logging;

namespace OneVOnePlugin
{
    public class OneVOnePlayer
    {
        public CCSPlayerController GetPlayer() => Player;
        
        public CCSPlayerController Player { get; init; }
        
        public OneVOnePlayer(CCSPlayerController player)
        {
            Player = player;
        }

        public void SetWeapon()
        {
            
        }
    }
    
    
    public class OneVOnePlugin : BasePlugin
    {
        public override string ModuleName => "1v1 Plugin";

        public override string ModuleVersion => "0.0.1";

        public override string ModuleAuthor => "n0tt1m";

        public override string ModuleDescription => "A 1v1 plugin";

        public override void Load(bool hotReload)
        {
            Logger.LogInformation("Plugin loaded successfully!");

            RegisterListener<Listeners.OnEntitySpawned>(entity =>
            {
                if (entity.DesignerName != "smokegrenade_projectile") return;

                var smoke = new CSmokeGrenadeProjectile(entity.Handle);

                // Changes smoke grenade colour to a random colour each time.
                Server.NextFrame(() =>
                {
                    smoke.SmokeColor.X = Random.Shared.NextSingle() * 255.0f;
                    smoke.SmokeColor.Y = Random.Shared.NextSingle() * 255.0f;
                    smoke.SmokeColor.Z = Random.Shared.NextSingle() * 255.0f;
                    Logger.LogInformation("Smoke grenade spawned with color {SmokeColor}", smoke.SmokeColor);
                });
            });
        }

        [GameEventHandler]
        public HookResult OnPlayerConnect(EventPlayerConnect @event, GameEventInfo info)
        {
            // Userid will give you a reference to a CCSPlayerController class
            Logger.LogInformation("Player {Name} has connected on {Map}!", @event.Userid.PlayerName, Server.MapName);

            return HookResult.Continue;
        }

        [GameEventHandler]
        public HookResult OnPlayerDisconnect(EventPlayerDisconnect @event, GameEventInfo info)
        {
            Logger.LogInformation("Player {Name} has disconnected!", @event.Userid.PlayerName);
        }

        [ConsoleCommand("css_issue_warning", "Issue warning to player")]
        public void OnCommand(CCSPlayerController? player, CommandInfo command)
        {
            Logger.LogWarning("Player shouldn't be doing that");
        }
    }
}
