using System;
using System.IO;
using Cobilas.CLI.Manager;
using System.Collections.Generic;
using Cobilas.CLI.Manager.Interfaces;
using Cobilas.CLI.ObjectiveList.Elements;

namespace Cobilas.CLI.ObjectiveList.FuncHub;
[CallMethod(nameof(ShowFunction.Start))]
internal static partial class ShowFunction {
	/// <summary>opc --i path</summary>
	private static readonly CLIKey arg121 = "{121}arg";
	/// <summary>opc -l status</summary>
	private static readonly CLIKey arg122 = "{122}arg";
	/// <summary>file/folder path</summary>
	private static readonly CLIKey arg123 = "{123}arg";
	private static readonly CLIKey iAlias = "--show/-s";
	private static readonly CLIKey arg1200 = "{1200}arg";
	private static readonly CLIKey arg1201 = "{1201}arg";
	private static readonly CLIKey opc_list = "--list/-l";
	private static readonly CLIKey opc_path = "--path/-p";
	private static readonly CLIKey opc_item = "--item/--i";
	private static readonly CLIKey otk_stk = "--otk/--stk";

	internal static void Start() {
		GlobalFunctionHub.EventGenericFunction += Run;
		GlobalFunctionHub.EventDefaultValue += DefaultValue;
		GlobalFunctionHub.EventTreatedValue += TreatedValue;
	}

	internal static IFunction CreateFunction()
		=> ElementFactory.CreateFunction(iAlias,
			ElementFactory.CreateOptionEnd(HelpFunction.opc_help, mandatory: false),
			ElementFactory.CreateOptionJump(opc_item, false, 3),
			ElementFactory.CreateOption(otk_stk, false),
			ElementFactory.CreateOptionJump(opc_path),
			ElementFactory.CreatArgument(arg121),
			ElementFactory.CreateOptionJump(opc_list, false, 1),
			ElementFactory.CreatArgument(arg122, false),
			ElementFactory.CreatArgument(arg123, false)
		);

	//{120}arg
	//{120-1}arg
	//{121}arg => item->path
	//{122}arg => list->status
	//{123}arg => folder path

	private static void Run(CLIKey alias, CLIValueOrder? value) {
		ExceptionMessages.ThrowIfNull(value, nameof(value));
		
		if (iAlias != alias) return;

		switch (value[arg1200]) {
			case nameof(HelpFunction.opc_help):
				Printer.Print("Allows you to view all tasks present in the .tskl file!");
				HelpFunction.ShowHelp();
				break;
			case "sw-item":
				string path = value[arg121]!;
				string tShow = value[arg1201]!;
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
			value.Add(arg1201, "o-s-item");
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
			value.Add(arg1200, "sw-item");
		else if (alias == opc_list)
			value.Add(arg1200, "sw-list");
		else if (alias == HelpFunction.opc_help)
			value.Add(arg1200, nameof(HelpFunction.opc_help));
		else if (alias == otk_stk)
			switch (list.CurrentKey) {
				case "--otk":
					value.Add(arg1201, "o-item");
					break;
				default:
					value.Add(arg1201, "o-s-item");
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
