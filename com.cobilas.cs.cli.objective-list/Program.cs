using System;
using System.Text;
using Cobilas.Collections;
using Cobilas.CLI.Manager;

namespace Cobilas.CLI.ObjectiveList {
    class Program {
        //2.0.0
        public const string version = "2.4.0";
        internal static string BaseDirectory => Environment.CurrentDirectory;
        internal static string FriendlyName => AppDomain.CurrentDomain.FriendlyName;

        static void Main(string[] args) {

            ErrorMensager error = new ErrorMensager();
            CLIArgCollection collection = new CLIArgCollection();
            CLICommand root = CLIBase.Create();
            try {
                if (ArrayManipulation.EmpytArray(args))
                    PrintError("Enter some argument.");

                if (CLICommand.Cateter(new StringArrayToIEnumerator(args), root, collection, error, out int funcID)) {
                    if (funcID == 0)
                        PrintError($"Command '{JoinArgs(args)}' not found.");
                    else if (!FuncHub.Invok(funcID, error, collection))
                        PrintError(error);
                } else PrintError(error);

            } catch {
                collection.Clear();
                root.Dispose();

                throw;
            }

            collection.Clear();
            root.Dispose();
            GC.Collect();
        }

        static string JoinArgs(string[] args) {
            StringBuilder builder = new StringBuilder();
            for (int I = 0; I < args.Length; I++)
                builder.AppendFormat("{0} ", args[I]);
            return builder.ToString().TrimEnd();
        }

        static void PrintError(ErrorMensager msm) {
            StringBuilder builder = new StringBuilder();
            foreach (var item in msm)
                builder.AppendLine(item);
            PrintError(builder.ToString());
        }

        static void PrintError(string msm)
            => throw new TaskListException(msm);
    }
}
