using Cobilas.CLI.Manager;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;

namespace Cobilas.CLI.ObjectiveList.FuncHub;

internal static class ShowFunction {
	private static CLIKey iAlias;

	internal static void Start() {
		iAlias = "--show/-s";
		GlobalFunctionHub.EventGenericFunction += ShowRun;
		GlobalFunctionHub.EventDefaultValue += ShowDefaultValue;
		GlobalFunctionHub.EventTreatedValue += ShowTreatedValue;
	}

	//{121}arg => item->path
	//{122}arg => list->status
	//{123}arg => folder path

	private static void ShowRun(CLIKey alias, CLIValueOrder? value) {
		ExceptionMessages.ThrowIfNull(value, nameof(value));
		string funcValue = value[GlobalFunctionHub.CLOVOFuncKey]!;
		
		if (iAlias != (CLIKey)funcValue) return;

		string opt_type = value["{120}arg"]!;
		string filePath = value["{123}arg"]!;
		Console.WriteLine(opt_type);
		switch (opt_type) {
			case "sw-item":
				string path = value["{121}arg"]!;
				ShowItem(path, filePath);
				break;
			case "sw-list":
				string status = value["{122}arg"]!;
				ShowList(status, filePath);
				break;
		}
	}

	private static void ShowDefaultValue(CLIKey alias, CLIValueOrder? value) {
		ExceptionMessages.ThrowIfNull(value, nameof(value));
		string funcValue = value[GlobalFunctionHub.CLOVOFuncKey]!;

		if (iAlias != (CLIKey)funcValue) return;

		if (alias == (CLIKey)"{122}arg")
			value.Add("{122}arg", "true");
		else if (alias == (CLIKey)"{123}arg")
			value.Add("{123}arg", Path.Combine(Environment.CurrentDirectory, "init.tskl"));
	}

	private static void ShowTreatedValue(CLIKey alias, CLIValueOrder? value, TokenList? list) {
		ExceptionMessages.ThrowIfNull(list, nameof(list));
		ExceptionMessages.ThrowIfNull(value, nameof(value));
		string funcValue = value[GlobalFunctionHub.CLOVOFuncKey]!;

		if (iAlias != (CLIKey)funcValue) return;

		if (alias == (CLIKey)"{121}arg")
			value.Add("{121}arg", list.CurrentKey);
		else if (alias == (CLIKey)"{122}arg")
			value.Add("{122}arg", list.CurrentKey);
		else if (alias == (CLIKey)"{123}arg")
			value.Add("{123}arg", list.CurrentKey);
		else if (alias == (CLIKey)"--item/--i")
			value.Add("{120}arg", "sw-item");
		else if (alias == (CLIKey)"--list/-l")
			value.Add("{120}arg", "sw-list");
	}

	private static void ShowItem(in string path, in string filePath) {
		string defaultFile = GetFile(filePath);
		XmlReaderSettings settings = new() {
			IgnoreComments = true,
			IgnoreWhitespace = true,
			DtdProcessing = DtdProcessing.Prohibit
		};
		using XmlReader reader = XmlReader.Create(defaultFile, settings);
		using XMLIRWElement element = reader.ReadXMLIRW();
		string _path = string.Empty,
			   title = string.Empty,
			   desc = string.Empty,
			   status = string.Empty;
		TaskList task = new();
		List<TaskListItem> list = [];
		foreach (XMLIRW item in element)
			if (item.Type == XmlNodeType.Element && item.Name == "tasks")
				foreach (XMLIRW item2 in (XMLIRWElement)item)
					if (item2.Type == XmlNodeType.Element && item2.Name == "tskl_item") {
						foreach (XMLIRWAttribute item3 in ((XMLIRWElement)item2).Attributes.Cast<XMLIRWAttribute>())
							switch (item3.Name) {
								case "path": _path = (string)item3.Text; break;
								case "title": title = (string)item3.Text; break;
								case "description": desc = (string)item3.Text; break;
								case "status": status = (string)item3.Text; break;
							}
						list.Add(new(TaskListItem.GetDimension(_path), title, desc, status));
					}
		foreach (TaskListItem item in list)
			TaskList.SetTaskList(item.path, 0, task, item);

		task.Print();
	}

