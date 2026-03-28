# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [3.0.0-ch.10] — (28/03/2026)
**Commit:** `cfd541d` — Author: BelicusBr

### Added
- Full `README.md` documentation for `com.cobilas.cs.cli.objective-list`, covering all CLI commands and their options:
  - `--version/-v` — Displays the current application version.
  - `--help/-h` — Lists all available program commands.
  - `init/-i` — Initializes a `.xml` task storage file at a given path.
  - `--rename/-r` — Renames the `.tskl` task file.
  - `--clear/-c` — Clears the task list.
  - `--show/-s --item/--i` — Displays a specific task item by tree path.
  - `--show/-s --list/-l` — Displays all task list items, with optional status filtering (`true`, `false`).
  - `--element/-e add` — Adds a new task (supports `--title/-t`, `--path/-p`, `--description/-d`).
  - `--element/-e remove` — Removes a task by tree path.
  - `set --replacetile/-rt` — Replaces the title of a specific task.
  - `set --replace/-rp` — Fully replaces a task (title, description, status).
  - `set --move/-m` — Moves a task to a different position in the tree.

---

## [3.0.0-ch.9] — (26/03/2026)
**Commit:** `28508ff` — Author: BelicusBr

### Added
- `HelpFunctions.cs` — New centralized help system with per-function help output (`VersionHelp`, `RenameHelp`, `InitHelp`, `ShowHelp`, `ClearHelp`, `ElementHelp`, `SetHelp`).
- `VersionFunction.cs` — Dedicated class for the `--version/-v` function, replacing the old inline handler in `UniFunctions.cs`.
- `opc_help` (`-help/--h/--?`) option support added to: `ClearFunction`, `ElementFunction`, `InitFunction`, `RenameFunction`, `SetFunction`, `ShowFunction`.
- `ValidateTaskPath(string)` — New utility method in `FunctionHubUtility` that validates task path format (numbers separated by `.`).
- `ValidateTaskStatus(string, bool)` — New utility method that validates task status values (`true`, `false`, `all`).
- Input validation for task paths in `AddElement`, `RemoveElement`, `ReplaceOption`, and `MoveOption`, printing descriptive errors on invalid input.
- Input validation for task status in `ShowList` and `ReplaceOption`.
- `GetTaskList` now accepts an optional `reorderList` parameter; `ShowList` uses it directly via `FunctionHubUtility`.
- `--tds` test function commented out from `Program.cs`.

### Changed
- `ClearFunction`: Refactored execution flow into a `switch` statement to support the new `opc_help` option.
- `ElementFunction`: Refactored to support `opc_help` and added path validation before element operations.
- `InitFunction`: Refactored with `switch` to support `opc_help`.
- `RenameFunction`: Refactored with `switch` to support `opc_help`; now ensures `.tskl` extension is appended to the new filename if missing.
- `SetFunction`: Refactored with `opc_help` support and added path/status validation before mutations.
- `ShowFunction`: Refactored to use `switch` on `arg120`; removed stale `Console.WriteLine` debug call; now delegates XML reading to `FunctionHubUtility.GetTaskList`.

### Removed
- `UniFunctions.cs` — Replaced by `VersionFunction.cs` and the new `HelpFunctions.cs`.

---

## [3.0.0-ch.8] — (26/03/2026)
**Commit:** `7bdaea3` — Author: BelicusBr

### Added
- `FunctionHubUtility.GetTaskList(string)` — Centralized XML task list reader using shared `XmlReaderSettings`.
- `FunctionHubUtility.SetTaskList(List<TaskListItem>, string)` — Centralized XML task list writer using shared `XmlWriterSettings`.
- `FunctionHubUtility.GetFile(string)` — Refactored with improved path validation (`IsInvalidPath`), explicit file/directory branching, and cleaner error handling.
- `IsInvalidPath(string)` — Private helper that checks for invalid path characters.
- Shared `XmlReaderSettings` and `XmlWriterSettings` static fields in `FunctionHubUtility`.

### Changed
- `ElementFunction`: Replaced all `TaskListFile.GetTaskList` and `TaskListFile.SetTaskList` calls with the new `FunctionHubUtility` equivalents.
- `CallMethodAttribute`: Changed `AllowMultiple` from `false` to `true` to support multiple attribute declarations per class.

---

## [3.0.0-ch.7] — (22/03/2026)
**Commit:** `0beaca6` — Author: BelicusBr

