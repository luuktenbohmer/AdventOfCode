declare global {
  interface Array<T> {
    sum(this: number[]): number;
    sum(selector: (item: T) => number): number;

    min(this: number[]): number;
    min(selector: (item: T) => number): number;

    max(): number;
    max(selector: (item: T) => number): number;

    sortBy<K extends number | string>(selector: (item: T) => K, direction?: "asc" | "desc"): T[];
    groupBy(selector: (item: T) => string): { key: string; value: T[] }[];
    selectMany<K>(selector: (item: T) => K[]): K[];
    distinct(): T[];
  }
}

Array.prototype.sum = function <T>(this: T[] | number[], selector?: (item: any) => number) {
  let array = this as number[];
  if (selector) {
    array = this.map(selector);
  }

  return array.reduce((a, b) => a + b, 0);
};

Array.prototype.min = function <T>(this: T[], selector?: (item: T) => number) {
  if (this.length === 0) {
    throw new Error("Cannot get min of empty array");
  }

  let array = this as number[];
  if (selector) {
    array = this.map(selector);
  }

  return Math.min(...array.filter((x) => x != null));
};

Array.prototype.max = function <T>(this: T[], selector?: (item: T) => number) {
  if (this.length === 0) {
    throw new Error("Cannot get max of empty array");
  }

  let array = this as number[];
  if (selector) {
    array = this.map(selector);
  }

  return Math.max(...array.filter((x) => x != null));
};

function isNumberSelector<T>(array: T[], selector: (item: T) => string | number): selector is (item: T) => number {
  return typeof selector(array[0]) === "number";
}

Array.prototype.sortBy = function <T, K extends string | number>(
  this: T[],
  selector: (item: T) => K,
  direction: "asc" | "desc" = "asc"
) {
  if (!this.length) {
    return [];
  }

  if (isNumberSelector(this, selector)) {
    const numberSelector = selector as (item: T) => number;
    return direction === "desc"
      ? this.sort((a, b) => numberSelector(b) - numberSelector(a))
      : this.sort((a, b) => numberSelector(a) - numberSelector(b));
  }

  const stringSelector = selector as (item: T) => string;
  return direction === "desc"
    ? this.sort((a, b) => stringSelector(b).localeCompare(stringSelector(a)))
    : this.sort((a, b) => stringSelector(a).localeCompare(stringSelector(b)));
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

Array.prototype.distinct = function <T>(this: T[]) {
  return [...new Set(this)];
};
