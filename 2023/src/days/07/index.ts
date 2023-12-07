import { KeyValue } from "../../extensions";

const types = [
  (cards: KeyValue<string>[]) => cards.some((c) => c.value.length === 5),
  (cards: KeyValue<string>[]) => cards.some((c) => c.value.length === 4),
  (cards: KeyValue<string>[]) =>
    cards.length === 2 &&
    ((cards[0].value.length === 2 && cards[1].value.length === 3) ||
      (cards[0].value.length === 3 && cards[1].value.length === 2)),
  (cards: KeyValue<string>[]) => cards.some((c) => c.value.length === 3),
  (cards: KeyValue<string>[]) => cards.filter((c) => c.value.length === 2).length === 2,
  (cards: KeyValue<string>[]) => cards.some((c) => c.value.length === 2),
  () => true,
];

const getType = (cards: string) => {
  for (let i = 0; i < types.length; i++) {
    if (types[i](cards.split("").groupBy((c) => c))) return i;
  }
};

const cardOrder = "23456789TJQKA";
const cardOrder2 = "J23456789TQKA";

export const solve = (input: string[]) => {
  return input
    .map((line) => {
      const split = line.split(" ");
      return {
        cards: split[0],
        bid: parseInt(split[1]),
      };
    })
    .sort((a, b) => {
      const typeDifference = getType(b.cards) - getType(a.cards);
      if (typeDifference) {
        return typeDifference;
      }

      for (let i = 0; i < a.cards.length; i++) {
        const cardDifference = cardOrder.indexOf(a.cards[i]) - cardOrder.indexOf(b.cards[i]);
        if (cardDifference) {
          return cardDifference;
        }
      }

      return 0;
    })
    .map((cards, i) => cards.bid * (i + 1))
    .sum();
};

export const solve2 = (input: string[]) => {
  return input
    .map((line) => {
      const [cards, bid] = line.split(" ");
      const notJ = cards.split("").filter((c) => c !== "J");
      const cardsBest =
        notJ.length === 0 ? cards : cards.replaceAll("J", notJ.groupBy((c) => c).maxBy((c) => c.value.length).key);

      return {
        cards,
        cardsBest,
        bid: parseInt(bid),
      };
    })
    .sort((a, b) => {
      const typeDifference = getType(b.cardsBest) - getType(a.cardsBest);
      if (typeDifference) {
        return typeDifference;
      }

      for (let i = 0; i < a.cards.length; i++) {
        const cardDifference = cardOrder2.indexOf(a.cards[i]) - cardOrder2.indexOf(b.cards[i]);
        if (cardDifference) {
          return cardDifference;
        }
      }

      return 0;
    })
    .map((cards, i) => cards.bid * (i + 1))
    .sum();
};
