using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Lab4B
{
    public partial class Form1 : Form
    {
        private string loadedFileName;
        private string htmlText;

        public Form1()
        {
            InitializeComponent();
            
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "HTML files (*.html;*.htm)|*.html;*.htm";
            openFileDialog1.Title = "Open HTML File";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Enable the "Check Tags" menu item.
                checkTagsToolStripMenuItem.Enabled = true;

                // You can now use the selected file path.
                string selectedFilePath = openFileDialog1.FileName;

                htmlText = File.ReadAllText(selectedFilePath);

                // Update the label with the loaded file's name.
                loadedFileName = Path.GetFileName(selectedFilePath);
                label1.Text = "Loaded: " + loadedFileName;

                // Handle the selected HTML file as needed.
                // For example, you can load and display the content in a TextBox or a WebBrowser control.
                // textBox1.Text = File.ReadAllText(selectedFilePath);
                // webBrowser1.Navigate(selectedFilePath);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void menuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void processToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void checkTagsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // Define the list of ignored tags (not container tags)
            string[] ignoredTags = { "img", "hr", "br" };

            // Initialize a Stack to keep track of opening tags
            Stack<string> tagStack = new Stack<string>();

            // Split the HTML text into individual tags
            string[] tags = htmlText.Split(new string[] { "<", ">" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string tag in tags)
            {
                if (!string.IsNullOrWhiteSpace(tag))
                {
                    // Check if the tag is an opening tag
                    if (!tag.StartsWith("/") && !ignoredTags.Contains(tag))
                    {
                        listBox1.Items.Add("Found opening tag: <" + tag + ">");
                        Console.WriteLine("added to listbox");
                        tagStack.Push(tag);
                    }
                    // Check if the tag is a closing tag
                    else if (tag.StartsWith("/") && !ignoredTags.Contains(tag.Substring(1)))
                    {
                        string openingTag = tagStack.Pop();
                        listBox1.Items.Add("Found closing tag: </" + tag.Substring(1) + ">");
                        if (tag.Substring(1) != openingTag)
                        {
                            listBox1.Items.Add("Not Balanced: " + openingTag);
                            return;
                        }
                    }
                    // Check if the tag is a non-container tag
                    else if (!ignoredTags.Contains(tag))
                    {
                        listBox1.Items.Add("Found non-container tag: <" + tag + ">");
                    }
                }
            }
        }
    }
}
