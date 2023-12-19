const countTotalLoad = (input: string[]) => {
  return [...input]
    .reverse()
    .map((line, i) => {
      return (line.split("O").length - 1) * (i + 1);
    })
    .sum();
};

const rollRocks = (input: string[]) => {
  input = [...input];
  for (let y = 0; y < input.length; y++) {
    const line = input[y];
    for (let x = 0; x < input[y].length; x++) {
      const char = line[x];
      if (char === "O") {
        let currentY = y;
        while (currentY > 0) {
          const charAbove = input[currentY - 1][x];
          if (charAbove !== ".") {
            break;
          }
          input[currentY] = input[currentY].replaceAt(x, ".");
          input[currentY - 1] = input[currentY - 1].replaceAt(x, "O");
          currentY--;
        }
      }
    }
  }

  return input;
};

const getAllIndexes = (numbers: number[], number: number) => {
  const indexes: number[] = [];
  for (let i = numbers.length - 1; i >= 0; i--) {
    if (numbers[i] === number) {
      indexes.push(i);
    }
  }

  return indexes;
};

export const solve = (input: string[]) => {
  input = rollRocks(input);
  return countTotalLoad(input);
};

export const solve2 = (input: string[]) => {
  let loads: number[] = [];
  let result = 0;
  for (let i = 0; i < 1_000_000_000; i++) {
    for (let d = 0; d < 4; d++) {
      input = rollRocks(input).rotate();
    }

    const totalLoad = countTotalLoad(input);

    if (loads.length >= 100 && loads.some((l) => l === totalLoad)) {
      const indexes = getAllIndexes(loads, totalLoad);
      for (const index of indexes) {
        let match = true;
        for (let x = 1; x <= i - index; x++) {
          if (loads[index - x] !== loads[i - x]) {
            match = false;
            break;
          }
        }

        if (match) {
          const sequenceLength = i - index;
          const untilEnd = 1_000_000_000 - i - 1;
          const end = untilEnd % sequenceLength;
          result = loads[index + end];

          break;
        }
      }
    }
    if (result) {
      break;
    }
    loads.push(totalLoad);
  }

  return result;
};
