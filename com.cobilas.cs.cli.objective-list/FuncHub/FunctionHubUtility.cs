using System;
using System.IO;
using System.Xml;
using System.Text;

namespace Cobilas.CLI.ObjectiveList.FuncHub;

internal static class FunctionHubUtility {
	private const string TsklFileNotFoundMessage = "The .tskl file was not found in the directory!";

	internal static string GetFile(in string? path) {
		ExceptionMessages.ThrowIfNullOrWhiteSpace(path, nameof(path));
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
				throw new FileNotFoundException(TsklFileNotFoundMessage);
		}
		return defaultFile;
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
