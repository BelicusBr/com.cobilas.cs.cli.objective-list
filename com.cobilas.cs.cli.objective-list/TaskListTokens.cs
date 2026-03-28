namespace Cobilas.CLI.ObjectiveList;
[System.Flags]
public enum TaskListTokens : byte {
	None = 0,
	Argument = 2,
	Option = 4,
	Function = 8,
	EndCode = 16
}