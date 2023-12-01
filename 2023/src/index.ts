import "./extensions";
import { getLastDay, readLines } from "./utils";

let [day = getLastDay()] = process.argv.slice(2);
day = day.padStart(2, "0");

const example = readLines(`./src/days/${day}/input/example.txt`);
const example2 = readLines(`./src/days/${day}/input/example2.txt`);
const input = readLines(`./src/days/${day}/input/input.txt`);
const input2 = readLines(`./src/days/${day}/input/input2.txt`);

const solution = require(`./days/${day}`);
const resultExample1 = solution.solve?.(example);
const result1 = solution.solve?.(input);

console.log("Part 1");
console.log("Example:", resultExample1);
console.log("Result:", result1);

console.log();

const resultExample2 = solution.solve2?.(example2.length > 0 ? example2 : example);
const result2 = solution.solve2?.(input2.length > 0 ? input2 : input);
if (resultExample2 !== undefined && result2 !== undefined) {
  console.log("Part 2");
  console.log("Example:", resultExample2);
  console.log("Result:", result2);
}
