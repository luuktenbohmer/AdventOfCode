type Race = {
  time: number;
  distance: number;
};

const calculateDistance = (time: number, holdTime: number) => (time - holdTime) * holdTime;

export const solve = (input: string[]) => {
  const times = input[0].split(" ").removeFalsy().slice(1).map(Number);
  const distances = input[1].split(" ").removeFalsy().slice(1).map(Number);

  const races: Race[] = [];
  for (let i = 0; i < times.length; i++) {
    races.push({
      time: times[i],
      distance: distances[i],
    });
  }

  return races
    .map((r) => {
      let winCount = 0;
      for (let j = 1; j < r.time; j++) {
        if (calculateDistance(r.time, j) > r.distance) {
          winCount++;
        }
      }

      return winCount;
    })
    .reduce((a, b) => a * b, 1);
};

export const solve2 = (input: string[]) => solve(input);
