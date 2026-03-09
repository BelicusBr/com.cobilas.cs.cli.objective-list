using System;
using Cobilas.CLI.Manager.Interfaces;
using Cobilas.CLI.ObjectiveList.Interfaces;

namespace Cobilas.CLI.ObjectiveList;

public static class IAliasExtension {
	public static bool AliasIsTypeCode(this IAlias? a, long typeCode) => AliasIsTypeCode(a, (TaskListTokens)typeCode);

	public static bool AliasIsTypeCode(this IAlias? a, TaskListTokens typeCode) { 
		ExceptionMessages.ThrowIfNull(a, nameof(a));
		ITypeCode? type = a as ITypeCode ?? throw new InvalidCastException($"The type '{a.GetType()}' does not inherit the interface '{nameof(ITypeCode)}'!");

		return type.IsTypeCode(typeCode);
	}
}
