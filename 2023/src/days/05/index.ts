type Mapping = {
  source: number;
  destination: number;
  range: number;
};

type MappingCategory = {
  from: string;
  to: string;

  mappings: Mapping[];
};

const parseMappings = (input: string[]) => {
  const categories: MappingCategory[] = [];
  let currentCategory: MappingCategory;

  for (const line of input.slice(2)) {
    if (!line) continue;

    const titleMatch = line.match(/(.*)-(?:.*)-(.*) map/);
    if (titleMatch) {
      const [, from, to] = titleMatch;
      currentCategory = { from, to, mappings: [] };
      categories.push(currentCategory);
      continue;
    }

    const mappingMatch = line.match(/(\d+) (\d+) (\d+)/);
    if (mappingMatch) {
      const [, destination, source, range] = mappingMatch;
      currentCategory.mappings.push({
        source: parseInt(source),
        destination: parseInt(destination),
        range: parseInt(range),
      });
    }
  }

  return categories;
};

const getLocation = (categories: MappingCategory[], seed: number) =>
  categories.reduce((acc, category) => {
    const mapping = category.mappings.find((m) => m.source <= acc && m.source + m.range > acc);
    if (mapping) return mapping.destination + (acc - mapping.source);
    return acc;
  }, seed);

const getMinLocation = (categories: MappingCategory[], seeds: number[]) => {
  return seeds.map((s) => getLocation(categories, s)).min();
};

export const solve = (input: string[]) => {
  const seeds = input[0]
    .match(/seeds: (.+)/)[1]
    .split(" ")
    .map(Number);

  const categories = parseMappings(input);

  return getMinLocation(categories, seeds);
};

// This is an extremely inefficient solution, but letting this
// run for a few minutes is faster than coming up with a good solution..
export const solve2 = (input: string[]) => {
  const seedRanges = input[0]
    .match(/seeds: (.+)/)[1]
    .split(" ")
    .map(Number);

  const categories = parseMappings(input);
  let min = 1_000_000_000;
  for (let i = 0; i < seedRanges.length; i += 2) {
    console.log("Range: ", seedRanges[i + 1]);
    for (let j = 0; j < seedRanges[i + 1]; j++) {
      const location = categories.reduce((acc, category) => {
        const mapping = category.mappings.find((m) => m.source <= acc && m.source + m.range > acc);
        if (mapping) return mapping.destination + (acc - mapping.source);
        return acc;
      }, seedRanges[i] + j);

      if (j % 1_000_000 === 0) {
        console.log(j);
      }

      if (location < min) {
        min = location;
      }
    }
  }

  return min;
};
