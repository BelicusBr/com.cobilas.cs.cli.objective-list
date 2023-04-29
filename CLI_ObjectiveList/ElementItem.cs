using System.Collections;
using Cobilas.Collections;
using System.Collections.Generic;

namespace Cobilas.CLI.ObjectiveList {
    internal class ElementItem : IReadOnlyList<ElementItem>, IEnumerable<ElementItem> {
        public bool status;
        public string title;
        public ElementPath path;
        public string description;
        public ElementItem parent;
        private ElementItem[] elements;

        public ElementItem this[int index] => elements[index];
        public int Count => ArrayManipulation.ArrayLength(elements);
        public bool IsReadOnly => ArrayManipulation.IsReadOnlySafe(elements);

        public ElementItem(bool status, string title, string description) {
            this.status = status;
            this.title = title;
            this.description = description;
        }

        public ElementItem(bool status, string title) : this(status, title, string.Empty) { }

        public ElementItem(string title) : this(false, title) { }

        public void Add(ElementItem item) {
            item.parent = this;
            ArrayManipulation.Add(item, ref elements);
        }

        public bool Remove(int index) {
            if (index >= Count) return false;
            elements[index].parent = null;
            ArrayManipulation.Remove(index, ref elements);
            return true;
        }

        public void Clear()
            => ArrayManipulation.ClearArraySafe(ref elements);

        public IEnumerator<ElementItem> GetEnumerator()
            => new ArrayToIEnumerator<ElementItem>(elements);

        IEnumerator IEnumerable.GetEnumerator()
            => new ArrayToIEnumerator<ElementItem>(elements);
    }
}
