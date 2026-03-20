using System;
using System.IO;
using Cobilas.CLI.Manager;

namespace Cobilas.CLI.ObjectiveList.FuncHub;

internal static class RenameFunctions {
	private static CLIKey iAlias;

	internal static void Start() {
		iAlias = "--rename/-r";
		GlobalFunctionHub.EventGenericFunction += RanameRun;
		GlobalFunctionHub.EventTreatedValue += RanameTreatedValue;
		GlobalFunctionHub.EventDefaultValue += RanameDefaultValue;
	}
	/*
	 {105}arg = old name
	 {106}arg = new name
	 {107}arg = folder path
	 */
	private static void RanameRun(CLIKey alias, CLIValueOrder? value) {
		ExceptionMessages.ThrowIfNull(value, nameof(value));
		string funcValue = value[GlobalFunctionHub.CLOVOFuncKey]!;

		if (iAlias != (CLIKey)funcValue) return;

		string oldName = value["{105}arg"]!;
		string newName = value["{106}arg"]!;
		string folderPath = value["{107}arg"]!;

		if (!Directory.Exists(folderPath))
			throw new DirectoryNotFoundException($"Directory '{folderPath}' not found!!!");

		oldName = Path.Combine(folderPath, oldName);

		if (!File.Exists(oldName))
			throw new FileNotFoundException($"File '{oldName}' not found!!!");

		File.Move(oldName, Path.Combine(folderPath, newName));
	}

	private static void RanameDefaultValue(CLIKey alias, CLIValueOrder? value) {
		ExceptionMessages.ThrowIfNull(value, nameof(value));
		string funcValue = value[GlobalFunctionHub.CLOVOFuncKey]!;

		if (iAlias != (CLIKey)funcValue) return;

		if (alias == (CLIKey)"{107}arg")
			value.Add("{107}arg", Environment.CurrentDirectory);
	}

	private static void RanameTreatedValue(CLIKey alias, CLIValueOrder? value, TokenList? list) {
		ExceptionMessages.ThrowIfNull(list, nameof(list));
		ExceptionMessages.ThrowIfNull(value, nameof(value));
		string funcValue = value[GlobalFunctionHub.CLOVOFuncKey]!;

		if (iAlias != (CLIKey)funcValue) return;

		if (alias == (CLIKey)"{105}arg")
			value.Add("{105}arg", list.CurrentKey);
		else if (alias == (CLIKey)"{106}arg")
			value.Add("{106}arg", list.CurrentKey);
		else if (alias == (CLIKey)"{107}arg")
			value.Add("{107}arg", list.CurrentKey);
	}
}
