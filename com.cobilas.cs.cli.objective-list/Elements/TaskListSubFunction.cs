using System;
using Cobilas.CLI.Manager;
using System.Collections.Generic;
using Cobilas.CLI.Manager.Interfaces;
using Cobilas.CLI.ObjectiveList.Interfaces;

namespace Cobilas.CLI.ObjectiveList.Elements;

public readonly struct TaskListSubFunction(string? alias, bool mandatory, params IOptionFunc[]? options) : IOptionFunc, ITypeCode {
	public bool Mandatory => mandatory;
	private readonly CLIValueOrder valueOrder = [];
	private readonly CLIKey alias = alias ?? throw new ArgumentNullException(nameof(alias));
	private readonly List<IOptionFunc> options = [.. options ?? throw new ArgumentNullException(nameof(options))];

	public string Alias => alias;
	public List<IOptionFunc> Options => options;
	public CLIValueOrder ValueOrder => valueOrder;
	public long TypeCode => (long)TaskListTokens.Function;

	public TaskListSubFunction(string? alias, params IOptionFunc[]? options) : this(alias, true, options) { }

	public bool Analyzer(TokenList? list, ErrorMessage? message) {
		ExceptionMessages.ThrowIfNull(list, nameof(list));
		ExceptionMessages.ThrowIfNull(message, nameof(message));
		TaskListTokens tokens;
		list.Move();
		for (int I = 0; I < options.Count; I++) {
			IOptionFunc opc = options[I];
			tokens = (TaskListTokens)list.CurrentValue;
			TaskDebug.Print($"[TLSF]{opc.Alias}|{list.CurrentKey}|{tokens}");
			if (opc.IsAlias(list.CurrentKey)) {
				if (opc.Analyzer(list, message))
					return true;
				else {
					if (tokens.HasFlag(TaskListTokens.EndCode))
						break;
				}
			}
		}

		tokens = (TaskListTokens)list.CurrentValue;
		TaskDebug.Print($"[P-TLSF]{list.CurrentKey}|{tokens}");
		if (!tokens.HasFlag(TaskListTokens.EndCode)) {
			((IOptionFunc)this).ExceptionMessage(list.Current, message);
			return true;
		}
		TaskDebug.Print($"[TLSF]{list.CurrentKey}|{tokens}");
		return false;
	}

	public bool GetValues(TokenList? list, ErrorMessage? message) {
		ExceptionMessages.ThrowIfNull(list, nameof(list));
		ExceptionMessages.ThrowIfNull(message, nameof(message));

		for (int I = 0; I < options.Count; I++) {
			IOptionFunc of = options[I];
			if (of.AliasIsTypeCode(list.CurrentValue)) {
				if (of.IsAlias(list.CurrentKey) || of.IsAlias("{ARG}")) {
					list.Move();
					of.TreatedValue(valueOrder, list);
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
		return this.alias == (CLIKey)alias;
	}

	public bool IsTypeCode(long typeCode) => IsTypeCode((TaskListTokens)typeCode);

	public bool IsTypeCode(TaskListTokens typeCode)
		=> ((TaskListTokens)TypeCode).HasFlag(typeCode);

	void IOptionFunc.DefaultValue(CLIValueOrder? valueOrder)
		=> CLIParse.GetFunction<Action<CLIKey, CLIValueOrder?>>(0u)(alias, valueOrder);

	void IOptionFunc.TreatedValue(CLIValueOrder? valueOrder, TokenList? list) {
		ExceptionMessages.ThrowIfNull(list, nameof(list));
		ExceptionMessages.ThrowIfNull(valueOrder, nameof(valueOrder));

		Func<CLIKey, int, string?> optionArgName = CLIParse.GetFunction<Func<CLIKey, int, string?>>(6u);

		if (options is not null)
			for (int I = 0; I < options.Count; I++) {
				KeyValuePair<string, long> temp = list.GetValueAndMove;
				string name = optionArgName(alias, I) ?? "{none-name}";
				valueOrder.Add(name, temp.Key);
			}
	}

	void IOptionFunc.ExceptionMessage(KeyValuePair<string, long> value, ErrorMessage? message) {
		ExceptionMessages.ThrowIfNull(message, nameof(message));
		if (value.Value == CLIParse.ArgumentCode) {
			message.ErroCode = 74;
			message.Message = $"The argument is not defined for the function ({alias})!";
		} else if (value.Value == (long)TaskListTokens.Option) {
			message.ErroCode = 75;
			message.Message = $"The option ({value.Key}) is not defined for the function ({alias})!";
		}
	}
}
