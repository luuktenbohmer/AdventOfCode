export class Graph<T> {
  private adjacencyList: Map<T, Map<T, number>>;

  constructor() {
    this.adjacencyList = new Map<T, Map<T, number>>();
  }

  addVertex(vertex: T): void {
    if (!this.adjacencyList.has(vertex)) {
      this.adjacencyList.set(vertex, new Map<T, number>());
    }
  }

  addEdge(source: T, destination: T, weight: number = 1): void {
    if (!this.adjacencyList.has(source)) {
      this.addVertex(source);
    }
    if (!this.adjacencyList.has(destination)) {
      this.addVertex(destination);
    }
    this.adjacencyList.get(source)?.set(destination, weight);
    this.adjacencyList.get(destination)?.set(source, weight);
  }

  getNeighbors(vertex: T): T[] {
    const neighbors: T[] = [];
    const edges = this.adjacencyList.get(vertex);
    if (edges) {
      for (const neighbor of edges.keys()) {
        neighbors.push(neighbor);
      }
    }
    return neighbors;
  }

  getWeight(source: T, destination: T): number | undefined {
    const edges = this.adjacencyList.get(source);

    return edges?.get(destination);
  }
}

export function bfs<T>(graph: Graph<T>, start: T, target: T): T[] | undefined {
  const visited = new Set<T>();
  const queue: T[][] = [[start]];

  while (queue.length > 0) {
    const path = queue.shift()!;
    const current = path[path.length - 1];

    if (!current) {
      continue;
    }

    if (current === target) {
      return path;
    }

    if (!visited.has(current)) {
      visited.add(current);
      const neighbors = graph.getNeighbors(current);
      for (const neighbor of neighbors) {
        queue.push([...path, neighbor]);
      }
    }
  }

  return undefined;
}

export function astar<T>(graph: Graph<T>, start: T, target: T, heuristic: (node: T) => number): T[] | undefined {
  const visited = new Set<T>();
  const queue: [T[], number][] = [[[start], 0]];

  while (queue.length > 0) {
    queue.sort((a, b) => a[1] - b[1]);
    const [path, cost] = queue.shift()!;
    const current = path[path.length - 1];

    if (!current) {
      continue;
    }

    if (current === target) {
      return path;
    }

    if (!visited.has(current)) {
      visited.add(current);
      const neighbors = graph.getNeighbors(current);
      for (const neighbor of neighbors) {
        const weight = graph.getWeight(current, neighbor);
        if (weight !== undefined) {
          const newPath = [...path, neighbor];
          const newCost = cost + weight + heuristic(neighbor);
          queue.push([newPath, newCost]);
        }
      }
    }
  }

  return undefined;
}
