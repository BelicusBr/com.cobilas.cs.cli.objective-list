namespace Cobilas.CLI.ObjectiveList.Interfaces;

internal interface ITypeCode {
	bool IsTypeCode(long typeCode);
	bool IsTypeCode(TaskListTokens typeCode);
}
