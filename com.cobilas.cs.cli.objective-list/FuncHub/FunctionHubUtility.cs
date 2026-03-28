using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections.Generic;

namespace Cobilas.CLI.ObjectiveList.FuncHub;

internal static class FunctionHubUtility {
	private static readonly List<char> invalidPathChars = [.. Path.GetInvalidPathChars()];
	private static readonly List<char> InvalidFileNameChars = [.. Path.GetInvalidFileNameChars()];
	private static readonly XmlReaderSettings r_settings = new() {
		IgnoreComments = true,
		IgnoreWhitespace = true,
		DtdProcessing = DtdProcessing.Prohibit
	};
	private static readonly XmlWriterSettings w_settings = new() {
		Indent = true,
		IndentChars = "\t",
		Encoding = Encoding.UTF8,
		NewLineOnAttributes = false
	};
	private const string TsklFileNotFoundMessage = "The .tskl file was not found in the directory!";

	internal static bool ValidateTaskStatus(string taskStatus, bool verifyCLIInput = false)
		=> taskStatus.ToLower() switch {
			"true" or "false" => true,
			_ => taskStatus == "all" && verifyCLIInput
		};

	internal static bool ValidateTaskPath(string taskPath) {
		bool number = false;
		foreach (char item in taskPath)
			if (char.IsNumber(item)) number = true;
			else if (item == '.' && number) number = false;
			else return false;
		return number;
	}

	internal static List<TaskListItem> GetTaskList(string filePath, bool reorderList = false) {
		List<TaskListItem> list = [];
		using XmlReader reader = XmlReader.Create(filePath, r_settings);
		list.PopList(reader.ReadXMLIRW());
		if (reorderList)
			list.ReorderTaskListItem();
		return list;
	}

	internal static void SetTaskList(List<TaskListItem> list, string filePath) {
		using FileStream stream = File.OpenWrite(filePath);
		stream.SetLength(0L);
		using XmlWriter writer = XmlWriter.Create(stream, w_settings);
		XMLIRWElement element = new("tasks");
		foreach (TaskListItem item in list) {
			XMLIRWElement tskl_item = new("tskl_item",
				new XMLIRWAttribute("path", item.Path),
				new XMLIRWAttribute("title", item.Title),
				new XMLIRWAttribute("description", item.Description),
				new XMLIRWAttribute("status", item.Status)
			);
			element.Add(tskl_item);
		}

		writer.WriterXMLIRW(new("root", element));
	}

	internal static string GetFile(string? path) {
		ExceptionMessages.ThrowIfNullOrWhiteSpace(path, nameof(path));

		if (!Path.IsPathRooted(path))
			path = Path.Combine(Environment.CurrentDirectory, path);

		if (IsInvalidPath(path))
			throw new InvalidDataException($"The path '{path}' is not valid!");
		else if (File.Exists(path))
			return path;
		else if (Directory.Exists(path))
			foreach (string item in Directory.GetFiles(path))
				if (Path.GetExtension(item) == ".tskl")
					return item;
		throw new FileNotFoundException(TsklFileNotFoundMessage);
	}

	internal static void WriteStartupFile(Stream stream)
		=> IWriteStartupFile(stream);

	internal static void WriteStartupFile(TextWriter writer)
		=> IWriteStartupFile(writer);

	internal static void PrintTaskItem(string path, string title, string description, string status) {
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

	internal static void PrintTaskItem(TaskListItem item)
		=> PrintTaskItem(item.Path, item.Title, item.Description, item.Status.ToString());

	internal static bool IsInvalidPath(string path, bool fileName = false) {
		foreach (char item in fileName ? InvalidFileNameChars : invalidPathChars)
			if (path.Contains(item))
				return true;
		return false;
	}

	private static void IWriteStartupFile(object stream) {
		XmlWriterSettings settings = new() {
			Indent = true,
			IndentChars = "\t",
			Encoding = Encoding.UTF8,
			NewLineOnAttributes = false
		};
		using XmlWriter writer = stream switch {
			TextWriter txw => XmlWriter.Create(txw, settings),
			_ => XmlWriter.Create((stream as Stream)!, settings)
		};

		writer.WriteStartDocument();
		writer.WriteStartElement("tasks");
		writer.WriteEndElement();
		writer.WriteEndDocument();
	}
}
