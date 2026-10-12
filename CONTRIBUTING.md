# Contributing

Thanks for helping! / ¡Gracias por colaborar! Issues and pull requests are welcome in English or Spanish.

## Reporting bugs
Open an issue and include:
- Mod version and game version.
- What you expected vs. what happened.
- The relevant part of `BepInEx/LogOutput.log` (set `3. Debug → LogConsumption` to `true` for extra detail).
- Other mods installed, if the problem might be a conflict.

## Suggesting changes
Open an issue first for anything that changes balance or defaults (for example gear costs per machine), so we can discuss it before you write code.

## Development setup
See "Building from source" in the [README](README.md). Quick recap:
1. Install the .NET SDK, the game, and a mod manager profile with BepInExPack and GK2 Localization API.
2. Copy `paths.props.example` to `paths.props` and edit the paths. This file is git-ignored.
3. `dotnet build FreeConveyors -c Release`, then copy `FreeConveyors.dll` and `FreeConveyors.Localization.json` from `FreeConveyors/bin/Release/netstandard2.1/` to your profile's `BepInEx/plugins/FreeConveyors/`.
4. Launch the game and check `BepInEx/LogOutput.log`.

Never commit game DLLs, decompiled game code, or `paths.props`.

## How the mod works
`Plugin.cs` patches `ConveyorWgoData.GetQuality()` with Harmony. A negative value means the object consumes gears; power sources are positive. Belts are set to 0, machines (vanilla 0) get a configurable negative value. Power is recalculated by the game from these values.

## Pull requests
- Keep changes focused; one topic per PR.
- Follow the existing code style and comment density.
- Test in game: build/remove belts and machines and check the power counter.
- If you add or rename a setting, update **both** `en` and `es` in `FreeConveyors.Localization.json` (other languages fall back to `en`).
- Translations: copy the `en` section under the game's language code (`de`, `fr`, `pt-br`, `ru`, `pl`, `ja`, `zh_cn`, `ko`, `tr`), translate the values, and keep the keys unchanged.
- New machines: add the id to `KnownMachines` in `Plugin.cs` and `machine.<id>.name` / `.description` to the JSON. Unlisted machines still work: they appear in settings automatically with the default cost.
- Update `thunderstore/CHANGELOG.md`. Bump the version in `Plugin.cs` and `thunderstore/manifest.json` only for releases (they must match).

## License
By contributing you agree that your contributions are licensed under the [MIT License](LICENSE).
