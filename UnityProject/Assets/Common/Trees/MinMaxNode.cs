
namespace Uag.AI.Common.Trees
{
    public class MinMaxNode : Node<int>
    {
        public enum NodeType
        {
            Min,
            Max
        }

        public NodeType type { get; private set; }

        public MinMaxNode(NodeType _type) : base()
        {
            type = _type;
        }

        public override Node<int> CreateChildNode()
        {
            NodeType type = NodeType.Max;
            if (this.type == NodeType.Max)
                type = NodeType.Min;
            return new MinMaxNode(type);
        }
    }
}

