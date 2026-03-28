using Cobilas.CLI.Manager;
using System.Collections.Generic;
using Cobilas.CLI.Manager.Exceptions;

namespace Cobilas.CLI.ObjectiveList.Elements;

public static class ErrorMessageList {

	public static void SetMessage(this ErrorMessage? e, KeyValuePair<int, string> message) {
		ExceptionMessages.ThrowIfNull(e, nameof(e));
		e.ErroCode = message.Key;
		e.Message = message.Value;
	}
}
