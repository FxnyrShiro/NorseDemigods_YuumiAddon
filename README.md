# YuumiAddon

[![Made with C#](https://img.shields.io/badge/Made%20with-C%23-purple)](#)
[![Platform](https://img.shields.io/badge/platform-BepInEx-green)](#)
[![Compatibility](https://img.shields.io/badge/Valheim-NorseDemigods-blue)](#)

A BepInEx plugin that integrates the Yuumi god from League of Legends into the [NorseDemigods](https://github.com/...) framework for Valheim.

## Features

- Adds Yuumi as a fully playable demigod with custom abilities.
- **Q (Prowling Projectile):** Custom projectile with homing logic, damage, and slow effects.
- **W (You and Me):** Attachment mechanic allowing Yuumi to anchor to an ally, inheriting their movement and providing buffs.
- **E (Zoomies):** Dynamic speed and shield buff for the anchored ally.
- **R (Final Chapter):** AoE ultimate with wave-based damage logic.
- Full integration with NorseDemigods stats and UI systems.

## Technical Overview

The plugin is built as a BepInEx mod and uses **Harmony** for runtime method patching to hook into Valheim's update loops and the NorseDemigods framework. 
- Abilities are implemented by injecting custom logic into `Update` / `FixedUpdate` cycles and handling input events.
- Yuumi's stats and behavior are inherited from the base Demigod classes, ensuring compatibility with the mod's progression systems.
- Jotunn is used as a dependency for asset loading and custom entity registration.

## Requirements

- BepInEx 5.4.2360+
- NorseDemigods 1.9.3+
- Jotunn 2.30.2+

## Installation

1. Install BepInEx for Valheim.
2. Install the NorseDemigods mod.
3. Install Jotunn.
4. Place `YuumiAddon.dll` in your `BepInEx/plugins` folder.
5. Place `Icons/yuumi_icon.png` in your `BepInEx/plugins/YuumiAddon` folder.

## Configuration

The mod utilizes the native NorseDemigods configuration system, allowing balance tweaks without recompilation. Configs can be found in:
- `BepInEx/config/NorseDemigods.cfg`
- `BepInEx/config/YuumiAddon.cfg`

## Compatibility

Designed to work with the latest version of NorseDemigods. Compatible with other mods that do not conflict with the NorseDemigods framework.

## Roadmap

- [ ] Add custom particle effects and sounds for abilities.
- [ ] Ensure visual synchronization of abilities in multiplayer.
- [ ] Balance tuning based on late-game testing.

## Support

For support and bug reports, please open an issue on the GitHub repository.
