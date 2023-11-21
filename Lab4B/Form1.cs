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
        /// <summary>
        /// Represents the name of the currently loaded HTML file.
        /// </summary>
        private string loadedFileName;

        /// <summary>
        /// Represents the text content of the currently loaded HTML file.
        /// </summary>
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
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        /// <remarks>
        /// This method is triggered when the user clicks the "Open" menu item.
        /// It prompts the user to select an HTML file using the OpenFileDialog, loads the file's content,
        /// and updates the UI components accordingly. If successful, the "Check Tags" menu item is enabled.
        /// </remarks>
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create an OpenFileDialog instance
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // Set the filter to allow only HTML files
            openFileDialog1.Filter = "HTML files (*.html;*.htm)|*.html;*.htm";

            // Set the dialog title
            openFileDialog1.Title = "Open HTML File";

            // Set the initial status in the UI
            fileStatusLabel.Text = "No File Loaded";
            htmlTagsListBox.Items.Clear();

            // Check if the user selected a file through the OpenFileDialog
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Enable the "Check Tags" menu item.
                checkTagsToolStripMenuItem.Enabled = true;

                // Read the content of the selected HTML file
                string selectedFilePath = openFileDialog1.FileName;
                htmlText = File.ReadAllText(selectedFilePath);

                // Update the label with the loaded file's name.
                loadedFileName = Path.GetFileName(selectedFilePath);
                fileStatusLabel.Text = "Loaded: " + loadedFileName;
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
        /// <remarks>
        /// This method is triggered when the user clicks the "Check Tags" menu item.
        /// It extracts HTML tags from the loaded HTML text and performs a tag balance check.
        /// The extracted tags are then checked for balance, and the results are displayed in the ListBox.
        /// </remarks>
        private void checkTagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Extract HTML tags from the loaded HTML text
            List<string> htmlTags = ExtractHtmlTags(htmlText);

            // Perform a tag balance check on the extracted HTML tags
            CheckHtmlTags(htmlTags);
        }

        /// <summary>
        /// Extracts HTML tags from the given HTML data.
        /// </summary>
        /// <param name="data">The HTML data.</param>
        /// <returns>A list of HTML tags.</returns>
        /// <remarks>
        /// The method uses a regular expression to match HTML tags in the provided data.
        /// It extracts the tag names, converting them to lowercase for consistency.
        /// Tags with attributes are truncated to include only the tag name.
        /// </remarks>
        /// Reference to the Regex Match: https://www.dotnetperls.com/regex, https://learn.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.match?view=net-7.0
        private List<string> ExtractHtmlTags(string data)
        {
            // List to store the extracted HTML tags
            List<string> htmlTags = new List<string>();

            // Regular expression pattern for matching HTML tags
            string tagPattern = @"<[^>]+>";

            // Use Regex to find matches of the pattern in the provided HTML data
            MatchCollection matches = Regex.Matches(data, tagPattern);

            // Iterate through each match (HTML tag) found
            foreach (Match match in matches)
            {
                // Get the full HTML tag from the match
                string tag = match.Value;

                // Find the index of the first space in the tag
                int spaceIndex = tag.IndexOf(' ');

                // Check if there is a space in the tag (indicating attributes)
                if (spaceIndex != -1)
                {
                    // If there is a space in the tag, that means it has attributes. 
                    // We only take the part before the space to get the tag name.
                    tag = tag.Substring(0, spaceIndex);

                    // Add the tag name to the list in lowercase, followed by '>'
                    htmlTags.Add(tag.ToLower() + '>');
                } 
                else
                {
                    // If no space, add the entire tag to the list in lowercase
                    htmlTags.Add(tag.ToLower());
                }
            }

            // Return the list of extracted HTML tags
            return htmlTags;
        }

        /// <summary>
        /// Checks the balance of HTML tags and displays the results.
        /// </summary>
        /// <param name="htmlTags">The list of HTML tags to check.</param>
        private void CheckHtmlTags(List<string> htmlTags)
        {
            // Counters to keep track of the number of opening and closing tags
            int openingTagsCounter = 0;
            int closingTagsCounter = 0;

            // Variable to track the current indentation level
            int indentationLevel = 0;

            // Stack to keep track of open tags for balanced checking
            Stack<string> tagStack = new Stack<string>();

            // Loop through each HTML tag in the provided list
            foreach (string tag in htmlTags)
            {
                // Check if the tag has at least two characters
                if (tag.Length >= 2)
                {
                    // Extract the first two characters of the tag
                    string firstTwoChars = tag.Substring(0, 2);

                    // Check if the tag is a closing tag
                    if (firstTwoChars == "</")
                    {
                        // Adjust the indentation level for closing tags
                        indentationLevel = Math.Max(0, indentationLevel - 2);

                        // Log the closing tag with proper indentation
                        htmlTagsListBox.Items.Add(new string(' ', indentationLevel) + $"Found closing tag: {tag}");

                        // Increment the closing tags counter
                        closingTagsCounter++;

                        // Check if the closing tag matches the last open tag
                        if (tagStack.Count > 0 && tagStack.Peek() == tag.Substring(2, tag.Length - 3))
                        {
                            // If matched, remove the last open tag from the stack
                            tagStack.Pop();
                        }
                    }

                    // Check if the tag is an opening tag and not an ignored tag
                    else if (!(tag.StartsWith("<hr") || tag.StartsWith("<br") || tag.StartsWith("<img") || tag.EndsWith("/>")))
                    {
                        // Ignore certain tags
                        if (tag.StartsWith("<a"))
                        {

                            // Push the opening tag onto the stack for balanced checking
                            tagStack.Push(tag.Substring(1, tag.Length - 1));

                            // Increment the opening tags counter
                            openingTagsCounter++;

                            // Adjust the indentation level for opening tags
                            indentationLevel += 2;
                        }

                        // Check if the tag is an opening tag and not an ignored tag


                        // Check if the tag is an opening tag and not an ignored tag
                        else
                        {
                            // Log the opening tag with proper indentation
                            htmlTagsListBox.Items.Add(new string(' ', indentationLevel) + $"Found opening tag: {tag}");

                            // Push the opening tag onto the stack for balanced checking
                            tagStack.Push(tag.Substring(1, tag.Length - 1));

                            // Increment the opening tags counter
                            openingTagsCounter++;

                            // Adjust the indentation level for opening tags
                            indentationLevel += 2;
                        }
                    }

                    else
                    {
                        // Log ignored tags as non-container tags
                        htmlTagsListBox.Items.Add(new string(' ', indentationLevel) + $"Found non-container tag: {tag}");
                    }
                }
            }

            // Check if the number of opening tags is equal to the number of closing tags
            if (openingTagsCounter == closingTagsCounter)
            {
                // If balanced, update the status label
                fileStatusLabel.Text = $"{loadedFileName} has balanced tags";
            }
            else
            {
                // If not balanced, update the status label
                fileStatusLabel.Text = $"{loadedFileName} does not have balanced tags";
            }
        }
    }
}
