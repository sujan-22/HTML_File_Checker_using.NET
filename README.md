# HTML tag checker

A Windows desktop app that checks whether the tags in an HTML file are balanced. Load a file, and it lists every tag as an indented tree and tells you whether each opening tag has a matching closing tag.

Built in C# with Windows Forms for a course lab in November 2023.

## How it works

1. **File, Open** loads an `.html` or `.htm` file.
2. **Check Tags** pulls every tag out of the file with a regular expression, keeps just the tag name (so `<a href="…">` becomes `<a>`) and lowercases it.
3. It walks the tags in order with a stack and lists each one, indented by how deeply it's nested: opening tags, closing tags, and non-container tags such as `<br>`, `<hr>`, `<img>` and self-closing tags, which don't need a partner.
4. It compares the number of opening and closing tags and reports whether the file is balanced.

## Limitations

- Balance is decided by counting, so mis-nested tags like `<b><i></b></i>` still count as balanced.
- Only `br`, `hr`, `img` and self-closing tags are treated as void. Other void elements (`meta`, `link`, `input`), comments and the doctype are counted as opening tags, so a typical HTML5 page is reported as unbalanced.

## Run it

Windows only. Open `Lab4B.sln` in Visual Studio with the .NET Framework 4.8 developer pack installed, then press F5.

## Stack

C# · Windows Forms · .NET Framework 4.8

Built by [Sujan Rokad](https://sujanrokad.com).
