const getSequences = (input: string[]) => {
  return input.map((line) => {
    const numbers = line.split(" ").map(Number);

    const sequences: number[][] = [numbers];
    while (sequences.at(-1).some((n) => n !== 0)) {
      const previousSequence = sequences.at(-1);

      const sequence = [];
      sequences.push(sequence);

      for (let i = 0; i < previousSequence.length - 1; i++) {
        sequence.push(previousSequence[i + 1] - previousSequence[i]);
      }
    }

    return sequences;
  });
};

export const solve = (input: string[]) => {
  return getSequences(input)
    .map((sequences) => sequences.map((s) => s.at(-1)).sum())
    .sum();
};

export const solve2 = (input: string[]) => {
  return getSequences(input)
    .map((sequences) => sequences.reverse().reduce((a, b) => b[0] - a, 0))
    .sum();
};
