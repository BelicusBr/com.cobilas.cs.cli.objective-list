using System;
using Cobilas.CLI.Manager;
using System.Collections.Generic;

namespace Cobilas.CLI.ObjectiveList.FuncHub;

internal static class GlobalFunctionHub {

	internal const string CLOVOFuncKey = "function";

	internal static event Func<string?, bool>? EventAnalyzerArguments;
	internal static event Action<CLIKey, CLIValueOrder?>? EventDefaultValue;
	internal static event Func<CLIKey, int, string?>? EventOptionArgumentName;
	internal static event Action<CLIKey, CLIValueOrder?>? EventGenericFunction;
	internal static event Action<CLIKey, CLIValueOrder?, TokenList?>? EventTreatedValue;
	internal static event Action<CLIKey, KeyValuePair<string, long>, ErrorMessage?>? EventInvalidArgument;

	internal static void DefaultValue(CLIKey alias, CLIValueOrder? valueOrder)
		=> EventDefaultValue?.Invoke(alias, valueOrder);

	internal static void TreatedValue(CLIKey alias, CLIValueOrder? valueOrder, TokenList? list)
		=> EventTreatedValue?.Invoke(alias, valueOrder, list);

	internal static bool AnalyzerArguments(string? value) {
		if (EventAnalyzerArguments is null) return false;
		return EventAnalyzerArguments(value);
	}

	internal static void GenericFunction(CLIKey alis, CLIValueOrder? order)
		=> EventGenericFunction?.Invoke(alis, order);

	internal static void InvalidArgument(CLIKey alias, KeyValuePair<string, long> value, ErrorMessage? message)
		=> EventInvalidArgument?.Invoke(alias, value, message);

	internal static string? OptionArgumentName(CLIKey alias, int index)
		=> EventOptionArgumentName?.Invoke(alias, index);

	internal static void ClearEvents() {
		EventDefaultValue = null;
		EventTreatedValue = null;
		EventGenericFunction = null;
		EventInvalidArgument = null;
		EventAnalyzerArguments = null;
		EventOptionArgumentName = null;
	}
}
