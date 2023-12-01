import fs from "fs";

export const getLastDay = (): string => {
  const files = fs.readdirSync("./src/days");

  return files[files.length - 1] ?? "";
};

export const readLines = (path: string) => {
  return fs.existsSync(path) ? fs.readFileSync(path).toString().replace(/\r\n/g, "\n").split("\n") : [];
};
