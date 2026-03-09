using Cobilas.CLI.Manager;
using Cobilas.CLI.Manager.Interfaces;
using Cobilas.CLI.ObjectiveList.Elements;
using Cobilas.CLI.ObjectiveList.FuncHub;
using Cobilas.Collections;
using System;
using System.Collections.Generic;
using System.Text;

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
	public const string version = "3.0.0";
	internal static string BaseDirectory => Environment.CurrentDirectory;
	internal static string FriendlyName => AppDomain.CurrentDomain.FriendlyName;

	static void Main(string[] args) {
		CLIParse.EndCode = (long)TaskListTokens.EndCode;
		CLIParse.ArgumentCode = (long)TaskListTokens.Argument;

		CLIParse.AddFunction(0u, GlobalFunctionHub.DefaultValue);
		CLIParse.AddFunction(1u, GlobalFunctionHub.TreatedValue);
		CLIParse.AddFunction(4u, Func);

		CLIParse.AddToken((long)TaskListTokens.Function,
			"--version", "-v",
			"help", "-h", "-?",
			"--rename", "-r",
			"init", "-i",
			"--show", "-s",
			"--element", "-e",
			"--clear", "-c",
			"set"
		);
		CLIParse.AddToken((long)TaskListTokens.Option,
			"--item", "--i",
			"--path", "-p",
			"--list", "-l",
			"add",
			"remove",
			"--title", "-t",
			"--description", "-d",
			"--replace", "-rp",
			"--status", "--s",
			"--move", "-m",
			"--moveto", "-mt",
			"--replacename", "-rn"
		);
		CLIParse.AddToken((long)(TaskListTokens.Option | TaskListTokens.EndCode),
			"-help",
			"--h", 
			"--?"
		);

		IFunction[] functions = {
			new TaskListFunction("--version/-v"),
			new TaskListFunction("help/-h/-?"),
			new TaskListFunction("--rename/-r"),
			new TaskListFunction("init/-i"),
			new TaskListFunction("--show/-s"),
			new TaskListFunction("--clear/-c"),
			new TaskListFunction("--element/-e"),
			new TaskListFunction("set"),
		};

		TokenList list = new(CLIParse.Parse(args));
		ErrorMessage message = new();
		list.Move();

		foreach (IFunction item in functions) {
			if (!item.IsAlias(list.CurrentKey)) continue;
			if (item.Analyzer(list, message)) {
				Console.WriteLine(message);
				return;
			}
			list.Reset();
			list.Move(2);
			if (item.GetValues(list, message)) {
				Console.WriteLine(message);
				return;
			}

			item.Run();
		}

		while (list.CurrentIndex < list.Count) {
			KeyValuePair<string, long> temp = list.GetValueAndMove;
			Console.WriteLine($"[{temp.Key}, {(TaskListTokens)temp.Value}]");
		}

		GlobalFunctionHub.ClearEvents();
		GC.Collect();
	}

	static void Func(CLIKey alis, CLIValueOrder? order) {
		if (alis == (CLIKey)"--version/-v") {
			Console.WriteLine($"{nameof(version)}: {version}");
		} else if (alis == (CLIKey)"--help/-h") {
			Console.WriteLine("Commands:");
			Console.WriteLine("\t--version/-v");
			Console.WriteLine("\t--help/-h");
		}
	}
}
