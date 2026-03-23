using System;
using System.Reflection;
using Cobilas.CLI.Manager;
using System.Collections.Generic;

namespace Cobilas.CLI.ObjectiveList.FuncHub;

internal static class GlobalFunctionHub {

	internal const string CLOVOFuncKey = "function";
	private const BindingFlags flags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

	internal static event Func<string?, bool>? EventAnalyzerArguments;
	internal static event Action<CLIKey, CLIValueOrder?>? EventDefaultValue;
	internal static event Func<CLIKey, int, string?>? EventOptionArgumentName;
	internal static event Action<CLIKey, CLIValueOrder?>? EventGenericFunction;
	internal static event Action<CLIKey, CLIValueOrder?, TokenList?>? EventTreatedValue;
	internal static event Action<CLIKey, KeyValuePair<string, long>, ErrorMessage?>? EventInvalidArgument;

	internal static void CallInitializers() {
		Assembly assembly = Assembly.GetExecutingAssembly();
		foreach (Type type in assembly.GetTypes()) {
			CallMethodAttribute? attribute = type.GetAttribute<CallMethodAttribute>();
			if (attribute is null) continue;
			MethodInfo? method = type.GetMethod(attribute.TargetMethod, flags);
			if (method is null) {
				Printer.PrintException($"The function '{attribute.TargetMethod}' was not found in the class '{type.FullName}'!");
				continue;
			}
			_ = method.Invoke(null, null);
		}
	}

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
