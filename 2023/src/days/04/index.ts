const getAmountWinning = (line: string) => {
  const card = line.split(":")[1];
  const splitCard = card.split("|").map((x) => x.trim());

  const splitNumbers = (numbers: string) =>
    numbers
      .split(" ")
      .filter((x) => x)
      .map((x) => parseInt(x));
  const numbers = splitNumbers(splitCard[0]);
  const winningNumbers = splitNumbers(splitCard[1]);

  return numbers.intersect(winningNumbers).length;
};

export const solve = (input: string[]) => {
  return input
    .map((line) => {
      const amountWinning = getAmountWinning(line);

      if (amountWinning === 0) {
        return 0;
      }

      return Math.pow(2, amountWinning - 1);
    })
    .sum();
};

export const solve2 = (input: string[]) => {
  const cards = Array(input.length).fill(1);

  for (let i = 0; i < input.length; i++) {
    const amountWinning = getAmountWinning(input[i]);
    for (let j = 0; j < amountWinning; j++) {
      cards[i + j + 1] += cards[i];
    }
  }

  return cards.sum();
};
