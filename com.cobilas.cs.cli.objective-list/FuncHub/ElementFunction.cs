using System;
using System.IO;
using Cobilas.CLI.Manager;
using System.Collections.Generic;
using Cobilas.CLI.Manager.Interfaces;
using Cobilas.CLI.ObjectiveList.Elements;

namespace Cobilas.CLI.ObjectiveList.FuncHub;
[CallMethod(nameof(ElementFunction.Start))]
internal static class ElementFunction {
	private const string defaultPath = "none";
	/// <summary>opc type</summary>
	private static readonly CLIKey arg140 = "{140}arg";
	/// <summary>add opc path</summary>
	private static readonly CLIKey arg141 = "{141}arg";
	/// <summary>opc title</summary>
	private static readonly CLIKey arg142 = "{142}arg";
	/// <summary>opc description</summary>
	private static readonly CLIKey arg143 = "{143}arg";
	/// <summary>remove opc path</summary>
	private static readonly CLIKey arg144 = "{144}arg";
	/// <summary>folder path</summary>
	private static readonly CLIKey arg145 = "{145}arg";
	/// <summary>opc type</summary>
	private static readonly CLIKey opc_add = "add";
	/// <summary>opc type</summary>
	private static readonly CLIKey opc_remove = "remove";
	private static readonly CLIKey iAlias = "--element/-e";
	private static readonly CLIKey opc_title = "--title/-t";
	private static readonly CLIKey opc_a_path = "--path/-p";
	private static readonly CLIKey opc_a_desc = "--description/-d";

	internal static void Start() {
		GlobalFunctionHub.EventGenericFunction += Run;
		GlobalFunctionHub.EventDefaultValue += DefaultValue;
		GlobalFunctionHub.EventTreatedValue += TreatedValue;
	}

	internal static IFunction CreateFunction()
		=> ElementFactory.CreateFunction(iAlias,
			ElementFactory.CreateOptionEnd(HelpFunction.opc_help, mandatory: false),
			//add function
			ElementFactory.CreateOptionJump(opc_add, false, 6),
			ElementFactory.CreateOptionJump(opc_a_path, false, 1),
			ElementFactory.CreatArgument(arg141),
			ElementFactory.CreateOption(opc_title),
			ElementFactory.CreatArgument(arg142),
			ElementFactory.CreateOptionJump(opc_a_desc, false, 1),
			ElementFactory.CreatArgument(arg143),
			//remove function
			ElementFactory.CreateOptionJump(opc_remove, false, 2),
			ElementFactory.CreateOption(opc_a_path),
			ElementFactory.CreatArgument(arg144),
			//element argument
			ElementFactory.CreatArgument(arg145, false)
		);

	//{140}arg opc type
	//{141}arg add opc path
	//{142}arg opc title
	//{143}arg opc description
	//{144}arg remove opc path
	//{145}arg folder path

	private static void Run(CLIKey alias, CLIValueOrder? value) {
		ExceptionMessages.ThrowIfNull(value, nameof(value));

		if (iAlias != alias) return;

		switch (value[arg140]) {
			case nameof(HelpFunction.opc_help):
				Printer.Print("Allows you to add and remove tasks from the .tskl file!");
				HelpFunction.ElementHelp();
				break;
			case nameof(opc_add):
				string path = value[arg141]!;
				string title = value[arg142]!;
				string description = value[arg143]!;
				AddElement(path, title, description, FunctionHubUtility.GetFile(value[arg145]));
				break;
			case nameof(opc_remove):
				path = value[arg144]!;
				RemoveElement(path, FunctionHubUtility.GetFile(value[arg145]));
				break;
		}
	}

	private static void DefaultValue(CLIKey alias, CLIValueOrder? value) {
		ExceptionMessages.ThrowIfNull(value, nameof(value));
		string funcValue = value[GlobalFunctionHub.CLOVOFuncKey]!;

		if (iAlias != (CLIKey)funcValue) return;
		if (alias == opc_a_path)
			value.Add(arg141, defaultPath);
		else if (alias == opc_a_desc)
			value.Add(arg143, string.Empty);
		else if (alias == arg145)
			value.Add(arg145, Path.Combine(Environment.CurrentDirectory, InitFunction.DefaultFileName));
	}

	private static void TreatedValue(CLIKey alias, CLIValueOrder? value, TokenList? list) {
		ExceptionMessages.ThrowIfNull(list, nameof(list));
		ExceptionMessages.ThrowIfNull(value, nameof(value));
		string funcValue = value[GlobalFunctionHub.CLOVOFuncKey]!;

		if (iAlias != (CLIKey)funcValue) return;

		if (alias == opc_add)
			value.Add(arg140, nameof(opc_add));
		else if (alias == opc_remove)
			value.Add(arg140, nameof(opc_remove));
		else if (alias == HelpFunction.opc_help)
			value.Add(arg140, nameof(HelpFunction.opc_help));
		else if (alias == arg141)
			value.Add(arg141, list.CurrentKey);
		else if (alias == arg142)
			value.Add(arg142, list.CurrentKey);
		else if (alias == arg143)
			value.Add(arg143, list.CurrentKey);
		else if (alias == arg144)
			value.Add(arg144, list.CurrentKey);
		else if (alias == arg145)
			value.Add(arg145, list.CurrentKey);
	}

	private static void AddElement(string path, string title, string description, string filePath) {
		List<TaskListItem> list = FunctionHubUtility.GetTaskList(filePath);

		if (path == defaultPath) {
			long index = 0;
			foreach (TaskListItem item in list)
				if (item[0] > index)
					index = item[0];
			list.Add(new((int)index + 1, title, description, "true"));
		} else {
			if (!FunctionHubUtility.ValidateTaskPath(path)) {
				Printer.PrintException($"The path '{path}' is not valid!");
				Printer.PrintException($"Use numbers and a '.' period to separate numbers.");
				Printer.PrintException($"Example: '0' or '0.1' or '0.1.5'.");
				return;
			}
			TaskPath path1 = new(path);
			bool condition = true;
			while (condition)
				for (int I = 0; I < list.Count; I++) {
					condition = false;
					if (list[I].TaskPath == path1) {
						condition = true;
						path1[path1.CellCount - 1]++;
						break;
					}
				}
			list.Add(new(path1.Cells, title, description, "true"));
		}

		FunctionHubUtility.SetTaskList(list, filePath);

		if (list.Count != 0)
			FunctionHubUtility.PrintTaskItem(list[list.Count - 1]);
	}

	private static void RemoveElement(string path, string filePath) {
		if (!FunctionHubUtility.ValidateTaskPath(path)) {
			Printer.PrintException($"The path '{path}' is not valid!");
			Printer.PrintException($"Use numbers and a '.' period to separate numbers.");
			Printer.PrintException($"Example: '0' or '0.1' or '0.1.5'.");
			return;
		}

		List<TaskListItem> list = FunctionHubUtility.GetTaskList(filePath);

		TaskPath path1 = new(path);
		for (int I = 0; I < list.Count; I++) {
			TaskListItem item = list[I];
			if (item.TaskPath == path1) {
				list.RemoveAt(I);
				Printer.EnableNewLine = false;
				Printer.Print("The task '[");
				Printer.PrintWarning(item.Path);
				Printer.Print(']');
				Printer.PrintWarning(item.Title);
				Printer.EnableNewLine = true;
				Printer.Print("' has been successfully removed!");
				FunctionHubUtility.SetTaskList(list, filePath);
				return;
			}
		}
		Printer.PrintException($"The task '{path}' could not be found to be removed!");
	}
}
