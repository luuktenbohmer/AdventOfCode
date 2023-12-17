type PossibleGroup = {
  /** Start index of group */
  startIndex: number;
  /** Total length */
  length: number;
  /** Start count of unused places */
  current: number;
  /** Known spring indexes relative to the group start */
  known: number[];
};

const getPossibleGroups = (records: string): PossibleGroup[] => {
  let currentGroup: PossibleGroup | undefined;
  const possibleGroups = [];
  for (let i = 0; i < records.length; i++) {
    if (records[i] === ".") {
      currentGroup = undefined;
      continue;
    }

    if (!currentGroup) {
      currentGroup = {
        startIndex: i,
        current: 0,
        length: 0,
        known: [],
      };
      possibleGroups.push(currentGroup);
    }

    currentGroup.length++;
    if (records[i] === "#") {
      currentGroup.known.push(i - currentGroup.startIndex);
    }
  }

  return possibleGroups;
};

const internalSolve = (possibleGroups: PossibleGroup[], groups: number[], results: Record<string, number>): number => {
  // return cached result if already calculated.
  const resultKey = `${possibleGroups.length}_${groups.length}_${possibleGroups[0]?.current}`;
  if (results[resultKey] !== undefined) {
    return results[resultKey];
  }

  const group = groups[0];
  let placeCount = 0;
  let skipped = false;
  while (possibleGroups.length > 0) {
    const possibleGroup = possibleGroups[0];
    for (let i = possibleGroup.current; i < possibleGroup.length; i++) {
      if (possibleGroup.known.some((k) => k === i - 1)) {
        skipped = true;
        break;
      }
      // Not enough space left in group.
      if (possibleGroup.length - i < group) {
        break;
      }

      // If the next place is a known spring, then continue.
      if (possibleGroup.known.find((k) => k === i + group)) {
        continue;
      }

      // Place found.
      if (groups.length === 1) {
        // The last group was placed.

        // Continue if known spring is missed.
        if (
          possibleGroups.slice(1).some((g) => g.known.length > 0) ||
          possibleGroup.known.some((k) => k >= i + group)
        ) {
          continue;
        }

        // All groups were placed correctly.
        placeCount++;
      } else {
        // Place next group(s) recursively.
        placeCount += internalSolve(
          [{ ...possibleGroup, current: i + group + 1 }, ...possibleGroups.slice(1)],
          groups.slice(1),
          results
        );
      }
    }

    // A known spring couldn't be matched in this branch.
    if (possibleGroup.known.some((known) => known >= possibleGroup.current) || skipped) {
      break;
    }

    // Remove group if no place was left.
    possibleGroups = possibleGroups.slice(1);
  }

  // Cache result.
  results[resultKey] = placeCount;

  return placeCount;
};

export const solve = (input: string[]) => {
  return input
    .map((line) => {
      const splitLine = line.split(" ");
      const records = splitLine[0];
      const groups = splitLine[1].split(",").map(Number);

      const possibleGroups = getPossibleGroups(records);
      return internalSolve(possibleGroups, groups, {});
    })
    .sum();
};

export const solve2 = (input: string[]) => {
  return input
    .map((line) => {
      const splitLine = line.split(" ");
      const records = Array(5).fill(splitLine[0]).join("?");
      const groups = Array(5).fill(splitLine[1]).join(",").split(",").map(Number);

      const possibleGroups = getPossibleGroups(records);
      return internalSolve(possibleGroups, groups, {});
    })
    .sum();
};
