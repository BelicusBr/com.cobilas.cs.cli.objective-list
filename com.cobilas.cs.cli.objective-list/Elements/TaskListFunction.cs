using System;
using Cobilas.CLI.Manager;
using System.Collections.Generic;
using Cobilas.CLI.Manager.Interfaces;
using Cobilas.CLI.ObjectiveList.Interfaces;

namespace Cobilas.CLI.ObjectiveList.Elements;

public readonly struct TaskListFunction(string? alias, params IOptionFunc[]? options) : IFunction, ITypeCode {
	private readonly CLIValueOrder valueOrder = [];
	private readonly CLIKey alias = alias ?? throw new ArgumentNullException(nameof(alias));
	private readonly List<IOptionFunc> options = [.. options ?? throw new ArgumentNullException(nameof(options))];

	public string Alias => alias;
	public List<IOptionFunc> Options => options;
	public CLIValueOrder ValueOrder => valueOrder;
	public long TypeCode => (long)TaskListTokens.Function;

	public bool Analyzer(TokenList? list, ErrorMessage? message) {
		ExceptionMessages.ThrowIfNull(list, nameof(list));
		ExceptionMessages.ThrowIfNull(message, nameof(message));
		list.Move();
		TaskListTokens tokens;
		for (int I = 0; I < options.Count; I++) {
			IOptionFunc opc = options[I];
			tokens = (TaskListTokens)list.CurrentValue;
			if (opc.IsAlias(list.CurrentKey)) {
				if (opc.Analyzer(list, message))
					return true;
				else {
					if (tokens.HasFlag(TaskListTokens.EndCode)) {
						list.Move();
						break;
					}
				}
				break;
			}
		}

		tokens = (TaskListTokens)list.CurrentValue;
		if (!tokens.HasFlag(TaskListTokens.EndCode)) {
			if (list.CurrentValue == CLIParse.ArgumentCode) {
				message.ErroCode = 74;
				message.Message = $"The argument is not defined for the function ({alias})!";
			} else if (list.CurrentValue == (long)TaskListTokens.Option) {
				message.ErroCode = 75;
				message.Message = $"The option ({list.CurrentKey}) is not defined for the function ({alias})!";
			}
			return true;
		}
		return false;
	}

	public bool GetValues(TokenList? list, ErrorMessage? message) {
		ExceptionMessages.ThrowIfNull(list, nameof(list));
		ExceptionMessages.ThrowIfNull(message, nameof(message));

		for (int I = 0; I < options.Count; I++) {
			IOptionFunc of = options[I];
			if (of.AliasIsTypeCode(list.CurrentValue)) {
				if (of.IsAlias(list.CurrentKey) || of.IsAlias("{ARG}")) {
					of.TreatedValue(valueOrder, list);
					list.Move();
				}
			} else {
				if (of.Mandatory) {
					of.ExceptionMessage(list.Current, message);
					return true;
				}
				else of.DefaultValue(valueOrder);
			}
		}
		return false;
	}

	public bool IsAlias(string? alias) {
		ExceptionMessages.ThrowIfNull(alias, nameof(alias));
		return this.alias == alias;
	}

	public void Run()
		=> Run(CLIParse.GetFunction<Action<CLIKey, CLIValueOrder?>>(4));

	public void Run(Action<CLIKey, CLIValueOrder?>? action)
		=> action?.Invoke(alias, valueOrder);

	public bool IsTypeCode(long typeCode) => IsTypeCode((TaskListTokens)typeCode);

	public bool IsTypeCode(TaskListTokens typeCode)
		=> ((TaskListTokens)TypeCode).HasFlag(typeCode);
}
