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
using System.IO;

namespace SRCTest
{
    public partial class Form1 : Form
    {
        struct RunData
        {
            public string runner;
            public string level;
            public string category;
            public List<KeyValuePair<string, bool>> variables;
            public string realtime;
            public string gametime;
            public bool primaryIsGameTime;
            public string platform;
            public string date;
            public string video;
            public string srcLink;
            public string runStatus;
        }

        List<RunData> Runs = new List<RunData>();

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
            treeView1.Nodes.Clear();

            Runs.Clear();

            //Creating the Client
            var client = new SpeedrunComClient();

            //Searching for a game called "Wind Waker"
            var game = client.Games.SearchGame(name: v);

            if (game != null)
            {
                //Printing all the categories of the game
                foreach (var category in game.Categories)
                {
                    Console.WriteLine(category.Name);

                    foreach (var run in category.Runs)
                    {
                        Runs.Add(CreateRunData(run));
                    }
                }

                CreateTree(game.Name, Runs);
            }
            else
            {
                MessageBox.Show("No Result for Game Search.");
            }
        }

        private RunData CreateRunData(Run run)
        {
            var runData = new RunData();

            runData.runner = (run.Player != null) ? run.Player.ToString() : "Player Name N/A";
            runData.level = (run.Level != null) ? run.Level.ToString() : "";
            runData.category = run.Category.ToString();
            runData.variables = new List<KeyValuePair<string, bool>>();
            for(int i = 0; i < run.VariableValues.Count; i++)
            {
                runData.variables.Add(new KeyValuePair<string, bool>(run.VariableValues[i].ToString(), run.VariableValues[i].Variable.IsSubcategory));
            }
            runData.realtime = run.Times.RealTime.ToString();
            runData.gametime = run.Times.GameTime.ToString();
            runData.primaryIsGameTime = run.Times.Primary == run.Times.GameTime;
            runData.platform = run.Platform.ToString();
            runData.date = run.Date.ToString();

            if(run.Videos != null && run.Videos.Links.Count > 0)
            {
                //This should be loop friendly for when I have time to work on this more.
                if (run.Videos.Links[0] != null)
                {
                    runData.video = run.Videos.Links[0].ToString();
                }
            }
            
            runData.srcLink = run.WebLink.ToString();
            runData.runStatus = run.Status.Type.ToString();
            return runData;
    }

        private void CreateTree(string name, List<RunData> runs)
        {
            MessageBox.Show("Number of runs found: " + runs.Count);

            var gameNode = CreateNodeHelper(name);
            treeView1.Nodes.Add(gameNode);

            var mainCategoryNode = CreateNodeHelper("Categories");
            var mainLevelsNode = CreateNodeHelper("Levels");

            Dictionary<string, List<RunData>> categories = new Dictionary<string, List<RunData>>();

            foreach(var run in runs)
            {
                if(!categories.Keys.Contains(run.category))
                {
                    categories[run.category] = new List<RunData>();
                }
                categories[run.category].Add(run);
            }

            foreach (var category in categories)
            {
                var categoryNode = CreateNodeHelper(category.Key);

                foreach (var run in category.Value)
                {
                    if ((run.runStatus == RunStatusType.Verified.ToString())
                    || (run.runStatus == RunStatusType.New.ToString() && includeNewCheckBox.Checked)
                    || (run.runStatus == RunStatusType.Rejected.ToString() && includeRejectedCheckBox.Checked))
                    {
                        if (!string.IsNullOrEmpty(run.level))
                        {
                            ManageLevelRun(mainLevelsNode, run);
                        } //if level is null, probably not an IL
                        else
                        {
                            ManageCategoryRun(run, categoryNode);
                        }
                    }
                }

                if (categoryNode.Nodes.Find("Levels", true).Length > 0)
                {
                    mainLevelsNode.Nodes.Add(categoryNode);
                }
                else if (categoryNode.Nodes.Count > 0)
                {
                    mainCategoryNode.Nodes.Add(categoryNode);
                }
            }

            gameNode.Nodes.Add(mainCategoryNode);
            gameNode.Nodes.Add(mainLevelsNode);
        }

        private TreeNode CreateRunNode(RunData run)
        {            
            var webLinkNode = CreateNodeHelper(run.srcLink);
            webLinkNode.Tag = "WebLink";

            var nodeLabel = ((run.primaryIsGameTime) ? run.gametime : run.realtime) + " by " + run.runner + " on " + run.platform;

            var runVariables = run.variables.FindAll(v => v.Value == false);

            foreach(var variable in runVariables)
            {
                nodeLabel += " - " + variable.Key;
            }

            var node = new TreeNode(nodeLabel);
            node.Nodes.Add(webLinkNode);

            return node;
        }