	private static void ShowList(string status, string filePath) {
		string defaultFile = GetFile(filePath);
		XmlReaderSettings settings = new() {
			IgnoreComments = true,
			IgnoreWhitespace = true,
			DtdProcessing = DtdProcessing.Prohibit
		};
		using XmlReader reader = XmlReader.Create(defaultFile, settings);
		using XMLIRWElement element = reader.ReadXMLIRW();
		string title = string.Empty,
			   desc = string.Empty,
			   path = string.Empty,
			   _status = string.Empty;
		foreach (XMLIRW item in element)
			if (item.Type == XmlNodeType.Element && item.Name == "tasks")
				foreach (XMLIRW item2 in (XMLIRWElement)item)
					if (item2.Type == XmlNodeType.Element && item2.Name == "tskl_item") {
						foreach (XMLIRWAttribute item3 in ((XMLIRWElement)item2).Attributes.Cast<XMLIRWAttribute>()) {
							switch (item3.Name) {
								case "path": path = (string)item3.Text; break;
								case "title": title = (string)item3.Text; break;
								case "description": desc = (string)item3.Text; break;
								case "status": _status = (string)item3.Text; break;
							}
						}
						if (_status == status || status == "all")
							PrintTaskItem(path, title, desc, _status);
					}
	}

	private static void PrintTaskItem(string path, string title, string description, string status) {
		Printer.EnableNewLine = false;
		Printer.Print(ConsoleColor.DarkGreen, "/=====[");
		Printer.Print(ConsoleColor.DarkCyan, path);
		Printer.Print(ConsoleColor.DarkGreen, "][");
		Printer.Print(ConsoleColor.Yellow, title);
		Printer.Print(ConsoleColor.DarkGreen, "][");
		Printer.Print(ConsoleColor.DarkCyan, status);
		if (!string.IsNullOrEmpty(description)) {
			Printer.Print(ConsoleColor.DarkGreen, "]>>\r\n");
			description = description.Replace("(LF)", "\n")
				.Replace("(CR)", "\r")
				.Replace("(CRLF)", "\r\n");
			Printer.Print(ConsoleColor.Green, description);
			Printer.Print("\r\n");
			Printer.Print(ConsoleColor.DarkGreen, "=====<<\r\n");
		} else {
			Printer.Print(ConsoleColor.DarkGreen, "]=====//\r\n");
		}
	}

	private static string GetFile(in string path) {
		string defaultFile = path;

		if (!File.Exists(defaultFile)) {
			bool exist = false;
			foreach (string item in Directory.GetFiles(Path.GetDirectoryName(path)!)) {
				if (Path.GetExtension(item) == ".tskl") {
					defaultFile = item;
					exist = true;
					break;
				}
			}
			if (!exist)
				throw new FileNotFoundException("The .tskl file was not found in the directory!");
		}
		return defaultFile;
	}

	private record struct TaskListItem(
		int[] path,
		string title,
		string description,
		string status
	) {
		internal readonly string PathToString()
			=> string.Join('.', path);

		internal static int[] GetDimension(string path) {
			string[] paths = path.Split('.', StringSplitOptions.RemoveEmptyEntries);
			return Array.ConvertAll(paths, stg => int.Parse(stg));
		}
	}

	private sealed class TaskList {
		private readonly Dictionary<int, object?> value = [];

		internal Dictionary<int, object?> List => this.value;

		public object? this[int index] {
			get => value[index];
			set => this.value[index] = value;
		}

		internal bool ContainsKey(int key) => value.ContainsKey(key);

		internal void Add(int key,  object value) => this.value.Add(key, value);

		internal void Print() {
			foreach (KeyValuePair<int, object?> item in value) {
				if (item.Value is TaskList tskl)
					tskl.Print();
				else if (item.Value is TaskListItem tskli) {
					PrintTaskItem(tskli.PathToString(), tskli.title, tskli.description, tskli.status);
				}
			}
		}

		internal static void SetTaskList(int[] dimension, int index, TaskList list, TaskListItem item) {
			int length = dimension.Length - 1;
			length = length < 0 ? 0 : length;
			try
			{
				//um erro cast mas isso acontece por não conter lista separada
				if (index < length) {
					if (!list.ContainsKey(dimension[index])) {
						Console.WriteLine($"h{{225}}{dimension[index]}");
						TaskList temp = new();
						list.Add(dimension[index], temp);
						SetTaskList(dimension, index + 1, temp, item);
					} else { 
						SetTaskList(dimension, index + 1, (TaskList)list[dimension[index]]!, item);
					}
				}
				else if (index == length) {
					Console.WriteLine($"h{{275}}{dimension[index]}");
					Console.WriteLine($"h{{275}}{index}|{length}|{dimension.Length}|{item.PathToString()}");
					list.Add(dimension[index], item);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"{index}|{length}|{dimension.Length}|{item.PathToString()}");
				Console.WriteLine(ex);
				throw;
			}
		}
	}
}
