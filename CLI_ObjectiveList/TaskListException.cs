using System;

namespace Cobilas.CLI.ObjectiveList {
    [Serializable]
    internal class TaskListException : Exception {
        public TaskListException() { }
        public TaskListException(string message) : base(message) { }
        public TaskListException(string message, Exception inner) : base(message, inner) { }
        protected TaskListException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        public override string ToString() => Message;
    }
}
