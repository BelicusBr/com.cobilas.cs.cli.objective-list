namespace Cobilas.CLI.ObjectiveList.FuncHub;

internal readonly struct TaskListItem {
	private readonly string title,
							description;
	private readonly bool status;
	private readonly TaskPath taskPath;

	internal string Title => title;
	internal bool Status => status;
	internal string Path => taskPath.Path;
	internal int[] Cells => taskPath.Cells;
	internal TaskPath TaskPath => taskPath;
	internal string Description => description;
	internal int CellCount => taskPath.CellCount;

	internal int this[int index] => taskPath[index];

	internal TaskListItem(string path, string title, string description, string status) {
		this.title = title;
		this.taskPath = new(path);
		this.description = description;
		this.status = bool.Parse(status);
	}

	internal TaskListItem(int path, string title, string description, string status) {
		this.title = title;
		this.taskPath = new(path);
		this.description = description;
		this.status = bool.Parse(status);
	}

	internal TaskListItem(int[] path, string title, string description, string status) {
		this.title = title;
		this.taskPath = new(path);
		this.description = description;
		this.status = bool.Parse(status);
	}

	internal bool IsBlock(TaskListItem item) => taskPath.IsBlock(item.taskPath);

	internal bool IsBlock(TaskPath item) => taskPath.IsBlock(item);
}
