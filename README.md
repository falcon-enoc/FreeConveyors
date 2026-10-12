# Free Conveyors

**English** · [Español](#español)

In vanilla Graveyard Keeper 2, every belt, splitter and station belt costs 1 gear, while machines cost nothing. Laying out a factory quickly becomes a power problem. This mod flips that: **belts are free, machines cost gears.**

## Defaults
| Object | Gear cost |
|---|---|
| Belts, underground belts, station belts, splitters | 0 (vanilla: 1) |
| Chests, output chests, pallets | vanilla (0) |
| Level 1 machines (`_t1`) | 3 each (vanilla: 0) |
| Level 2 machines, bioreactor, any other machine | 5 each (vanilla: 0) |

## Settings
Every belt type and every machine has its own setting. Install **Mod Settings Menu** to edit them in-game (Main menu / pause → Mod Settings); they are detected automatically. Without it, edit `BepInEx/config/enoc.gk2.freeconveyors.cfg`. Changes apply when the power is recalculated (build or remove a belt).

Texts follow the game language (English and Spanish included; other languages fall back to English).

## Requirements
- BepInExPack
- GK2 Localization API (installed automatically)
- Mod Settings Menu (optional, for in-game settings)

---

## Español

En el juego original cada cinta, divisor y cinta de estación cuesta 1 engranaje y las máquinas no cuestan nada, así que organizar una fábrica se vuelve un problema de energía. Este mod lo invierte: **las cintas son gratis y las máquinas consumen engranajes.**

### Valores por defecto
| Objeto | Costo en engranajes |
|---|---|
| Cintas, subterráneas, de estación, divisores | 0 (original: 1) |
| Cofres, cofres de salida, palets | original (0) |
| Máquinas de nivel 1 (`_t1`) | 3 c/u (original: 0) |
| Máquinas de nivel 2, biorreactor y cualquier otra | 5 c/u (original: 0) |

### Configuración
Cada tipo de cinta y cada máquina tiene su propia opción. Con **Mod Settings Menu** instalado se editan dentro del juego (Mod Settings); se detectan solas. Sin él, edita `BepInEx/config/enoc.gk2.freeconveyors.cfg`. Los cambios se aplican al recalcular la energía (construir o quitar una cinta).

Los textos siguen el idioma del juego (incluye inglés y español; los demás idiomas usan inglés).

---

## Building from source
Requires the [.NET SDK](https://dotnet.microsoft.com/download) (8.0 or newer), the game, and a mod manager profile with **BepInExPack** and **GK2 Localization API** installed (the build references their DLLs).

1. Copy `paths.props.example` to `paths.props` and set `GameDir` (and `ProfileDir` if your profile is not `Default`).
2. `dotnet build FreeConveyors -c Release`
3. Optional: `powershell -File thunderstore\build-package.ps1` creates `dist/FreeConveyors-<version>.zip` for Thunderstore.

No game files are included in this repository; the build references them from your own installation.

## Contributing
See [CONTRIBUTING.md](CONTRIBUTING.md).

## License
[MIT](LICENSE). This is an unofficial fan mod, not affiliated with or endorsed by Lazy Bear Games or tinyBuild.