### Added
- `ElementFactory.StartTokens()` — Registers all CLI tokens (functions, options, end-options) with `CLIParse` at startup.
- `ElementFactory.CreateTDSTokens()` — Registers tokens for the experimental `--tds` function.
- `ElementFactory.CreateTokens(long, params string[])` — Private helper to batch-register tokens by splitting aliases on `/`.
- `CallMethodAttribute.cs` — New internal attribute targeting classes, storing a target method name for reflection-based initialization.
- `ClearFunction.cs` — New function handler for `--clear/-c`, supporting file clearing and console feedback.
- `ElementFunction.cs` — New function handler for `--element/-e`, supporting `add` and `remove` sub-commands.
- `FunctionHubUtility.cs` (initial version) — Utility class with `GetFile`, `WriteStartupFile`, and `PrintTaskItem` helpers.
- `GlobalFunctionHub.cs` — Event-driven hub wiring `DefaultValue`, `TreatedValue`, and `GenericFunction` delegates.

### Changed
- `ElementFactory.cs`: Added `using Cobilas.CLI.Manager` import required by `StartTokens`.
- `TaskListArgument`: Visibility changed from `public` to `internal`.

---

## [3.0.0-ch.6] — (21/03/2026)
**Commit:** `c1ab2cb` — Author: BelicusBr

### Added
- `ShowFunction.cs` — Initial implementation of the `--show/-s` command supporting `--item/--i` (show single task by path) and `--list/-l` (show all tasks filtered by status).
- Proper XML startup file generation in `InitFunction`: now writes a valid `<tasks>` root element with `XmlWriter` instead of creating an empty file.
- Existence check in `InitFunction` — skips creation and notifies user if the `.tskl` file already exists.

### Changed
- `RenameFunctions.cs`: Added console feedback after a successful file rename operation.

---

## [3.0.0-ch.5] — (20/03/2026)
**Commit:** `4a6f325` — Author: BelicusBr

### Added
- `ElementFactory.cs` — Static factory for creating `IFunction`, `IArgument`, and `IOptionFunc` instances (`CreatFunction`, `CreatArgument`, `CreateOption`, `CreateOptionEnd`, `CreateOptionJump`).
- `ITypeCode` interface implemented by `TaskListArgument`.

### Changed
- `TaskListArgument`: Refactored to use `GlobalFunctionHub` for `DefaultValue`, `TreatedValue`, `AnalyzerArguments`, and `InvalidArgument` instead of raw `CLIParse.GetFunction` delegates.
- `TaskListArgument.IsAlias`: Now handles empty string gracefully without throwing.
- `TaskListArgument.TreatedValue`: Simplified to a single expression delegate call.
- `TaskListArgument.GetAlias`: Alias now uses the `{ARG}/alias` format for internal keying.
- Visibility of `TaskListArgument` and `TaskListFunction` changed from `public` to `internal`.

---

## [3.0.0-ch.4] — (16/03/2026)
**Commit:** `172af90` — Author: BelicusBr

### Changed
- `TaskListArgument.Analyzer`: Fixed nullable type on `analyzer_arg` (`Func<string?, bool>`); added `TaskDebug.Print` tracing calls.
- `TaskListArgument.DefaultValue`: Updated delegate signature to `Action<CLIKey, CLIValueOrder?>` to pass the alias explicitly.
- `TaskListArgument.TreatedValue`: Refactored to move the list cursor before invoking the delegate, using `Action<CLIKey, CLIValueOrder?, TokenList?>`.
- `TaskListArgument.ExceptionMessage`: Corrected nullable annotation on the error message delegate.
- `TaskListArgument.IsAlias`: Fixed comparison to use `CLIKey` cast.
- `TaskListFunction.Analyzer`: Refactored iteration logic — `list.Move()` now occurs per-option inside the loop; added `TaskDebug.Print` tracing; updated error codes (`74→743`, `75→753`); fixed option type detection to use `HasFlag`.
- `TaskListFunction.TreatedValue`: Corrected order of `list.Move()` and `of.TreatedValue()` calls.

---

## [3.0.0-ch.3] — (09/03/2026)
**Commit:** `469f214` — Author: BelicusBr

### Added
- `ITypeCode` interface implemented by `TaskListArgument` (`IsTypeCode(long)`, `IsTypeCode(TaskListTokens)`).
- `TaskListArgument.GetAlias(string?)`: Private static helper with null/empty validation and `{ARG}/alias` aliasing.

### Changed
- `TaskListArgument`: Constructor alias initialization replaced with `GetAlias` call.
- `TaskListFunction`: Added `ITypeCode` interface; refactored to a primary constructor pattern storing alias, options, and a local `CLIValueOrder`.

---

