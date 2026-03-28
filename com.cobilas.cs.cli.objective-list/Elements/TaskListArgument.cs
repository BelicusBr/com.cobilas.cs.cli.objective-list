using System;
using Cobilas.CLI.Manager;
using System.Collections.Generic;
using Cobilas.CLI.Manager.Interfaces;
using Cobilas.CLI.ObjectiveList.Interfaces;
using Cobilas.CLI.ObjectiveList.FuncHub;

namespace Cobilas.CLI.ObjectiveList.Elements;

internal readonly struct TaskListArgument(string? alias, bool mandatory) : IArgument, ITypeCode {
	private readonly bool mandatory = mandatory;
	private readonly CLIKey alias = GetAlias(alias);

	public string Alias => alias;
	public bool Mandatory => mandatory;
	public long TypeCode => CLIParse.ArgumentCode;

	public bool Analyzer(TokenList? list, ErrorMessage? message) {
		ExceptionMessages.ThrowIfNull(list, nameof(list));
		ExceptionMessages.ThrowIfNull(message, nameof(message));
		if (list.CurrentValue != TypeCode) {
			if (!mandatory) return false;
			ExceptionMessage(list.Current, message);
			return true;
		} else if (GlobalFunctionHub.AnalyzerArguments(list.CurrentKey)) {
			ExceptionMessage(list.Current, message);
			return true;
		}
		return false;
	}

	public void DefaultValue(CLIValueOrder? valueOrder)
		=> GlobalFunctionHub.DefaultValue(alias, valueOrder);

	public void ExceptionMessage(KeyValuePair<string, long> value, ErrorMessage? message) {
		ExceptionMessages.ThrowIfNull(message, nameof(message));
		TaskListTokens tokens = (TaskListTokens)value.Value;
		if (tokens.HasFlag(TaskListTokens.Option)) { }
		else if (tokens.HasFlag(TaskListTokens.Function)) { }
		else if (tokens.HasFlag(TaskListTokens.EndCode)) { }
		else GlobalFunctionHub.InvalidArgument(alias, value, message);
	}

	public bool IsAlias(string? alias) {
		ExceptionMessages.ThrowIfNull(alias, nameof(alias));
		if (alias == string.Empty) return false;
		return this.alias == (CLIKey)alias;
	}

	public void TreatedValue(CLIValueOrder? valueOrder, TokenList? list) 
		=> GlobalFunctionHub.TreatedValue(alias, valueOrder, list);

	public bool IsTypeCode(long typeCode) => IsTypeCode((TaskListTokens)typeCode);

	public bool IsTypeCode(TaskListTokens typeCode)
		=> ((TaskListTokens)TypeCode).HasFlag(typeCode);

	private static string GetAlias(string? alias) {
		ExceptionMessages.ThrowIfNullOrEmpty(alias, nameof(alias));
		return $"{{ARG}}/{alias}";
	}
}
