using System;
using System.Collections;
using Cobilas.CLI.Manager;
using System.Collections.Generic;

namespace Cobilas.CLI.ObjectiveList
{
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
    /*
     *[14]root init/-i --help/-h
     *[18]root --rename/-r --help/-h
     *[15]root --show/-s --help/-h
     *[17]root --clear/-c --help/-h
     *[16]root --element/-e --help/-h
     *[19]root set --help/-h
     */
    internal struct HelpFuncs : IEnumerable<KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>> {
        IEnumerator<KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>> IEnumerable<KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>>.GetEnumerator() {
            yield return new KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>(14, InitHelpFunc);
            yield return new KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>(15, ShowHelpFunc);
            yield return new KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>(16, ElementHelpFunc);
            yield return new KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>(17, ClearHelpFunc);
            yield return new KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>(18, RenameHelpFunc);
            yield return new KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>(19, SetHelpFunc);
        }

        IEnumerator IEnumerable.GetEnumerator()
            => (IEnumerator)(IEnumerable<KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>>)this;

        private bool SetHelpFunc(ErrorMensager error, CLIArgCollection collection) {
            //===========================[set replace title]=====================================
            Console.WriteLine("{0} [set] [--replacetile/-rt]\r\n", Program.FriendlyName);
            Console.WriteLine("arguments");
            PrintHelp("{arg:file path}                  ", "Relative or full path of the target file.\r\n");

            Console.WriteLine("mandatory options");
            PrintHelp("[--path/-p] {arg}                ", "Represents the path in the task list tree.[example: 1.2.5]");
            PrintHelp("[--title/-t] {arg}               ", "The task title.\r\n");
            //===========================[set replace description]=====================================
            Console.WriteLine("{0} [set] [--replacedesc/-rd]\r\n", Program.FriendlyName);
            Console.WriteLine("arguments");
            PrintHelp("{arg:file path}                  ", "Relative or full path of the target file.\r\n");

            Console.WriteLine("mandatory options");
            PrintHelp("[--path/-p] {arg}                ", "Represents the path in the task list tree.[example: 1.2.5]");
            PrintHelp("[--description/-d] {arg}         ", "The task description.\r\n");
            //===========================[set replace status]=====================================
            Console.WriteLine("{0} [set] [--replacestatus/-rs]\r\n", Program.FriendlyName);
            Console.WriteLine("arguments");
            PrintHelp("{arg:file path}                  ", "Relative or full path of the target file.\r\n");

            Console.WriteLine("mandatory options");
            PrintHelp("[--path/-p] {arg}                ", "Represents the path in the task list tree.[example: 1.2.5]");
            PrintHelp("[--status/--s] {arg[true|false]} ", "The status of the task.\r\n");
            //===========================[set move]=====================================
            Console.WriteLine("{0} [set] [--move/-m]\r\n", Program.FriendlyName);
            Console.WriteLine("arguments");
            PrintHelp("{arg:file path}                  ", "Relative or full path of the target file.\r\n");

            Console.WriteLine("mandatory options");
            PrintHelp("[--path/-p] {arg}                ", "Represents the path in the task list tree.[example: 1.2.5]");
            PrintHelp("[--moveto/-mt] {arg}             ", "The path to move to.[example:1.0.1 to 1.0.2]\r\n");
            return true;
        }

        private bool ElementHelpFunc(ErrorMensager error, CLIArgCollection collection) {
            //==============================[element add]==============================
            Console.WriteLine("{0} [--element/-e] [add]\r\n", Program.FriendlyName);
            Console.WriteLine("arguments");
            PrintHelp("{arg:file path}          ", "Relative or full path of the target file.\r\n");

            Console.WriteLine("mandatory options");
            PrintHelp("[--title/-t] {arg}       ", "The task title.\r\n");

            Console.WriteLine("options");
            PrintHelp("[--path/-p] {arg}        ", "Represents the path in the task list tree.[example: 1.2.5]");
            PrintHelp("[--description/-d] {arg} ", "The task description.\r\n");
            //==============================[element remove]=============================
            Console.WriteLine("{0} [--element/-e] [remove]\r\n", Program.FriendlyName);
            Console.WriteLine("arguments");
            PrintHelp("{arg:file path}          ", "Relative or full path of the target file.\r\n");

            Console.WriteLine("mandatory options");
            PrintHelp("[--path/-p] {arg}        ", "Represents the path in the task list tree.[example: 1.2.5]");
            return true;
        }

        private bool ClearHelpFunc(ErrorMensager error, CLIArgCollection collection) {
            Console.WriteLine("{0} [--clear/-c]\r\n", Program.FriendlyName);
            Console.WriteLine("arguments");
            PrintHelp("{arg:file path}    ", "Relative or full path of the target file.");
            return true;
        }

        private bool ShowHelpFunc(ErrorMensager error, CLIArgCollection collection) {
            //=============================[show item]=======================================
            Console.WriteLine("{0} [--show/-s] [--item/--i]\r\n", Program.FriendlyName);
            Console.WriteLine("arguments");
            PrintHelp("{arg:file path}    ", "Relative or full path of the target file.\r\n");

            Console.WriteLine("options");
            PrintHelp("[--path/-p]        ", "Represents the path in the task list tree.[example: 1.2.5]");
            PrintHelp("\t{opc-arg:path}   ", "path in the task list tree.\r\n");
            //=============================[show list]=======================================
            Console.WriteLine("{0} [--show/-s] [--list/-l]\r\n", Program.FriendlyName);
            Console.WriteLine("arguments");
            PrintHelp("{arg:file path}    ", "Relative or full path of the target file.");
            return true;
        }

        private bool RenameHelpFunc(ErrorMensager error, CLIArgCollection collection) {
            Console.WriteLine("{0} [--rename/-r]\r\n", Program.FriendlyName);
            Console.WriteLine("arguments");
            PrintHelp("{arg1:folder path}      ", "Relative or full directory path of the target file.");
            PrintHelp("{arg2:file name}        ", "The name of the target file.");
            PrintHelp("{arg3:new file name}    ", "The new name for the target file.");
            return true;
        }

        private bool InitHelpFunc(ErrorMensager error, CLIArgCollection collection) {
            Console.WriteLine("{0} [init/-i]\r\n", Program.FriendlyName);
            Console.WriteLine("arguments");
            PrintHelp("{arg:file path}    ", "Relative or full path of the target file.");
            return true;
        }

        private static void PrintHelp(string func, string msm) {
            Console.Write("\t{0}", func);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("{0}\r\n", msm);
            Console.ResetColor();
        }
    }
}
