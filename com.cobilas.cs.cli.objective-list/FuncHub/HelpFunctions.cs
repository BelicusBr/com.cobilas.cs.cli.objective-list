using System;
using Cobilas.CLI.Manager;

namespace Cobilas.CLI.ObjectiveList.FuncHub;
[CallMethod(nameof(HelpFunctions.Start))]
internal static class HelpFunctions {
	private static readonly CLIKey iAlias = "help/-h/-?";
	private const string descriptionOpcHelp = "Describe how this function works and what options it offers!";

	internal static void Start() {
		GlobalFunctionHub.EventGenericFunction += Run;
	}

	internal static void ShowHelp() => Run("-h", null);

	private static void Run(CLIKey alias, CLIValueOrder? value) {

		if (iAlias != alias) return;

		Printer.Print(">>>===================////");
		VersionHelp();
		Printer.Print(">>>===================////");
		RenameHelp();
		Printer.Print(">>>===================////");
		InitHelp();
		Printer.Print(">>>===================////");
		IShowHelp();
		Printer.Print(">>>===================////");
		ClearHelp();
		Printer.Print(">>>===================////");
		ElementHelp();
		Printer.Print(">>>===================////");
		SetHelp();
		Printer.Print(">>>===================////");
	}

	internal static void VersionHelp() {
		Printer.EnableNewLine = false;
		Printer.Print($"{Program.FriendlyName} [");
		Printer.PrintWarning("--version/-v");
		Printer.Print("] [");
		Printer.PrintWarning("options");
		Printer.EnableNewLine = true;
		Printer.Print("]\r\n");

		Printer.Print("[Options]======>>>");
		OPCHelp();
	}

	internal static void RenameHelp() {
		Printer.EnableNewLine = false;
		Printer.Print($"{Program.FriendlyName} [");
		Printer.PrintWarning("--rename/-r");
		Printer.Print("] [");
		Printer.PrintWarning("options");
		Printer.Print("] [arg1:");
		Printer.PrintWarning("old name");
		Printer.Print("] [arg2:");
		Printer.PrintWarning("new name");
		Printer.Print("] [arg3:");
		Printer.PrintWarning("folder path");
		Printer.EnableNewLine = true;
		Printer.Print("]\r\n");

		Printer.Print("[Options]======>>>");
		OPCHelp();

		Printer.Print("[Argument]======>>>");
		Printer.EnableNewLine = false;
		Printer.Print("[");
		Printer.PrintWarning("arg1");
		Printer.Print("]\t|\t");
		Printer.Print("The current name of the .tskl! file.\r\n");

		Printer.Print("[");
		Printer.PrintWarning("arg2");
		Printer.Print("]\t|\t");
		Printer.Print("The new name for the .tskl file!\r\n");

		Printer.Print("[");
		Printer.PrintWarning("arg3:Is optional");
		Printer.Print("]\t|\t");
		Printer.EnableNewLine = true;
		Printer.Print("The directory where the .tskl file is located!\r\n");
	}

	internal static void InitHelp() {
		Printer.EnableNewLine = false;
		Printer.Print($"{Program.FriendlyName} [");
		Printer.PrintWarning("init/-i");
		Printer.Print("] [");
		Printer.PrintWarning("options");
		Printer.Print("] [arg1:");
		Printer.PrintWarning("folder/file path");
		Printer.EnableNewLine = true;
		Printer.Print("]\r\n");

		Printer.Print("[Options]======>>>");
		OPCHelp();

		Printer.Print("[Argument]======>>>");
		FileAndFolderArgument(1);
	}

	internal static void IShowHelp() {
		Printer.EnableNewLine = false;
		Printer.Print($"{Program.FriendlyName} [");
		Printer.PrintWarning("--show/-s");
		Printer.Print("] [");
		Printer.PrintWarning("options");
		Printer.Print("] [arg1:");
		Printer.PrintWarning("folder/file path");
		Printer.EnableNewLine = true;
		Printer.Print("]\r\n");

		Printer.Print("[Options]======>>>");
		OPCHelp();

		Printer.EnableNewLine = false;
		Printer.PrintWarning("--item/--i");
		Printer.Print("\t|\tAllows you to display a specific task!\r\n");
		Printer.Print("[");
		Printer.PrintWarning("--item/--i");
		Printer.Print("][Options]======>>>\r\n");
		Printer.Print("[");
		Printer.PrintWarning("--otk/--stk:Is optional");
		Printer.EnableNewLine = true;
		Printer.Print("]");
		Printer.Print("\t> [default value: --otk]");
		Printer.Print("\t> --otk:I only displayed the specific task!");
		Printer.Print("\t> --stk:Display the specific task and its subtasks!\r\n");

		Printer.EnableNewLine = false;
		Printer.Print("[");
		Printer.PrintWarning("--path/-p");
		Printer.EnableNewLine = true;
		Printer.Print("]\t|\tThe path to the task! [Example: 0.0.1]\r\n");

		Printer.EnableNewLine = false;
		Printer.PrintWarning("--list/-l");
		Printer.Print("\t|\tAllows you to view all tasks!\r\n");
		Printer.Print("[");
		Printer.PrintWarning("--list/-l");
		Printer.EnableNewLine = true;
		Printer.Print("][Argument]======>>>");
		Printer.Print("Valid arguments!");
		Printer.Print("\t> 'true': Display tasks marked as 'true'.");
		Printer.Print("\t> 'false': Display tasks marked as 'false'.");
		Printer.Print("\t> 'all': I displayed all the tasks.\r\n");

		Printer.Print("[Argument]======>>>");
		FileAndFolderArgument(1);
	}

