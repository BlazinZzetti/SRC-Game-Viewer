using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpeedrunComSharp;

namespace SRCTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();                   
        }

        private TreeNode CreateNodeHelper(string nameValue)
        {
            var node = new TreeNode(nameValue);
            node.Name = nameValue;
            return node;
        }

        private void SearchSRC(string v)
        {
            int runCount = 0;
            treeView1.Nodes.Clear();
            TreeNode gameNode = CreateNodeHelper(v);
            treeView1.Nodes.Add(gameNode);
            //Creating the Client
            var client = new SpeedrunComClient();

            //Searching for a game called "Wind Waker"
            var game = client.Games.SearchGame(name: v);

            var mainCategoryNode = CreateNodeHelper("Categories");
            //Printing all the categories of the game
            foreach (var category in game.Categories)
            {
                var categoryNode = CreateNodeHelper(category.Name);

                Console.WriteLine(category.Name);
                foreach (var variables in category.Variables)
                {
                    var variableNode = new TreeNode(variables.Name);
                    Console.WriteLine("-" + variables.Name);
                    foreach (var value in variables.Values)
                    {
                        var valueNode = CreateNodeHelper(value.Value);
                        Console.WriteLine("- -" + value.Value);
                        variableNode.Nodes.Add(valueNode);
                    }
                    categoryNode.Nodes.Add(variableNode);
                }

                var mainRunsNode = CreateNodeHelper("Runs");
                var mainLevelNode = CreateNodeHelper("Levels");
                var levelList = new List<string>();

                foreach(var run in category.Runs)
                {
                    var runVariables = new List<KeyValuePair<string, string>>();
                    foreach (var variableValue in run.VariableValues)
                    {
                        runVariables.Add(new KeyValuePair<string, string>(variableValue.Name, variableValue.Value));
                    }

                    var mainTime = run.Times.Primary.ToString();
                    var player = run.Player.Name;
                    var runNode = new TreeNode(mainTime + " by " + player);
                    var webLinkNode = CreateNodeHelper(run.WebLink.ToString());
                    webLinkNode.Tag = "WebLink";

                    runNode.Nodes.Add(webLinkNode);
                    runCount++;

                    var level = run.Level;
                    var levelNode = new TreeNode();
                    if (level != null)
                    {
                        if (!levelList.Contains(level.Name))
                        {
                            levelList.Add(level.Name);
                            levelNode.Text = level.Name;
                            levelNode.Name = level.Name;
                            mainLevelNode.Nodes.Add(levelNode);
                        }
                        else
                        {
                            levelNode = mainLevelNode.Nodes.Find(level.Name, false)[0];
                        }
                        levelNode.Nodes.Add(runNode);
                    } //if level is null, probably not an IL
                    else
                    {
                        if (runVariables.Count > 0)
                        {
                            var varNode = categoryNode.Nodes.Find(runVariables[0].Value, true)[0];
                            varNode.Nodes.Add(runNode);
                        }
                        else
                        {
                            mainRunsNode.Nodes.Add(runNode);
                        }
                    }                   
                }

                categoryNode.Nodes.Add(mainLevelNode);
                categoryNode.Nodes.Add(mainRunsNode);
                mainCategoryNode.Nodes.Add(categoryNode);
            }
            gameNode.Nodes.Add(mainCategoryNode);

            MessageBox.Show("Number of runs found" + runCount);
            return;

            //Searching for the category "Any%"
            var noWarps = game.Categories.First(category => category.Name == "No Warps");

            var noWarpsNeutral = noWarps.Variables.First(subCategory => subCategory.Values.First(subCat => subCat.Value == "Neutral").Value == "Neutral");

            var noWarpsNeutralBoards = client.Leaderboards.GetLeaderboardForFullGameCategory(noWarpsNeutral.GameID, noWarps.ID, top: 1);
            //Finding the World Record of the category
            //var worldRecord = noWarpsNeutral.Category.WorldRecord;

            //Printing the World Record's information
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if(e.Node.Tag == "WebLink")
            {
                System.Diagnostics.Process.Start(new ProcessStartInfo
                {
                    FileName = e.Node.Text,
                    UseShellExecute = true
                });
            }
        }

        private void SearchSRCButton_Click(object sender, EventArgs e)
        {
            SearchSRC(srcSearchTextBox.Text);
        }
    }
}
