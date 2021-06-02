using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryTree
{
    // Tree constructors
    public BinaryTree()
    {

    }

    public BinaryTree(BinaryTreeNode _root)
    {
        root = _root;
    }

    public class BinaryTreeNode
    {
        public int index;
        public Object data;
        public BinaryTreeNode left;
        public BinaryTreeNode right;
    }

    public BinaryTreeNode root;

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

    public BinaryTreeNode Find(int index)
    {
        return Find(index, root);
    }

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

    public void TraversePreOrder(BinaryTreeNode parent)
    {
        if (parent != null)
        {
            Debug.Log(parent.index);
            TraversePreOrder(parent.left);
            TraversePreOrder(parent.right);
        }
    }

    public void TraversePostOrder(BinaryTreeNode parent)
    {
        if (parent != null)
        {
            TraversePostOrder(parent.left);
            TraversePostOrder(parent.right);
            Debug.Log(parent.index);
        }
    }

    public void TraverseInOrder(BinaryTreeNode parent)
    {
        if (parent != null)
        {
            TraverseInOrder(parent.left);
            Debug.Log(parent);
            TraverseInOrder(parent.right);
        }
    }

}