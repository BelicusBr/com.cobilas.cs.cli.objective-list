using System;
using System.IO;
using System.Collections;
using Cobilas.CLI.Manager;
using System.Collections.Generic;

namespace Cobilas.CLI.ObjectiveList {
    /*root --version/-v
     *root --help/-h
     *root --rename/-r {arg:folder path} {arg:file name} {arg:new file name}
     *root init/-i {arg:file path}
     *root --show/-s --item/--i --path/-p {opc:arg} {arg:file path}
     *root --show/-s --list/-l {arg:file path}
     *root --clear/-c {arg:file path}
     *root --element/-e add --path/-p {opc:arg} --title/-t {opc:arg} --description/-d {opc:arg} {arg:file path}
     *root --element/-e remove --path/-p {opc:arg} {arg:file path}
     *root set --replacetile/-rt --path/-p {opc:arg} --title/-t {opc:arg} {arg:file path}
     *root set --replacedesc/-rd --path/-p {opc:arg} --description/-d {opc:arg} {arg:file path}
     *root set --replacestatus/-rs --path/-p {opc:arg} --status/--s {opc:arg[true|false]} {arg:file path}
     *root set --move/-m --path/-p {opc:arg} --moveto/-mt {opc:arg} {arg:file path}
     *root init/-i --help/-h
     *root --rename/-r --help/-h
     *root --show/-s --help/-h
     *root --clear/-c --help/-h
     *root --element/-e --help/-h
     *root set --help/-h
     */
    internal struct ElementFunc : IEnumerable<KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>> {
        IEnumerator<KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>> IEnumerable<KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>>.GetEnumerator() {
            yield return new KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>(5, AddElementFunc);
            yield return new KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>(7, RemoveElementFunc);
        }

        IEnumerator IEnumerable.GetEnumerator()
            => (IEnumerator)(IEnumerable<KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>>)this;

        //root add --element/-e --path/-p {opc:arg} --title/-t {opc:arg} --description/-d {opc:arg} {arg:file path}
        private bool AddElementFunc(ErrorMensager error, CLIArgCollection collection) {
            string path = string.Empty;
            string title = string.Empty;
            string filePath = string.Empty;
            string description = string.Empty;

            foreach (CLIArg item in collection) {
                switch (item.Arg) {
                    case "--path/-p":
                        path = item.Value;
                        break;
                    case "--title/-t":
                        title = item.Value;
                        break;
                    case "--description/-d":
                        description = item.Value;
                        break;
                    case "{arg}0":
                        filePath = item.Value;
                        break;
                }
            }

            if (!Path.IsPathRooted(filePath))
                filePath = Path.Combine(Program.BaseDirectory, filePath);

            if (!File.Exists(filePath)) {
                error.Add($"'{filePath}' not exists!");
                return false;
            } else if (!ElementPath.ItsValid(path)) {
                error.Add($"[{path}] it is not valid.");
                return false;
            }

            using (OTVL_ElementList list = new OTVL_ElementList(filePath)) {
                if (string.IsNullOrEmpty(path)) {
                    int num1 = 0;
                    while (list.Contains(path = $"0.{num1}"))
                        ++num1;
                }
                else path = $"0.{path}";

                ElementPath elementPath = ElementPath.GetParent(new ElementPath(path));
                if (elementPath.ToString() != "0" && !list.Contains(elementPath.ToString())) {
                    error.Add($"Path '{elementPath}' not exists!");
                    return false;
                }

                if (list.Contains(path)) {
                    error.Add($"Element '[{path}]{title}' exists!");
                    return false;
                }
                OTVL_Element element = new OTVL_Element();
                element.title = title;
                element.status = false;
                element.description = description;
                element.path = new ElementPath(path);
                list.Add(element);
                Console.WriteLine("Element '[{0}]{1}' has been added!", element.path, element.title);
                list.UnLoad();
            }

            return true;
        }

        //root remove --element/-e --path/-p {opc:arg} {arg:file path}
        private bool RemoveElementFunc(ErrorMensager error, CLIArgCollection collection) {
            string path = $"0.{collection[collection.IndexOf("--path/-p")].Value}";
            string filePath = collection[collection.IndexOf($"{CLICMDArg.alias}0")].Value;

            if (!Path.IsPathRooted(filePath))
                filePath = Path.Combine(Program.BaseDirectory, filePath);

            if (!File.Exists(filePath)) {
                error.Add($"'{filePath}' not exists!");
                return false;
            } else if (!ElementPath.ItsValid(path)) {
                error.Add($"[{path}] it is not valid.");
                return false;
            }

            using (OTVL_ElementList list = new OTVL_ElementList(filePath)) {
                if (!list.Contains(path)) {
                    error.Add($"Path '{path}' not exists!");
                    return false;
                }

                Console.WriteLine("Element '[{0}]{1}' has been removed!", path, list[path].title);
                _ = list.Remove(path);
                list.UnLoad();
            }

            return true;
        }
    }
}
