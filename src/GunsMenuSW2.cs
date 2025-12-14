using Microsoft.Extensions.DependencyInjection;
using SwiftlyS2.Shared.Plugins;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Commands;
using SwiftlyS2.Shared.Menus;
using SwiftlyS2.Core.Menus.OptionsBase;
using Microsoft.Extensions.Configuration;
using SwiftlyS2.Shared.Players;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GunsMenuSW2;

[PluginMetadata(
    Id = "GunsMenuSW2",
    Version = "1.0.0",
    Name = "GunsMenu", 
    Author = "nicedayzhu",
    Description = "SwiftlyS2 Version of GunsMenu. Credit: https://github.com/asapverneri/CS2-Gunsmenu",
    Website = "https://github.com/nicedayzhu/GunsMenuSW2")]
public partial class GunsMenuSW2 : BasePlugin {
    public GunsMenuConfig Config { get; set; } = new();

    public GunsMenuSW2(ISwiftlyCore core) : base(core)
    {
    }

    public override void ConfigureSharedInterface(IInterfaceManager interfaceManager) {
    }

    public override void UseSharedInterface(IInterfaceManager interfaceManager) {
    }

    public override void Load(bool hotReload) {
        // Load Config
        Core.Configuration.InitializeJsonWithModel<GunsMenuConfig>("gunsmenu", "GunsMenu");
        var configSection = Core.Configuration.Manager.GetSection("GunsMenu");
        if (configSection.Exists())
        {
            Config = configSection.Get<GunsMenuConfig>() ?? new GunsMenuConfig();
        }

        // Register commands via attributes (recommended approach)
        Core.Registrator.Register(this);
    }

    public override void Unload() {
        // Commands registered via attributes are automatically unregistered
    }

    private bool CheckPermission(ICommandContext context)
    {
        if (string.IsNullOrEmpty(Config.FlagForCommands))
            return true;

        if (!context.IsSentByPlayer)
            return false;

        var player = context.Sender!;
        return Core.Permission.PlayerHasPermission(player.SteamID, Config.FlagForCommands);
    }

    [Command("guns", registerRaw: true)]
    [CommandAlias("menu", registerRaw: true)]
    private void OnGunsCommand(ICommandContext context)
    {
        if (!context.IsSentByPlayer)
        {
            context.Reply("This command can only be used by players!");
            return;
        }

        if (!CheckPermission(context))
        {
            context.Reply("You don't have permission to use this command.");
            return;
        }

        var player = context.Sender!;
        if (!player.IsValid) return;

        if (player.Pawn == null || !player.Pawn.IsValid || player.Controller == null || player.Controller.PawnIsAlive == false)
        {
            context.Reply("You must be alive to use this command.");
            return;
        }

        ShowMenu(player, "Guns Menu", Helpers.Weapons.Where(w => !Config.WeaponBlacklist.Contains(w.Key, StringComparer.OrdinalIgnoreCase)));
    }

    [Command("secondary", registerRaw: true)]
    private void OnSecondaryCommand(ICommandContext context)
    {
        if (!context.IsSentByPlayer)
        {
            context.Reply("This command can only be used by players!");
            return;
        }

        if (!CheckPermission(context))
        {
            context.Reply("You don't have permission to use this command.");
            return;
        }

        var player = context.Sender!;
        if (!player.IsValid) return;

        if (player.Pawn == null || !player.Pawn.IsValid || player.Controller == null || player.Controller.PawnIsAlive == false)
        {
            context.Reply("You must be alive to use this command.");
            return;
        }

        ShowMenu(player, "Secondary Weapons", Helpers.Weapons.Where(w => w.Value.Type == Helpers.WeaponType.Secondary && !Config.WeaponBlacklist.Contains(w.Key, StringComparer.OrdinalIgnoreCase)));
    }

