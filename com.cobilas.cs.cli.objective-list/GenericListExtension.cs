using System;
using System.Xml;
using System.Linq;
using System.Collections.Generic;
using Cobilas.CLI.ObjectiveList.FuncHub;

namespace Cobilas.CLI.ObjectiveList;

internal static class GenericListExtension {

	internal static void ReorderTaskListItem(this List<TaskListItem> list) {
		List <TaskListItem>? sorted = list.Select(
			_item => new { 
				item = _item, 
				path = new TaskPath(_item.Path) 
			}).OrderBy(x => x.path)
			.Select(x => x.item)
			.ToList();

		list.Clear();
		list.AddRange(sorted);
	}

	internal static void PopList(this List<TaskListItem> list, XMLIRWElement element) {
		ParseTaskItems(list, element);
		ValidateTaskList(list);
	}

	private static void ParseTaskItems(List<TaskListItem> list, XMLIRWElement element) {
		IEnumerable<XMLIRWElement> taskNodes = element
			.OfType<XMLIRWElement>()
			.Where(e => e.Name == "tasks")
			.SelectMany(e => e.OfType<XMLIRWElement>())
			.Where(e => e.Name == "tskl_item");

		foreach (XMLIRWElement node in taskNodes) {
			Dictionary<string, string> attrs = node.Attributes
				.Cast<XMLIRWAttribute>()
				.ToDictionary(a => a.Name, a => (string)a.Text);

			attrs.TryGetValue("path", out var path);
			attrs.TryGetValue("title", out var title);
			attrs.TryGetValue("description", out var desc);
			attrs.TryGetValue("status", out var status);

			if (!FunctionHubUtility.ValidateTaskPath(path)) {
				Printer.PrintException($"The path '{path}' is not valid!");
				Printer.PrintException($"Use numbers and a '.' period to separate numbers.");
				Printer.PrintException($"Example: '0' or '0.1' or '0.1.5'.");
				return;
			} else if (!FunctionHubUtility.ValidateTaskStatus(status)) {
				Printer.PrintException($"The input value '{status}' is not a valid status!");
				Printer.PrintException("Valid input values: 'true', 'false', or 'all'.");
				return;
			}
			ExceptionMessages.ThrowIfNullOrEmpty(title, nameof(title));

			list.Add(new(TaskPath.GetDimension(path), title, desc ?? string.Empty, status));
		}
	}

	private static void ValidateTaskList(List<TaskListItem> list) {
		for (int i = 0; i < list.Count; i++) {
			TaskListItem current = list[i];

			int occurrences = list.Count(item => item.Path == current.Path);
			if (occurrences > 1)
				throw new InvalidOperationException(
					$"The task '{current.TaskPath}' is duplicated in the list!");

			bool hasCoupledTask = list.Any(current.IsBlock);
			if (!hasCoupledTask)
				throw new InvalidOperationException(
					$"The subtask '{current.TaskPath}' is not coupled to a task!");
		}
	}
}
