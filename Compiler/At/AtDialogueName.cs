using System;

namespace Compiler.At {
	public class AtDialogueName : AtStatement {
		private DialogueLine name;

		public AtDialogueName(DialogueLine param) {
			name = param;
		}

		public override bool Run(DialogueFile file) {
			if (DialogueCompiler.Instance.DialogueName == null) {
				DialogueCompiler.Instance.DialogueName = name.Content;
			} else {
				if (DialogueCompiler.Instance.DialogueName != name.Content) {
					DialogueCompiler.Instance.PrintWarning(name, "Dialogue name is already defined. Only the first dialogue name is used");
				}
			}

			return false;
		}
	}
}

