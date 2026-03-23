using System;
using System.IO;
using System.Xml;
using Cobilas.CLI.Manager;
using System.Collections.Generic;

namespace Cobilas.CLI.ObjectiveList.FuncHub;
[CallMethod(nameof(ShowFunction.Start))]
internal static partial class ShowFunction {
	private static readonly CLIKey iAlias = "--show/-s";
	private static readonly CLIKey arg120 = "{120}arg";
	private static readonly CLIKey arg120_1 = "{120-1}arg";
	private static readonly CLIKey arg121 = "{121}arg";
	private static readonly CLIKey arg122 = "{122}arg";
	private static readonly CLIKey arg123 = "{123}arg";
	private static readonly CLIKey otk_stk = "--otk/--stk";
	private static readonly CLIKey opc_item = "--item/--i";
	private static readonly CLIKey opc_list = "--list/-l";

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

		string opt_type = value[arg120]!;
		string filePath = value[arg123]!;
		Console.WriteLine(opt_type);
		switch (opt_type) {
			case "sw-item":
				string path = value[arg121]!;
				string tShow = value[arg120_1]!;
				ShowItem(path, tShow == "o-item", filePath);
				break;
			case "sw-list":
				string status = value[arg122]!;
				ShowList(status, filePath);
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
		else if (alias == otk_stk)
			switch (list.CurrentKey) {
				case "--otk":
					value.Add("{120-1}arg", "o-item");
					break;
				default:
					value.Add("{120-1}arg", "o-s-item");
					break;
			}
	}

	private static void ShowItem(in string path, in bool oneTask, in string filePath) {
		string defaultFile = FunctionHubUtility.GetFile(filePath);
		XmlReaderSettings settings = new() {
			IgnoreComments = true,
			IgnoreWhitespace = true,
			DtdProcessing = DtdProcessing.Prohibit
		};
		using XmlReader reader = XmlReader.Create(defaultFile, settings);
		using XMLIRWElement element = reader.ReadXMLIRW();
		List<TaskListItem> list = [];
		list.PopList(element);
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
		status = status.ToLower();
		string defaultFile = FunctionHubUtility.GetFile(filePath);
		XmlReaderSettings settings = new() {
			IgnoreComments = true,
			IgnoreWhitespace = true,
			DtdProcessing = DtdProcessing.Prohibit
		};
		using XmlReader reader = XmlReader.Create(defaultFile, settings);
		using XMLIRWElement element = reader.ReadXMLIRW();
		List<TaskListItem> list = [];
		list.PopList(element);
		list.ReorderTaskListItem();
		foreach (TaskListItem item in list)
			if (item.Status.ToString().ToLower() == status || status == "all")
				FunctionHubUtility.PrintTaskItem(item);
	}
}
