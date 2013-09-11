using System;

namespace Compiler.At {
	public class AtNewLine : AtCommand {
		public AtNewLine(string command, params string[] args) {
		}

		public override string Run(DialogueFile file, DialogueLine line) {
			return "%0A";
		}
	}
}

