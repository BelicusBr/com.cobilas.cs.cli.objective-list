using System;
using System.Diagnostics.CodeAnalysis;

namespace Cobilas.CLI.ObjectiveList.FuncHub;

internal readonly struct TaskPath : IComparable<TaskPath>, IComparable, IEquatable<TaskPath> {
	private readonly int[] path;
	private readonly string _path;

	public TaskPath(string path) { 
		_path = path;
		this.path = GetDimension(path);
	}

	public TaskPath(int[] path) {
		this.path = path;
		_path = string.Join('.', path);
	}

	public TaskPath(int path) {
		this.path = [path];
		_path = string.Join('.', path);
	}

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

	public bool IsBlock(TaskPath other) {
		if (other.path.Length < path.Length)
			return false;
		for (int I = 0; I < path.Length; I++)
			if (path[I] != other.path[I])
				return false;
		return true;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
		=> obj is TaskPath tkp && Equals(tkp);

	public override int GetHashCode() => base.GetHashCode();

	public bool Equals(TaskPath other)
		=> _path == other._path;

	public override string ToString() => _path;

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
