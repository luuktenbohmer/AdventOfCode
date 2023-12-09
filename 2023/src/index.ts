import { program } from "commander";
import "./extensions";
import { getLastDay, readLines } from "./utils";

program.argument("[day]").option("-e, --example", "Only run examples");
program.parse();
const options = program.opts();

let [day = getLastDay()] = program.args;
day = day.padStart(2, "0");

const example = readLines(`./src/days/${day}/input/example.txt`);
const example2 = readLines(`./src/days/${day}/input/example2.txt`);
const input = readLines(`./src/days/${day}/input/input.txt`);
const input2 = readLines(`./src/days/${day}/input/input2.txt`);

const solution = require(`./days/${day}`);
const resultExample1 = solution.solve?.(example);

console.log("Part 1");
console.log("Example:", resultExample1 ?? "TODO");

if (!options.example) {
  const result1 = solution.solve?.(input);
  console.log("Result:", result1 ?? "TODO");
}

console.log();

const resultExample2 = solution.solve2?.(example2.length > 0 ? example2 : example);
console.log("Part 2");
console.log("Example:", resultExample2 ?? "TODO");

if (!options.example) {
  const result2 = solution.solve2?.(input2.length > 0 ? input2 : input);
  console.log("Result:", result2 ?? "TODO");
}
