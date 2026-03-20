using System;
using System.IO;
using Cobilas.CLI.Manager;

namespace Cobilas.CLI.ObjectiveList.FuncHub;

internal static class InitFunction {
	private static CLIKey iAlias;

	internal static void Start() {
		iAlias = "init/-i";
		GlobalFunctionHub.EventDefaultValue += InitDefaultValue;
		GlobalFunctionHub.EventTreatedValue += InitTreatedValue;
		GlobalFunctionHub.EventGenericFunction += InitRun;
	}

	//{111}arg folder path

	private static void InitRun(CLIKey alias, CLIValueOrder? value) {
		ExceptionMessages.ThrowIfNull(value, nameof(value));
		string funcValue = value[GlobalFunctionHub.CLOVOFuncKey]!;

		if (iAlias != (CLIKey)funcValue) return;

		string folderPath = value["{111}arg"]!;

		if (!Directory.Exists(folderPath))
			throw new DirectoryNotFoundException($"Directory '{folderPath}' not found!!!");

		folderPath = Path.Combine(folderPath, "init.tskl");

		File.CreateText(folderPath).Dispose();
	}

	private static void InitDefaultValue(CLIKey alias, CLIValueOrder? value) {
		ExceptionMessages.ThrowIfNull(value, nameof(value));
		string funcValue = value[GlobalFunctionHub.CLOVOFuncKey]!;

		if (iAlias != (CLIKey)funcValue) return;
		if (alias == (CLIKey)"{111}arg")
			value.Add("{111}arg", Environment.CurrentDirectory);
	}

	private static void InitTreatedValue(CLIKey alias, CLIValueOrder? value, TokenList? list) {
		ExceptionMessages.ThrowIfNull(list, nameof(list));
		ExceptionMessages.ThrowIfNull(value, nameof(value));
		string funcValue = value[GlobalFunctionHub.CLOVOFuncKey]!;

		if (iAlias != (CLIKey)funcValue) return;
		if (alias == (CLIKey)"{111}arg")
			value.Add("{111}arg", list.CurrentKey);
	}
}
