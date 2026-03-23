using System;
using System.IO;
using Cobilas.CLI.Manager;

namespace Cobilas.CLI.ObjectiveList.FuncHub;
[CallMethod(nameof(RenameFunction.Start))]
internal static class RenameFunction {
	private static readonly CLIKey arg105 = "{105}arg";
	private static readonly CLIKey arg106 = "{106}arg";
	private static readonly CLIKey arg107 = "{107}arg";
	private static readonly CLIKey iAlias = "--rename/-r";

	internal static void Start() {
		GlobalFunctionHub.EventGenericFunction += Run;
		GlobalFunctionHub.EventTreatedValue += TreatedValue;
		GlobalFunctionHub.EventDefaultValue += DefaultValue;
	}
	/*
	 {105}arg = old name
	 {106}arg = new name
	 {107}arg = folder path
	 */
	private static void Run(CLIKey alias, CLIValueOrder? value) {
		ExceptionMessages.ThrowIfNull(value, nameof(value));

		if (iAlias != alias) return;

		string oldName = value[arg105]!;
		string newName = value[arg106]!;
		string folderPath = value[arg107]!;

		if (!Directory.Exists(folderPath))
			throw new DirectoryNotFoundException($"Directory '{folderPath}' not found!!!");

		oldName = Path.Combine(folderPath, oldName);

		if (!File.Exists(oldName))
			throw new FileNotFoundException($"File '{oldName}' not found!!!");

		File.Move(oldName, Path.Combine(folderPath, newName));

		Printer.EnableNewLine = false;
		Printer.Print("The file '");
		Printer.PrintWarning(Path.GetFileName(oldName));
		Printer.Print("' has been renamed to '");
		Printer.PrintWarning(newName);
		Printer.EnableNewLine = true;
		Printer.Print("'!!!");
	}

	private static void DefaultValue(CLIKey alias, CLIValueOrder? value) {
		ExceptionMessages.ThrowIfNull(value, nameof(value));
		string funcValue = value[GlobalFunctionHub.CLOVOFuncKey]!;

		if (iAlias != (CLIKey)funcValue) return;

		if (alias == arg107)
			value.Add(arg107, Environment.CurrentDirectory);
	}

	private static void TreatedValue(CLIKey alias, CLIValueOrder? value, TokenList? list) {
		ExceptionMessages.ThrowIfNull(list, nameof(list));
		ExceptionMessages.ThrowIfNull(value, nameof(value));
		string funcValue = value[GlobalFunctionHub.CLOVOFuncKey]!;

		if (iAlias != (CLIKey)funcValue) return;

		if (alias == arg105)
			value.Add(arg105, list.CurrentKey);
		else if (alias == arg106)
			value.Add(arg106, list.CurrentKey);
		else if (alias == arg107)
			value.Add(arg107, list.CurrentKey);
	}
}
