using System.IO;
using System.Xml;
using System.Text;
using System.Collections.Generic;

namespace Cobilas.CLI.ObjectiveList.FuncHub; 
internal static class TaskListFile {
	internal static List<TaskListItem> GetTaskList(string filePath) {
		XmlReaderSettings settings = new() {
			IgnoreComments = true,
			IgnoreWhitespace = true,
			DtdProcessing = DtdProcessing.Prohibit
		};
		List<TaskListItem> list = [];
		using XmlReader reader = XmlReader.Create(filePath, settings);
		list.PopList(reader.ReadXMLIRW());
		return list;
	}

	internal static void SetTaskList(List<TaskListItem> list, string filePath) {
		using FileStream stream = File.OpenWrite(filePath);
		stream.SetLength(0L);
		XmlWriterSettings wsettings = new() {
			Indent = true,
			IndentChars = "\t",
			Encoding = Encoding.UTF8,
			NewLineOnAttributes = false
		};
		using XmlWriter writer = XmlWriter.Create(stream, wsettings);
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
}
