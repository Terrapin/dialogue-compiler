using System;
using System.Collections.Generic;
using Compiler;

namespace Compiler.At {
	public abstract class AtStatement {
		public class NoSuchStatementException : Exception {
			public NoSuchStatementException(string statement) : base(statement) {
			}
		}

		private static Dictionary<string, Func<DialogueLine, AtStatement>> statements = new Dictionary<string, Func<DialogueLine, AtStatement>>();

		static AtStatement() {
			statements["@include"] = (param) => new AtInclude(param);
		}

		public static AtStatement GetStatement(string name, DialogueLine param) {
			if (statements.ContainsKey(name)) {
				return statements[name](param);
			}

			if (name[0] == '@') {
				return new NoSuchStatement(param);
			}

			return new NoopStatement();
		}

		public abstract bool Run(DialogueFile file);
	}

	public class NoSuchStatement : AtStatement {
		private string statement;

		public NoSuchStatement(DialogueLine line) {
			statement = line.LineType;
		}

		public override bool Run(DialogueFile file) {
			throw new NotImplementedException(statement);
		}
	}

	public class NoopStatement : AtStatement {
		public override bool Run(DialogueFile file) {
			return true;
		}
	}
}

