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
    internal struct SetFunc : IEnumerable<KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>> {
        IEnumerator<KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>> IEnumerable<KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>>.GetEnumerator() {
            yield return new KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>(10, SetReplaceTitleFunc);
            yield return new KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>(11, SetReplaceDescriptionFunc);
            yield return new KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>(12, SetReplaceStatusFunc);
            yield return new KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>(13, SetMoveFunc);
        }

        IEnumerator IEnumerable.GetEnumerator()
            => (IEnumerator)(IEnumerable<KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>>)this;

        private bool SetReplaceTitleFunc(ErrorMensager error, CLIArgCollection collection) {
            string path = $"0.{collection[collection.IndexOf("--path/-p")].Value}";
            string newTitle = collection[collection.IndexOf("--title/-t")].Value;
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
                list[path].title = newTitle;
                list.UnLoad();
            }
            return true;
        }

        private bool SetReplaceDescriptionFunc(ErrorMensager error, CLIArgCollection collection) {
            string path = $"0.{collection[collection.IndexOf("--path/-p")].Value}";
            string newTitle = collection[collection.IndexOf("--description/-d")].Value;
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
                list[path].description = newTitle;
                list.UnLoad();
            }
            return true;
        }

        private bool SetReplaceStatusFunc(ErrorMensager error, CLIArgCollection collection) {
            string path = $"0.{collection[collection.IndexOf("--path/-p")].Value}";
            string newTitle = collection[collection.IndexOf("--status/--s")].Value;
            string filePath = collection[collection.IndexOf($"{CLICMDArg.alias}0")].Value;

            if (!Path.IsPathRooted(filePath))
                filePath = Path.Combine(Program.BaseDirectory, filePath);

            if (!File.Exists(filePath)) {
                error.Add($"'{filePath}' not exists!");
                return false;
            } else if (!ElementPath.ItsValid(path)) {
                error.Add($"[{path}] it is not valid.");
                return false;
            } else if (bool.TryParse(newTitle, out bool result)) {
                using OTVL_ElementList list = new OTVL_ElementList(filePath); 
                if (!list.Contains(path)) {
                    error.Add($"Path '{path}' not exists!");
                    return false;
                }
                list[path].status = result;
                list.UnLoad();
            } else {
                error.Add($"[{newTitle}] invalid value, use boolean values.[true|false]");
                return false;
            }

            return true;
        }

        private bool SetMoveFunc(ErrorMensager error, CLIArgCollection collection) {
            string path = $"0.{collection[collection.IndexOf("--path/-p")].Value}";
            string newPath = $"0.{collection[collection.IndexOf("--moveto/-mt")].Value}";
            string filePath = collection[collection.IndexOf($"{CLICMDArg.alias}0")].Value;

            if (!Path.IsPathRooted(filePath))
                filePath = Path.Combine(Program.BaseDirectory, filePath);

            if (!File.Exists(filePath)) {
                error.Add($"'{filePath}' not exists!");
                return false;
            } else if (!ElementPath.ItsValid(path)) {
                error.Add($"[{path}] it is not valid.");
                return false;
            } else if (!ElementPath.ItsValid(newPath)) {
                error.Add($"[{newPath}] it is not valid.");
                return false;
            }

            using (OTVL_ElementList list = new OTVL_ElementList(filePath)) {
                if (!list.Contains(path)) {
                    error.Add($"Path '{path}' not exists!");
                    return false;
                } else if (!list.Contains(newPath)) {
                    error.Add($"Path '{newPath}' not exists!");
                    return false;
                }
                OTVL_Element e1 = list[path];
                OTVL_Element e2 = list[newPath];
                e1.path = new ElementPath(newPath);
                e2.path = new ElementPath(path);
                list.UnLoad();
            }
            return true;
        }
    }
}
