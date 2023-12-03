import { isNumber } from "../../utils";

type NumberCell = {
  type: "number";
  numberString: string;
  value: number;
};

type SymbolCell = {
  type: "symbol";
};

type GearCell = {
  type: "gear";
};

type EmptyCell = {
  type: "empty";
};

type Cell = NumberCell | SymbolCell | GearCell | EmptyCell;

const getGrid = (input: string[]) => {
  const grid: Cell[][] = [];

  for (let x = 0; x < input.length; x++) {
    const line = input[x];
    grid.push([]);

    for (let y = 0; y < line.length; y++) {
      let char = line[y];

      if (isNumber(char)) {
        const number: NumberCell = {
          type: "number",
          numberString: "",
          value: 0,
        };

        while (isNumber(char)) {
          number.numberString += char;
          grid[x].push(number);
          y++;
          char = line[y];
        }

        number.value = parseInt(number.numberString);
      }

      if (char === ".") {
        grid[x].push({ type: "empty" });
        continue;
      }

      if (char === "*") {
        grid[x].push({ type: "gear" });
        continue;
      }

      if (char) {
        grid[x].push({ type: "symbol" });
      }
    }
  }

  return grid;
};

const getAdjacentCells = (grid: Cell[][], x: number, y: number) => {
  const cells: (Cell | undefined)[] = [
    grid[x - 1]?.[y - 1],
    grid[x - 1]?.[y],
    grid[x - 1]?.[y + 1],
    grid[x][y - 1],
    grid[x][y + 1],
    grid[x + 1]?.[y - 1],
    grid[x + 1]?.[y],
    grid[x + 1]?.[y + 1],
  ];

  return cells;
};

export const solve = (input: string[]) => {
  const grid = getGrid(input);
  const numbers: NumberCell[] = [];
  for (let x = 0; x < grid.length; x++) {
    for (let y = 0; y < grid[0].length; y++) {
      const cell = grid[x][y];
      if (cell.type !== "number") {
        continue;
      }

      const adjacentCells = getAdjacentCells(grid, x, y);
      if (adjacentCells.some((c) => c?.type === "symbol" || c?.type === "gear")) {
        numbers.push(cell);
      }
    }
  }

  return numbers.distinct().sum((n) => n.value);
};

export const solve2 = (input: string[]) => {
  const grid = getGrid(input);
  const gears: [number, number][] = [];
  for (let x = 0; x < grid.length; x++) {
    for (let y = 0; y < grid[0].length; y++) {
      const cell = grid[x][y];
      if (cell.type !== "gear") {
        continue;
      }

      const adjacentCells = getAdjacentCells(grid, x, y);
      const isNumberCell = (cell: Cell): cell is NumberCell => cell.type === "number";
      const adjacentNumbers: NumberCell[] = adjacentCells.filter(isNumberCell).distinct();
      if (adjacentNumbers.length === 2) {
        gears.push([adjacentNumbers[0].value, adjacentNumbers[1].value]);
      }
    }
  }

  return gears.map(([a, b]) => a * b).sum();
};