	internal static void ClearHelp() {
		Printer.EnableNewLine = false;
		Printer.Print($"{Program.FriendlyName} [");
		Printer.PrintWarning("--clear/-c");
		Printer.Print("] [");
		Printer.PrintWarning("options");
		Printer.Print("] [arg1:");
		Printer.PrintWarning("folder/file path");
		Printer.EnableNewLine = true;
		Printer.Print("]\r\n");

		Printer.Print("[Options]======>>>");
		OPCHelp();

		Printer.Print("[Argument]======>>>");
		FileAndFolderArgument(1);
	}

	internal static void ElementHelp() {
		Printer.EnableNewLine = false;
		Printer.Print($"{Program.FriendlyName} [");
		Printer.PrintWarning("--element/-e");
		Printer.Print("] [");
		Printer.PrintWarning("options");
		Printer.Print("] [arg1:");
		Printer.PrintWarning("folder/file path");
		Printer.EnableNewLine = true;
		Printer.Print("]\r\n");

		Printer.Print("[Options]======>>>");
		OPCHelp();
		Printer.EnableNewLine = false;
		Printer.PrintWarning("add");
		Printer.Print("\t|\tAllows you to add a new task!\r\n");
		Printer.Print("[");
		Printer.PrintWarning("add");
		Printer.Print("][Options]======>>>\r\n");
		Printer.Print("[");
		Printer.PrintWarning("--path/-p:Is optional");
		Printer.Print("]\t|\tThe path to the task! [Example: 0.0.1]\r\n");

		Printer.Print("[");
		Printer.PrintWarning("--title/-t");
		Printer.Print("]\t|\tDefine the task title!\r\n");

		Printer.Print("[");
		Printer.PrintWarning("--description/-d");
		Printer.EnableNewLine = true;
		Printer.Print("]\t|\tDefine the task description!\r\n");

		Printer.EnableNewLine = false;
		Printer.PrintWarning("remove");
		Printer.Print("\t|\tAllows you to remove a specific task!\r\n");
		Printer.Print("[");
		Printer.PrintWarning("remove");
		Printer.Print("][Options]======>>>\r\n");
		Printer.Print("[");
		Printer.PrintWarning("--path/-p");
		Printer.EnableNewLine = true;
		Printer.Print("]\t|\tThe path to the task! [Example: 0.0.1]\r\n");

		Printer.Print("[Argument]======>>>");
		FileAndFolderArgument(1);
	}

	internal static void SetHelp() {
		Printer.EnableNewLine = false;
		Printer.Print($"{Program.FriendlyName} [");
		Printer.PrintWarning("set");
		Printer.Print("] [");
		Printer.PrintWarning("options");
		Printer.Print("] [arg1:");
		Printer.PrintWarning("folder/file path");
		Printer.EnableNewLine = true;
		Printer.Print("]\r\n");

		Printer.Print("[Options]======>>>");
		OPCHelp();
		Printer.EnableNewLine = false;
		Printer.PrintWarning("--replace/-rp");
		Printer.Print("\t|\tIt allows you to redefine the attributes of a task!\r\n");
		Printer.Print("[");
		Printer.PrintWarning("--replace/-rp");
		Printer.Print("][Options]======>>>\r\n");
		Printer.Print("[");
		Printer.PrintWarning("--path/-p");
		Printer.Print("]\t|\tThe path to the task! [Example: 0.0.1]\r\n");

		Printer.Print("[");
		Printer.PrintWarning("--title/-t:Is optional");
		Printer.Print("]\t|\tRedefine the task title!\r\n");

		Printer.Print("[");
		Printer.PrintWarning("--description/-d:Is optional");
		Printer.Print("]\t|\tRedefine the task description!\r\n");

		Printer.Print("[");
		Printer.PrintWarning("--status/--s:Is optional");
		Printer.EnableNewLine = true;
		Printer.Print("]\t|\tRedefine the task status!\r\n");

		Printer.EnableNewLine = false;
		Printer.PrintWarning("--move/-m");
		Printer.Print("\t|\tAllows you to move a specific task to another location!\r\n");
		Printer.Print("[");
		Printer.PrintWarning("--move/-m");
		Printer.Print("][Options]======>>>\r\n");
		Printer.Print("[");
		Printer.PrintWarning("--path/-p");
		Printer.Print("]\t|\tThe path to the task! [Example: 0.0.1]\r\n");
		
		Printer.Print("[");
		Printer.PrintWarning("--moveto/-mt");
		Printer.EnableNewLine = true;
		Printer.Print("]\t|\tThe new position of the task.\r\n");


		Printer.Print("[Argument]======>>>");
		FileAndFolderArgument(1);
	}

	private static void FileAndFolderArgument(int index) {
		Printer.EnableNewLine = false;
		Printer.Print("[");
		Printer.PrintWarning($"arg{index}:Is optional");
		Printer.Print("]\t|\t");
		Printer.EnableNewLine = true;
		Printer.Print("The directory where the .tskl file is located, or the full path to the .tskl file!\r\n");
	}

	private static void OPCHelp() {
		Printer.EnableNewLine = false;
		Printer.PrintWarning("-help/--h/--?");
		Printer.Print("\t|\t");
		Printer.EnableNewLine = true;
		Printer.Print(descriptionOpcHelp);
		Printer.Print(string.Empty);
	}
}
