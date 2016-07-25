using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Uag.AI.Common.Trees
{
    public class Node
    {
        private List<Node> m_children;

        public ReadOnlyCollection<Node> children { get { return m_children.AsReadOnly(); } }
        public int value;

        public Node()
        {
            m_children = new List<Node>();
            value = 0;
        }

        public void AddNode(int _value)
        {
            Node node = CreateChildNode();
            node.value = _value;
            m_children.Add(node);
        }

        public virtual Node CreateChildNode()
        {
            return new Node();
        }
    }
}

