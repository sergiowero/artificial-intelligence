
namespace Uag.AI.Common.Trees
{
    public class MinMaxNode : Node<int>
    {
        public enum NodeType
        {
            Min,
            Max
        }

        public NodeType Type { get; private set; }

        public MinMaxNode(NodeType _type) : base()
        {
            Type = _type;
        }

        public override Node<int> CreateChildNode()
        {
            NodeType type = NodeType.Max;
            if (Type == NodeType.Max)
                type = NodeType.Min;
            return new MinMaxNode(type);
        }
    }
}

