using System;
using System.Text;
using Cobilas.Collections;
using Cobilas.CLI.Manager;
using System.Collections.Generic;

namespace Cobilas.CLI.ObjectiveList; 
internal class Program {
	/*root --version/-v
	 *root --help/-h
	 *root --rename/-r {arg:folder path} {arg:file name} {arg:new file name}
	 *root init/-i {arg:file path}
	 *root --show/-s --item/--i --path/-p {opc:arg} {arg:file path}
	 *root --show/-s --list/-l {arg:file path}
	 *root --clear/-c {arg:file path}
	 *root --element/-e add --path/-p {opc:arg} --title/-t {opc:arg} --description/-d {opc:arg} {arg:file path}
	 *root --element/-e remove --path/-p {opc:arg} {arg:file path}
	 *root set --replacetile/-rt --path/-p {opc:arg} --title/-t {opc:arg} {arg:file path}
	 *root set --replacedesc/-rd --path/-p {opc:arg} --description/-d {opc:arg} {arg:file path}
	 *root set --replacestatus/-rs --path/-p {opc:arg} --status/--s {opc:arg[true|false]} {arg:file path}
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
	 */
	public const string version = "3.0.0";
	internal static string BaseDirectory => Environment.CurrentDirectory;
	internal static string FriendlyName => AppDomain.CurrentDomain.FriendlyName;

	static void Main(string[] args) {
		CLIParse.EndCode = (long)TaskListTokens.EndCode;
		CLIParse.ArgumentCode = (long)TaskListTokens.Argument;

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
			"--replacetile", "-rt",
			"--replacedesc", "-rd",
			"--replacestatus", "-rs",
			"--status", "--s",
			"--move", "-m",
			"--moveto", "-mt"
		);
		CLIParse.AddToken((long)(TaskListTokens.Option | TaskListTokens.EndCode),
			"-help",
			"--h", 
			"--?"
		);

		TokenList list = new(CLIParse.Parse(args));
		list.Move();

		while (list.CurrentIndex < list.Count) {
			KeyValuePair<string, long> temp = list.GetValueAndMove;
			Console.WriteLine($"[{temp.Key}, {(TaskListTokens)temp.Value}]");
		}
	}
}
