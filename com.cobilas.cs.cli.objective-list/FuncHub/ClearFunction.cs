using System;
using System.IO;
using Cobilas.CLI.Manager;

namespace Cobilas.CLI.ObjectiveList.FuncHub;
[CallMethod(nameof(ClearFunction.Start))]
internal static class ClearFunction {
	private static readonly CLIKey iAlias = "--clear/-c";
	private static readonly CLIKey arg131 = "{131}arg";

	internal static void Start() {
		GlobalFunctionHub.EventGenericFunction += Run;
		GlobalFunctionHub.EventDefaultValue += DefaultValue;
		GlobalFunctionHub.EventTreatedValue += TreatedValue;
	}

	//{131}arg file path

	private static void Run(CLIKey alias, CLIValueOrder? value) {
		ExceptionMessages.ThrowIfNull(value, nameof(value));

		if (iAlias != alias) return;

		string filePath = FunctionHubUtility.GetFile(value[arg131]);

		using FileStream file = File.Open(filePath, FileMode.Open);
		file.SetLength(0L);
		FunctionHubUtility.WriteStartupFile(file);

		Printer.EnableNewLine = false;
		Printer.Print("The file '");
		Printer.PrintWarning(filePath);
		Printer.EnableNewLine = true;
		Printer.Print("' was successfully cleaned!");
	}

	private static void DefaultValue(CLIKey alias, CLIValueOrder? value) {
		ExceptionMessages.ThrowIfNull(value, nameof(value));
		string funcValue = value[GlobalFunctionHub.CLOVOFuncKey]!;

		if (iAlias != (CLIKey)funcValue) return;

		if (alias == arg131)
			value.Add(arg131, Path.Combine(Environment.CurrentDirectory, InitFunction.DefaultFileName));
	}

	private static void TreatedValue(CLIKey alias, CLIValueOrder? value, TokenList? list) {
		ExceptionMessages.ThrowIfNull(list, nameof(list));
		ExceptionMessages.ThrowIfNull(value, nameof(value));
		string funcValue = value[GlobalFunctionHub.CLOVOFuncKey]!;

		if (iAlias != (CLIKey)funcValue) return;

		if (alias == arg131)
			value.Add(arg131, list.CurrentKey);
	}
}
