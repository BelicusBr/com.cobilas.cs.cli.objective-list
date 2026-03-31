using System;
using System.IO;
using Cobilas.CLI.Manager;
using System.Collections.Generic;

namespace Cobilas.CLI.ObjectiveList.FuncHub;
[CallMethod(nameof(SetFunction.Start))]
internal static class SetFunction {
	private static readonly CLIKey arg150 = "{150}arg";
	private static readonly CLIKey arg151 = "{151}arg";
	private static readonly CLIKey arg152 = "{152}arg";
	private static readonly CLIKey arg153 = "{153}arg";
	private static readonly CLIKey arg154 = "{154}arg";
	private static readonly CLIKey arg155 = "{155}arg";
	private static readonly CLIKey arg156 = "{156}arg";
	private static readonly CLIKey arg157 = "{157}arg";
	private static readonly CLIKey opc_move = "--move/-m";
	private static readonly CLIKey opc_title = "--title/-t";
	private static readonly CLIKey opc_help = "-help/--h/--?";
	private static readonly CLIKey opc_status = "--status/--s";
	private static readonly CLIKey opc_replace = "--replace/-rp";
	private static readonly CLIKey opc_description = "--description/-d";
	private static readonly CLIKey iAlias = "set";

	internal static void Start() {
		GlobalFunctionHub.EventGenericFunction += Run;
		GlobalFunctionHub.EventDefaultValue += DefaultValue;
		GlobalFunctionHub.EventTreatedValue += TreatedValue;
	}

	//{151}arg => -rp/path
	//{152}arg => title
	//{153}arg => description
	//{154}arg => status
	//{155}arg => -m/path
	//{156}arg => moveto
	//{157}arg => file path

	private static void Run(CLIKey alias, CLIValueOrder? value) {
		ExceptionMessages.ThrowIfNull(value, nameof(value));

		if (iAlias != alias) return;

		switch (value[arg150]) {
			case nameof(opc_help):
				Printer.Print("It allows you to change certain aspects of the task!");
				HelpFunctions.SetHelp();
				break;
			case nameof(opc_replace):
				string path = value[arg151]!;
				string title = value[arg152]!;
				string desc = value[arg153]!;
				string status = value[arg154]!;
				ReplaceOption(path, title, desc, status, FunctionHubUtility.GetFile(value[arg157]));
				break;
			case nameof(opc_move):
				path = value[arg155]!;
				string moveTo = value[arg156]!;
				MoveOption(path, moveTo, FunctionHubUtility.GetFile(value[arg157]));
				break;
		}
	}

	private static void DefaultValue(CLIKey alias, CLIValueOrder? value) {
		ExceptionMessages.ThrowIfNull(value, nameof(value));
		string funcValue = value[GlobalFunctionHub.CLOVOFuncKey]!;

		if (iAlias != (CLIKey)funcValue) return;

		if (alias == opc_description)
			value.Add(arg153, string.Empty);
		else if (alias == opc_title)
			value.Add(arg152, string.Empty);
		else if (alias == opc_status)
			value.Add(arg154, string.Empty);
		else if (alias == arg157)
			value.Add(arg157, Path.Combine(Environment.CurrentDirectory, InitFunction.DefaultFileName));
	}

	private static void TreatedValue(CLIKey alias, CLIValueOrder? value, TokenList? list) {
		ExceptionMessages.ThrowIfNull(list, nameof(list));
		ExceptionMessages.ThrowIfNull(value, nameof(value));
		string funcValue = value[GlobalFunctionHub.CLOVOFuncKey]!;

		if (iAlias != (CLIKey)funcValue) return;

		if (alias == arg151)
			value.Add(arg151, list.CurrentKey);
		else if (alias == arg152)
			value.Add(arg152, list.CurrentKey);
		else if (alias == arg153)
			value.Add(arg153, list.CurrentKey);
		else if (alias == arg154)
			value.Add(arg154, list.CurrentKey);
		else if (alias == arg155)
			value.Add(arg155, list.CurrentKey);
		else if (alias == arg156)
			value.Add(arg156, list.CurrentKey);
		else if (alias == arg157)
			value.Add(arg157, list.CurrentKey);
		else if (alias == opc_move)
			value.Add(arg150, nameof(opc_move));
		else if (alias == opc_replace)
			value.Add(arg150, nameof(opc_replace));
		else if (alias == opc_help)
			value.Add(arg150, nameof(opc_help));
	}

