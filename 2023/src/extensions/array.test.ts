import "./array";

describe("Array", () => {
  describe("sum", () => {
    it("returns the sum of the array", () => {
      expect([1, 2, 3].sum()).toEqual(6);
    });

    it("returns the sum of the array with a selector", () => {
      expect(["1", "2", "3"].sum((x) => parseInt(x))).toEqual(6);
    });
  });

  describe("min", () => {
    it("returns the min of the array", () => {
      expect([6, 1, 3].min()).toEqual(1);
    });

    it("returns the min of the array with a selector", () => {
      expect(["5", "1", "3"].min((x) => parseInt(x))).toEqual(1);
    });

    it("throws an error if the array is empty", () => {
      expect(() => [].min()).toThrowError("Cannot get min of empty array");
    });
  });

  describe("minBy", () => {
    it("returns the min of the array by the selector", () => {
      const i1 = { value: 1 };
      const i2 = { value: 2 };
      const i3 = { value: 3 };

      const input = [i1, i2, i3];
      expect(input.minBy((x) => x.value)).toEqual(i1);
    });

    it("throws an error if the array is empty", () => {
      expect(() => [].minBy((x) => x)).toThrowError("Cannot get min of empty array");
    });
  });

  describe("max", () => {
    it("returns the max of the array", () => {
      expect([6, 1, 3].max()).toEqual(6);
    });

    it("returns the max of the array with a selector", () => {
      expect(["5", "1", "3"].max((x) => parseInt(x))).toEqual(5);
    });

    it("throws an error if the array is empty", () => {
      expect(() => [].max()).toThrowError("Cannot get max of empty array");
    });
  });

  describe("maxBy", () => {
    it("returns the max of the array by the selector", () => {
      const i1 = { value: 1 };
      const i2 = { value: 2 };
      const i3 = { value: 3 };

      const input = [i1, i2, i3];
      expect(input.maxBy((x) => x.value)).toEqual(i3);
    });

    it("throws an error if the array is empty", () => {
      expect(() => [].maxBy((x) => x)).toThrowError("Cannot get max of empty array");
    });
  });

  describe("sortBy", () => {
    it("sorts the array by the selector", () => {
      expect(["1", "2", "3"].sortBy((x) => parseInt(x))).toEqual(["1", "2", "3"]);
      expect(["3", "2", "1"].sortBy((x) => parseInt(x))).toEqual(["1", "2", "3"]);
    });

    it("sorts the array by the selector in descending order", () => {
      expect(["1", "2", "3"].sortBy((x) => parseInt(x), "desc")).toEqual(["3", "2", "1"]);
      expect(["3", "2", "1"].sortBy((x) => parseInt(x), "desc")).toEqual(["3", "2", "1"]);
    });

    it("sorts the array by the selector when the selector returns a string", () => {
      expect(["1", "2", "3"].sortBy((x) => x)).toEqual(["1", "2", "3"]);
      expect(["3", "2", "1"].sortBy((x) => x)).toEqual(["1", "2", "3"]);
    });

    it("sorts the array by the selector in descending order when the selector returns a string", () => {
      expect(["1", "2", "3"].sortBy((x) => x, "desc")).toEqual(["3", "2", "1"]);
      expect(["3", "2", "1"].sortBy((x) => x, "desc")).toEqual(["3", "2", "1"]);
    });

    it("returns an empty array if the input array is empty", () => {
      expect([].sortBy((x) => x)).toEqual([]);
    });
  });

  describe("distinct", () => {
    it("returns the distinct values of the array", () => {
      expect([1, 2, 3, 1, 2, 3].distinct()).toEqual([1, 2, 3]);
    });
  });

  describe("distinctBy", () => {
    it("returns the distinct values of the array by the selector", () => {
      const i1 = { value: 1 };
      const i2 = { value: 2 };
      const i3 = { value: 3 };

      const input = [i1, i2, i3, i1, i2, i3];
      expect(input.distinctBy((x) => x.value)).toEqual([i1, i2, i3]);
    });
  });

  describe("selectMany", () => {
    it("returns the flattened array", () => {
      expect(
        [
          [1, 2],
          [3, 4],
        ].selectMany((x) => x)
      ).toEqual([1, 2, 3, 4]);
    });
  });

  describe("groupBy", () => {
    it("groups the array by the selector", () => {
      const i1 = { group: "a", value: 1 };
      const i2 = { group: "b", value: 2 };
      const i3 = { group: "a", value: 3 };

      const input = [i1, i2, i3];
      const expected = [
        { key: "a", value: [i1, i3] },
        { key: "b", value: [i2] },
      ];
      expect(input.groupBy((x) => x.group)).toEqual(expected);
    });
  });

  describe("intersect", () => {
    it("returns the intersection of the arrays", () => {
      expect([1, 2, 3].intersect([2, 3, 4])).toEqual([2, 3]);
    });
  });

  describe("intersectBy", () => {
    it("returns the intersection of the arrays by the selector", () => {
      const i1 = { value: 1 };
      const i2 = { value: 2 };
      const i3 = { value: 3 };

      const input = [i1, i2, i3];
      expect(input.intersectBy([i2, i3, { value: 4 }], (x) => x.value)).toEqual([i2, i3]);
    });
  });

  describe("except", () => {
    it("returns the difference of the arrays", () => {
      expect([1, 2, 3].except([2, 3, 4])).toEqual([1]);
    });
  });

  describe("exceptBy", () => {
    it("returns the difference of the arrays by the selector", () => {
      const i1 = { value: 1 };
      const i2 = { value: 2 };
      const i3 = { value: 3 };

      const input = [i1, i2, i3];
      expect(input.exceptBy([i2, i3, { value: 4 }], (x) => x.value)).toEqual([i1]);
    });
  });

  describe("transpose", () => {
    it("transposes the array", () => {
      expect(["123", "456"].transpose()).toEqual(["14", "25", "36"]);
    });
  });
});
