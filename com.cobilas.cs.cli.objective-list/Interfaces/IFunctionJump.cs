namespace Cobilas.CLI.ObjectiveList.Interfaces;

internal interface IFunctionJump {
	bool JumpAll { get; }
	int CountJump { get; }
	bool JumpInGetValue { get; }
	void SetJumpInGetValue(bool value);
}
