using System;
using System.Text;
using Cobilas.Collections;

namespace Cobilas.CLI.ObjectiveList {
    internal struct ElementPath : IDisposable, IEquatable<ElementPath> {
        private int[] indexs;

        public int Cell => ArrayManipulation.ArrayLength(indexs);

        public static ElementPath Root => new ElementPath("0");
        public static ElementPath Empty => new ElementPath(string.Empty);

        public ElementPath(string path) {
            indexs = Array.Empty<int>();
            string[] paths = path.Split('.', StringSplitOptions.RemoveEmptyEntries);
            for (int I = 0; I < paths.Length; I++)
                ArrayManipulation.Add(int.Parse(paths[I]), ref indexs);
        }

        public override string ToString() {
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

        public static bool IsChild(ElementPath parent, ElementPath child)
            => parent == GetParent(child);

        public static ElementPath GetParent(ElementPath target) {
            int[] indexs = (int[])target.indexs.Clone();
            Array.Resize(ref indexs, target.indexs.Length - 1);
            return new ElementPath() {
                indexs = indexs
            };
        }

        public static bool operator ==(ElementPath A, ElementPath B) => A.Equals(B);
        public static bool operator !=(ElementPath A, ElementPath B) => !(A == B);
    }
}
