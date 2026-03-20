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

	internal static IFunction CreatFunction(string? alias, params IOptionFunc[]? options)
		=> new TaskListFunction(alias, TaskListTokens.Function, options);

	private static IOptionFunc ICreateOption(
		string? alias,
		bool mandatory,
		int countJump,
		bool jumpAll,
		TaskListTokens token) => new TaskListOption(alias, mandatory, countJump, jumpAll, token);
}
