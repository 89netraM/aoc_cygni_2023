#include <stdlib.h>
#include <stdio.h>

typedef struct {
	long x;
	long y;
} Coord;

typedef struct {
	long number;
	Coord a;
	Coord b;
	Coord c;
} Number;

typedef struct {
	char symbol;
	Coord coord;
} Symbol;

int main() {
	FILE* file = fopen("./input.txt", "r");
	long y = 1;
	long x = 1;
	Number numbers[1500];
	int numbersCount = 0;
	Symbol symbols[1500];
	int symbolsCount = 0;
	while (!feof(file)) {
		char ch = fgetc(file);
		if (ch == '\n') {
			y++;
			x = 1;
			continue;
		}
		if ('0' <= ch && ch <= '9') {
			if (numbersCount == 0) {
				numbers[numbersCount].number = ch - '0';
				numbers[numbersCount].a.x = x;
				numbers[numbersCount].a.y = y;
				numbersCount++;
			} else {
				Number* prev = &numbers[numbersCount - 1];
				if (prev->a.y == y && prev->a.x == x - 1) {
					prev->number = prev->number * 10 + (ch - '0');
					prev->b.x = x;
					prev->b.y = y;
				} else if (prev->b.y == y && prev->b.x == x - 1) {
					prev->number = prev->number * 10 + (ch - '0');
					prev->c.x = x;
					prev->c.y = y;
				} else {
					numbers[numbersCount].number = ch - '0';
					numbers[numbersCount].a.x = x;
					numbers[numbersCount].a.y = y;
					numbersCount++;
				}
			}
		} else if (ch != '.' && ch != '\r' && ch != EOF) {
			symbols[symbolsCount].symbol = ch;
			symbols[symbolsCount].coord.x = x;
			symbols[symbolsCount].coord.y = y;
			symbolsCount++;
		}
		x++;
	}

	if (getenv("part")[4] == '1') {
		long sum = 0;
		for (int nI = 0; nI < numbersCount; nI++) {
			Number n = numbers[nI];
			for (int sI = 0; sI < symbolsCount; sI++) {
				Coord s = symbols[sI].coord;
				if ((s.x == n.a.x - 1 && s.y == n.a.y - 1) || (s.x == n.a.x - 1 && s.y == n.a.y) ||
					(s.x == n.a.x - 1 && s.y == n.a.y + 1) || (s.x == n.a.x && s.y == n.a.y - 1) ||
					(s.x == n.a.x && s.y == n.a.y + 1) || (s.x == n.a.x + 1 && s.y == n.a.y - 1) ||
					(s.x == n.a.x + 1 && s.y == n.a.y + 1) || ((n.b.x == 0 && n.b.y == 0) && (s.x == n.a.x + 1 && s.y == n.a.y))) {
					sum += n.number;
					goto numbersEnd;
				}
				if (!(n.b.x == 0 && n.b.y == 0)) {
					if ((s.x == n.b.x + 1 && s.y == n.b.y - 1) || (s.x == n.b.x + 1 && s.y == n.b.y + 1) ||
						((n.c.x == 0 && n.c.y == 0) && (s.x == n.b.x + 1 && s.y == n.b.y))) {
						sum += n.number;
						goto numbersEnd;
					}
					if (!(n.c.x == 0 && n.c.y == 0)) {
						if ((s.x == n.c.x + 1 && s.y == n.c.y - 1) || (s.x == n.c.x + 1 && s.y == n.c.y + 1) ||
							(s.x == n.c.x + 1 && s.y == n.c.y)) {
							sum += n.number;
							goto numbersEnd;
						}

					}
				}
			}
			numbersEnd:
		}
		printf("%ld\n", sum);
	} else {
		long sum = 0;
		for (int sI = 0; sI < symbolsCount; sI++) {
			Symbol s = symbols[sI];
			if (s.symbol != '*') continue;
			long product = 1;
			int count = 0;
			for (int nI = 0; nI < numbersCount; nI++) {
				Coord c = s.coord;
				Number n = numbers[nI];
				if ((c.x == n.a.x - 1 && c.y == n.a.y - 1) || (c.x == n.a.x - 1 && c.y == n.a.y) ||
					(c.x == n.a.x - 1 && c.y == n.a.y + 1) || (c.x == n.a.x && c.y == n.a.y - 1) ||
					(c.x == n.a.x && c.y == n.a.y + 1) || (c.x == n.a.x + 1 && c.y == n.a.y - 1) ||
					(c.x == n.a.x + 1 && c.y == n.a.y + 1) || ((n.b.x == 0 && n.b.y == 0) && (c.x == n.a.x + 1 && c.y == n.a.y))) {
					product *= n.number;
					count++;
				}
				if (!(n.b.x == 0 && n.b.y == 0)) {
					if ((c.x == n.b.x + 1 && c.y == n.b.y - 1) || (c.x == n.b.x + 1 && c.y == n.b.y + 1) ||
						((n.c.x == 0 && n.c.y == 0) && (c.x == n.b.x + 1 && c.y == n.b.y))) {
						product *= n.number;
						count++;
					}
					if (!(n.c.x == 0 && n.c.y == 0)) {
						if ((c.x == n.c.x + 1 && c.y == n.c.y - 1) || (c.x == n.c.x + 1 && c.y == n.c.y + 1) ||
							(c.x == n.c.x + 1 && c.y == n.c.y)) {
							product *= n.number;
							count++;
						}

					}
				}
			}
			if (count == 2) {
				sum += product;
			}
		}
		printf("%ld\n", sum);
	}
}