## [3.0.0-ch.2] — (05/03/2026)
**Commit:** `63523b5` — Author: BelicusBr

### Added
- `Elements/ErrorMessageList.cs` — Extension method `SetMessage` for `ErrorMessage` to set code and message in one call.
- `Elements/TaskListArgument.cs` — Initial `IArgument` implementation with `Analyzer`, `DefaultValue`, `TreatedValue`, `ExceptionMessage`, and `IsAlias`.
- `Elements/TaskListFunction.cs` — Initial stub `IFunction` implementation.
- `Elements/TaskListOption.cs` — Initial `IOptionFunc` implementation.
- `Interfaces/ITypeCode.cs` — New interface defining `IsTypeCode` contract.

### Changed
- `CLIBase.cs` and `ElementFunc.cs`: Both files wrapped in `#if false` / `#endif` to disable legacy code without deletion.

---

## [3.0.0-ch.1] — (01/03/2026)
**Commit:** `d86975a` — Author: BelicusBr

### Changed
- `Program.cs`: Migrated from legacy namespace/class style to file-scoped namespace; bumped version constant from `2.4.1` to `3.0.0`; added detailed CLI command map as inline comments.
- `com.cobilas.cs.cli.objective-list.csproj`: Updated `<Version>` from `2.4.1` to `3.0.0`; upgraded package references (`Cobilas.CLI.Manager` `1.3.0→2.0.1`, `Cobilas.Core.Net4x` `1.3.1→2.9.0`).

---

## [3.0.0-rc.2] — (28/02/2026)
**Commit:** `f8fa45a` — Author: BelicusBr

### Changed
- Replaced legacy `com.cobilas.cs.cli.objective-list.sln` (Visual Studio solution format) with the new `com.cobilas.cs.cli.objective-list.slnx` (lightweight XML solution format).

---

## [3.0.0-rc.1] — (28/02/2026)
**Commit:** `fc97509` — Author: BelicusBr

### Added
- `CHANGELOG.md` file created (empty placeholder).

---

## [2.4.1] — (02/02/2024)
**Commit:** `92cf167` — Author: BelicusBr

### Changed
- `ElementContainer.cs`: Added `using System.Linq` import.

---

## [2.4.0] — (31/01/2024)
**Commit:** `2371c78` — Author: BelicusBr

### Changed
- Internal refactoring and updates across the `2.x` branch (pre-3.0 legacy codebase).

---

## [2.3.0] — (22/08/2023)
**Commit:** `d74eb12` — Author: BelicusBr

### Changed
- Version update targeting the `2.3.0` milestone.

---

## [2.1.8-a1] — (22/08/2023)
**Commit:** `deaaa8f` — Author: BelicusBr

### Changed
- Alpha patch update for version `2.1.8`.

---

## [2.1.8] — (29/04/2023)
**Commit:** `d52e277` — Author: BelicusBr

### Changed
- Release update for version `2.1.8`.

---

## [1.11.1.rc.1] — (28/02/2023)
**Commit:** `2ac2f31` — Author: BelicusBr

### Changed
- Release candidate patch for version `1.11.1`.

---

## [1.11.1] — (27/02/2023)
**Commit:** `b58bcd9` — Author: BelicusBr

### Changed
- Release update for version `1.11.1`.

---

## [1.10.0] — (26/02/2023)
**Commit:** `e65ffab` — Author: BelicusBr

### Changed
- Release update for version `1.10.0`.

---

## [1.9.1.ch.1] — (24/02/2023)
**Commit:** `85b2909` — Author: BelicusBr

### Changed
- Patch update for version `1.9.1`.

---

## [1.9.1] — (23/02/2023)
**Commit:** `0c020aa` — Author: BelicusBr

### Changed
- Release update for version `1.9.1`.

---

## [1.8.4.ch.1] — (21/02/2023)
**Commit:** `24b1310` — Author: BelicusBr

### Changed
- Patch update for version `1.8.4`.

---

## [1.8.4] — (21/02/2023)
**Commit:** `8d94a36` — Author: BelicusBr

### Changed
- Release update for version `1.8.4`.

---

## [1.7.0.ch.1] — (20/02/2023)
**Commit:** `2ee6038` — Author: BelicusBr

### Changed
- Patch update for version `1.7.0`.

---

## [1.7.0] — (20/02/2023)
**Commit:** `babb422` — Author: BelicusBr

### Changed
- Release update for version `1.7.0`.

---

## [1.6.0] — (19/02/2023)
**Commit:** `4b156f6` — Author: BelicusBr

### Changed
- Release update for version `1.6.0`.
