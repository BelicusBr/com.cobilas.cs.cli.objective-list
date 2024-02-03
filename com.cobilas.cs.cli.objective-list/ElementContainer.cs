using System;
using System.Xml;
using System.Collections;
using Cobilas.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cobilas.CLI.ObjectiveList {
    internal class ElementContainer : IDisposable, IReadOnlyList<ElementItem>, IEnumerable<ElementItem> {

        private ElementItem[] elements;

        public ElementItem this[int index] => elements[index];
        public int Count => ArrayManipulation.ArrayLength(elements);
        public ElementItem this[ElementPath path] => GetElementItem(path);
        public bool IsReadOnly => ArrayManipulation.IsReadOnlySafe(elements);

        public void Add(ElementItem item)
            => ArrayManipulation.Add(item, ref elements);

        public void Add(ElementPath path, ElementItem item) {
            if (Contains(path))
                GetElementItem(path).Add(item);
        }

        public void Clear()
            => ArrayManipulation.ClearArraySafe(ref elements);

        public bool Remove(int index) {
            if (index >= Count) return false;
            ArrayManipulation.Remove(index, ref elements);
            return true;
        }

        public bool Remove(ElementPath path) {
            if (Contains(path)) return false;
            return GetElementItem(ElementPath.GetParent(path)).Remove(path[path.Cell - 1]);
        }

        public void Dispose() {
            for (int I = 0; I < Count; I++)
                elements[I].Clear();
            Clear();
        }

        public bool Contains(ElementPath path)
            => Contains(path.Indexs);

        public bool Contains(int[] indexs) {
            try {
                return GetElementItem(indexs) != null;
            } catch {
                return false;
            }
        }

        public ElementItem GetElementItem(ElementPath path)
            => GetElementItem(path.Indexs);

        public ElementItem GetElementItem(int[] indexs) {
            ElementItem element = null;
            for (int I = 0; I < ArrayManipulation.ArrayLength(indexs); I++) {
                if (I == 0) element = elements[indexs[I]];
                else element = element[indexs[I]];
            }
            return element;
        }

        public IEnumerator<ElementItem> GetEnumerator()
            => new ArrayToIEnumerator<ElementItem>(elements);

        IEnumerator IEnumerable.GetEnumerator()
            => new ArrayToIEnumerator<ElementItem>(elements);

        /// <param name="status">
        /// <br>0 = show all</br>
        /// <br>1 = show true</br>
        /// <br>2 = show false</br>
        /// </param>
        public static void PrintElementContainer(ElementContainer container, byte status)
            => PrintElementContainer(container.GetEnumerator(), 1, status);

        public static void PrintElementContainer(ElementItem container) {
            string s_tab = string.Empty.PadRight(1, '/');
            Print($"{s_tab}=====[", ConsoleColor.DarkGreen);
            Print(container.path.ToString(), ConsoleColor.DarkCyan);
            Print($"][", ConsoleColor.DarkGreen);
            Print(container.status.ToString(), ConsoleColor.DarkCyan);
            Print($"][", ConsoleColor.DarkGreen);
            Print(container.title, ConsoleColor.Yellow);
            Print($"]>>\r\n", ConsoleColor.DarkGreen);
            if (!string.IsNullOrEmpty(container.description)) {
                Print(s_tab, ConsoleColor.DarkGreen);
                Print(container.description, ConsoleColor.Green);
                Print("\r\n", ConsoleColor.DarkGreen);
            }
            PrintElementContainer(container.GetEnumerator(), 2, 0);
            Print($"{s_tab}=====<<\r\n", ConsoleColor.DarkGreen);
        }

        private static void PrintElementContainer(IEnumerator<ElementItem> elements, int tab, byte status) {
            while (elements.MoveNext()) {
                ElementItem element = elements.Current;
                if (status == 1 && !element.status) {
                    PrintElementContainer(element.GetEnumerator(), tab, status);
                    continue;
                } else if (status == 2 && element.status) {
                    PrintElementContainer(element.GetEnumerator(), tab, status);
                    continue;
                }
                string s_tab = string.Empty.PadRight(tab, '/');
                Print($"{s_tab}=====[", ConsoleColor.DarkGreen);
                Print(element.path.ToString(), ConsoleColor.DarkCyan);
                Print($"][", ConsoleColor.DarkGreen);
                Print(element.status.ToString(), ConsoleColor.DarkCyan);
                Print($"][", ConsoleColor.DarkGreen);
                Print(element.title, ConsoleColor.Yellow);
                Print($"]>>\r\n", ConsoleColor.DarkGreen);
                if (!string.IsNullOrEmpty(element.description)) {
                    Print(s_tab, ConsoleColor.DarkGreen);
                    Print(element.description, ConsoleColor.Green);
                    Print("\r\n", ConsoleColor.DarkGreen);
                }
                PrintElementContainer(element.GetEnumerator(), tab + 1, status);
                Print($"{s_tab}=====<<\r\n", ConsoleColor.DarkGreen);
            }
        }

        private static void ElementContainerToElementTag(XMLIRWElement root, ElementItem item, string path) {
            XMLIRWElement element = new XMLIRWElement("element",
                    new XMLIRWAttribute("path", path),
                    new XMLIRWAttribute("status", item.status)
                );
            root.Add(element);

            element.Add(new XMLIRWElement("title", item.title));
            if (!string.IsNullOrEmpty(item.description))
                element.Add(new XMLIRWElement("descriptio", item.description));

            for (int I = 0; I < item.Count; I++)
                ElementContainerToElementTag(element, item[I], $"{path}.{I}");
        }

        private static void ElementTagToElementContainer(XMLIRWElement tag, ElementContainer container) {
            bool noVersion = false;
            foreach (XMLIRW item in tag) {
                if (item is XMLIRWElement element) {
                    if (!element.NoAttributes)
                        foreach (XMLIRWAttribute item2 in element.Attributes.Cast<XMLIRWAttribute>()) {
                            if (item2 is XMLIRWAttribute attri && attri.Name == "version")
                                switch ((string)attri.Text) {
                                    case "2.0":
                                        noVersion = true;
                                        ElementTagToElementContainer2_0(element, container);
                                        break;
                                }
                        }
                    if (!noVersion) {
                        ElementTagToElementContainer1_0(item as XMLIRWElement, container);
                        break;
                    }
                }
            }
                    
        }

        private static void ElementTagToElementContainer1_0(XMLIRWElement tag, ElementContainer container) {
            foreach (XMLIRW e1 in tag) {
                if (e1 is XMLIRWElement ele1 && ele1.Name == "element") {
                    ElementPath path = ElementPath.Empty;
                    ElementPath parent;
                    bool status = false;
                    string title = string.Empty;
                    string description = string.Empty;
                    foreach (XMLIRW e2 in ele1) {
                        if (e2 is XMLIRWAttribute attri)
                            switch (attri.Name) {
                                case "path":
                                    path = new ElementPath(((string)attri.Text).Remove(0, 2));
                                    break;
                                case "status":
                                    status = Convert.ToBoolean(attri.Text);
                                    break;
                            }  
                    }
                    foreach (XMLIRW e3 in ele1) {
                        if (e3 is XMLIRWElement ele2)
                            switch (ele2.Name) {
                                case "title":
                                    title = (string)ele2.Text;
                                    break;
                                case "descriptio":
                                    description = (string)ele2.Text;
                                    break;
                            }  
                    }
                    parent = ElementPath.GetParent(path);
                    if (parent == ElementPath.Root) {
                        ElementItem element = new ElementItem(status, title, description);
                        element.path = path;
                        container.Add(element);
                    } else {
                        if (container.Contains(path.Indexs)) {
                            ElementItem elementItem = container.GetElementItem(path.Indexs);
                            ElementItem element = new ElementItem(status, title, description);
                            element.path = path;
                            elementItem.Add(element);
                        }
                    }
                }
            }
            // tag.ForEach((ElementTag T) => {
            //     if (T.Name == "element") {
            //         ElementPath path = ElementPath.Empty;
            //         ElementPath parent = ElementPath.Empty;
            //         bool status = false;
            //         string title = string.Empty;
            //         string description = string.Empty;
            //         T.ForEach((ElementAttribute E) => {
            //             switch (E.Name) {
            //                 case "path":
            //                     path = new ElementPath(E.Value.ValueToString.Remove(0, 2));
            //                     break;
            //                 case "status":
            //                     status = E.Value.ValueToBool;
            //                     break;
            //             }
            //         });
            //         T.ForEach((ElementTag T2) => {
            //             switch (T2.Name) {
            //                 case "title":
            //                     title = T2.Value.ValueToString;
            //                     break;
            //                 case "descriptio":
            //                     description = T2.Value.ValueToString;
            //                     break;
            //             }
            //         });
            //         parent = ElementPath.GetParent(path);
            //         if (parent == ElementPath.Root)
            //             container.Add(new ElementItem(status, title, description));
            //         else {
            //             if (container.Contains(path.Indexs)) {
            //                 ElementItem elementItem = container.GetElementItem(path.Indexs);
            //                 elementItem.Add(new ElementItem(status, title, description));
            //             }
            //         }
            //     }
            // });
        }

        private static void ElementTagToElementContainer2_0(XMLIRWElement tag, ElementContainer container) {
            foreach (XMLIRW e1 in tag) {
                if (e1 is XMLIRWElement ele1 && ele1.Name == "element") {
                    ElementItem elementItem = new ElementItem(string.Empty);
                    foreach (XMLIRW e2 in ele1) {
                        if (e2 is XMLIRWAttribute attri)
                            switch (attri.Name) {
                                case "path":
                                    elementItem.path = new ElementPath((string)attri.Text);
                                    ElementPath pathtemp = ElementPath.GetParent(elementItem.path);
                                    if (pathtemp == ElementPath.Root)
                                        container.Add(elementItem);
                                    else {
                                        ElementItem temp = container.GetElementItem(pathtemp.Indexs);
                                        temp.Add(elementItem);
                                    }
                                    break;
                                    case "status":
                                        elementItem.status = Convert.ToBoolean(attri.Text);
                                        break;
                            }
                    }
                    foreach (XMLIRW e3 in ele1) {
                        if (e3 is XMLIRWElement ele2)
                            switch (ele2.Name) {
                                case "title":
                                    elementItem.title = (string)ele2.Text;
                                    break;
                                case "descriptio":
                                    elementItem.description = (string)ele2.Text;
                                    break;
                                case "element":
                                    ElementTagToElementContainer2_0(ele1, container);
                                    break;
                            }
                    }
                }
            }
            // tag.ForEach((ElementTag T) => {
            //     if (T.Name == "element") {
            //         ElementItem elementItem = new ElementItem(string.Empty);
            //         T.ForEach((ElementAttribute E) => {
            //             switch (E.Name) {
            //                 case "path":
            //                     elementItem.path = new ElementPath(E.Value.ValueToString);
            //                     ElementPath pathtemp = ElementPath.GetParent(elementItem.path);
            //                     if (pathtemp == ElementPath.Root)
            //                         container.Add(elementItem);
            //                     else {
            //                         ElementItem temp = container.GetElementItem(pathtemp.Indexs);
            //                         temp.Add(elementItem);
            //                     }
            //                     break;
            //                 case "status":
            //                     elementItem.status = E.Value.ValueToBool;
            //                     break;
            //             }
            //         });
            //         T.ForEach((ElementTag T2) => {
            //             switch (T2.Name) {
            //                 case "title":
            //                     elementItem.title = T2.Value.ValueToString;
            //                     break;
            //                 case "descriptio":
            //                     elementItem.description = T2.Value.ValueToString;
            //                     break;
            //                 case "element":
            //                     ElementTagToElementContainer2_0(T, container);
            //                     break;
            //             }
            //         });
            //     }
            // });
        }

        private static void Print(string txt, ConsoleColor color) {
            Console.ForegroundColor = color;
            Console.Write(txt);
            Console.ResetColor();
        }

        public static explicit operator ElementContainer(XMLIRWElement E) {
            ElementContainer res = new ElementContainer();
            ElementTagToElementContainer(E, res);
            return res;
        }

        public static explicit operator XMLIRWElement(ElementContainer E) {
            XMLIRWElement root = new XMLIRWElement("Root", new XMLIRWAttribute("version", "2.0"));
            for (int I = 0; I < E.Count; I++)
                ElementContainerToElementTag(root, E[I], I.ToString());
            return root;
        }
    }
}
