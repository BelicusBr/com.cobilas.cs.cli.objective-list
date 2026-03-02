using System;
using System.Text;
using Cobilas.Collections;
using Cobilas.CLI.Manager;

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
	public const string version = "3.0.0";
	internal static string BaseDirectory => Environment.CurrentDirectory;
	internal static string FriendlyName => AppDomain.CurrentDomain.FriendlyName;

	static void Main(string[] args) {

		ErrorMensager error = new ErrorMensager();
		CLIArgCollection collection = new CLIArgCollection();
		CLICommand root = CLIBase.Create();
		try {
			if (ArrayManipulation.EmpytArray(args))
				PrintError("Enter some argument.");

			if (CLICommand.Cateter(new StringArrayToIEnumerator(args), root, collection, error, out int funcID)) {
				if (funcID == 0)
					PrintError($"Command '{JoinArgs(args)}' not found.");
				else if (!FuncHub.Invok(funcID, error, collection))
					PrintError(error);
			} else PrintError(error);

		} catch {
			collection.Clear();
			root.Dispose();

			throw;
		}

		collection.Clear();
		root.Dispose();
		GC.Collect();
	}

	static string JoinArgs(string[] args) {
		StringBuilder builder = new StringBuilder();
		for (int I = 0; I < args.Length; I++)
			builder.AppendFormat("{0} ", args[I]);
		return builder.ToString().TrimEnd();
	}

	static void PrintError(ErrorMensager msm) {
		StringBuilder builder = new StringBuilder();
		foreach (var item in msm)
			builder.AppendLine(item);
		PrintError(builder.ToString());
	}

	static void PrintError(string msm)
		=> throw new TaskListException(msm);
}
