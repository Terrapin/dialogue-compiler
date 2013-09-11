using System;

namespace Compiler.At {
	public class AtCurrentChapter : AtCommand {
		public AtCurrentChapter(string name, params string[] args) {
		}

		public override string Run(DialogueFile file, DialogueLine line) {
			return String.Format("*{0}*", DialogueCompiler.ChapterVariable);
		}
	}
}

