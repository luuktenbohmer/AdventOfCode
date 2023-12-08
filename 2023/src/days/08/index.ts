import { lcm } from "../../utils";

type Element = {
  node: string;
  leftString: string;
  rightString: string;
  left?: Element;
  right?: Element;
  stepsToZ?: number;
};

const getElements = (input: string[]) => {
  const elements = new Map<string, Element>();
  input.slice(2).map((line) => {
    const [, node, leftString, rightString] = line.match(/(.*) = \((.*), (.*)\)/);
    elements.set(node, { node, leftString, rightString });
  });

  for (const element of elements.values()) {
    element.left = elements.get(element.leftString);
    element.right = elements.get(element.rightString);
  }

  return elements;
};

export const solve = (input: string[]) => {
  const directions = input[0];
  const elements = getElements(input);

  let current: Element = elements.get("AAA");
  let i = 0;
  while (true) {
    const direction = directions[i % directions.length];

    if (direction === "L") {
      current = current.left;
    } else {
      current = current.right;
    }

    i++;

    if (current.node === "ZZZ") {
      return i;
    }
  }
};

export const solve2 = (input: string[]) => {
  const directions = input[0];
  const elements = getElements(input);

  const stepsToZ = [...elements.values()]
    .filter((e) => e.node.endsWith("A"))
    .map((c) => {
      let current = c;
      let i = 0;
      while (true) {
        const direction = directions[i % directions.length];

        if (direction === "L") {
          current = current.left;
        } else {
          current = current.right;
        }

        i++;

        if (current.node.endsWith("Z")) {
          return i;
        }
      }
    });

  return stepsToZ.reduce(lcm);
};
