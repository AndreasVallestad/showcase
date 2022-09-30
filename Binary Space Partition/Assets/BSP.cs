using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valley.BSP
{
    public class BSP : MonoBehaviour
    {
        //TODO: Refactor so minSize refers to partition size instead of room size, handle all borderspace logic in Room script. Pass difference as argument in CreateRoom()
        [SerializeField]
        private GameObject _partitionPrefab;
        [SerializeField]
        private GameObject _roomPrefab;

        private static int _depth = 6; //The depth value of how many splits will be performed. 
        private static int _minSize = 5; //Minimum possible size of the room within a partition
        private static int _borderSpace = 1; //Minimum required space between partition side and the room it contains.
        
        Node _rootNode;


        public void Awake()
        {
            _rootNode = new Node(0, 0, 60, 60, 0);
            _rootNode.Split();

            foreach(Node n in GetAllLeafNodes(_rootNode))
            {
                var partition = Instantiate(_partitionPrefab);
                partition.transform.position = new Vector3(n.X, n.Y, 0f);
                //Scale the sprite via 9-sliced spriterenderer to prevent edge deforming.
                partition.GetComponent<SpriteRenderer>().size = new Vector2(n.Width, n.Height);
                
                //Create room
                n.Room = Instantiate(_roomPrefab).GetComponent<Room>();
                n.Room.CreateRoom(n, _minSize);
                /*
                if (n.Sibling != null && n.Sibling.Room != null)
                {
                    //Draw line between partition and sibling partition
                    var thisPos = new Vector2(n.Room.X + n.Room.Width/2, n.Room.Y + n.Room.Height/2);
                    var thatPos = new Vector2(n.Sibling.Room.X + n.Sibling.Room.Width/2, n.Sibling.Room.Y + n.Sibling.Room.Height/2);
                    Debug.DrawLine(thisPos, thatPos, Color.black, Mathf.Infinity);
                }
                */
            }

            
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
            
            foreach(Node n in GetAllLeafNodes(_rootNode))
            {
                if (n.Room == null || n.Sibling == null || n.Sibling.Room == null)
                    continue;
                var thisPos = new Vector2(n.Room.X + n.Room.Width/2, n.Room.Y + n.Room.Height/2);
                var thatPos = new Vector2(n.Sibling.Room.X + n.Sibling.Room.Width/2, n.Sibling.Room.Y + n.Sibling.Room.Height/2);
                Debug.DrawLine(thisPos, thatPos, Color.black);
            }
            
        }

        private List<Node> GetAllChildNodes(Node node)
        {
            List<Node> results = new List<Node>();

            //Get all child nodes
            if (node.LeftChild != null)
            {
                //Add the current nodes left child
                results.Add(node.LeftChild);
                //Recursively add the childs children as well
                foreach(Node n in GetAllChildNodes(node.LeftChild))
                    results.Add(n);
            }
                
            if (node.RightChild != null)
            {
                //Add the current nodes left child
                results.Add(node.RightChild);
                //Recursively add the childs children as well
                foreach(Node n in GetAllChildNodes(node.RightChild))
                results.Add(n);
            }
                

            //Recursively add the childrens children
            
            

            return results;
        }

        //Get all leaf nodes below the parameter node in the tree hierarchy
        private List<Node> GetAllLeafNodes(Node node)
        {
            List<Node> results = new List<Node>();
            //Get all leftChild nodes
            if (node.LeftChild.IsLeaf)
                results.Add(node.LeftChild);
            else
            {
                //If not a leaf, recurse deeper to check if its children are leaves
                foreach(Node n in GetAllLeafNodes(node.LeftChild))
                    results.Add(n);
            }
            //Get all rightChild nodes
            if (node.RightChild.IsLeaf)
                results.Add(node.RightChild);
            else
            {
                //If not a leaf, recurse deeper to check if its children are leaves
                foreach(Node n in GetAllLeafNodes(node.RightChild))
                    results.Add(n);
            }
            return results;
        }

        public class Node
        {
            public int X, Y, Width, Height;
            public Node Parent;
            public Node Sibling;
            public Node LeftChild;
            public Node RightChild;
            public Room Room;
            public int Depth;
            public bool IsLeaf = false;

            public Node (int x, int y, int width, int height, int depth)
            {
                this.X = x;
                this.Y = y;
                this.Width = width;
                this.Height = height;
                this.Depth = depth;
                Debug.Log("X = " + this.X + " | Y = " + this.Y + " | Width = " + this.Width + " Height = " + this.Height);
            }

            public void Split()
            {
                //Cancel continued recursive splitting if the correct amount of splits has ben performed, or the room is already too small to split (meaning not large enough to split without going below min room size for its child nodes)
                if (Depth + 1 >= BSP._depth)
                {
                    IsLeaf = true;
                    return;
                }

                bool SplitX = System.Convert.ToBoolean(Random.Range(0, 2));
                int splitPos;
                
                #region X-Split
                //Do X-split if the partition is wide enough to contain two new partitions and their rooms without going below minimum restrictions. Otherwise attempt a Y-split instead.
                if (SplitX && Width >= BSP._minSize*2 + _borderSpace*4)
                {
                    //Set a split position
                    splitPos = Random.Range(_minSize +2, Width - _minSize-1); // positions are 0-indexed and max is exclusive. Range is 1 to width-1. Meaning one in from either side.
                    Debug.Log("["+Width+", " + Height +"] X-Split: Relative X = " + splitPos + " | Absolute X = " + (X + splitPos));

                    //Create new nodes on either side of the split
                    Debug.Log("New LeftNode: ");
                    LeftChild = new Node(X, Y, splitPos, Height, Depth+1);
                    LeftChild.Parent = this;
                    Debug.Log("New RightNode: ");
                    RightChild = new Node(X + splitPos, Y, Width - splitPos, Height, Depth+1);
                    RightChild.Parent = this;


                    LeftChild.Split();
                    LeftChild.Sibling = RightChild;
                    RightChild.Split();
                    RightChild.Sibling = LeftChild;
                    return;
                }
                #endregion

                #region Y-Split //Left child is in the context the top child. Right is bottom child
                //Cancel split if partition isn't tall enough  for the split partitions and rooms to fit their minimum sizes.
                if (Height < BSP._minSize*2 + _borderSpace*4)
                {
                    IsLeaf = true;
                    return;
                }

                //Set a split position
                splitPos = Random.Range(_minSize +2, Height - _minSize -1); // positions are 0-indexed and max is exclusive. Range is 1 to height-1. Meaning one in from either side.
                Debug.Log("["+Width+", " + Height +"] Y-Split: Relative Y = " + splitPos + " | Absolute Y = " + (Y + splitPos));

                //Create new nodes on either side of the split
                Debug.Log("New BotNode: ");
                LeftChild = new Node(X, Y, Width, splitPos, Depth+1);
                LeftChild.Parent = this;
                Debug.Log("New TopNode: ");
                RightChild = new Node(X, Y + splitPos, Width, Height - splitPos, Depth+1);
                RightChild.Parent = this;


                LeftChild.Split();
                LeftChild.Sibling = RightChild;
                RightChild.Split();
                RightChild.Sibling = LeftChild;
                #endregion

                
            }
        }
    }
}