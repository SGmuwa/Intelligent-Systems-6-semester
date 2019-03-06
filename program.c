#include <locale.h>
#include <stdio.h>
#include <limits.h>
#include "UserInterface.h"

/*
Вызов основной программы
int argc - количество аргументов к программе.
char * argv[] - указатель на массив аргументов к программе.
Возвращает: код ошибки.
*/
int main(int argc, char * argv[]) {
	setlocale(LC_ALL, "rus");
	size_t countSets = UserInterface_GetUnsignedLongLongIntLimit("Количество множетсв: ", 0u, SIZE_MAX - 1);
	float * sets = (float*)malloc(countSets * sizeof(float));
	char buffer[256];
	for (size_t i = 0; i < countSets; i++) {
#ifdef _MSC_VER
		sprintf_s(buffer, sizeof(buffer),
#else
		sprintf(buffer, 
#endif
			"Множество %z =", i);
		sets[i] = UserInterface_GetFloatLimit(buffer, 0.0f, 1.0f);
	}
	float result = 0.0;
	switch (UserInterface_GetChek("0 - пересечение\n1 - объединение", 1))
	{
	case 1:
		for (size_t i = 0; i < countSets; i++)
			if (sets[i] > result)
				result = sets[i];
		break;
	default:
		break;
	}
}
