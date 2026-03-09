using System;
using Cobilas.CLI.Manager;

namespace Cobilas.CLI.ObjectiveList.FuncHub;

public static class GlobalFunctionHub {

	public static event Action<CLIValueOrder?>? EventDefaultValue;
	public static event Action<CLIValueOrder?, TokenList?>? EventTreatedValue;

	public static void DefaultValue(CLIValueOrder? valueOrder)
		=> EventDefaultValue?.Invoke(valueOrder);

	public static void TreatedValue(CLIValueOrder? valueOrder, TokenList? list)
		=> EventTreatedValue?.Invoke(valueOrder, list);

	public static void ClearEvents() {
		EventDefaultValue = null;
		EventTreatedValue = null;
	}
}
