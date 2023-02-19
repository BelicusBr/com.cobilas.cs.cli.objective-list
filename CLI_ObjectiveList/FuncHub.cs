using System;
using Cobilas.CLI.Manager;
using System.Collections.Generic;

namespace Cobilas.CLI.ObjectiveList {
    internal struct FuncHub {
        private static readonly Dictionary<int, Func<ErrorMensager, CLIArgCollection, bool>> funcs = new Dictionary<int, Func<ErrorMensager, CLIArgCollection, bool>>(GetFuncs());

        public static bool Invok(int id, ErrorMensager error, CLIArgCollection clt)
            => funcs[id](error, clt);

        private static IEnumerable<KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>>> GetFuncs() {
            foreach (KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>> item in new UniFunc())
                yield return item;
            foreach (KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>> item in new ElementFunc())
                yield return item;
            foreach (KeyValuePair<int, Func<ErrorMensager, CLIArgCollection, bool>> item in new ShowFunc())
                yield return item;
        }
    }
}
