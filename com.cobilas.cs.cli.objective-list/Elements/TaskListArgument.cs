using System;
using Cobilas.CLI.Manager;
using System.Collections.Generic;
using Cobilas.CLI.Manager.Interfaces;
using Cobilas.CLI.ObjectiveList.Interfaces;

namespace Cobilas.CLI.ObjectiveList.Elements;

public readonly struct TaskListArgument(string? alias, bool mandatory) : IArgument, ITypeCode {
	private readonly bool mandatory = mandatory;
	private readonly CLIKey alias = GetAlias(alias);

	public string Alias => alias;
	public bool Mandatory => mandatory;
	public long TypeCode => CLIParse.ArgumentCode;

	public TaskListArgument(string? alias) : this(alias, true) { }

	public bool Analyzer(TokenList? list, ErrorMessage? message) {
		ExceptionMessages.ThrowIfNull(list, nameof(list));
		ExceptionMessages.ThrowIfNull(message, nameof(message));
		Func<string?, bool> analyzer_arg = CLIParse.GetFunction<Func<string?, bool>>(2u);
		TaskDebug.Print($"[TLA]{list.CurrentKey}|{(TaskListTokens)list.CurrentValue}");
		if (list.CurrentValue != TypeCode) {
			if (!mandatory) return false;
			ExceptionMessage(list.Current, message);
			return true;
		} else if (analyzer_arg(list.CurrentKey)) {
			ExceptionMessage(list.Current, message);
			return true;
		}
		list.Move();
		TaskDebug.Print($"[TLA]{list.CurrentKey}|{(TaskListTokens)list.CurrentValue}");
		return false;
	}

	public void DefaultValue(CLIValueOrder? valueOrder)
		=> CLIParse.GetFunction<Action<CLIKey, CLIValueOrder?>>(0u)(alias, valueOrder);

	public void ExceptionMessage(KeyValuePair<string, long> value, ErrorMessage? message) {
		ExceptionMessages.ThrowIfNull(message, nameof(message));
		TaskListTokens tokens = (TaskListTokens)value.Value;
		if (tokens.HasFlag(TaskListTokens.Option)) { }
		else if (tokens.HasFlag(TaskListTokens.Function)) { }
		else if (tokens.HasFlag(TaskListTokens.EndCode)) { }
		else CLIParse.GetFunction<Action<CLIKey, KeyValuePair<string, long>, ErrorMessage?>>(3u)(alias, value, message);
	}

	public bool IsAlias(string? alias) {
		ExceptionMessages.ThrowIfNullOrEmpty(alias, nameof(alias));
		return this.alias == (CLIKey)alias;
	}

	public void TreatedValue(CLIValueOrder? valueOrder, TokenList? list) {
		ExceptionMessages.ThrowIfNull(list, nameof(list));
		list.Move();
		CLIParse.GetFunction<Action<CLIKey, CLIValueOrder?, TokenList?>>(1u)(alias, valueOrder, list);
	}

	public bool IsTypeCode(long typeCode) => IsTypeCode((TaskListTokens)typeCode);

	public bool IsTypeCode(TaskListTokens typeCode)
		=> ((TaskListTokens)TypeCode).HasFlag(typeCode);

	private static string GetAlias(string? alias) {
		ExceptionMessages.ThrowIfNullOrEmpty(alias, nameof(alias));
		return $"{{ARG}}/{alias}";
	}
}
