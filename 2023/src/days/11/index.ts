type Point = {
  x: number;
  y: number;
};
type Galaxy = {
  number: number;
  position: Point;
};

const getGalaxies = (input: string[]) => {
  const galaxies: Galaxy[] = [];
  for (let y = 0; y < input.length; y++) {
    for (let x = 0; x < input[y].length; x++) {
      if (input[y][x] === "#") {
        galaxies.push({ number: galaxies.length + 1, position: { x, y } });
      }
    }
  }

  return galaxies;
};

const expandUniverse = (input: string[], galaxies: Galaxy[], expansion: number) => {
  // Vertical
  for (let i = input.length - 1; i >= 0; i--) {
    if (!input[i].includes("#")) {
      galaxies.filter((g) => g.position.y > i).forEach((g) => (g.position.y += expansion));
    }
  }

  // Horizontal
  for (let i = input[0].length - 1; i >= 0; i--) {
    if (input.every((r) => r[i] !== "#")) {
      galaxies.filter((g) => g.position.x > i).forEach((g) => (g.position.x += expansion));
    }
  }
};

const getSumOfDistances = (galaxies: Galaxy[]) => {
  const getDistance = (p1: Point, p2: Point) => Math.abs(p1.x - p2.x) + Math.abs(p1.y - p2.y);
  const distances: number[] = [];
  for (let i = 0; i < galaxies.length; i++) {
    for (let j = i + 1; j < galaxies.length; j++) {
      const distance = getDistance(galaxies[i].position, galaxies[j].position);
      distances.push(distance);
    }
  }

  return distances.sum();
};

export const solve = (input: string[]) => {
  const galaxies = getGalaxies(input);

  expandUniverse(input, galaxies, 1);

  return getSumOfDistances(galaxies);
};

export const solve2 = (input: string[]) => {
  const galaxies = getGalaxies(input);

  expandUniverse(input, galaxies, 999_999);

  return getSumOfDistances(galaxies);
};
