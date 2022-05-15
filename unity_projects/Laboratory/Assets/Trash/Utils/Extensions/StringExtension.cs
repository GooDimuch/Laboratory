using System;
using System.Collections.Generic;
using System.Linq;

public static class StringExtension {
	public static IEnumerable<string> Split(this string str, Func<char, bool> controller) {
		var nextPiece = 0;

		for (var c = 0; c < str.Length; c++) {
			if (!controller(str[c])) { continue; }

			yield return str.Substring(nextPiece, c - nextPiece);
			nextPiece = c + 1;
		}

		yield return str.Substring(nextPiece);
	}

	public static string TrimMatchingQuotes(this string input, char quote) {
		if (input.Length >= 2 && input[0] == quote && input[input.Length - 1] == quote)
			return input.Substring(1, input.Length - 2);

		return input;
	}

	/// <summary>
	/// Убирает все пробелы и табы не только сначала и сконца, но и внутри строки.
	/// </summary>
	/// <param name="input">Строка для обработки.</param>
	/// <returns>Возврещает строку без пробелов и табов.</returns>
	public static string TrimAll(this string input) =>
			new string(input.ToCharArray().Where(c => !char.IsWhiteSpace(c)).ToArray());

	public static string FirstCharToUpper(this string input) {
		switch (input) {
			case null: throw new ArgumentNullException(nameof(input));
			case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
			default: return input.First().ToString().ToUpper() + input.Substring(1);
		}
	}
	
	public static string Escape(this string path) => path.Replace("\\", "\\\\");
}