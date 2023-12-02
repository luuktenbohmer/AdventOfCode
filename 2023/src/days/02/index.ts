const getMaxGames = (input: string[]) => {
  return input.map((line) => {
    const gameMatch = line.match(/Game (\d*): (.*)/);
    const number = parseInt(gameMatch[1]);
    const rounds = gameMatch[2].split(";");

    const cubeRounds = rounds.map((round) => {
      const cubes = round.split(",").map((cube) => {
        const cubeMatch = cube.trim().match(/(\d+) (.*)/);
        return {
          color: cubeMatch[2],
          count: parseInt(cubeMatch[1]),
        };
      });

      return {
        red: cubes.find((cube) => cube.color === "red")?.count,
        blue: cubes.find((cube) => cube.color === "blue")?.count,
        green: cubes.find((cube) => cube.color === "green")?.count,
      };
    });

    return {
      number,
      red: cubeRounds.map((round) => round.red).max(),
      blue: cubeRounds.map((round) => round.blue).max(),
      green: cubeRounds.map((round) => round.green).max(),
    };
  });
};

export const solve = (input: string[]) => {
  return getMaxGames(input)
    .map(({ number, red, blue, green }) => {
      if (red <= 12 && blue <= 14 && green <= 13) {
        return number;
      } else {
        return 0;
      }
    })
    .sum();
};

export const solve2 = (input: string[]) => {
  return getMaxGames(input)
    .map(({ red, blue, green }) => red * blue * green)
    .sum();
};
