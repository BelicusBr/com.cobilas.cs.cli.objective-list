using System;
using System.IO;
using Cobilas.CLI.Manager;
using Cobilas.CLI.Manager.Interfaces;
using Cobilas.CLI.ObjectiveList.Elements;

namespace Cobilas.CLI.ObjectiveList.FuncHub;
[CallMethod(nameof(ClearFunction.Start))]
internal static class ClearFunction {
	private static readonly CLIKey arg130 = "{130}arg";
	private static readonly CLIKey arg131 = "{131}arg";
	private static readonly CLIKey iAlias = "--clear/-c";

	internal static void Start() {
		GlobalFunctionHub.EventGenericFunction += Run;
		GlobalFunctionHub.EventDefaultValue += DefaultValue;
		GlobalFunctionHub.EventTreatedValue += TreatedValue;
	}

	internal static IFunction CreateFunction()
		=> ElementFactory.CreateFunction(iAlias,
			ElementFactory.CreateOptionEnd(HelpFunction.opc_help, mandatory: false),
			ElementFactory.CreatArgument(arg131, false)
		);

	//{131}arg file path

	private static void Run(CLIKey alias, CLIValueOrder? value) {
		ExceptionMessages.ThrowIfNull(value, nameof(value));

		if (iAlias != alias) return;

		switch (value[arg130]) {
			case nameof(HelpFunction.opc_help):
				Printer.Print("Remove all tasks in the .tskl file!");
				HelpFunction.ClearHelp();
				break;
			default:
				string filePath = FunctionHubUtility.GetFile(value[arg131]);

				using (FileStream file = File.Open(filePath, FileMode.Open)) {
					file.SetLength(0L);
					FunctionHubUtility.WriteStartupFile(file);

					Printer.EnableNewLine = false;
					Printer.Print("The file '");
					Printer.PrintWarning(filePath);
					Printer.EnableNewLine = true;
					Printer.Print("' was successfully cleaned!");
				}
				break;
		}
	}

	private static void DefaultValue(CLIKey alias, CLIValueOrder? value) {
		ExceptionMessages.ThrowIfNull(value, nameof(value));
		string funcValue = value[GlobalFunctionHub.CLOVOFuncKey]!;

		if (iAlias != (CLIKey)funcValue) return;

		if (alias == arg131)
			value.Add(arg131, Path.Combine(Environment.CurrentDirectory, InitFunction.DefaultFileName));
		else if (alias == HelpFunction.opc_help)
			value.Add(arg130, "none");
	}

	private static void TreatedValue(CLIKey alias, CLIValueOrder? value, TokenList? list) {
		ExceptionMessages.ThrowIfNull(list, nameof(list));
		ExceptionMessages.ThrowIfNull(value, nameof(value));
		string funcValue = value[GlobalFunctionHub.CLOVOFuncKey]!;

		if (iAlias != (CLIKey)funcValue) return;

		if (alias == arg131)
			value.Add(arg131, list.CurrentKey);
		else if (alias == HelpFunction.opc_help)
			value.Add(arg130, nameof(HelpFunction.opc_help));
	}
}
