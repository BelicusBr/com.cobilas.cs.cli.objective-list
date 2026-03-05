using System;
using Cobilas.CLI.Manager;
using System.Collections.Generic;
using Cobilas.CLI.Manager.Interfaces;

namespace Cobilas.CLI.ObjectiveList.Elements;

public readonly struct TaskListFunction : IFunction {
	public CLIValueOrder ValueOrder => throw new NotImplementedException();

	public List<IOptionFunc> Options => throw new NotImplementedException();

	public string Alias => throw new NotImplementedException();

	public long TypeCode => throw new NotImplementedException();

	public bool Analyzer(TokenList? list, ErrorMessage? message)
	{
		throw new NotImplementedException();
	}

	public bool GetValues(TokenList? list, ErrorMessage? message)
	{
		throw new NotImplementedException();
	}

	public bool IsAlias(string? alias)
	{
		throw new NotImplementedException();
	}

	public void Run()
	{
		throw new NotImplementedException();
	}

	public void Run(Action<CLIKey, CLIValueOrder?>? action)
	{
		throw new NotImplementedException();
	}
}
