const getGrids = (input: string[]) => {
  let currentGrid: string[] = [];
  const grids: string[][] = [currentGrid];
  for (const line of input) {
    if (line === "") {
      currentGrid = [];
      grids.push(currentGrid);
      continue;
    }

    currentGrid.push(line);
  }

  return grids;
};

const getMirrorPosition = (grid: string[], errors: number) => {
  for (let i = 1; i < grid.length; i++) {
    let top = i - 1;
    let bottom = i;
    let foundErrors = 0;
    while (top >= 0 && bottom < grid.length) {
      for (let j = 0; j < grid[top].length; j++) {
        if (grid[top][j] !== grid[bottom][j]) {
          foundErrors++;
        }
      }

      top--;
      bottom++;
    }

    if (foundErrors === errors) return i;
  }

  return 0;
};

export const solve = (input: string[]) => {
  return getGrids(input)
    .map((grid) => {
      // Check for vertical mirror.
      const h = getMirrorPosition(grid, 0);
      const v = getMirrorPosition(grid.transpose(), 0);

      return h * 100 + v;
    })
    .sum();
};

export const solve2 = (input: string[]) => {
  return getGrids(input)
    .map((grid) => {
      // Check for vertical mirror.
      const h = getMirrorPosition(grid, 1);
      const v = getMirrorPosition(grid.transpose(), 1);

      return h * 100 + v;
    })
    .sum();
};
