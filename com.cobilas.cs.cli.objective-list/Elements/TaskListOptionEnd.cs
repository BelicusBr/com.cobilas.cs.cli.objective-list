using System;
using Cobilas.CLI.Manager;
using System.Collections.Generic;
using Cobilas.CLI.Manager.Interfaces;
using Cobilas.CLI.ObjectiveList.Interfaces;

namespace Cobilas.CLI.ObjectiveList.Elements;

public readonly struct TaskListOptionEnd : IOptionFunc, ITypeCode {
	public bool Mandatory => throw new NotImplementedException();

	public string Alias => throw new NotImplementedException();

	public long TypeCode => throw new NotImplementedException();

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
