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
using System.Linq;

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

            //Creating the Client
            var client = new SpeedrunComClient();

            //Searching for a game called "Wind Waker"
            var game = client.Games.SearchGame(name: v);

            TreeNode gameNode = CreateNodeHelper(game.Name);
            treeView1.Nodes.Add(gameNode);

            var mainCategoryNode = CreateNodeHelper("Categories");
            var mainLevelsNode = CreateNodeHelper("Levels");
            //Printing all the categories of the game
            foreach (var category in game.Categories)
            {
                var categoryNode = CreateNodeHelper(category.Name);

                Console.WriteLine(category.Name);

                foreach (var run in category.Runs)
                {
                    if ((run.Status.Type == RunStatusType.Verified)
                    || (run.Status.Type == RunStatusType.New && includeNewCheckBox.Checked)
                    || (run.Status.Type == RunStatusType.Rejected && includeRejectedCheckBox.Checked))
                    {
                        if (run.Level != null)
                        {
                            ManageLevelRun(mainLevelsNode, run);
                            runCount++;
                        } //if level is null, probably not an IL
                        else
                        {
                            ManageCategoryRun(run, categoryNode);
                            runCount++;
                        }
                    }
                }             

                if (categoryNode.Nodes.Find("Levels", true).Length > 0)
                {
                    mainLevelsNode.Nodes.Add(categoryNode);
                }
                else if(categoryNode.Nodes.Count > 0)
                {
                    mainCategoryNode.Nodes.Add(categoryNode);
                }
            }
            gameNode.Nodes.Add(mainCategoryNode);
            gameNode.Nodes.Add(mainLevelsNode);

            MessageBox.Show("Number of runs found: " + runCount);
        }

        private TreeNode CreateRunNode(Run run)
        {            
            var webLinkNode = CreateNodeHelper(run.WebLink.ToString());
            webLinkNode.Tag = "WebLink";

            var playerName = (run.Player != null) ? run.Player.Name : "Player Name N/A";

            var nodeLabel = run.Times.Primary.ToString() + " by " + playerName + " on " + run.Platform.ToString();

            var runVariables = run.VariableValues.ToList().FindAll(v => v.Variable.IsSubcategory == false);

            foreach(var variable in runVariables)
            {
                nodeLabel += " - " + variable.Value;
            }

            var node = new TreeNode(nodeLabel);
            node.Nodes.Add(webLinkNode);

            return node;
        }

        private void ManageCategoryRun(Run run, TreeNode categoryNode)
        {
            var subCategories = run.VariableValues.ToList().FindAll(v => v.Variable.IsSubcategory == true);
            var runNode = CreateRunNode(run);

            if (subCategories.Count > 0)
            {
                var varNodeToUse = new TreeNode();

                var subCatTreeNodeQueue = new List<TreeNode>();
                subCatTreeNodeQueue.Add(categoryNode);
                //Make sure all nodes are made.
                for (int i = 0; i < subCategories.Count; i++)
                {
                    if (subCatTreeNodeQueue[i].Nodes.Find(subCategories[i].Value, true).Length > 0)
                    {
                        subCatTreeNodeQueue.Add(subCatTreeNodeQueue[i].Nodes.Find(subCategories[i].Value, true)[0]);
                    }
                    else
                    {
                        //Create new sub category node
                        var newSubCatNode = CreateNodeHelper(subCategories[i].Value);
                        subCatTreeNodeQueue[i].Nodes.Add(newSubCatNode);
                        subCatTreeNodeQueue.Add(newSubCatNode);
                    }
                }

                //Attach the first sub cat to the category.  Secondary cats should already be attached with the code above.
                //if (!(categoryNode.Nodes.Find(subCatTreeNodeQueue[1].Name, true).Length > 0))
                //{
                //    categoryNode.Nodes.Add(subCatTreeNodeQueue[1]);
                //}

                //get the last item in the list to attach the run to.
                varNodeToUse = subCatTreeNodeQueue[subCatTreeNodeQueue.Count - 1];

                varNodeToUse.Nodes.Add(runNode);
            }
            else
            {
                categoryNode.Nodes.Add(runNode);
            }
        }

        private void ManageLevelRun(TreeNode mainLevelsNode, Run run)
        {
            var runNode = CreateRunNode(run);
            var levelNode = new TreeNode();
            var level = run.Level;
            var isNewLevel = !(mainLevelsNode.Nodes.Find(level.Name, false).Length > 0);

            if (isNewLevel)
            {
                levelNode.Text = level.Name;
                levelNode.Name = level.Name;
                mainLevelsNode.Nodes.Add(levelNode);
            }
            else
            {
                levelNode = mainLevelsNode.Nodes.Find(level.Name, false)[0];
            }

            var categoryNode = new TreeNode();
            var categoryName = run.Category.ToString();
            if (levelNode.Nodes.Find(categoryName, false).Length > 0)
            {
                categoryNode = levelNode.Nodes.Find(categoryName, false)[0];
            }
            else
            {
                categoryNode = CreateNodeHelper(categoryName);
                levelNode.Nodes.Add(categoryNode);
            }

            categoryNode.Nodes.Add(runNode);
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
