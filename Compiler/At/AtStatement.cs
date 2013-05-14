using System;
using System.Collections.Generic;
using Compiler;

namespace At {
	public abstract class AtStatement {
		public class NoSuchStatementException : Exception {
			public NoSuchStatementException(string statement) : base(statement) {
			}
		}

		private static Dictionary<string, Func<string, AtStatement>> statements = new Dictionary<string, Func<string, AtStatement>>();

		static AtStatement() {
			statements["include"] = (param) => new AtInclude(param);
		}

		public static AtStatement GetStatement(string name, string param) {
			if (statements.ContainsKey(name)) {
				return statements[name](param);
			}

			throw new NoSuchStatementException(name);
		}

		public abstract void Run(DialogueFile file);
	}
}

