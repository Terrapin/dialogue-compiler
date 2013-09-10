using System;
using System.Collections.Generic;
using Compiler;

namespace Compiler.At {
	public abstract class AtStatement {

		private static Dictionary<string, Func<DialogueLine, AtStatement>> statements = new Dictionary<string, Func<DialogueLine, AtStatement>> {
			{"@include", (param) => new AtInclude(param)},
			{"@chapter", (param) => new AtChapter(param)},
			{"dialogue_name", (param) => new AtDialogueName(param)},
			{"initial_settings", (param) => new AtInitialSettings(param)},
		};

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
		private DialogueLine statement;

		public NoSuchStatement(DialogueLine line) {
			statement = line;
		}

		public override bool Run(DialogueFile file) {
			DialogueCompiler.Instance.Error(statement, "No such statement ({0})", statement.LineType);
			return false;
		}
	}

	public class NoopStatement : AtStatement {
		public override bool Run(DialogueFile file) {
			return true;
		}
	}
}

