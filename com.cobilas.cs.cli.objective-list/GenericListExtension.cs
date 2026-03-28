using Cobilas.CLI.ObjectiveList.FuncHub;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Cobilas.CLI.ObjectiveList;

internal static class GenericListExtension {
	internal static void ReorderTaskListItem(this List<TaskListItem> list) {
		List <TaskListItem>? newList = list.Select(
			_item => new { 
				item = _item, 
				path = new TaskPath(_item.Path) 
			}).OrderBy(x => x.path)
			.Select(x => x.item)
			.ToList();
		list.Clear();
		list.AddRange(newList);
		newList.Clear();
		newList.Capacity = 0;
		newList = null;
	}

	internal static void PopList(this List<TaskListItem> list, XMLIRWElement element) {
		string title = string.Empty,
			   desc = string.Empty,
			   path = string.Empty,
			   _status = string.Empty;
		foreach (XMLIRW item in element)
			if (item.Type == XmlNodeType.Element && item.Name == "tasks")
				foreach (XMLIRW item2 in (XMLIRWElement)item)
					if (item2.Type == XmlNodeType.Element && item2.Name == "tskl_item") {
						foreach (XMLIRWAttribute item3 in ((XMLIRWElement)item2).Attributes.Cast<XMLIRWAttribute>())
							switch (item3.Name) {
								case "path": path = (string)item3.Text; break;
								case "title": title = (string)item3.Text; break;
								case "description": desc = (string)item3.Text; break;
								case "status": _status = (string)item3.Text; break;
							}
						list.Add(new(TaskPath.GetDimension(path), title, desc, _status));
					}
	}
}
