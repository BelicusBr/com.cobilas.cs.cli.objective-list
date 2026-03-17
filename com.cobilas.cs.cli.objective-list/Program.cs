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

		CLIParse.AddFunction(0u, GlobalFunctionHub.DefaultValue);
		CLIParse.AddFunction(1u, GlobalFunctionHub.TreatedValue);
		CLIParse.AddFunction(2u, GlobalFunctionHub.AnalyzerArguments);
		CLIParse.AddFunction(3u, GlobalFunctionHub.InvalidArgument);
		CLIParse.AddFunction(4u, GlobalFunctionHub.GenericFunction);

		UniFunctions.InitFunctions();

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
		/* Poderia tratar as opções das funções como uma lista em vez de lista e sublista
		 * Antiga forma
		 * --rename/-r
		 * |-> -help/--h/--?
		 * |-> --replacename/-rn
		 * |   |-> rm-rpn-fln-arg
		 * |   |-> rm-rpn-nfln-arg
		 * |-> rm-arg
		 * Nova forma
		 * --rename/-r
		 * |-> -help/--h/--?
		 * |-> --replacename/-rn
		 * |-> rm-rpn-fln-arg
		 * |-> rm-rpn-nfln-arg
		 * |-> rm-arg
		 */
		IFunction[] functions = {
			new TaskListFunction("--version/-v", new TaskListOptionEnd("-help/--h/--?", false)),
			new TaskListFunction("help/-h/-?"),
			new TaskListFunction("--rename/-r",
				new TaskListOptionEnd("-help/--h/--?", false),
				new TaskListOption("--replacename/-rn", false, 
					new TaskListArgument("rm-rpn-fln-arg"),
					new TaskListArgument("rm-rpn-nfln-arg")
				),
				new TaskListArgument("rm-arg")
			),
			new TaskListFunction("init/-i",
				new TaskListOptionEnd("-help/--h/--?", false),
				new TaskListArgument("it-arg", false)
			),
			new TaskListFunction("--show/-s",
				new TaskListOptionEnd("-help/--h/--?", false),
				new TaskListSubFunction("--item/--i", false,
					new TaskListOption("--path/-p",
						new TaskListArgument("it-ph-arg")
					)
				),
				new TaskListOption("--list/-l", false),
				new TaskListArgument("sw-arg", false)
			),
			new TaskListFunction("--clear/-c",
				new TaskListOptionEnd("-help/--h/--?", false),
				new TaskListArgument("clr-arg", false)
			),
			new TaskListFunction("--element/-e",
				new TaskListOptionEnd("-help/--h/--?", false),
				new TaskListSubFunction("add", false,
					new TaskListOption("--path/-p", false, new TaskListArgument("emt-a-ph-arg")),
					new TaskListOption("--title/-t", new TaskListArgument("emt-a-tt-arg")),
					new TaskListOption("--description/-d", false, new TaskListArgument("emt-a-dt-arg"))
				),
				new TaskListSubFunction("remove", false,
					new TaskListOption("--path/-p", new TaskListArgument("emt-r-ph-arg"))
				),
				new TaskListArgument("emt-arg", false)
			),
			new TaskListFunction("set",
				new TaskListOptionEnd("-help/--h/--?", false),
				new TaskListSubFunction("--replace/-rp", false,
					new TaskListOption("--path/-p", new TaskListArgument("st-r-ph-arg")),
					new TaskListOption("--title/-t", false, new TaskListArgument("emt-r-tt-arg")),
					new TaskListOption("--description/-d", false, new TaskListArgument("emt-r-dt-arg")),
					new TaskListOption("--status/--s", false, new TaskListArgument("emt-r-stt-arg"))
				),
				new TaskListSubFunction("--move/-m", false,
					new TaskListOption("--path/-p", new TaskListArgument("st-m-ph-arg"))
				),
				new TaskListArgument("st-arg", false)
			),
		};

		TokenList list = new(CLIParse.Parse(args));
		ErrorMessage message = new();
		list.Move();
		bool runFunction = false;

		foreach (IFunction item in functions) {
			if (!item.IsAlias(list.CurrentKey)) continue;
			TaskDebug.Print($"{item.Alias}|{list.CurrentKey}");
			if (item.Analyzer(list, message)) {
				Console.WriteLine(message);
				return;
			}
			list.Reset();
			list.Move();
			if (item.GetValues(list, message)) {
				Console.WriteLine(message);
				return;
			}

			item.Run();
			runFunction = true;
		}

		if (!runFunction) {
			if (list.CurrentValue == (long)TaskListTokens.EndCode)
				Console.WriteLine($"Nenhuma função foi chamado!!!");
			else Console.WriteLine($"Elemento '{list.CurrentKey}' não identificado!!!");
			CLIParse.GetFunction<Action<CLIKey, CLIValueOrder?>>(4)("-h", null);
		}

		TaskDebug.Print(list);

		GlobalFunctionHub.ClearEvents();
		GC.Collect();
	}
}
