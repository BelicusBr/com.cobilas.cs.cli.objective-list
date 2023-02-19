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
    internal struct ShowFunc : IEnumerable<KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>> {
        IEnumerator<KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>> IEnumerable<KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>>.GetEnumerator() {
            yield return new KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>(2, ShowItemFunc);
            yield return new KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>(9, ShowListFunc);
        }

        IEnumerator IEnumerable.GetEnumerator()
            => (IEnumerator)(IEnumerable<KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>>)this;

        private bool ShowItemFunc(ErrorMensager error, CLIArgCollection collection) {
            string path = $"0.{collection[collection.IndexOf("--path/-p")].Value}";
            string filePath = collection[collection.IndexOf($"{CLICMDArg.alias}0")].Value;

            if (!Path.IsPathRooted(filePath))
                filePath = Path.Combine(Program.BaseDirectory, filePath);

            if (!File.Exists(filePath)) {
                error.Add($"'{filePath}' not exists!");
                return false;
            }

            using (OTVL_ElementList list = new OTVL_ElementList(filePath)) {
                if (!list.Contains(path)) {
                    error.Add($"Path '{path}' not exists!");
                    return false;
                }
                PrintElement(list[path]);
            }
            return true;
        }

        private bool ShowListFunc(ErrorMensager error, CLIArgCollection collection) {
            string filePath = collection[collection.IndexOf($"{CLICMDArg.alias}0")].Value;

            if (!Path.IsPathRooted(filePath))
                filePath = Path.Combine(Program.BaseDirectory, filePath);

            if (!File.Exists(filePath)) {
                error.Add($"'{filePath}' not exists!");
                return false;
            }

            using (OTVL_ElementList list = new OTVL_ElementList(filePath))
                foreach (var item in list)
                    PrintElement(item);
            return true;
        }

        private void PrintElement(OTVL_Element element) {
            Print("/=====[", ConsoleColor.DarkGreen);
            Print(element.title, ConsoleColor.Yellow);
            Print("]=====\r\n", ConsoleColor.DarkGreen);

            Print("Path[", ConsoleColor.DarkGray);
            Print(element.path.ToString(), ConsoleColor.DarkCyan);
            Print("]--Status[", ConsoleColor.DarkGray);
            Print(element.status.ToString(), ConsoleColor.DarkBlue);
            Print("]\r\n", ConsoleColor.DarkGray);

            if (!string.IsNullOrEmpty(element.description)) {
                Print("Description:\r\n", ConsoleColor.Green);
                Console.WriteLine(element.description);
            }

            Print("/===== ===== =====\r\n", ConsoleColor.DarkGreen);
        }

        private void Print(string txt, ConsoleColor color) {
            Console.ForegroundColor = color;
            Console.Write(txt);
            Console.ResetColor();
        }
    }
}
