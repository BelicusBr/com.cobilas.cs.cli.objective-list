using System;
using Cobilas.CLI.Manager;
using System.Collections.Generic;
using Cobilas.CLI.Manager.Interfaces;

namespace Cobilas.CLI.ObjectiveList.Elements;

public readonly struct TaskListArgument(string? alias, bool mandatory) : IArgument {
	private readonly CLIKey alias = alias ?? throw new ArgumentNullException(nameof(alias));
	private readonly bool mandatory = mandatory;

	public string Alias => alias;
	public bool Mandatory => mandatory;
	public long TypeCode => CLIParse.ArgumentCode;

	public TaskListArgument(string? alias) : this(alias, true) { }

	public bool Analyzer(TokenList? list, ErrorMessage? message) {
		ExceptionMessages.ThrowIfNull(list, nameof(list));
		ExceptionMessages.ThrowIfNull(message, nameof(message));
		Func<string, bool> analyzer_arg = CLIParse.GetFunction<Func<string, bool>>(2u);
		if (list.CurrentValue != TypeCode) {
			if (!mandatory) return false;
			ExceptionMessage(list.Current, message);
			return true;
		} else if (analyzer_arg(list.CurrentKey)) {
			ExceptionMessage(list.Current, message);
			return true;
		}
		return false;
	}

	public void DefaultValue(CLIValueOrder? valueOrder)
		=> CLIParse.GetFunction<Action<CLIValueOrder?>>(0u)(valueOrder);

	public void ExceptionMessage(KeyValuePair<string, long> value, ErrorMessage? message) {
		ExceptionMessages.ThrowIfNull(message, nameof(message));
		TaskListTokens tokens = (TaskListTokens)value.Value;
		if (tokens.HasFlag(TaskListTokens.Option)) { }
		else if (tokens.HasFlag(TaskListTokens.Function)) { }
		else if (tokens.HasFlag(TaskListTokens.EndCode)) { }
		else {
			CLIParse.GetFunction<Action<CLIKey, KeyValuePair<string, long>, ErrorMessage>>(3u)(alias, value, message);
		}
	}

	public bool IsAlias(string? alias) {
		ExceptionMessages.ThrowIfNullOrEmpty(alias, nameof(alias));
		return this.alias == alias;
	}

	public void TreatedValue(CLIValueOrder? valueOrder, TokenList? list)
		=> CLIParse.GetFunction<Action<CLIValueOrder?, TokenList?>>(1u)(valueOrder, list);
}
