export const solve = (input: string[]) => {
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

  return input
    .reverse()
    .map((line, i) => {
      return (line.split("O").length - 1) * (i + 1);
    })
    .sum();
};

export const solve2 = (input: string[]) => {};
