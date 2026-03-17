using System;
using Cobilas.CLI.Manager;

namespace Cobilas.CLI.ObjectiveList.FuncHub;

internal static class UniFunctions {
	internal static void InitFunctions() {
		GlobalFunctionHub.EventGenericFunction += ShowVersion;
		GlobalFunctionHub.EventGenericFunction += ShowHelp;
		GlobalFunctionHub.EventTreatedValue += RenameTaskListFile;
	}

	private static void ShowVersion(CLIKey alias, CLIValueOrder? value) {
		if (alias == (CLIKey)"--version/-v")
			Console.WriteLine($"Version: {Program.version}");
	}

	private static void ShowHelp(CLIKey alias, CLIValueOrder? value) {
		if (alias == (CLIKey)"help/-h/-?")
			Console.WriteLine("Help function!!!");
	}

	private static void RenameTaskListFile(CLIKey alias, CLIValueOrder? value, TokenList? list) {
		if (alias == (CLIKey)"rm-rpn-fln-arg") {
			Console.WriteLine($"rm-rpn-fln-arg:{list.CurrentKey}");
		} else if (alias == (CLIKey)"rm-rpn-nfln-arg") {
			Console.WriteLine($"rm-rpn-nfln-arg:{list.CurrentKey}");
		} else if (alias == (CLIKey)"rm-arg") {
			Console.WriteLine($"rm-arg:{list.CurrentKey}");
		}
	}
}
