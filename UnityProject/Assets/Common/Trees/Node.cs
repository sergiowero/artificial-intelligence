using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Uag.AI.Common.Trees
{
    public class Node<T>
    {
        public List<Node<T>> m_children;

        public ReadOnlyCollection<Node<T>> Children { get { return m_children.AsReadOnly(); } }
        public T Value;

        public Node()
        {
            m_children = new List<Node<T>>();
            Value = default(T);
        }

        public void AddNode(T _value)
        {
            Node<T> node = CreateChildNode();
            node.Value = _value;
            m_children.Add(node);
        }

        public virtual Node<T> CreateChildNode()
        {
            return new Node<T>();
        }
    }
}

