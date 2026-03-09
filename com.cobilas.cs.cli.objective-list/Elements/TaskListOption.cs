using System;
using Cobilas.CLI.Manager;
using System.Collections.Generic;
using Cobilas.CLI.Manager.Interfaces;
using Cobilas.CLI.ObjectiveList.Interfaces;

namespace Cobilas.CLI.ObjectiveList.Elements;

public readonly struct TaskListOption(string? alias, bool mandatory, params IArgument[]? arguments) : IOption, ITypeCode {
	private readonly bool mandatory = mandatory;
	private readonly CLIKey alias = alias ?? throw new ArgumentNullException(nameof(alias));
	private readonly List<IArgument> arguments = [.. arguments ?? throw new ArgumentNullException(nameof(arguments))];

	public string Alias => alias;
	public bool Mandatory => mandatory;
	public List<IArgument>? Options => arguments;
	public long TypeCode => (long)TaskListTokens.Option;

	public bool Analyzer(TokenList? list, ErrorMessage? message)
	{
		throw new NotImplementedException();
	}

	public void DefaultValue(CLIValueOrder? valueOrder)
	{
		throw new NotImplementedException();
	}

	public void ExceptionMessage(KeyValuePair<string, long> value, ErrorMessage? message)
	{
		throw new NotImplementedException();
	}

	public bool IsAlias(string? alias)
	{
		throw new NotImplementedException();
	}

	public void TreatedValue(CLIValueOrder? valueOrder, TokenList? list)
	{
		throw new NotImplementedException();
	}

	public bool IsTypeCode(long typeCode) => IsTypeCode((TaskListTokens)typeCode);

	public bool IsTypeCode(TaskListTokens typeCode)
		=> ((TaskListTokens)TypeCode).HasFlag(typeCode);
}
