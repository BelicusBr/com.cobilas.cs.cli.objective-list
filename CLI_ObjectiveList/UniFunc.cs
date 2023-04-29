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
    internal struct UniFunc : IEnumerable<KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>> {
        IEnumerator<KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>> IEnumerable<KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>>.GetEnumerator() {
            yield return new KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>(1, InitFunc);
            yield return new KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>(3, helpFunc);
            yield return new KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>(4, versionFunc);
            yield return new KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>(6, ClearFunc);
            yield return new KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>(8, renameFunc);
        }

        IEnumerator IEnumerable.GetEnumerator()
            => (IEnumerator)(IEnumerable<KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>>)this;

        private bool InitFunc(ErrorMensager error, CLIArgCollection collection) {
            string filePath = collection[collection.IndexOf($"{CLICMDArg.alias}0")].Value;
            if (!Path.IsPathRooted(filePath))
                filePath = Path.Combine(Program.BaseDirectory, filePath);

            string folderPath = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(folderPath)) {
                error.Add($"'{folderPath}' not found!");
                return false;
            } else if (File.Exists(filePath)) {
                error.Add($"'{filePath}' exists!");
                return false;
            }

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\r\n";
            using (FileStream stream = File.Create(filePath)) {
                using XmlWriter writer = XmlWriter.Create(stream, settings);
                writer.WriteElementTag(
                        new ElementTag("Root", new ElementAttribute("version", "2.0"))
                    );
                Console.WriteLine($"File \"{filePath}\" create!");
            }
            return true;
        }

        private bool ClearFunc(ErrorMensager error, CLIArgCollection collection) {
            string filePath = collection[collection.IndexOf($"{CLICMDArg.alias}0")].Value;
            if (!Path.IsPathRooted(filePath))
                filePath = Path.Combine(Program.BaseDirectory, filePath);

            if (!File.Exists(filePath)) {
                error.Add($"'{filePath}' not exists!");
                return false;
            }
            
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\r\n";
            using (FileStream stream = File.Create(filePath)) {
                using XmlWriter writer = XmlWriter.Create(stream, settings);
                writer.WriteElementTag(
                        new ElementTag("Root", new ElementAttribute("version", "2.0"))
                    );
                Console.WriteLine($"File \"{filePath}\" clear!");
            }
            return true;
        }

        private bool helpFunc(ErrorMensager error, CLIArgCollection collection) {
            Console.WriteLine(Program.FriendlyName);
            PrintHelp("[--help/-h]        ", "Displays the list of program commands.");
            PrintHelp("[--version/-v]     ", "Displays the program version.");
            PrintHelp("[init/-i]          ", "Initializes a file in .xml format where tasks will be stored.");
            PrintHelp("[--rename/-r]      ", "Renames the file where tasks are stored.");
            PrintHelp("[--clear/-c]       ", "Clears the task list.\r\n");

            PrintHelp("[--element/-e]     ", "Changes the task list.");
            PrintHelp("[--show/-s]        ", "Displays task list items.");
            PrintHelp("[set]              ", "Change specific items in task list.\r\n");

            Console.WriteLine("For more information:");
            PrintHelp($"{Program.FriendlyName} ", "[command] [--help/-h]");
            return true;
        }

        private static void PrintHelp(string func, string msm) {
            Console.Write("\t{0}", func);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("{0}\r\n", msm);
            Console.ResetColor();
        }

        private bool versionFunc(ErrorMensager error, CLIArgCollection collection) {
            Console.WriteLine("version-{0}", Program.version);
            return true;
        }

        private bool renameFunc(ErrorMensager error, CLIArgCollection collection) {
            string folderPath = collection[collection.IndexOf($"{CLICMDArg.alias}0")].Value;
            string arg1 = collection[collection.IndexOf($"{CLICMDArg.alias}1")].Value;
            string arg2 = collection[collection.IndexOf($"{CLICMDArg.alias}2")].Value;

            if (!Path.IsPathRooted(folderPath))
                folderPath = Path.Combine(Program.BaseDirectory, folderPath);

            arg1 = Path.Combine(folderPath, arg1);
            arg2 = Path.Combine(folderPath, arg2);

            if (!Directory.Exists(folderPath)) {
                error.Add($"'{folderPath}' not found!");
                return false;
            } else if (!File.Exists(arg1)) {
                error.Add($"'{arg1}' not exists!");
                return false;
            } else if (arg1 == arg2) {
                error.Add($"'{arg1}' is equal to '{arg2}'");
                return false;
            }

            File.Copy(arg1, arg2);
            File.Delete(arg1);

            return true;
        }
    }
}
