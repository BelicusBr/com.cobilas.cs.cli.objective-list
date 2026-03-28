namespace Cobilas.CLI.ObjectiveList.FuncHub;

internal readonly struct TaskListItem {
	private readonly string path,
							title,
							description;
	private readonly bool status;
	private readonly int[] cells;
	private readonly TaskPath taskPath;

	internal int this[int index] => cells[index];

	internal TaskListItem(string path, string title, string description, string status) {
		this.path = path;
		this.title = title;
		this.taskPath = new(path);
		this.description = description;
		this.status = bool.Parse(status);
		this.cells = TaskPath.GetDimension(path);
	}

	internal TaskListItem(int path, string title, string description, string status) {
		this.title = title;
		this.cells = [path];
		this.taskPath = new(path);
		this.path = path.ToString();
		this.description = description;
		this.status = bool.Parse(status);
	}

	internal TaskListItem(int[] path, string title, string description, string status) {
		this.cells = path;
		this.title = title;
		this.taskPath = new(path);
		this.description = description;
		this.status = bool.Parse(status);
		this.path = string.Join('.', path);
	}

	internal string Path => path;
	internal int[] Cells => cells;
	internal string Title => title;
	internal bool Status => status;
	internal TaskPath TaskPath => taskPath;
	internal string Description => description;
}
