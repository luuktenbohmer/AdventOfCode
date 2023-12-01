export const solve = (input: string[]) => {
  const sum = input
    .map((line) => {
      const numbers = line.replace(/\D+/g, "");

      return parseInt((numbers[0] ?? "") + (numbers[numbers.length - 1] ?? ""));
    })
    .sum();

  return sum;
};

const replacements = [
  ["one", "1"],
  ["two", "2"],
  ["three", "3"],
  ["four", "4"],
  ["five", "5"],
  ["six", "6"],
  ["seven", "7"],
  ["eight", "8"],
  ["nine", "9"],
] as const;

export const solve2 = (input: string[]) => {
  const sum = input
    .map((line) => {
      let numbers = "";
      for (let i = 0; i < line.length; i++) {
        if (/\d/.test(line[i] ?? "")) {
          numbers += line[i];
          continue;
        }

        for (const replacement of replacements) {
          if (line.substring(i).startsWith(replacement[0])) {
            numbers += replacement[1];
          }
        }
      }

      const firstLast = (numbers[0] ?? "") + (numbers[numbers.length - 1] ?? "");
      return parseInt(firstLast);
    })
    .sum();

  return sum;
};
