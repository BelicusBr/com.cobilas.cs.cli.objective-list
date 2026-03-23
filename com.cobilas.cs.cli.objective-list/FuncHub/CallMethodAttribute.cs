using System;

namespace Cobilas.CLI.ObjectiveList.FuncHub;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
internal sealed class CallMethodAttribute(string targetMethod) : Attribute {
    private readonly string targetMethod = targetMethod;

    internal string TargetMethod => targetMethod;
}
