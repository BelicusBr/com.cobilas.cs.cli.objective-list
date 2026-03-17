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

	public TaskListOption(string? alias, bool mandatory) : this(alias, mandatory, []) { }
	public TaskListOption(string? alias, params IArgument[]? arguments) : this(alias, true, arguments) { }

	public bool Analyzer(TokenList? list, ErrorMessage? message) {
		ExceptionMessages.ThrowIfNull(list, nameof(list));
		ExceptionMessages.ThrowIfNull(message, nameof(message));
		if (arguments is not null) {
			if (list.CurrentValue != TypeCode) {
				if (Mandatory) {
					ExceptionMessage(list.Current, message);
					return true;
				}
			} else if (!IsAlias(list.CurrentKey))
				if (Mandatory) {
					ExceptionMessage(list.Current, message);
					return true;
				}
			list.Move();
			foreach (IArgument item in arguments) {
				if (item.Analyzer(list, message))
					return true;
			}
		}
		return false;
	}

	public void DefaultValue(CLIValueOrder? valueOrder) {
		if (arguments.Count != 0) {
			foreach (var item in arguments)
				item.DefaultValue(valueOrder);
		} else CLIParse.GetFunction<Action<CLIKey, CLIValueOrder?>>(0u)(alias, valueOrder);
	}

	public void ExceptionMessage(KeyValuePair<string, long> value, ErrorMessage? message) {
		ExceptionMessages.ThrowIfNull(message, nameof(message));
		if (value.Value == (long)CLIDefaultToken.Function) {
			message.ErroCode = 27;
			message.Message = $"The element '({(CLIDefaultToken)value.Value})[{value.Key}]' is not an option!!!";
		} else {
			message.ErroCode = 22;
			message.Message = $"The element '({(CLIDefaultToken)value.Value}){value.Key}' is called before '({(CLIDefaultToken)TypeCode}){alias}'!!!";
		}
	}

	public bool IsAlias(string? alias) {
		ExceptionMessages.ThrowIfNullOrEmpty(alias, nameof(alias));
		return this.alias == (CLIKey)alias;
	}

	public void TreatedValue(CLIValueOrder? valueOrder, TokenList? list) {
		if (arguments.Count != 0) { 
			foreach (var item in arguments)
				item.TreatedValue(valueOrder, list);
		} else CLIParse.GetFunction<Action<CLIKey, CLIValueOrder?, TokenList?>>(1u)(alias, valueOrder, list);
	}

	public bool IsTypeCode(long typeCode) => IsTypeCode((TaskListTokens)typeCode);

	public bool IsTypeCode(TaskListTokens typeCode)
		=> ((TaskListTokens)TypeCode).HasFlag(typeCode);
}
