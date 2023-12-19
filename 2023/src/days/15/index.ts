const hash = (s: string) => {
  let result = 0;

  for (const char of s) {
    result += char.charCodeAt(0);
    result *= 17;
    result %= 256;
  }

  return result;
};

export const solve = (input: string[]) => {
  const strings = input[0].split(",");
  return strings.map(hash).sum();
};

type Lens = {
  label: string;
  focalLength: number;
};

type Box = {
  lenses: Lens[];
};

export const solve2 = (input: string[]) => {
  const steps = input[0].split(",");
  const boxes: Box[] = Array<Box>(256);
  for (let i = 0; i < boxes.length; i++) {
    boxes[i] = { lenses: [] };
  }

  for (const step of steps) {
    if (step.includes("-")) {
      const [label] = step.split("-");
      const labelHash = hash(label);
      const existingLensIndex = boxes[labelHash].lenses.findIndex((l) => l.label === label);
      if (existingLensIndex !== -1) {
        boxes[labelHash].lenses.splice(existingLensIndex, 1);
      }
    } else if (step.includes("=")) {
      const [label, focalLength] = step.split("=");
      const labelHash = hash(label);
      const existingLens = boxes[labelHash].lenses.find((l) => l.label === label);
      if (existingLens) {
        existingLens.focalLength = Number(focalLength);
      } else {
        boxes[labelHash].lenses.push({ label, focalLength: Number(focalLength) });
      }
    }
  }

  return boxes
    .map((b, i) => {
      return b.lenses.map((l, j) => (1 + i) * (j + 1) * l.focalLength).sum();
    })
    .sum();
};