    [Command("primary", registerRaw: true)]
    private void OnPrimaryCommand(ICommandContext context)
    {
        if (!context.IsSentByPlayer)
        {
            context.Reply("This command can only be used by players!");
            return;
        }

        if (!CheckPermission(context))
        {
            context.Reply("You don't have permission to use this command.");
            return;
        }

        var player = context.Sender!;
        if (!player.IsValid) return;

        if (player.Pawn == null || !player.Pawn.IsValid || player.Controller == null || player.Controller.PawnIsAlive == false)
        {
            context.Reply("You must be alive to use this command.");
            return;
        }

        ShowMenu(player, "Primary Weapons", Helpers.Weapons.Where(w => w.Value.Type == Helpers.WeaponType.Primary && !Config.WeaponBlacklist.Contains(w.Key, StringComparer.OrdinalIgnoreCase)));
    }

    private void ShowMenu(IPlayer player, string title, IEnumerable<KeyValuePair<string, Helpers.Weapon>> weapons)
    {
        if (player == null || !player.IsValid)
            return;

        if (Core.MenusAPI == null)
        {
            player.SendChat("Menu system is not available.");
            return;
        }

        // Close any existing menu for this player first
        var existingMenu = Core.MenusAPI.GetCurrentMenu(player);
        if (existingMenu != null)
        {
            Core.MenusAPI.CloseMenuForPlayer(player, existingMenu);
        }

        // Create menu using fluent builder API (recommended approach)
        var builder = Core.MenusAPI.CreateBuilder();
        
        // Configure menu using fluent API
        builder.Design.SetMenuTitle(title);
        builder.Design.SetMaxVisibleItems(5);

        // Add weapon options
        foreach (var weapon in weapons)
        {
            // Capture variables in loop scope to avoid closure issues
            string displayName = weapon.Key;
            string weaponKey = weapon.Key;
            var weaponGiveName = weapon.Value.GiveName;

            var option = new ButtonMenuOption(displayName);
            option.Click += async (sender, args) =>
            {
                if (args.Player == null || !args.Player.IsValid)
                    return;

                // Capture player reference and weapon info for use in NextTick callback
                var targetPlayer = args.Player;
                var weaponType = weapon.Value.Type;
                var weaponSlot = weapon.Value.Slot;
                
                // Determine weapon slot (Primary = GEAR_SLOT_RIFLE, Secondary = GEAR_SLOT_PISTOL)
                var slot = weaponType == Helpers.WeaponType.Primary 
                    ? SwiftlyS2.Shared.SchemaDefinitions.gear_slot_t.GEAR_SLOT_RIFLE 
                    : SwiftlyS2.Shared.SchemaDefinitions.gear_slot_t.GEAR_SLOT_PISTOL;

                // Schedule weapon operations on main thread (GiveItem must be called from main thread)
                Core.Scheduler.NextTick(() =>
                {
                    if (targetPlayer == null || !targetPlayer.IsValid)
                        return;

                    var pawn = targetPlayer.Pawn;
                    if (pawn == null || pawn.WeaponServices == null || pawn.ItemServices == null)
                        return;

                    // Remove existing weapon in the same slot before giving new one
                    try
                    {
                        pawn.WeaponServices.RemoveWeaponBySlot(slot);
                    }
                    catch
                    {
                        // If slot removal fails, continue anyway
                    }

                    // Give new weapon
                    pawn.ItemServices.GiveItem(weaponGiveName);
                    targetPlayer.SendChat($"You selected {displayName}.");
                    
                    // Switch to the weapon after a short delay to ensure weapon is fully created
                    // Use Delay instead of NextTick to avoid double refresh
                    Core.Scheduler.Delay(1, () =>
                    {
                        if (targetPlayer == null || !targetPlayer.IsValid)
                            return;

                        var pawnForSwitch = targetPlayer.Pawn;
                        if (pawnForSwitch == null || pawnForSwitch.WeaponServices == null)
                            return;

                        try
                        {
                            // Select weapon by slot to immediately switch to it
                            pawnForSwitch.WeaponServices.SelectWeaponBySlot(slot);
                        }
                        catch
                        {
                            // If slot selection fails, try selecting by designer name as fallback
                            try
                            {
                                pawnForSwitch.WeaponServices.SelectWeaponByDesignerName(weaponGiveName);
                            }
                            catch
                            {
                                // If both methods fail, continue anyway
                            }
                        }
                    });
                });
                
                await Task.CompletedTask;
            };
            builder.AddOption(option);
        }

        // Build and open menu
        var menu = builder.Build();
        Core.MenusAPI.OpenMenuForPlayer(player, menu);
    }
}