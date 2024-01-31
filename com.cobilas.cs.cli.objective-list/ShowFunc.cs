using System;
using System.IO;
using System.Xml;
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
    internal struct ShowFunc : IEnumerable<KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>> {
        IEnumerator<KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>> IEnumerable<KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>>.GetEnumerator() {
            yield return new KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>(2, ShowItemFunc);
            yield return new KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>(9, ShowListFunc);
        }

        IEnumerator IEnumerable.GetEnumerator()
            => (IEnumerator)(IEnumerable<KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>>)this;

        private bool ShowItemFunc(ErrorMensager error, CLIArgCollection collection) {
            string path = collection[collection.IndexOf("--path/-p")].Value;
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

            using (XmlReader reader = XmlReader.Create(filePath)) {
                using ElementContainer list = (ElementContainer)reader.ReadXMLIRW();
                ElementPath path1 = new ElementPath(path);
                if (!list.Contains(path1)) {
                    error.Add($"Path '{path}' not exists!");
                    return false;
                }
                ElementContainer.PrintElementContainer(list[path1]);
            }
            return true;
        }

        private bool ShowListFunc(ErrorMensager error, CLIArgCollection collection) {
            string filePath = collection[collection.IndexOf($"{CLICMDArg.alias}0")].Value;
            string s_status = collection.Contains("--status/--s") ? collection[collection.IndexOf($"--status/--s")].Value : null;
            bool status = false;

            if (!Path.IsPathRooted(filePath))
                filePath = Path.Combine(Program.BaseDirectory, filePath);

            if (!File.Exists(filePath)) {
                error.Add($"'{filePath}' not exists!");
                return false;
            }

            if (!string.IsNullOrEmpty(s_status))
                if (!bool.TryParse(s_status, out status)) {
                    error.Add($"[{s_status}] invalid value, use boolean values.[true|false]");
                    return false;
                }
            using (XmlReader reader = XmlReader.Create(filePath)) {
                byte r_status = (byte)(!string.IsNullOrEmpty(s_status) ? status ? 1 : 2 : 0);
                ElementContainer.PrintElementContainer((ElementContainer)reader.ReadXMLIRW(), r_status);
            }
            return true;
        }
    }
}
