declare global {
  interface Array<T> {
    sum(this: number[]): number;
    sum(selector: (item: T) => number): number;

    min(this: number[]): number;
    min(selector: (item: T) => number): number;
    minBy(selector: (item: T) => number): T;

    max(): number;
    max(selector: (item: T) => number): number;
    maxBy(selector: (item: T) => number): T;

    sortBy<K extends number | string>(selector: (item: T) => K, direction?: "asc" | "desc"): T[];
    groupBy(selector: (item: T) => string): KeyValue<T>[];
    selectMany<K>(selector: (item: T) => K[]): K[];

    distinct(): T[];
    distinctBy(selector: (item: T) => unknown): T[];

    intersect(other: T[]): T[];
    intersectBy(other: T[], selector: (item: T) => unknown): T[];

    except(other: T[]): T[];
    exceptBy(other: T[], selector: (item: T) => unknown): T[];

    trimAll(this: string[]): string[];
    removeFalsy(this: (T | null | undefined)[]): T[];

    transpose(this: string[]): string[];
    transpose(this: T[][]): T[][];

    rotate(this: string[]): string[];
  }
}

export type KeyValue<T> = { key: string; value: T[] };

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

Array.prototype.minBy = function <T>(this: T[], selector: (item: T) => number) {
  if (this.length === 0) {
    throw new Error("Cannot get min of empty array");
  }

  const array = this.map(selector);
  const min = Math.min(...array.filter((x) => x != null));
  return this[array.indexOf(min)];
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

Array.prototype.maxBy = function <T>(this: T[], selector: (item: T) => number) {
  if (this.length === 0) {
    throw new Error("Cannot get max of empty array");
  }

  const array = this.map(selector);
  const max = Math.max(...array.filter((x) => x != null));
  return this[array.indexOf(max)];
};

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

Array.prototype.distinctBy = function <T>(this: T[], selector: (item: T) => string) {
  const set = new Set();
  return this.filter((item) => {
    const key = selector(item);
    if (set.has(key)) {
      return false;
    }

    set.add(key);
    return true;
  });
};

Array.prototype.intersect = function <T>(this: T[], other: T[]) {
  return this.filter((x) => other.includes(x));
};

Array.prototype.intersectBy = function <T>(this: T[], other: T[], selector: (item: T) => string) {
  const otherSet = new Set(other.map(selector));
  return this.filter((item) => otherSet.has(selector(item)));
};

Array.prototype.except = function <T>(this: T[], other: T[]) {
  return this.filter((x) => !other.includes(x));
};

Array.prototype.exceptBy = function <T>(this: T[], other: T[], selector: (item: T) => string) {
  const otherSet = new Set(other.map(selector));
  return this.filter((item) => !otherSet.has(selector(item)));
};

Array.prototype.trimAll = function (this: string[]) {
  return this.map((x) => x.trim());
};

Array.prototype.removeFalsy = function <T>(this: (T | null | undefined)[]) {
  return this.filter((x) => x);
};

function isStringArray<T>(array: string[] | T[][]): array is string[] {
  return typeof array[0] === "string";
}

// @ts-ignore
Array.prototype.transpose = function <T>(this: string[] | T[][]) {
  if (this.length === 0) return [];

  if (isStringArray(this)) {
    return this[0].split("").map((_, colIndex) => this.map((row) => row[colIndex]).join(""));
  } else {
    return this[0].map((_, colIndex) => this.map((row) => row[colIndex]));
  }
};

Array.prototype.rotate = function (this: string[]) {
  if (this.length === 0) return [];
  return this[0].split("").map((val, index) =>
    this.map((row) => row[index])
      .reverse()
      .join("")
  );
};
