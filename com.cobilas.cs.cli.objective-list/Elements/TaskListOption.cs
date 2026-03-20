using Cobilas.CLI.Manager;
using Cobilas.CLI.Manager.Interfaces;
using Cobilas.CLI.ObjectiveList.FuncHub;
using Cobilas.CLI.ObjectiveList.Interfaces;
using System;
using System.Collections.Generic;

namespace Cobilas.CLI.ObjectiveList.Elements;

internal readonly struct TaskListOption(
	string? alias, 
	bool mandatory, 
	int countJump, 
	bool jumpAll,
	TaskListTokens token) : IOptionFunc, IFunctionJump, ITypeCode {
	private readonly bool jumpAll = jumpAll;
	private readonly CLIValueOrder iValue = [];
	private readonly int countJump = countJump;
	private readonly bool mandatory = mandatory;
	private readonly TaskListTokens token = token;
	private readonly CLIKey alias = alias ?? throw new ArgumentNullException(nameof(alias));

	public string Alias => alias;
	public bool JumpAll => jumpAll;
	public int CountJump => countJump;
	public bool Mandatory => mandatory;
	public long TypeCode => (long)token;
	public bool JumpInGetValue {
		get {
			if (iValue.ContainsKey("jump-gv"))
				return iValue["jump-gv"]?.ToLower() == "true";
			return false;
		}
	}

	public bool Analyzer(TokenList? list, ErrorMessage? message) {
		ExceptionMessages.ThrowIfNull(list, nameof(list));
		ExceptionMessages.ThrowIfNull(message, nameof(message));
		
		return false;
	}

	public void DefaultValue(CLIValueOrder? valueOrder)
		=> GlobalFunctionHub.DefaultValue(alias, valueOrder);

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
		ExceptionMessages.ThrowIfNull(alias, nameof(alias));
		if (alias == string.Empty) return false;
		return this.alias == (CLIKey)alias;
	}

	public void TreatedValue(CLIValueOrder? valueOrder, TokenList? list)
		=> GlobalFunctionHub.TreatedValue(alias, valueOrder, list);

	void IFunctionJump.SetJumpInGetValue(bool value) {
		if (!iValue.ContainsKey("jump-gv"))
			iValue.Add("jump-gv", bool.FalseString);
		iValue["jump-gv"] = value.ToString();
	}

	public bool IsTypeCode(long typeCode) => IsTypeCode((TaskListTokens)typeCode);

	public bool IsTypeCode(TaskListTokens typeCode)
		=> ((TaskListTokens)TypeCode).HasFlag(typeCode);
}
