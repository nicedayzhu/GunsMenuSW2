<div align="center">
  <img src="https://pan.samyyc.dev/s/VYmMXE" />
</div>

# GunsMenuSW2

[English](README.md) | [中文](README_CN.md)

`GunsMenuSW2` is a port of [CS2-Gunsmenu](https://github.com/asapverneri/CS2-Gunsmenu) to SwiftlyS2. It allows players to claim weapons through a simple menu interface.

## ✨ Features

- **Weapon Menu**: Provides selection menus for primary and secondary weapons.
- **Permission Control**: Supports restricting command usage via the SwiftlyS2 permission system.
- **Blacklist**: Specific weapons can be disabled in the configuration file.
- **Native API**: Uses SwiftlyS2's native Menu API and `ItemServices`, without relying on complex hacks.
- **Auto Give**: Automatically gives the weapon to the player after selection and handles slot replacement.

## 🛠️ Installation

1. Ensure [SwiftlyS2](https://github.com/swiftly-solution/swiftlys2) is installed.
2. Download the latest version of `GunsMenuSW2`.
3. Place the `GunsMenuSW2` folder into the `addons/swiftlys2/plugins` directory.
4. Restart the server or load the plugin.

## ⚙️ Configuration

The plugin configuration file is located at `addons/swiftlys2/configs/plugins/gunsmenu.json`. It will be automatically generated after the first run.

```json
{
  "PermissionForCommands": "", // Permission flag required for commands, leave empty for everyone
  "Blacklist": [ // List of disabled weapons (use weapon display name)
    "negev",
    "m249"
  ]
}
```

## 💻 Commands

| Command | Description | Permission |
| --- | --- | --- |
| `guns` or `menu` | Opens the all weapons menu | Controlled by `PermissionForCommands` |
| `primary` | Opens the primary weapon menu | Controlled by `PermissionForCommands` |
| `secondary` | Opens the secondary weapon menu | Controlled by `PermissionForCommands` |

## 🏗️ Building

This project is built using .NET 10.0 (or higher).

### Prerequisites

Before building, you need to set up SwiftlyS2 dependencies. You have two options:

**Option 1: Using SwiftlyS2 Source Code (Recommended for Development)**
1. Clone the [SwiftlyS2 repository](https://github.com/swiftly-solution/swiftlys2).
2. Ensure the `swiftlys2` project is located at `../../swiftlys2` relative to this project (or modify the reference path in `GunsMenuSW2.csproj`).
3. The project file already references the SwiftlyS2 source code via `ProjectReference`.

**Option 2: Using NuGet Package**
1. Uncomment the `PackageReference` line in `GunsMenuSW2.csproj`:
   ```xml
   <PackageReference Include="SwiftlyS2.CS2" Version="1.0.0" ExcludeAssets="runtime" PrivateAssets="all" />
   ```
2. Comment out or remove the `ProjectReference` line:
   ```xml
   <!-- <ProjectReference Include="..\..\swiftlys2\managed\managed.csproj" ExcludeAssets="runtime" PrivateAssets="all" /> -->
   ```

### Build Steps

1. Clone this repository.
2. Set up SwiftlyS2 dependencies using one of the options above.
3. Run `dotnet build`.

## 🔗 Recommended Plugins

- **[K4-AlwaysWeaponSkins-SwiftlyS2](https://github.com/K4ryuu/K4-AlwaysWeaponSkins-SwiftlyS2)** - A powerful weapon skin plugin that automatically applies your inventory weapon skins to picked-up weapons. Works perfectly with `GunsMenuSW2`, allowing weapons obtained through the menu to display your owned skins. Supports map-spawned weapons, dropped weapons, and various other scenarios while preserving ammo state.

## 👏 Credits

- **[asapverneri](https://github.com/asapverneri)** - Author of the original [CS2-Gunsmenu](https://github.com/asapverneri/CS2-Gunsmenu).
- **SwiftlyS2 Team** - The powerful CS2 server framework.

---
<div align="center">
  Made with ❤️ by nicedayzhu
</div>
