using System;

namespace Compiler.At {
	public class AtDialogueName : AtStatement {
		private string name;

		public AtDialogueName(DialogueLine param) {
			name = param.Content;
		}

		public override bool Run(DialogueFile file) {
			if (DialogueCompiler.Instance.DialogueName == null) {
				DialogueCompiler.Instance.DialogueName = name;
			} else {
				//Raise warning
			}

			return false;
		}
	}
}