        private void ManageCategoryRun(RunData run, TreeNode categoryNode)
        {
            //run.variables is key variableName, value = isSubCategory
            var subCategories = run.variables.FindAll(v => v.Value == true);
            var runNode = CreateRunNode(run);

            if (subCategories.Count > 0)
            {
                var varNodeToUse = new TreeNode();

                var subCatTreeNodeQueue = new List<TreeNode>();
                subCatTreeNodeQueue.Add(categoryNode);
                //Make sure all nodes are made.
                for (int i = 0; i < subCategories.Count; i++)
                {
                    if (subCatTreeNodeQueue[i].Nodes.Find(subCategories[i].Key, true).Length > 0)
                    {
                        subCatTreeNodeQueue.Add(subCatTreeNodeQueue[i].Nodes.Find(subCategories[i].Key, true)[0]);
                    }
                    else
                    {
                        //Create new sub category node
                        var newSubCatNode = CreateNodeHelper(subCategories[i].Key);
                        subCatTreeNodeQueue[i].Nodes.Add(newSubCatNode);
                        subCatTreeNodeQueue.Add(newSubCatNode);
                    }
                }

                //get the last item in the list to attach the run to.
                varNodeToUse = subCatTreeNodeQueue[subCatTreeNodeQueue.Count - 1];

                varNodeToUse.Nodes.Add(runNode);
            }
            else
            {
                categoryNode.Nodes.Add(runNode);
            }
        }

        private void ManageLevelRun(TreeNode mainLevelsNode, RunData run)
        {
            var runNode = CreateRunNode(run);
            var levelNode = new TreeNode();
            var level = run.level;
            var isNewLevel = !(mainLevelsNode.Nodes.Find(level, false).Length > 0);

            if (isNewLevel)
            {
                levelNode.Text = level;
                levelNode.Name = level;
                mainLevelsNode.Nodes.Add(levelNode);
            }
            else
            {
                levelNode = mainLevelsNode.Nodes.Find(level, false)[0];
            }

            var categoryNode = new TreeNode();
            var categoryName = run.category;
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

        private void csvExportButton_Click(object sender, EventArgs e)
        {
            if(Runs.Count > 1)
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    sfd.AddExtension = true;
                    sfd.DefaultExt = "csv";
                    sfd.Filter = "csv (*.csv)|*.csv";

                    var result = sfd.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(sfd.FileName))
                    {
                        if (!File.Exists(sfd.FileName))
                        {
                            using (var stream = File.CreateText(sfd.FileName))
                            {
                                //Game Name as first row
                                stream.WriteLine(treeView1.Nodes[0].Text);

                                //Dynamicly determine the number of columns needed for variable data.
                                var maxVariableCount = 0;
                                {
                                    foreach (var run in Runs)
                                    {
                                        if (run.variables.Count > maxVariableCount)
                                        {
                                            maxVariableCount = run.variables.Count;
                                        }
                                    }
                                }
                                var variableCount = (checkBox1.Checked) ? numericUpDown1.Value : maxVariableCount;

                                string valueHeader = "Runner,";
                                valueHeader += "Level,";
                                valueHeader += "Category,";

                                for (int i = 0; i < variableCount; i++)
                                {
                                    valueHeader += "Variable " + (i + 1) + ",";
                                    valueHeader += "Is Variable " + (i + 1) + " Sub Category,";
                                }

                                valueHeader += "Real Time,";
                                valueHeader += "Game Time,";
                                valueHeader += "Primary Time is Game Time,";

                                valueHeader += "Platform,";
                                valueHeader += "Date,";
                                valueHeader += "Video,";
                                valueHeader += "SRC Link,";
                                valueHeader += "Run Status";

                                stream.WriteLine(valueHeader);

                                foreach (var run in Runs)
                                {
                                    //runDataString += run + ",";
                                    string runDataString = run.runner + ",";
                                    runDataString += run.level + ",";
                                    runDataString += run.category + ",";

                                    for (int i = 0; i < variableCount; i++)
                                    {
                                        if (run.variables.Count < i + 1)
                                        {
                                            runDataString += ",";
                                            runDataString += ",";
                                        }
                                        else
                                        {
                                            runDataString += run.variables[i].Key + ",";
                                            runDataString += run.variables[i].Value + ",";
                                        }
                                    }

                                    runDataString += run.realtime + ",";
                                    runDataString += run.gametime + ",";
                                    runDataString += run.primaryIsGameTime + ",";

                                    runDataString += run.platform + ",";
                                    runDataString += run.date + ",";
                                    runDataString += run.video + ",";
                                    runDataString += run.srcLink + ",";
                                    runDataString += run.runStatus;

                                    stream.WriteLine(runDataString);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No Run Data to Export");
            }
        }
    }
}
