<div align="center">
  <img src="https://pan.samyyc.dev/s/VYmMXE" />
</div>

# GunsMenuSW2

[English](README.md) | [中文](README_CN.md)

`GunsMenuSW2` 是 [CS2-Gunsmenu](https://github.com/asapverneri/CS2-Gunsmenu) 的 SwiftlyS2 移植版本。它允许玩家通过简单的菜单界面领取武器。

## ✨ 功能特性

- **武器菜单**：提供主武器和副武器的选择菜单。
- **权限控制**：支持通过 SwiftlyS2 的权限系统限制命令使用。
- **黑名单**：可在配置文件中禁用特定武器。
- **原生 API**：使用 SwiftlyS2 的原生菜单 API 和 `ItemServices`，无需依赖复杂的 hack。
- **自动给予**：选择武器后自动给予玩家，并处理槽位替换。

## 🛠️ 安装

1. 确保已安装 [SwiftlyS2](https://github.com/swiftly-solution/swiftlys2)。
2. 下载最新版本的 `GunsMenuSW2`。
3. 将 `GunsMenuSW2` 文件夹放入 `addons/swiftlys2/plugins` 目录中。
4. 重启服务器或加载插件。

## ⚙️ 配置

插件配置文件位于 `addons/swiftlys2/configs/plugins/gunsmenu.json`。首次运行插件后会自动生成。

```json
{
  "PermissionForCommands": "", // 命令所需的权限标志，留空为所有人可用
  "Blacklist": [ // 禁用的武器列表 (使用武器显示名称)
    "negev",
    "m249"
  ]
}
```

## 💻 命令

| 命令 | 描述 | 权限 |
| --- | --- | --- |
| `guns` 或 `menu` | 打开所有武器菜单 | 由 `PermissionForCommands` 控制 |
| `primary` | 打开主武器菜单 | 由 `PermissionForCommands` 控制 |
| `secondary` | 打开副武器菜单 | 由 `PermissionForCommands` 控制 |

## 🏗️ 构建

本项目使用 .NET 10.0 (或更高版本) 构建。

### 前置要求

在构建之前，你需要设置 SwiftlyS2 依赖。有两种方式：

**方式 1：使用 SwiftlyS2 源代码（推荐用于开发）**
1. 克隆 [SwiftlyS2 仓库](https://github.com/swiftly-solution/swiftlys2)。
2. 确保 `swiftlys2` 项目位于相对于本项目的 `../../swiftlys2` 路径（或者修改 `GunsMenuSW2.csproj` 中的引用路径）。
3. 项目文件已通过 `ProjectReference` 引用 SwiftlyS2 源代码。

**方式 2：使用 NuGet 包**
1. 在 `GunsMenuSW2.csproj` 中取消注释 `PackageReference` 行：
   ```xml
   <PackageReference Include="SwiftlyS2.CS2" Version="1.0.0" ExcludeAssets="runtime" PrivateAssets="all" />
   ```
2. 注释掉或删除 `ProjectReference` 行：
   ```xml
   <!-- <ProjectReference Include="..\..\swiftlys2\managed\managed.csproj" ExcludeAssets="runtime" PrivateAssets="all" /> -->
   ```

### 构建步骤

1. 克隆本仓库。
2. 使用上述方式之一设置 SwiftlyS2 依赖。
3. 运行 `dotnet build`。

## 🔗 推荐插件

- **[K4-AlwaysWeaponSkins-SwiftlyS2](https://github.com/K4ryuu/K4-AlwaysWeaponSkins-SwiftlyS2)** - 一个强大的武器皮肤插件，可以自动将玩家的库存武器皮肤应用到拾取的武器上。与 `GunsMenuSW2` 完美配合，让你通过菜单领取的武器也能显示你拥有的皮肤。支持地图生成的武器、掉落武器等多种场景，同时保留弹药状态。

## 👏 致谢

- **[asapverneri](https://github.com/asapverneri)** - 原版 [CS2-Gunsmenu](https://github.com/asapverneri/CS2-Gunsmenu) 作者。
- **SwiftlyS2 Team** - 强大的 CS2 服务器框架。

---
<div align="center">
  Made with ❤️ by nicedayzhu
</div>

