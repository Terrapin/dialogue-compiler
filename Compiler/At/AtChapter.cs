using System;

namespace Compiler.At {
	public class AtChapter : AtStatement {
		DialogueLine line;

		public AtChapter(DialogueLine param) {
			line = param;
		}

		public override bool Run(DialogueFile file) {
			if (file.ChapterName != null) {
				DialogueCompiler.Instance.Error(line, "File already has a chapter name");
			} else if (DialogueCompiler.Instance.HasChapter(line.Content)) {
				DialogueCompiler.Instance.Error(line, "Chapter name '{0}' is used for another file", line.Content);
			} else {
				file.ChapterName = line.Content;
				DialogueCompiler.Instance.ProvideChapter(line.Content);
			}

			return false;
		}
	}
}

