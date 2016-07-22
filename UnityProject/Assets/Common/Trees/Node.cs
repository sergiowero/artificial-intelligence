using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Uag.AI.Common.Trees
{
    public class Node<T>
    {
        private List<Node<T>> m_children;

        public ReadOnlyCollection<Node<T>> children { get { return m_children.AsReadOnly(); } }
        public T value;

        public Node()
        {
            m_children = new List<Node<T>>();
            value = default(T);
        }

        public void AddNode(T _value)
        {
            Node<T> node = CreateChildNode();
            node.value = _value;
            m_children.Add(node);
        }

        public virtual Node<T> CreateChildNode()
        {
            return new Node<T>();
        }
    }
}

