declare global {
  interface Array<T> {
    sum(): number;
    sumBy(selector: (item: T) => number): number;
    minBy(selector: (item: T) => number): number;
    maxBy(selector: (item: T) => number): number;
    sortBy(selector: (item: T) => number, direction?: "asc" | "desc"): T[];
    groupBy(selector: (item: T) => string): { key: string; value: T[] }[];
    selectMany<K>(selector: (item: T) => K[]): K[];
  }
}

Array.prototype.sum = function (this: number[]) {
  return this.reduce((a, b) => a + b, 0);
};
Array.prototype.sumBy = function <T>(this: T[], selector: (item: T) => number) {
  return this.map(selector).sum();
};

Array.prototype.minBy = function <T>(this: T[], selector: (item: T) => number) {
  return Math.min(...this.map(selector));
};
Array.prototype.maxBy = function <T>(this: T[], selector: (item: T) => number) {
  return Math.max(...this.map(selector));
};

Array.prototype.sortBy = function <T>(this: T[], selector: (item: T) => number, direction: "asc" | "desc" = "asc") {
  return direction === "desc"
    ? this.sort((a, b) => selector(b) - selector(a))
    : this.sort((a, b) => selector(a) - selector(b));
};

Array.prototype.groupBy = function <T, K extends string | number>(this: T[], selector: (item: T) => K) {
  return this.reduce((groups, item) => {
    const group = selector(item);

    let existingGroup = groups.find((g) => g.key === group);
    if (!existingGroup) {
      existingGroup = {
        key: group,
        value: [],
      };
      groups.push(existingGroup);
    }

    existingGroup.value.push(item);

    return groups;
  }, [] as { key: K; value: T[] }[]);
};

Array.prototype.selectMany = function <T, K>(this: T[], selector: (item: T) => K[]) {
  return this.reduce((result, item) => {
    result.push(...selector(item));
    return result;
  }, [] as K[]);
};

Array.prototype.groupBy = function <T>(this: T[], selector: (item: T) => string) {
  const grouped = this.reduce((groups, item) => {
    const group = selector(item);
    groups[group] ??= [];
    groups[group]!.push(item);
    return groups;
  }, {} as Record<string | number, T[]>);

  return Object.keys(grouped).map((key) => ({
    key,
    value: grouped[key] as T[],
  }));
};
