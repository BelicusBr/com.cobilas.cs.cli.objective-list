using System;
using System.IO;
using System.Xml;
using System.Text;
using Cobilas.CLI.Manager;

namespace Cobilas.CLI.ObjectiveList.FuncHub;
[CallMethod(nameof(InitFunction.Start))]
internal static class InitFunction {
	internal const string DefaultFileName = "init.tskl";

	private static readonly CLIKey iAlias = "init/-i";
	private static readonly CLIKey arg111 = "{111}arg";
	private const string InitFileCreatedMessage = "The init.tskl file has already been created!!!";
	private const string InitFileAlreadyExistsMessage = "The init.tskl file was not created because it already exists!";

	internal static void Start() {
		GlobalFunctionHub.EventGenericFunction += Run;
		GlobalFunctionHub.EventDefaultValue += DefaultValue;
		GlobalFunctionHub.EventTreatedValue += TreatedValue;
	}

	//{111}arg folder path

	private static void Run(CLIKey alias, CLIValueOrder? value) {
		ExceptionMessages.ThrowIfNull(value, nameof(value));

		if (iAlias != alias) return;

		string folderPath = value[arg111]!;

		if (!Directory.Exists(folderPath))
			throw new DirectoryNotFoundException($"Directory '{folderPath}' not found!!!");

		folderPath = Path.Combine(folderPath, DefaultFileName);

		if (!File.Exists(folderPath)) {
			FunctionHubUtility.WriteStartupFile(File.CreateText(folderPath));
			Printer.Print(InitFileCreatedMessage);
		} else Printer.Print(InitFileAlreadyExistsMessage);
	}

	private static void DefaultValue(CLIKey alias, CLIValueOrder? value) {
		ExceptionMessages.ThrowIfNull(value, nameof(value));
		string funcValue = value[GlobalFunctionHub.CLOVOFuncKey]!;

		if (iAlias != (CLIKey)funcValue) return;
		if (alias == arg111)
			value.Add(arg111, Environment.CurrentDirectory);
	}

	private static void TreatedValue(CLIKey alias, CLIValueOrder? value, TokenList? list) {
		ExceptionMessages.ThrowIfNull(list, nameof(list));
		ExceptionMessages.ThrowIfNull(value, nameof(value));
		string funcValue = value[GlobalFunctionHub.CLOVOFuncKey]!;

		if (iAlias != (CLIKey)funcValue) return;
		if (alias == arg111)
			value.Add(arg111, list.CurrentKey);
	}
}
