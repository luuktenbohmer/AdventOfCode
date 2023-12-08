export const gcd = (a, b) => (a ? gcd(b % a, a) : b);
export const lcm = (a, b) => (a * b) / gcd(a, b);
