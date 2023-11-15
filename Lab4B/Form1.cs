/*
Class:                  Form1.cs
Author:                 Sujan Rokad
Date:                   November 15, 2023
Authorship statement:   I, Sujan Rokad, 000882948 certify that this material is my original work. No other person's work has been used without due acknowledgement.

Purpose:                The Form1 class represents the main form of the Lab4B application. This Windows Forms application allows users to load an HTML file, extract and analyze its tags, and check the balance of opening and closing tags. The purpose of this class is to provide the user interface and implement the logic for loading HTML files, extracting tags, and performing a tag balance check. The application aims to assist users in identifying potential HTML tag structure issues within loaded files.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Lab4B
{
    /// <summary>
    /// Represents the main form of the application.
    /// </summary>
    public partial class Form1 : Form
    {
        private string loadedFileName;
        private string htmlText;

        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            
        }

        /// <summary>
        /// Event handler for when the "Open" menu item is clicked.
        /// </summary>
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "HTML files (*.html;*.htm)|*.html;*.htm";
            openFileDialog1.Title = "Open HTML File";

            label1.Text = "No File Loaded";
            listBox1.Items.Clear();

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Enable the "Check Tags" menu item.
                checkTagsToolStripMenuItem.Enabled = true;

                // Read the selected HTML file
                string selectedFilePath = openFileDialog1.FileName;
                htmlText = File.ReadAllText(selectedFilePath);

                // Update the label with the loaded file's name.
                loadedFileName = Path.GetFileName(selectedFilePath);
                label1.Text = "Loaded: " + loadedFileName;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Event handler for when the "Check Tags" menu item is clicked.
        /// </summary>
        private void checkTagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> htmlTags = ExtractHtmlTags(htmlText);
            CheckHtmlTags(htmlTags);
        }

        /// <summary>
        /// Extracts HTML tags from the given HTML data.
        /// </summary>
        /// <param name="data">The HTML data.</param>
        /// <returns>A list of HTML tags.</returns>
        /// Reference to the Regex Match: https://www.dotnetperls.com/regex, https://learn.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.match?view=net-7.0
        private List<string> ExtractHtmlTags(string data)
        {
            List<string> htmlTags = new List<string>();
            string pattern = @"<[^>]+>";

            MatchCollection matches = Regex.Matches(data, pattern);

            foreach (Match match in matches)
            {
                string tag = match.Value;
                int spaceIndex = tag.IndexOf(' ');

                if (spaceIndex != -1)
                {
                    // If there is a space in the tag, that means it has attributes. 
                    // We only take the part before the space to get the tag name.
                    tag = tag.Substring(0, spaceIndex);
                    htmlTags.Add(tag + '>');
                } 
                else
                {
                    htmlTags.Add(tag);
                }
            }

            return htmlTags;
        }

        /// <summary>
        /// Checks the balance of HTML tags and displays the results.
        /// </summary>
        /// <param name="htmlTags">The list of HTML tags to check.</param>
        private void CheckHtmlTags(List<string> htmlTags)
        {
            int counter1 = 0;
            int counter2 = 0;
            int indent = 0;
            Stack<string> tagStack = new Stack<string>();

            foreach (string tag in htmlTags)
            {
                if (tag.Length >= 2)
                {
                    string firstTwoChars = tag.Substring(0, 2);

                    if (firstTwoChars == "</")
                    {
                        indent = Math.Max(0, indent - 2);
                        listBox1.Items.Add(new string(' ', indent) + $"Found closing tag: {tag}");
                        counter2++;
                        if (tagStack.Count > 0 && tagStack.Peek() == tag.Substring(2, tag.Length - 3))
                        {
                            tagStack.Pop();
                        }
                    }
                    else if (!(tag.StartsWith("<hr") || tag.StartsWith("<br") || tag.StartsWith("<img") || tag.EndsWith("/>")))
                    {
                        // Ignore certain tags
                        if (tag.StartsWith("<a"))
                        {
                            listBox1.Items.Add(new string(' ', indent) + $"Found opening tag: {tag}");
                            tagStack.Push(tag.Substring(1, tag.Length - 1));
                            counter1++;
                            indent += 2;
                        }
                        else
                        {
                            listBox1.Items.Add(new string(' ', indent) + $"Found opening tag: {tag}");
                            tagStack.Push(tag.Substring(1, tag.Length - 1));
                            counter1++;
                            indent += 2;
                        }
                    }
                }
            }

            if (counter1 == counter2)
            {
                label1.Text = $"{loadedFileName} is balanced";
            }
            else
            {
                label1.Text = $"{loadedFileName} is not balanced";
            }
        }
    }
}
