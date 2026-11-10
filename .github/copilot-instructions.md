# GitHub Copilot Instructions for HALO: Rimworld Psychic Pulse Armory (Continued) Mod

## Mod Overview and Purpose

The HALO: Rimworld Psychic Pulse Armory (Continued) mod is a reupload of the original Helixien and Kikohi mod that has been scrapped and integrated into a new version. The mod introduces 8 new and unique weapons based on Forerunner weapons from Halo 4 and Halo 5, which are designed to be non-lethal but powerful, applying a "Psychic Shock" condition to targets. The weapons work differently from other weapons, focusing on debuffing and capturing pawns rather than causing lethal damage.

## Key Features and Systems

### Psychic Pulse Weapons
- **Psychic Shock Condition**: Instead of dealing damage, these weapons apply a "Psychic Shock" condition that debuffs the target and can eventually down them.
- **Non-Lethal**: The weapons are designed to be non-lethal, making them suitable for capturing specific pawns in raids without causing accidental harm.

### Weapon Variations
- **Different Levels of Effectiveness**: Each weapon has a different level of effectiveness based on its type and quality.

### Integration
- **Ancient Temples**: Weapons can spawn in ancient temples, be rewarded through quests, or obtained from traders.
- **Asset Bundles**: The mod uses Asset Bundles to avoid doubling the mod size.

### Compatibility
- **Combat Extended**: A patch for Combat Extended is available but not provided by the mod author.

## Coding Patterns and Conventions

### C# Coding Patterns
- **CustomHediffWithComps.cs**: Implements a custom hediff with components that manages the "Psychic Shock" condition.
- **HRPPA_HediffDefOf.cs**: Defines hediff definitions for the mod.
- **Projectile_ExplosivePsychicPulse.cs**: Handles the explosive version of the psychic pulse projectile.
- **Projectile_PsychicPulse.cs**: Manages the standard psychic pulse projectile.
- **ThingDef_PsychicPulse.cs**: Defines the thing definitions for the psychic pulse weapons.

### XML Integration
- **DamageDefs**: Contains custom damage definitions for the mod.
- **HediffDefs**: Defines hediff definitions related to the "Psychic Shock" condition.
- **SoundDefs**: Includes sound definitions for the weapons.
- **ThingCategoryDefs**: Defines thing category definitions for the mod.
- **ThingDefs**: Contains definitions for the melee archotech weapons.

## Harmony Patching

The mod does not rely on Harmony patching for its core functionality. However, for compatibility with other mods like Combat Extended, Harmony patches may be applied by third-party patches.

## Suggestions for Copilot

### General Suggestions
- **Focus on Custom Hediffs and Conditions**: Copilot should prioritize generating code and XML definitions related to the "Psychic Shock" condition and custom hediffs.
- **Projectile Handling**: Copilot should assist in creating and managing projectile logic for the psychic pulse weapons.
- **ThingDef and DamageDef**: Copilot should help in defining and managing the thing and damage definitions for the new weapons.

### Specific Code Snippets
- **CustomHediffWithComps.cs**: Generating methods to manage the "Psychic Shock" condition, such as the `Tick` method.
- **Projectile_ExplosivePsychicPulse.cs**: Implementing the `Explode` method to handle the explosion logic.
- **Projectile_PsychicPulse.cs**: Implementing the `Impact` method to handle the impact logic.

### Specific XML Snippets
- **DamageDefs**: Creating custom damage definitions that align with the psychic pulse system.
- **HediffDefs**: Defining hediff definitions for the "Psychic Shock" condition.
- **SoundDefs**: Including sound definitions for the weapons.
- **ThingCategoryDefs**: Defining thing category definitions for the mod.
- **ThingDefs**: Creating thing definitions for the melee archotech weapons.

By following these guidelines, Copilot can effectively assist in the development and maintenance of the HALO: Rimworld Psychic Pulse Armory (Continued) mod.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

