using System;
using System.Reflection;
using Cobilas.CLI.Manager;
using Cobilas.CLI.Manager.Interfaces;
using Cobilas.CLI.ObjectiveList.Elements;
using Cobilas.CLI.ObjectiveList.FuncHub;

namespace Cobilas.CLI.ObjectiveList; 
internal class Program {
	/*root --version/-v
	 *root --help/-h
	 *root --rename/-r --replacename/-rn {opc-arg:file name} {opc-arg:new file name} {arg:folder path}
	 *root init/-i {arg:file path}
	 *root --show/-s --item/--i --path/-p {opc:arg} {arg:file path}
	 *root --show/-s --list/-l {arg:file path}
	 *root --clear/-c {arg:file path}
	 *root --element/-e add --path/-p {opc:arg} --title/-t {opc:arg} --description/-d {opc:arg} {arg:file path}
	 *root --element/-e remove --path/-p {opc:arg} {arg:file path}
	 *root set --replace/-rp --path/-p {opc:arg} --title/-t {opc:arg} {arg:file path}
	 *root set --replace/-rp --path/-p {opc:arg} --description/-d {opc:arg} {arg:file path}
	 *root set --replace/-rp --path/-p {opc:arg} --status/--s {opc:arg[true|false]} {arg:file path}
	 *root set --move/-m --path/-p {opc:arg} --moveto/-mt {opc:arg} {arg:file path}
	 *root init/-i --help/-h
	 *root --rename/-r --help/-h
	 *root --show/-s --help/-h
	 *root --clear/-c --help/-h
	 *root --element/-e --help/-h
	 *root set --help/-h
	 */
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
			ElementFactory.CreatFunction("--version/-v",
				ElementFactory.CreateOptionEnd("-help/--h/--?", mandatory:false)
			),
			ElementFactory.CreatFunction("help/-h/-?"),
			ElementFactory.CreatFunction("--rename/-r",
				ElementFactory.CreateOptionEnd("-help/--h/--?", mandatory:false),
				ElementFactory.CreatArgument("{105}arg"),
				ElementFactory.CreatArgument("{106}arg"),
				ElementFactory.CreatArgument("{107}arg", false)
			),
			ElementFactory.CreatFunction("init/-i",
				ElementFactory.CreateOptionEnd("-help/--h/--?", mandatory:false),
				ElementFactory.CreatArgument("{111}arg", false)
			),
			ElementFactory.CreatFunction("--show/-s",
				ElementFactory.CreateOptionEnd("-help/--h/--?", mandatory:false),
				ElementFactory.CreateOptionJump("--item/--i", false, 3),
				ElementFactory.CreateOption("--otk/--stk", false),
				ElementFactory.CreateOptionJump("--path/-p"),
				ElementFactory.CreatArgument("{121}arg"),
				ElementFactory.CreateOptionJump("--list/-l", false, 1),
				ElementFactory.CreatArgument("{122}arg", false),
				ElementFactory.CreatArgument("{123}arg", false)
			),
			ElementFactory.CreatFunction("--clear/-c",
				ElementFactory.CreateOptionEnd("-help/--h/--?", mandatory:false),
				ElementFactory.CreatArgument("{131}arg", false)
			),
			ElementFactory.CreatFunction("--element/-e",
				ElementFactory.CreateOptionEnd("-help/--h/--?", mandatory:false),
				//add function
				ElementFactory.CreateOptionJump("add", false, 6),
				ElementFactory.CreateOptionJump("--path/-p", false, 1),
				ElementFactory.CreatArgument("{141}arg"),
				ElementFactory.CreateOption("--title/-t"),
				ElementFactory.CreatArgument("{142}arg"),
				ElementFactory.CreateOptionJump("--description/-d", false, 1),
				ElementFactory.CreatArgument("{143}arg"),
				//remove function
				ElementFactory.CreateOptionJump("remove", false, 2),
				ElementFactory.CreateOption("--path/-p"),
				ElementFactory.CreatArgument("{144}arg"),
				//element argument
				ElementFactory.CreatArgument("{145}arg", false)
			),
			ElementFactory.CreatFunction("set",
				ElementFactory.CreateOptionEnd("-help/--h/--?", mandatory:false),
				//replace function
				ElementFactory.CreateOptionJump("--replace/-rp", false, 8),
				ElementFactory.CreateOption("--path/-p"),
				ElementFactory.CreatArgument("{151}arg"),
				ElementFactory.CreateOptionJump("--title/-t", false, 1),
				ElementFactory.CreatArgument("{152}arg"),
				ElementFactory.CreateOptionJump("--description/-d", false, 1),
				ElementFactory.CreatArgument("{153}arg"),
				ElementFactory.CreateOptionJump("--status/--s", false, 1),
				ElementFactory.CreatArgument("{154}arg"),
				//move function
				ElementFactory.CreateOptionJump("--move/-m", false, 4),
				ElementFactory.CreateOption("--path/-p"),
				ElementFactory.CreatArgument("{155}arg"),
				ElementFactory.CreateOption("--moveto/-mt"),
				ElementFactory.CreatArgument("{156}arg"),
				//element argument
				ElementFactory.CreatArgument("{157}arg", false)
			)
#if DEBUG
			, ElementFactory.CreatFunction("--tds",
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
			if (item.Analyzer(list, message)) {
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
			HelpFunctions.ShowHelp();
		}

		TaskDebug.Print(list);

		GlobalFunctionHub.ClearEvents();
		GC.Collect();
	}
}