	private static void ReplaceOption(
		string path,
		string title,
		string description,
		string status,
		string filePath) {


		if (!FunctionHubUtility.ValidateTaskPath(path)) {
			Printer.PrintException($"The path '{path}' is not valid!");
			Printer.PrintException($"Use numbers and a '.' period to separate numbers.");
			Printer.PrintException($"Example: '0' or '0.1' or '0.1.5'.");
			return;
		}

		filePath = FunctionHubUtility.GetFile(filePath);

		List<TaskListItem> list = FunctionHubUtility.GetTaskList(filePath);
		TaskPath path1 = new(path);
		for (int I = 0; I < list.Count; I++)
			if (list[I].TaskPath == path1) {
				string _title = string.IsNullOrEmpty(title) ? list[I].Title : title;
				string _description = description switch {
							"\\cls" => string.Empty,
							_ => string.IsNullOrEmpty(description) ? list[I].Description : description
						};
				string _status = string.IsNullOrEmpty(status) ? list[I].Status.ToString().ToLower() : status;

				if (!string.IsNullOrEmpty(title) ||
					!string.IsNullOrEmpty(description) ||
					!string.IsNullOrEmpty(status)) {
					Printer.EnableNewLine = false;
					Printer.Print("Task '");
					Printer.PrintWarning(path);
					Printer.EnableNewLine = true;
					Printer.Print("' underwent the following changes.");

					if (list[I].Title != _title) {
						Printer.EnableNewLine = false;
						Printer.Print("The title '");
						Printer.PrintWarning(list[I].Title);
						Printer.Print("' has been changed to '");
						Printer.PrintWarning(_title);
						Printer.EnableNewLine = true;
						Printer.Print("'.");
					}
					if (description == "\\cls")
						Printer.Print("The description has been removed!");
					else if (list[I].Description != _description)
						if (string.IsNullOrEmpty(list[I].Description)) {
							Printer.EnableNewLine = false;
							Printer.Print("The description '");
							Printer.PrintWarning(TreatDescription(_description));
							Printer.EnableNewLine = true;
							Printer.Print("' has been added!");
						} else {
							Printer.EnableNewLine = false;
							Printer.Print("The description '");
							Printer.PrintWarning(TreatDescription(list[I].Description));
							Printer.Print("' has been changed to '");
							Printer.PrintWarning(TreatDescription(_description));
							Printer.EnableNewLine = true;
							Printer.Print("'.");
						}
					if (list[I].Status.ToString().ToLower() != _status.ToLower()) {
						Printer.EnableNewLine = false;
						Printer.Print("The status '");
						Printer.PrintWarning(list[I].Status.ToString().ToLower());
						Printer.Print("' has been changed to '");
						Printer.PrintWarning(_status.ToLower());
						Printer.EnableNewLine = true;
						Printer.Print("'.");
					}
				}
				if (!FunctionHubUtility.ValidateTaskStatus(_status)) {
					Printer.PrintException($"The input value '{_status}' is not a valid status!");
					Printer.PrintException("Valid input values: 'true', 'false', or 'all'.");
					return;
				}

				list[I] = new(path, _title, _description, _status.ToLower());
				break;
			}
		FunctionHubUtility.SetTaskList(list, filePath);
	}

	private static void MoveOption(string path, string moveTo, string filePath) {

		if (!FunctionHubUtility.ValidateTaskPath(path)) {
			Printer.PrintException($"The path '{path}' is not valid!");
			Printer.PrintException($"Use numbers and a '.' period to separate numbers.");
			Printer.PrintException($"Example: '0' or '0.1' or '0.1.5'.");
			return;
		} else if (!FunctionHubUtility.ValidateTaskPath(moveTo)) {
			Printer.PrintException($"The path '{moveTo}' is not valid!");
			Printer.PrintException($"Use numbers and a '.' period to separate numbers.");
			Printer.PrintException($"Example: '0' or '0.1' or '0.1.5'.");
			return;
		}

		filePath = FunctionHubUtility.GetFile(filePath);

		List<TaskListItem> list = FunctionHubUtility.GetTaskList(filePath);

		if (!IsValidPath(list, path)) {
			Printer.PrintException($"Task '{path}' was not found!");
			return;
		} else if (!IsValidPath(list, moveTo)) {
			Printer.PrintException($"Task '{moveTo}' was not found!");
			return;
		}
		TaskPath path1 = new(path);
		TaskPath _moveTo = new(moveTo);
		int k_path = -1,
			k_moveTo = -1;

		for (int I = 0; I < list.Count; I++)
			if (path1 == list[I].TaskPath)
				k_path = I;
			else if (_moveTo == list[I].TaskPath)
				k_moveTo = I;

		list[k_moveTo] = new(path, list[k_moveTo].Title, list[k_moveTo].Description, list[k_moveTo].Status.ToString().ToLower());
		list[k_path] = new(moveTo, list[k_path].Title, list[k_path].Description, list[k_path].Status.ToString().ToLower());

		//Task '{0}' has been moved to path '{1}'!
		Printer.EnableNewLine = false;
		Printer.Print("Task '");
		Printer.PrintWarning(path);
		Printer.Print("' has been moved to path '");
		Printer.PrintWarning(moveTo);
		Printer.EnableNewLine = false;
		Printer.Print("'!");

		FunctionHubUtility.SetTaskList(list, filePath);
	}

	private static bool IsValidPath(List<TaskListItem> list, string path) {
		foreach (TaskListItem item in list)
			if (path == item.Path)
				return true;
		return false;
	}

	private static string TreatDescription(string description)
		=> description.Replace("(LF)", "\n").Replace("(CR)", "\r").Replace("(CRLF)", "\r\n");
}
