using System;

namespace Cobilas.CLI.ObjectiveList;

internal static class Printer {

	internal static bool EnableNewLine = true;

	internal static void PrintException(object? obj)
		=> Print(ConsoleColor.DarkRed, obj);

	internal static void PrintWarning(object? obj)
		=> Print(ConsoleColor.DarkYellow, obj);

	internal static void Print(object? obj)
		=> Print(ConsoleColor.Gray, obj);

	internal static void Print(ConsoleColor color, object? obj) {
		ConsoleColor oldColor = Console.ForegroundColor;
		Console.ForegroundColor = color;
		if (EnableNewLine)
			Console.WriteLine(obj);
		else Console.Write(obj);
			Console.ForegroundColor = oldColor;
	}
}
