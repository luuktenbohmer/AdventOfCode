export const isNumber = (s: string) => {
  if (!s) {
    return false;
  }

  return s >= "0" && s <= "9";
};
