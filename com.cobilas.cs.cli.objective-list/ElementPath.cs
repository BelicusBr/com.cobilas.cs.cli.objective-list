using System;
using System.Text;
using System.Linq;
using Cobilas.Collections;

namespace Cobilas.CLI.ObjectiveList {
    internal struct ElementPath : IDisposable, IEquatable<ElementPath>, IComparable<ElementPath> {
        private int[] indexs;

        public int[] Indexs => indexs;
        public int Cell => ArrayManipulation.ArrayLength(indexs);

        public static ElementPath Root => new ElementPath("-1");
        public static ElementPath Empty => new ElementPath(string.Empty);

        public int this[int index] => indexs[index];

        public ElementPath(string path) {
            indexs = Array.Empty<int>();
            string[] paths = path.Split('.', StringSplitOptions.RemoveEmptyEntries);
            for (int I = 0; I < paths.Length; I++)
                ArrayManipulation.Add(int.Parse(paths[I]), ref indexs);
        }

        public override string ToString() {
            if (Cell == 0) return string.Empty;
            StringBuilder builder = new StringBuilder();
            foreach (int item in indexs)
                builder.AppendFormat("{0}.", item);
            return builder.ToString().TrimEnd('.');
        }

        public void Dispose()
            => ArrayManipulation.ClearArraySafe(ref indexs);

        public override int GetHashCode() => base.GetHashCode();

        public override bool Equals(object obj)
            => obj is ElementPath eph && Equals(eph);

        public bool Equals(ElementPath other) {
            if (other.indexs.Length != indexs.Length) return false;
            for (int I = 0; I < indexs.Length; I++)
                if (other.indexs[I] != indexs[I])
                    return false;
            return true;
        }

        public int CompareTo(ElementPath other) {
            if (Cell < other.Cell) return -1;
            else if (Cell > other.Cell) return 1;
            return indexs[Cell - 1] > other.indexs[Cell - 1] ? 1 : (indexs[Cell - 1] < other.indexs[Cell - 1] ? -1 : 0);
        }

        public static bool ItsValid(string path) {
            if (string.IsNullOrEmpty(path)) return false;
            else if (path.Length == 0) return false;
            return path.All((c) => char.IsNumber(c) || c == '.') && char.IsNumber(path[0]) && char.IsNumber(path[path.Length - 1]);
        }

        public static bool IsChild(ElementPath parent, ElementPath child)
            => parent == GetParent(child);

        public static ElementPath GetParent(ElementPath target) {
            int count = target.indexs.Length - 1;
            if (count <= 0) return ElementPath.Root;
            int[] indexs = (int[])target.indexs.Clone();
            Array.Resize(ref indexs, target.indexs.Length - 1);
            return new ElementPath() {
                indexs = indexs
            };
        }

        public static explicit operator ElementPath(string stg) => new ElementPath(stg);
        public static explicit operator string(ElementPath stg) => stg.ToString();

        public static bool operator ==(ElementPath A, ElementPath B) => A.Equals(B);
        public static bool operator !=(ElementPath A, ElementPath B) => !(A == B);

        public static bool operator >(ElementPath A, ElementPath B) => A.CompareTo(B) > 0;
        public static bool operator <(ElementPath A, ElementPath B) => A.CompareTo(B) < 0;
    }
}
