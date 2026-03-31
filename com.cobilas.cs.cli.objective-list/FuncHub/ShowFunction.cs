using System;
using System.IO;
using System.Xml;
using Cobilas.CLI.Manager;
using System.Collections.Generic;

namespace Cobilas.CLI.ObjectiveList.FuncHub;
[CallMethod(nameof(ShowFunction.Start))]
internal static partial class ShowFunction {
	private static readonly CLIKey arg120 = "{120}arg";
	private static readonly CLIKey arg121 = "{121}arg";
	private static readonly CLIKey arg122 = "{122}arg";
	private static readonly CLIKey arg123 = "{123}arg";
	private static readonly CLIKey iAlias = "--show/-s";
	private static readonly CLIKey opc_list = "--list/-l";
	private static readonly CLIKey arg120_1 = "{120-1}arg";
	private static readonly CLIKey opc_item = "--item/--i";
	private static readonly CLIKey otk_stk = "--otk/--stk";
	private static readonly CLIKey opc_help = "-help/--h/--?";

	internal static void Start() {
		GlobalFunctionHub.EventGenericFunction += Run;
		GlobalFunctionHub.EventDefaultValue += DefaultValue;
		GlobalFunctionHub.EventTreatedValue += TreatedValue;
	}

	//{120}arg
	//{120-1}arg
	//{121}arg => item->path
	//{122}arg => list->status
	//{123}arg => folder path

	private static void Run(CLIKey alias, CLIValueOrder? value) {
		ExceptionMessages.ThrowIfNull(value, nameof(value));
		
		if (iAlias != alias) return;

		switch (value[arg120]) {
			case nameof(opc_help):
				Printer.Print("Allows you to view all tasks present in the .tskl file!");
				HelpFunctions.ShowHelp();
				break;
			case "sw-item":
				string path = value[arg121]!;
				string tShow = value[arg120_1]!;
				ShowItem(path, tShow == "o-item", FunctionHubUtility.GetFile(value[arg123]));
				break;
			case "sw-list":
				string status = value[arg122]!;
				ShowList(status, FunctionHubUtility.GetFile(value[arg123]));
				break;
		}
	}

	private static void DefaultValue(CLIKey alias, CLIValueOrder? value) {
		ExceptionMessages.ThrowIfNull(value, nameof(value));
		string funcValue = value[GlobalFunctionHub.CLOVOFuncKey]!;

		if (iAlias != (CLIKey)funcValue) return;

		if (alias == arg122)
			value.Add(arg122, "all");
		else if (alias == arg123)
			value.Add(arg123, Path.Combine(Environment.CurrentDirectory, InitFunction.DefaultFileName));
		else if (alias == otk_stk)
			value.Add(arg120_1, "o-s-item");
	}

	private static void TreatedValue(CLIKey alias, CLIValueOrder? value, TokenList? list) {
		ExceptionMessages.ThrowIfNull(list, nameof(list));
		ExceptionMessages.ThrowIfNull(value, nameof(value));
		string funcValue = value[GlobalFunctionHub.CLOVOFuncKey]!;

		if (iAlias != (CLIKey)funcValue) return;

		if (alias == arg121)
			value.Add(arg121, list.CurrentKey);
		else if (alias == arg122)
			value.Add(arg122, list.CurrentKey);
		else if (alias == arg123)
			value.Add(arg123, list.CurrentKey);
		else if (alias == opc_item)
			value.Add(arg120, "sw-item");
		else if (alias == opc_list)
			value.Add(arg120, "sw-list");
		else if (alias == opc_help)
			value.Add(arg120, nameof(opc_help));
		else if (alias == otk_stk)
			switch (list.CurrentKey) {
				case "--otk":
					value.Add(arg120_1, "o-item");
					break;
				default:
					value.Add(arg120_1, "o-s-item");
					break;
			}
	}

	private static void ShowItem(in string path, in bool oneTask, in string filePath) {
		string defaultFile = FunctionHubUtility.GetFile(filePath);

		if (!FunctionHubUtility.ValidateTaskPath(path)) {
			Printer.PrintException($"The path '{path}' is not valid!");
			Printer.PrintException($"Use numbers and a '.' period to separate numbers.");
			Printer.PrintException($"Example: '0' or '0.1' or '0.1.5'.");
			return;
		}

		List<TaskListItem> list = FunctionHubUtility.GetTaskList(defaultFile);
		if (oneTask) {
			foreach (TaskListItem item in list)
				if (item.Path == path) {
					FunctionHubUtility.PrintTaskItem(item);
					break;
				}
			return;
		}
		list.ReorderTaskListItem();
		TaskPath subPath = new(path);
		foreach (TaskListItem item in list) {
			if (subPath.IsBlock(item.TaskPath))
				FunctionHubUtility.PrintTaskItem(item);
		}
	}

	private static void ShowList(string status, string filePath) {
		if (!FunctionHubUtility.ValidateTaskStatus(status, true)) {
			Printer.PrintException($"The input value '{status}' is not a valid status!");
			Printer.PrintException("Valid input values: 'true', 'false', or 'all'.");
			return;
		}

		status = status.ToLower();
		string defaultFile = FunctionHubUtility.GetFile(filePath);

		List<TaskListItem> list = FunctionHubUtility.GetTaskList(defaultFile, true);
		foreach (TaskListItem item in list)
			if (item.Status.ToString().ToLower() == status || status == "all")
				FunctionHubUtility.PrintTaskItem(item);
	}
}
