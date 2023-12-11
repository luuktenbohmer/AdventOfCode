type Point = [number, number];

type Direction = "north" | "east" | "south" | "west";
type OriginatedPoint = {
  point: Point;
  origin: Direction;
};

const connections = new Map<string | undefined, Direction[]>([
  ["|", ["north", "south"]],
  ["-", ["east", "west"]],
  ["L", ["north", "east"]],
  ["J", ["north", "west"]],
  ["7", ["south", "west"]],
  ["F", ["south", "east"]],
]);

const getNext = (grid: string[], current: OriginatedPoint): OriginatedPoint => {
  const [x, y] = current.point;
  const currentConnection = grid[y][x];
  const directions = connections.get(currentConnection) ?? [];
  const direction = directions.find((d) => d !== current.origin);

  switch (direction) {
    case "north":
      return { point: [x, y - 1], origin: "south" };
    case "east":
      return { point: [x + 1, y], origin: "west" };
    case "south":
      return { point: [x, y + 1], origin: "north" };
    case "west":
      return { point: [x - 1, y], origin: "east" };
  }
};

const getConnected = (grid: string[], origin: Point): OriginatedPoint[] => {
  const [x, y] = origin;
  const connected: OriginatedPoint[] = [];

  const getConnectionOrEmpty = (direction: string) => {
    return connections.get(direction) ?? [];
  };

  const north = grid[y - 1]?.[x];
  if (getConnectionOrEmpty(north).includes("south")) {
    connected.push({ point: [x, y - 1], origin: "south" });
  }

  const south = grid[y + 1]?.[x];
  if (getConnectionOrEmpty(south).includes("north")) {
    connected.push({ point: [x, y + 1], origin: "north" });
  }

  const east = grid[y]?.[x + 1];
  if (getConnectionOrEmpty(east).includes("west")) {
    connected.push({ point: [x + 1, y], origin: "west" });
  }

  const west = grid[y]?.[x - 1];
  if (getConnectionOrEmpty(west).includes("east")) {
    connected.push({ point: [x - 1, y], origin: "east" });
  }

  return connected;
};

const getS = (grid: string[]): Point => {
  const sX = grid.map((row) => row.indexOf("S")).filter((i) => i !== -1)[0];
  const sY = grid.findIndex((row) => row.includes("S"));

  return [sX, sY];
};

const getId = (point: Point) => point[0] * 10_000 + point[1];

export const solve = (grid: string[]) => {
  const s = getS(grid);

  let currents = getConnected(grid, s);
  let count = 1;

  while (currents.distinctBy((d) => getId(d.point)).length !== 1) {
    currents = currents.map((p) => getNext(grid, p));
    count++;
  }

  return count;
};

// There might have been an easier solution...
export const solve2 = (grid: string[]) => {
  const s = getS(grid);
  const connected = getConnected(grid, s);

  let current = connected[0];
  const partOfLoop: number[] = [getId([s[0] * 2, s[1] * 2]), getId([s[0] + current.point[0], s[1] + current.point[1]])];

  while (current.point[0] !== s[0] || current.point[1] !== s[1]) {
    const currentAdd: Point = [current.point[0] * 2, current.point[1] * 2];
    partOfLoop.push(getId(currentAdd));
    current = getNext(grid, current);

    const middleAdd: Point = [(currentAdd[0] + current.point[0] * 2) / 2, (currentAdd[1] + current.point[1] * 2) / 2];
    partOfLoop.push(getId(middleAdd));
  }

  const checked: number[] = [...partOfLoop];
  let containedCount = 0;

  let extendedGrid: string[] = grid.map((row) => row.split("").join("."));
  for (let i = extendedGrid.length - 1; i >= 1; i--) {
    extendedGrid.splice(i, 0, Array(extendedGrid[0].length).fill(".").join(""));
  }

  for (let y = 0; y < extendedGrid.length; y++) {
    for (let x = 0; x < extendedGrid[y].length; x++) {
      const id = getId([x, y]);
      if (checked.includes(id) || partOfLoop.includes(id)) {
        continue;
      }

      checked.push(id);

      // Search way out
      const adjacents: Point[] = [[x, y]];
      const visited: number[] = [id];

      let currentX = x;
      let currentY = y;
      let contained = true;
      while (adjacents.length) {
        const current = adjacents.shift();
        currentX = current[0];
        currentY = current[1];

        if (extendedGrid[currentY]?.[currentX] === undefined) {
          contained = false;
          continue;
        }

        const adjacentsToAdd = (
          [
            [currentX + 1, currentY],
            [currentX - 1, currentY],
            [currentX, currentY + 1],
            [currentX, currentY - 1],
          ] as Point[]
        ).filter((a) => !partOfLoop.includes(getId(a)) && !visited.includes(getId(a)));

        adjacents.push(...adjacentsToAdd);
        checked.push(...adjacentsToAdd.map(getId));
        visited.push(...adjacentsToAdd.map(getId));
      }

      if (contained) {
        containedCount += visited.filter((v) => Math.floor(v / 10_000) % 2 === 0 && v % 2 === 0).length;
      }
    }
  }

  return containedCount;
};
