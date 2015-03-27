using System.Collections.Generic;

namespace MonoGraph
{
    public class AdjacencyGraph<TVertex, TEdge> where TEdge : IEdgeInterface<TVertex, TEdge>
    {
        private readonly Dictionary<TVertex, List<TEdge>> VertexEdgeDictionary;

        public AdjacencyGraph()
        {
            VertexEdgeDictionary = new Dictionary<TVertex, List<TEdge>>();
        }

        public void AddVertex(TVertex vertex)
        {
            if (ContainsVertex(vertex)) {
                throw new DuplicateVertexException(string.Format("Tried to add vertex '{0}' to the graph twice", vertex));
            }

            VertexEdgeDictionary.Add(vertex, new List<TEdge>());
        }

        private void CheckEdge(TEdge edge)
        {
            // Throws an exception if the edge already exists
            if (ContainsEdge(edge)) {
                throw new DuplicateEdgeException(string.Format("Tried to add edge '{0}' to the graph twice", edge));
            }
        }

        public void AddDirectedEdge(TEdge edge)
        {
            CheckEdge(edge);

            VertexEdgeDictionary[edge.First].Add(edge);
        }

        public void AddBidirectionalEdge(TEdge edge)
        {
            var rerverseEdge = edge.Reversed();

            CheckEdge(edge);
            CheckEdge(rerverseEdge);

            VertexEdgeDictionary[edge.First].Add(edge);
            VertexEdgeDictionary[edge.Second].Add(rerverseEdge);
        }

        public bool ContainsVertex(TVertex vertex)
        {
            return VertexEdgeDictionary.ContainsKey(vertex);
        }

        public bool ContainsEdge(TEdge edge)
        {
            return VertexEdgeDictionary[edge.First].Contains(edge);
        }

        public IEnumerable<TVertex> VertexIterator()
        {
            foreach(TVertex vertex in VertexEdgeDictionary.Keys)
            {
                yield return vertex;
            }
        }

        public IEnumerable<TEdge> EdgeIterator(TVertex vertex)
        {
            foreach(TEdge edge in VertexEdgeDictionary[vertex])
            {
                yield return edge;
            }
        }
    }
}