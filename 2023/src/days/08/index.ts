type Element = {
  node: string;
  leftString: string;
  rightString: string;
  left?: Element;
  right?: Element;
};

export const solve = (input: string[]) => {
  const directions = input[0];

  const elements = new Map<string, Element>();
  input.slice(2).map((line) => {
    const [, node, leftString, rightString] = line.match(/(.*) = \((.*), (.*)\)/);
    elements.set(node, { node, leftString, rightString });
  });

  for (const entry of elements.entries()) {
    entry[1].left = elements.get(entry[1].leftString);
    entry[1].right = elements.get(entry[1].rightString);
  }

  let current = elements.values().next().value;
  let i = 0;
  while (true) {
    const direction = directions[i % directions.length];

    if (direction === "L") {
      current = current.left;
    } else {
      current = current.right;
    }

    if (current.node === "ZZZ") {
      return i + 1;
    }

    i++;
  }
};
