using System;
using Cobilas.CLI.Manager;
using System.Collections.Generic;
using Cobilas.CLI.Manager.Interfaces;
using Cobilas.CLI.ObjectiveList.Interfaces;

namespace Cobilas.CLI.ObjectiveList.Elements;

public readonly struct TaskListOptionEnd(string? alias, bool mandatory) : IOptionFunc, ITypeCode {
	private readonly bool mandatory = mandatory;
	private readonly CLIKey alias = alias ?? throw new ArgumentNullException(nameof(alias));

	public string Alias => alias;
	public bool Mandatory => mandatory;
	public long TypeCode => (long)(TaskListTokens.Option | TaskListTokens.EndCode);

	public TaskListOptionEnd(string? alias) : this(alias, true) { }

	public bool Analyzer(TokenList? list, ErrorMessage? message) {
		ExceptionMessages.ThrowIfNull(list, nameof(list));
		ExceptionMessages.ThrowIfNull(message, nameof(message));
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
		return false;
	}

	public void DefaultValue(CLIValueOrder? valueOrder)
		=> CLIParse.GetFunction<Action<CLIKey, CLIValueOrder?>>(0u)(alias, valueOrder);

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

	public void TreatedValue(CLIValueOrder? valueOrder, TokenList? list)
		=> CLIParse.GetFunction<Action<CLIKey, CLIValueOrder?, TokenList?>>(1u)(alias, valueOrder, list);

	public bool IsTypeCode(long typeCode) => IsTypeCode((TaskListTokens)typeCode);

	public bool IsTypeCode(TaskListTokens typeCode)
		=> ((TaskListTokens)TypeCode).HasFlag(typeCode);
}
