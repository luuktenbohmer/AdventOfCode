const isPossible = (springs: string, groups: Groups): boolean => {
  const foundGroups: number[] = [];
  let currentGroup = 0;
  for (const spring of springs) {
    if (spring === "#") {
      currentGroup++;
    } else {
      if (currentGroup > 0) {
        foundGroups.push(currentGroup);
      }

      currentGroup = 0;
    }
  }

  if (currentGroup > 0) {
    foundGroups.push(currentGroup);
  }

  return (
    foundGroups.length === groups.groups.length && foundGroups.every((group, index) => group === groups.groups[index])
  );
};

const fillSprings = (springs: string, groups: Groups, index: number) => {
  if (springs.split("#").length - 1 === groups.total) {
    return isPossible(springs, groups) ? 1 : 0;
  }

  for (let i = index; i < springs.length; i++) {
    if (springs[i] === "?") {
      const yesSpring = springs.replaceAt(i, "#");
      const noSpring = springs.replaceAt(i, ".");
      return fillSprings(yesSpring, groups, i + 1) + fillSprings(noSpring, groups, i + 1);
    }
  }

  return 0;
};

type Groups = { groups: number[]; total: number };

export const solve = (input: string[]) => {
  return input
    .map((line) => {
      const splitLine = line.split(" ");
      const springs = splitLine[0];
      const groups = splitLine[1].split(",").map(Number);

      return fillSprings(springs, { groups, total: groups.sum() }, 0);
    })
    .sum();
};

export const solve2 = (input: string[]) => {
  return input
    .map((line) => {
      const splitLine = line.split(" ");
      const springs = Array(5).fill(splitLine[0]).join("?");
      const groups = Array(5).fill(splitLine[1]).join(",").split(",").map(Number);

      return 0;
    })
    .sum();
};
