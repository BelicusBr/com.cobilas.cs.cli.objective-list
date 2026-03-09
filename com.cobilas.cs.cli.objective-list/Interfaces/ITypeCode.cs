namespace Cobilas.CLI.ObjectiveList.Interfaces;

public interface ITypeCode {
	bool IsTypeCode(long typeCode);
	bool IsTypeCode(TaskListTokens typeCode);
}
