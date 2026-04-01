using System;
using Cobilas.CLI.Manager;
using Cobilas.CLI.Manager.Interfaces;
using Cobilas.CLI.ObjectiveList.Elements;

namespace Cobilas.CLI.ObjectiveList.FuncHub;
[CallMethod(nameof(VersionFunction.Start))]
internal static class VersionFunction {
	private static readonly CLIKey arg100 = "{100}arg";
	private static readonly CLIKey iAlias = "--version/-v";

	internal static void Start() {
		GlobalFunctionHub.EventGenericFunction += Run;
		GlobalFunctionHub.EventDefaultValue += DefaultValue;
		GlobalFunctionHub.EventTreatedValue += TreatedValue;
	}

	internal static IFunction CreateFunction()
		=> ElementFactory.CreateFunction(iAlias,
			ElementFactory.CreateOptionEnd(HelpFunction.opc_help, mandatory: false)
		);

	private static void Run(CLIKey alias, CLIValueOrder? value) {
		ExceptionMessages.ThrowIfNull(value, nameof(value));

		if (iAlias != alias) return;

		switch (value[arg100]) {
			case nameof(HelpFunction.opc_help):
				Printer.Print("Show the current version of the app!");
				HelpFunction.VersionHelp();
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

		if (alias == HelpFunction.opc_help)
			value.Add(arg100, "none");
	}

	private static void TreatedValue(CLIKey alias, CLIValueOrder? value, TokenList? list) {
		ExceptionMessages.ThrowIfNull(list, nameof(list));
		ExceptionMessages.ThrowIfNull(value, nameof(value));
		string funcValue = value[GlobalFunctionHub.CLOVOFuncKey]!;

		if (iAlias != (CLIKey)funcValue) return;

		if (alias == HelpFunction.opc_help)
			value.Add(arg100, nameof(HelpFunction.opc_help));
	}
}
