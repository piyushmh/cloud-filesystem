using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudStorage
{
    public class TreeNode
    {
        public string nodeKey  { get; set; }
        public string nodeValue  { get; set; }
        public string nodePath { get; set; }

        Dictionary<string, TreeNode> children = new Dictionary<string, TreeNode> ();
        public TreeNode (string key, string value, string path)
        {
            nodeKey = key;
            nodeValue = value;
            nodePath = path;
        }

        List<TreeNode> getAllChildrenInfo()
        {
            List<TreeNode> childrens = new List<TreeNode> ();
            foreach(KeyValuePair<string, TreeNode> item in children)
            {
                childrens.Add (item.Value);
            }
            return childrens;
        }

        public static List<string> makeCombinedInfo(TreeNode rootnode, string currentPath)
        {
            List<string> allNames = new List<string> ();

            if (0 != rootnode.children.Count) {
                foreach (KeyValuePair<string, TreeNode> item in rootnode.children) {
                    List<string> childEntries = null;
                    if (currentPath.Equals ("_")) {
                        childEntries = makeCombinedInfo (item.Value, currentPath + item.Key);
                    } else {
                        childEntries = makeCombinedInfo (item.Value, currentPath + "_" + item.Key);
                    }
                    allNames.AddRange (childEntries);
                }
            } else if(false == rootnode.nodeKey.Equals("_")){
                allNames.Add (currentPath);
            }
            return allNames;
        }

        public static void AddNodeToTree(TreeNode rootNode, string key, string value)
        {
            string[] directories = key.Split('_');

            if (null == directories)
                return;

            TreeNode parentNode = rootNode;
            int currentCount = 0;
            string DirPath = "";
            foreach (string directory in directories) {
                if(directory.Length != 0)
                {
                    

                    if (0 == parentNode.children.Count
                        || false == parentNode.children.ContainsKey (directory)) {

                        TreeNode childNode = null;                       
                        string val = string.Format(directories[currentCount] + System.Environment.NewLine + "uploaded on: - Owner: - Size: -");
                        
                        if (currentCount == directories.Length - 1) {
                            DirPath = string.Format(DirPath + directory);
                            childNode = new TreeNode (directory, value, DirPath);
                        } else {
                            DirPath = string.Format(DirPath + directory + "_");
                            childNode = new TreeNode (directory, val, DirPath);
                        }

                        parentNode.children.Add (directory, childNode);
                        parentNode = childNode;
                    } else {
                        if (currentCount != directories.Length - 1)
                        {
                            DirPath = string.Format(DirPath + directory + "_");
                        }
                        parentNode = parentNode.children[directory];
                    }

                    currentCount++;
                }
            }
        }

        public static List<TreeNode> fetcAllChildren(string key, TreeNode rootNode)
        {
            string[] directories = key.Split('_');
            if (null == directories) {
                return null;
            }

            TreeNode parentNode = rootNode;
            foreach (string directory in directories) {
                if (directory.Length != 0) {
                    if (false == parentNode.children.ContainsKey (directory)) {
                        return null;
                    }
                    parentNode = parentNode.children [directory];
                }
            }

            return parentNode.getAllChildrenInfo ();
        }

        public static List<string> deleteNodeAndGetAllAffected(string key, TreeNode rootNode)
        {
            string[] directories = key.Split('_');
            if (null == directories) {
                return null;
            }

            TreeNode parentNode = rootNode;
            TreeNode grandParent = rootNode;

            foreach (string directory in directories) {
                if (directory.Length != 0) {
                    if (false == parentNode.children.ContainsKey (key)) {
                        return null;
                    }
                    grandParent = parentNode;
                    parentNode = parentNode.children [directory];
                }
            }

            grandParent.children.Remove (parentNode.nodeKey);

            List<string> allChildren =  makeCombinedInfo(parentNode,key);

            if (parentNode == grandParent) {
                parentNode.children.Clear ();
            }

            return allChildren;
        }
    }
}

