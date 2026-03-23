using System;
using Cobilas.CLI.Manager;

namespace Cobilas.CLI.ObjectiveList.FuncHub;
[CallMethod(nameof(UniFunctions.Start))]
internal static class UniFunctions {
	internal static void Start() {
		GlobalFunctionHub.EventGenericFunction += ShowVersion;
		GlobalFunctionHub.EventGenericFunction += ShowHelp;
	}

	private static void ShowVersion(CLIKey alias, CLIValueOrder? value) {
		if (alias == (CLIKey)"--version/-v")
			Console.WriteLine($"Version: {Program.version}");
	}

	private static void ShowHelp(CLIKey alias, CLIValueOrder? value) {
		if (alias == (CLIKey)"help/-h/-?")
			Console.WriteLine("Help function!!!");
	}
}
