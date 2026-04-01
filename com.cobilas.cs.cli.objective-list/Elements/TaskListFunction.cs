using System;
using Cobilas.CLI.Manager;
using System.Collections.Generic;
using Cobilas.CLI.Manager.Interfaces;
using Cobilas.CLI.ObjectiveList.FuncHub;
using Cobilas.CLI.ObjectiveList.Interfaces;

namespace Cobilas.CLI.ObjectiveList.Elements;

internal readonly struct TaskListFunction(string? alias, TaskListTokens token, params IOptionFunc[]? options) : IFunction, ITypeCode, ICLIAnalyzer {
	private readonly TaskListTokens token = token;
	private readonly CLIValueOrder valueOrder = [];
	private readonly CLIKey alias = alias ?? throw new ArgumentNullException(nameof(alias));
	private readonly List<IOptionFunc> options = [.. options ?? throw new ArgumentNullException(nameof(options))];

	public string Alias => alias;
	public long TypeCode => (long)token;
	public List<IOptionFunc> Options => options;
	public CLIValueOrder ValueOrder => valueOrder;

	public bool Analyzer(TokenList? list, ErrorMessage? message) {
		ExceptionMessages.ThrowIfNull(list, nameof(list));
		ExceptionMessages.ThrowIfNull(message, nameof(message));
		TaskListTokens tokens;
		list.Move();
		if (options.Count != 0) { 
			for (int I = 0; I < options.Count; I++) {
				IOptionFunc opc = options[I];
				tokens = (TaskListTokens)list.CurrentValue;
				if (opc.IsAlias(list.CurrentKey) || (opc.IsAlias("{ARG}") && opc.AliasIsTypeCode(tokens))) {
					if (((ICLIAnalyzer)opc).Analyzer(list, message))
						return true;
					if (opc.AliasIsTypeCode(TaskListTokens.Option | TaskListTokens.EndCode))
						I = Jump(opc as IFunctionJump, options.Count, I);
					list.Move();
				} else {
					if (!opc.AliasIsTypeCode(TaskListTokens.Option | TaskListTokens.EndCode))
						I = Jump(opc as IFunctionJump, options.Count, I);
				}
			}
		}

		tokens = (TaskListTokens)list.CurrentValue;
		if (!tokens.HasFlag(TaskListTokens.EndCode)) {
			if (list.CurrentValue == CLIParse.ArgumentCode) {
				message.ErroCode = 743;
				message.Message = $"The argument is not defined for the function ({alias})!";
			} else if (tokens.HasFlag(TaskListTokens.Option)) {
				message.ErroCode = 753;
				message.Message = $"The option ({list.CurrentKey}) is not defined for the function ({alias})!";
			}
			return true;
		}
		return false;
	}

	public bool GetValues(TokenList? list, ErrorMessage? message) {
		ExceptionMessages.ThrowIfNull(list, nameof(list));
		ExceptionMessages.ThrowIfNull(message, nameof(message));

		list.Move();
		valueOrder.Add(GlobalFunctionHub.CLOVOFuncKey, alias);
		for (int I = 0; I < options.Count; I++) {
			IOptionFunc of = options[I];
			TaskListTokens tokens = (TaskListTokens)list.CurrentValue;
			if (of.IsAlias(list.CurrentKey) || (of.IsAlias("{ARG}") && of.AliasIsTypeCode(tokens))) {
				of.TreatedValue(valueOrder, list);
				if (of.AliasIsTypeCode(TaskListTokens.Option | TaskListTokens.EndCode))
					I = Jump(of as IFunctionJump, options.Count, I);
				list.Move();
			} else {
				if (of.Mandatory) {
					of.ExceptionMessage(list.Current, message);
					return true;
				} else of.DefaultValue(valueOrder);
				if (!of.AliasIsTypeCode(TaskListTokens.Option | TaskListTokens.EndCode))
					I = Jump(of as IFunctionJump, options.Count, I);
			}
		}
		return false;
	}

	public bool IsAlias(string? alias) {
		ExceptionMessages.ThrowIfNull(alias, nameof(alias));
		if (alias == string.Empty) return false;
		return this.alias == (CLIKey)alias;
	}

	public void Run() => Run(GlobalFunctionHub.GenericFunction);

	public void Run(Action<CLIKey, CLIValueOrder?>? action)
		=> action?.Invoke(alias, valueOrder);

	public bool IsTypeCode(long typeCode) => IsTypeCode((TaskListTokens)typeCode);

	public bool IsTypeCode(TaskListTokens typeCode)
		=> ((TaskListTokens)TypeCode).HasFlag(typeCode);

	private static int Jump(IFunctionJump? jump, int count, int index) {
		if (jump is not null)
			if (jump.JumpAll) {
				jump.SetJumpInGetValue(jump.JumpAll);
				index = count;
			} else if (jump.CountJump != 0) {
				jump.SetJumpInGetValue(true);
				index += jump.CountJump;
			}
		return index;
	}
}
