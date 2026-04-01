using Cobilas.CLI.Manager;
using Cobilas.CLI.Manager.Interfaces;

namespace Cobilas.CLI.ObjectiveList.Elements;

internal static class ElementFactory {

	internal static IOptionFunc CreateOptionEnd(string? alias, bool mandatory = true)
		=> ICreateOption(alias, mandatory, 0, true, TaskListTokens.Option | TaskListTokens.EndCode);

	internal static IOptionFunc CreateOptionJump(string? alias, bool mandatory = true, int countJump = 0)
		=> ICreateOption(alias, mandatory, countJump, false, TaskListTokens.Option);

	internal static IOptionFunc CreateOption(string? alias, bool mandatory = true) 
		=> ICreateOption(alias, mandatory, 0, false, TaskListTokens.Option);

	internal static IArgument CreatArgument(string? alias, bool mandatory = true)
		=> new TaskListArgument(alias, mandatory);

	internal static IFunction CreateFunction(string? alias, params IOptionFunc[]? options)
		=> new TaskListFunction(alias, TaskListTokens.Function, options);

	internal static void StartTokens() {
		// create functions
		CreateTokens((long)TaskListTokens.Function,
			"--version/-v",
			"help/-h/-?",
			"--rename/-r",
			"init/-i",
			"--show/-s",
			"--element/-e",
			"--clear/-c",
			"set"
		);
		// create options
		CreateTokens((long)TaskListTokens.Option,
			"--item/--i",
			"--path/-p",
			"--list/-l",
			//"--to-json-output/-tjo",
			"--otk/--stk",
			"add",
			"remove",
			"--title/-t",
			"--description/-d",
			"--replace/-rp",
			"--status/--s",
			"--move/-m",
			"--moveto/-mt",
			"--replacename/-rn"
		);
		// create end option
		CreateTokens((long)(TaskListTokens.Option | TaskListTokens.EndCode),
			"-help/--h/--?"
		);
	}

	internal static void CreateTDSTokens() {
		CreateTokens((long)TaskListTokens.Function, "--tds");
		CreateTokens((long)TaskListTokens.Option, "-op1", "-op2");
	}

	private static void CreateTokens(long tokenId, params string[] tokens) {
		foreach (string item in tokens)
			if (item != string.Empty)
				CLIParse.AddToken(tokenId, item.Split('/', System.StringSplitOptions.RemoveEmptyEntries));
	}

	private static IOptionFunc ICreateOption(
		string? alias,
		bool mandatory,
		int countJump,
		bool jumpAll,
		TaskListTokens token) => new TaskListOption(alias, mandatory, countJump, jumpAll, token);
}
