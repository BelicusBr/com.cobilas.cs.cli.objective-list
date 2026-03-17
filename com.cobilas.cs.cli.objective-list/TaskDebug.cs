#undef PRINT
#undef PRINT_TRACE

using Cobilas.CLI.Manager;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Cobilas.CLI.ObjectiveList;

internal static class TaskDebug {
	internal static void Print(string format, params object[]? args) {
#if PRINT
		Console.WriteLine(format, args);
		Trace();
#endif
#if PRINT_TRACE
		Trace();
#endif
	}

	internal static void Print(object? arg) {
#if PRINT
		Console.WriteLine(arg);
#endif
#if PRINT_TRACE
		Trace();
#endif
	}

	internal static void Print(string format, params string[]? args)
		=> Print(format, (object[]?)args);

	internal static void Print(string? arg)
		=> Print((object?)arg);

	internal static void Print(TokenList? list) {
#if PRINT
		ExceptionMessages.ThrowIfNull(list, nameof(list));
		list.Reset();
		list.Move();

		while (list.CurrentIndex < list.Count) {
			KeyValuePair<string, long> temp = list.GetValueAndMove;
			Console.WriteLine($"[{temp.Key}, {(TaskListTokens)temp.Value}]");
		}
#endif
#if PRINT_TRACE
		Trace();
#endif
	}

	private static void Trace() {
		Console.WriteLine(new StackTrace(1, true));
	}
}
