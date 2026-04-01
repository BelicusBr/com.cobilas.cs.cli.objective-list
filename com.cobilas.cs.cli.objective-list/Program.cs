using System;
using System.Reflection;
using Cobilas.CLI.Manager;
using Cobilas.CLI.Manager.Interfaces;
using Cobilas.CLI.ObjectiveList.FuncHub;
using Cobilas.CLI.ObjectiveList.Elements;

namespace Cobilas.CLI.ObjectiveList; 
internal class Program {
	/* id das funções
	 * 0 => default value
	 * 1 => treated value for arguments
	 * 2 => analyzer for arguments
	 * 3 => invalid argument
	 * 4 => generic function
	 * 6 => option argument name
	 */
	internal static string version {
		get {
			Version? entryVersion = Assembly.GetEntryAssembly()?.GetName().Version;
			return entryVersion?.ToString() ?? "Unknown";
		}
	}
	internal static string BaseDirectory => Environment.CurrentDirectory;
	internal static string FriendlyName => AppDomain.CurrentDomain.FriendlyName;

	private static void Main(string[] args) {

		CLIParse.EndCode = (long)TaskListTokens.EndCode;
		CLIParse.ArgumentCode = (long)TaskListTokens.Argument;

		GlobalFunctionHub.CallInitializers();

		ElementFactory.StartTokens();
#if DEBUG
		ElementFactory.CreateTDSTokens();
#endif

		IFunction[] functions = [
			VersionFunction.CreateFunction(),
			HelpFunction.CreateFunction(),
			RenameFunction.CreateFunction(),
			InitFunction.CreateFunction(),
			ShowFunction.CreateFunction(),
			ClearFunction.CreateFunction(),
			ElementFunction.CreateFunction(),
			SetFunction.CreateFunction()
#if DEBUG
			, ElementFactory.CreateFunction("--tds",
				ElementFactory.CreateOptionEnd("-help/--h/--?", mandatory:false),
				ElementFactory.CreateOptionJump("-op1", false, 2),
				ElementFactory.CreatArgument("op1-1-arg"),
				ElementFactory.CreatArgument("op1-2-arg"),
				ElementFactory.CreateOptionJump("-op2", false, 2),
				ElementFactory.CreatArgument("op2-1-arg"),
				ElementFactory.CreatArgument("op2-2-arg"),
				ElementFactory.CreatArgument("tds-arg", false)
			)
#endif
		];

		TokenList list = new(CLIParse.Parse(args));
		ErrorMessage message = new();
		list.Move();
		bool runFunction = false;

		foreach (IFunction item in functions) {
			if (!item.IsAlias(list.CurrentKey)) continue;
			TaskDebug.Print($"{item.Alias}|{list.CurrentKey}");
			if (((ICLIAnalyzer)item).Analyzer(list, message)) {
				Printer.PrintException(message);
				return;
			}
			list.Reset();
			list.Move();
			if (item.GetValues(list, message)) {
				Printer.PrintException(message);
				return;
			}

			item.Run();
			runFunction = true;
		}

		if (!runFunction) {
			if (list.CurrentValue == (long)TaskListTokens.EndCode)
				Printer.PrintException("No function was called!!!");
			else Printer.PrintException($"Element '{list.CurrentKey}' not identified!!!");
			HelpFunction.CallShowHelp();
		}

		TaskDebug.Print(list);

		GlobalFunctionHub.ClearEvents();
		GC.Collect();
	}
}
