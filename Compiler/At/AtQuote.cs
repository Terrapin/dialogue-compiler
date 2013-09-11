using System;

namespace Compiler.At {
	public class AtQuote : AtCommand {
		public AtQuote(string command, params string[] args) {
		}

		public override string Run(DialogueFile file, DialogueLine line) {
			return "%22";
		}
	}
}

