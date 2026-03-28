using System;
using Cobilas.CLI.Manager;

namespace Cobilas.CLI.ObjectiveList.FuncHub;
[CallMethod(nameof(VersionFunction.Start))]
internal static class VersionFunction {
	private static readonly CLIKey arg100 = "{100}arg";
	private static readonly CLIKey iAlias = "--version/-v";
	private static readonly CLIKey opc_help = "-help/--h/--?";

	internal static void Start() {
		GlobalFunctionHub.EventGenericFunction += Run;
		GlobalFunctionHub.EventDefaultValue += DefaultValue;
		GlobalFunctionHub.EventTreatedValue += TreatedValue;
	}

	private static void Run(CLIKey alias, CLIValueOrder? value) {
		ExceptionMessages.ThrowIfNull(value, nameof(value));

		if (iAlias != alias) return;

		switch (value[arg100]) {
			case nameof(opc_help):
				Printer.Print("Show the current version of the app!");
				HelpFunctions.VersionHelp();
				break;
			default:
				Printer.EnableNewLine = false;
				Printer.Print("Version: ");
				Printer.EnableNewLine = true;
				Printer.PrintWarning(Program.version);
				break;
		}
	}

	private static void DefaultValue(CLIKey alias, CLIValueOrder? value) {
		ExceptionMessages.ThrowIfNull(value, nameof(value));
		string funcValue = value[GlobalFunctionHub.CLOVOFuncKey]!;

		if (iAlias != (CLIKey)funcValue) return;

		if (alias == opc_help)
			value.Add(arg100, "none");
	}

	private static void TreatedValue(CLIKey alias, CLIValueOrder? value, TokenList? list) {
		ExceptionMessages.ThrowIfNull(list, nameof(list));
		ExceptionMessages.ThrowIfNull(value, nameof(value));
		string funcValue = value[GlobalFunctionHub.CLOVOFuncKey]!;

		if (iAlias != (CLIKey)funcValue) return;

		if (alias == opc_help)
			value.Add(arg100, nameof(opc_help));
	}
}
