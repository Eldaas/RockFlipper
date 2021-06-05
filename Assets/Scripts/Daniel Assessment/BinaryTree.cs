using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryTree
{
    #region Constructors
    public BinaryTree()
    {

    }

    public BinaryTree(BinaryTreeNode _root)
    {
        root = _root;
    }
    #endregion

    #region Fields
    public BinaryTreeNode root;
    #endregion

    #region Public Methods
    /// <summary>
    /// Creates a new node at the specified index.
    /// </summary>
    /// <param name="index">The index at which to generate a new node.</param>
    /// <returns>True if successful, false if unsuccessful.</returns>
    public bool CreateNodeAtIndex(int index)
    {
        BinaryTreeNode before = null;
        BinaryTreeNode after = root;

        while (after != null)
        {
            before = after;
            if (index < after.index)
            {
                after = after.left;
            }
            else if (index > after.index)
            {
                after = after.right;
            }
            else
            {
                return false;
            }
        }

        BinaryTreeNode newNode = new BinaryTreeNode();
        newNode.index = index;

        if (root == null)
        {
            root = newNode;
        }
        else
        {
            if (index < before.index)
            {
                before.left = newNode;
            }
            else
            {
                before.right = newNode;
            }
        }

        return true;
    }

    /// <summary>
    /// Creates a new node at the next available index.
    /// </summary>
    /// <param name="node">The node to be inserted into the binary tree.</param>
    /// <returns>True if successful, false if unsuccessful.</returns>
    public bool CreateNode(BinaryTreeNode node)
    {
        if (node != null)
        {
            if (Find(node.index) == null)
            {
                BinaryTreeNode before = null;
                BinaryTreeNode after = root;

                while (after != null)
                {
                    before = after;

                    if (node.index < after.index)
                    {
                        after = after.left;
                    }
                    else if (node.index > after.index)
                    {
                        after = after.right;
                    }
                    else
                    {
                        return false;
                    }
                }

                if (root == null)
                {
                    root = node;
                }
                else
                {
                    if (node.index < before.index)
                    {
                        before.left = node;
                    }
                    else
                    {
                        before.right = node;
                    }
                }

                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Overload for the Find() method, to be used when the root node doesn't need to be specified.
    /// </summary>
    /// <param name="index">The index of the binary tree containing the desired node.</param>
    /// <returns>If found, returns the node. If not found, returns null.</returns>
    public BinaryTreeNode Find(int index)
    {
        return Find(index, root);
    }

    /// <summary>
    /// Searches the binary tree for HighScores data with name matching the input query. 
    /// </summary>
    /// <param name="query">The string to be searched for within each of the HighScores records held within the binary tree.</param>
    /// <returns>If found, returns the node. If not found, returns null.</returns>
    public BinaryTreeNode Find(string query)
    {
        BinaryTreeNode node = root;
        bool found = false;

        while (node != null && !found)
        {
            if (node.data.name == query)
            {
                found = true;
                return node;
            }

            node = node.right;
        }

        return null;
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Searches the binary tree for a particular node at the input index.
    /// </summary>
    /// <param name="index">The index to find within the binary tree.</param>
    /// <param name="parent">The node to start traversing through the binary tree from.</param>
    /// <returns>If found, returns the node. If not found, returns null.</returns>
    private BinaryTreeNode Find(int index, BinaryTreeNode parent)
    {
        if (parent != null)
        {
            if (index == parent.index)
            {
                return parent;
            }
            else if (index < parent.index)
            {
                return Find(index, parent.left);
            }
            else
            {
                return Find(index, parent.right);
            }
        }
        else
        {
            return null;
        }

    }
    #endregion

    /// <summary>
    /// Represents a single node within a binary tree structure.
    /// </summary>
    public class BinaryTreeNode
    {
        public int index;
        public HighScores.DreamloData.Dreamlo.Leaderboard.HighScoreRecord data;
        public BinaryTreeNode left;
        public BinaryTreeNode right;
    }
}