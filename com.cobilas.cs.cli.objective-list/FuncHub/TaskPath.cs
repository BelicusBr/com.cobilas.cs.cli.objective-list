using System;
using System.Text;
using Cobilas.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Cobilas.CLI.ObjectiveList.FuncHub;

internal readonly struct TaskPath : IComparable<TaskPath>, IComparable, IEquatable<TaskPath> {
	private readonly int[] path;

	internal int[] Cells => path;
	internal string Path => string.Join('.', path);
	internal int CellCount => ArrayManipulation.ArrayLength(path);

	internal int this[int index] {
		get => path[index];
		set => path[index] = value;
	}

	internal TaskPath(int[] path) => this.path = path;

	internal TaskPath(string path) : this(GetDimension(path)) { }

	internal TaskPath(int path) : this([path]) { }

	public int CompareTo(object? obj) {
		if (obj is TaskPath tskp)
			return CompareTo(tskp);
		return 0;
	}

	public int CompareTo(TaskPath other) {
		int count = path.Length >= other.path.Length ? other.path.Length : path.Length;
		for (int I = 0; I < count; I++) {
			if (path[I] != other.path[I]) {
				if (path[I] > other.path[I])
					return 1;
				else return -1;
			}
		}
		return path.Length > other.path.Length ? 1 :
			path.Length < other.path.Length ? -1 : 0;
	}

	internal bool IsBlock(TaskPath other) {
		int num1 = other.path.Length > path.Length ? other.path.Length : path.Length;
		int num2 = other.path.Length < path.Length ? other.path.Length : path.Length;

		if ((num1 - num2) > 1)
			return false;

		for (int I = 0; I < num2; I++)
			if (path[I] != other.path[I])
				return false;
		return true;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
		=> obj is TaskPath tkp && Equals(tkp);

	public override int GetHashCode() => base.GetHashCode();

	public bool Equals(TaskPath other) {
		if (other.CellCount != CellCount)
			return false;
		for (int I = 0; I < CellCount; I++)
			if (other.path[I] != path[I])
				return false;
		return true;
	}

	public override string ToString() => Path;

	public string ToString(int cellCount) {
		ExceptionMessages.ThrowIfGreaterThan(cellCount, CellCount, nameof(cellCount));
		StringBuilder builder = new();
		for (int I = 0; I < cellCount; I++)
			builder.Append(path[I]).Append('.');
		return builder.ToString().TrimEnd('.');
	}

	internal static int[] GetDimension(string path) {
		string[] paths = path.Split('.', StringSplitOptions.RemoveEmptyEntries);
		return Array.ConvertAll(paths, stg => int.Parse(stg));
	}

	public static bool operator ==(TaskPath A, TaskPath B) => A.Equals(B);
	public static bool operator !=(TaskPath A, TaskPath B) => !A.Equals(B);

	public static bool operator >(TaskPath A, TaskPath B) => A.CompareTo(B) > 0;
	public static bool operator <(TaskPath A, TaskPath B) => A.CompareTo(B) < 0;
	public static bool operator >=(TaskPath A, TaskPath B) => A.CompareTo(B) >= 0;
	public static bool operator <=(TaskPath A, TaskPath B) => A.CompareTo(B) <= 0;
}
