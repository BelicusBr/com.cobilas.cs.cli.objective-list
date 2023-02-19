using System;
using System.IO;
using System.Xml;
using System.Collections;
using Cobilas.Collections;
using System.Collections.Generic;

namespace Cobilas.CLI.ObjectiveList {
    internal class OTVL_ElementList : IDisposable, IReadOnlyList<OTVL_Element>, ICollection<OTVL_Element> {
        private bool disposedValue;
        private OTVL_Element[] elements;
        private readonly string filePath;

        public int Count => ArrayManipulation.ArrayLength(elements);
        public bool IsReadOnly => elements != null && elements.IsReadOnly;

        public OTVL_Element this[int index] => elements[index];
        public OTVL_Element this[string path] => elements[IndexOf(path)];

        public OTVL_ElementList(string filePath) {
            elements = Array.Empty<OTVL_Element>();
            this.filePath = filePath;
            Load();
        }

        ~OTVL_ElementList()
            => Dispose(disposing: false);

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public IEnumerator<OTVL_Element> GetEnumerator() {
            for (int I = 0; I < Count; I++)
                yield return this[I];
        }

        protected virtual void Dispose(bool disposing)  {
            if (!disposedValue) {
                if (disposing) {
                    Clear();
                }
                disposedValue = true;
            }
        }

        public void Add(OTVL_Element element)
            => ArrayManipulation.Add(element, ref elements);

        public bool Remove(string path) {
            if (!Contains(path)) return false;
            ArrayManipulation.Remove(IndexOf(path), ref elements);
            return true;
        }

        public bool Contains(string path)
            => IndexOf(path) >= 0;

        public int IndexOf(string path) {
            ElementPath elementPath = new ElementPath(path);
            for (int I = 0; I < Count; I++)
                if (this[I].path == elementPath)
                    return I;
            return -1;
        }

        public void Clear()
            => ArrayManipulation.ClearArraySafe(ref elements);

        public void UnLoad() {
            using FileStream stream = File.OpenWrite(filePath); stream.SetLength(0L);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\r\n";
            using XmlWriter writer = XmlWriter.Create(stream, settings);
            writer.WriteElementTag((ElementTag)this);
        }

        private void Load() {
            using FileStream stream = File.OpenRead(filePath);
            using XmlReader reader = XmlReader.Create(stream);
            reader.GetElementTag().ForEach((ElementTag e) => {
                if (e.Name == "element") {
                    OTVL_Element element = new OTVL_Element();
                    Add(element);
                    e.ForEach((ElementAttribute a) => {
                        switch (a.Name) {
                            case "path": element.path = new ElementPath(a.Value.ValueToString); break;
                            case "status": element.status = a.Value.ValueToBool; break;
                        }
                    });
                    e.ForEach((ElementTag e1) => {
                        switch (e1.Name) {
                            case "title": element.title = e1.Value.ValueToString; break;
                            case "descriptio": element.description = e1.Value.ValueToString; break;
                        }
                    });
                }
            });
        }

        IEnumerator IEnumerable.GetEnumerator() => (IEnumerator)this;
        bool ICollection<OTVL_Element>.Remove(OTVL_Element item) => false;
        bool ICollection<OTVL_Element>.Contains(OTVL_Element item) => false;
        void ICollection<OTVL_Element>.CopyTo(OTVL_Element[] array, int arrayIndex) { }

        public static explicit operator ElementTag(OTVL_ElementList A) {
            ElementTag res = new ElementTag("Root");
            foreach (OTVL_Element item in A)
                res.Add((ElementTag)item);
            return res;
        }
    }
}
