using Cobilas.CLI.Manager;

namespace Cobilas.CLI.ObjectiveList.FuncHub;

internal static class RenameFunctions {
	internal static void InitFunctions() { }

	private static void RanameTaskListFile(CLIKey alias, CLIValueOrder? value, TokenList? list) {
		if (alias == (CLIKey)"rm-rpn-fln-arg") { }
		else if (alias == (CLIKey)"rm-rpn-nfln-arg") { }
		else if (alias == (CLIKey)"rm-arg") { }
	}
}
