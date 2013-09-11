using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

namespace Compiler.At {
	public abstract class AtCommand {
		private static Dictionary<string, Func<string, string[], AtCommand>> commands = new Dictionary<string, Func<string, string[], AtCommand>> {
			{"newline", (name, args) => new AtNewLine(name, args)},
			{"linefeed", (name, args) => new AtNewLine(name, args)},
			{"lf", (name, args) => new AtNewLine(name, args)},
			{"br", (name, args) => new AtNewLine(name, args)},
			{"cr", (name, args) => new AtNewLine(name, args)},
			{"crlf", (name, args) => new AtNewLine(name, args)},
			
			{"quote", (name, args) => new AtQuote(name, args)},
			{"q", (name, args) => new AtQuote(name, args)},

			{"switch", (name, args) => new AtSwitch(name, args)},
		};
		private static Regex commandPattern = new Regex(@"@([a-zA-Z]+)(?:;|\((?:([0-9a-zA-Z]+),?)+\))");

		public static string ExecuteCommands(DialogueFile file, DialogueLine line) {
			int startAt = 0;
			bool keep = true;
			var newLine = new StringBuilder(line.Content.Length);
			var match = commandPattern.Match(line.Content, startAt);
			while (match.Success) {
				newLine.Append(line.Content.Substring(startAt, match.Index - startAt));

				var commandName = match.Groups[1].Value;
				if (commands.ContainsKey(commandName)) {
					var args = new string[match.Groups[2].Captures.Count];
					for (int i = 0; i < match.Groups[2].Captures.Count; ++i) {
						args[i] = match.Groups[2].Captures[i].Value;
					}

					var command = commands[commandName](commandName, args);

					var replacement = command.Run(file, line);
					if (replacement == null) {
						keep = false;
					} else {
						newLine.Append(replacement);
					}

				} else {
					(new NoSuchCommand(commandName, null)).Run(file, line);
				}
				
				startAt = match.Index + match.Length;
				match = commandPattern.Match(line.Content, startAt);
			}

			newLine.Append(line.Content.Substring(startAt));

			return keep ? newLine.ToString() : null;
		}

		public abstract string Run(DialogueFile file, DialogueLine line);
	}

	public class NoSuchCommand : AtCommand {
		private string command;

		public NoSuchCommand(string command, params string[] args) {
			this.command = command;
		}

		public override string Run(DialogueFile file, DialogueLine line) {
			DialogueCompiler.Instance.Error(line, "No such command ({0})", command);
			return null;
		}
	}
}

