
#ifndef IndefiniteSets_FLOAT
// Тип данных для хранения значений с плавающей запятой.
#define IndefiniteSets_FLOAT double
#endif // !IndefiniteSets_FLOAT

#ifndef IndefiniteSets_CHAR
// Тип данных для хранения символов.
#define IndefiniteSets_CHAR char
#endif // !IndefiniteSets_CHAR

// Массив символов. Нечто похожее на строку.
struct IndefiniteSets_CharArray {
	// Количество допустимых символов.
	size_t count;
	// Указатель на первый символ.
	IndefiniteSets_CHAR * string;
};

// Отрезок ОТ и ДО включительно.
struct IndefiniteSets_Range {
	// Минимальное допустимое значение.
	IndefiniteSets_FLOAT min;
	// Максимальное допустимое значение.
	IndefiniteSets_FLOAT max;
};

/*
Представление термы нечётких множеств.
Это некоторое соответсвие:
Большой это от 180 до 300
*/
struct IndefiniteSets_Terma {
	// Имя термы.
	struct IndefiniteSets_CharArray name;
	// Разброс, которому соответсвует терма.
	struct IndefiniteSets_Range range;
};

// Массив терм.
struct IndefiniteSets_TermaArray {
	// Границы массива терм.
	struct IndefiniteSets_Range globalRange;
	// Количество терм.
	size_t count;
	// Указатель на первую терму.
	struct IndefiniteSets_Terma * terma;
};

/*
Правило, которое делает вывод:
если first, то second.
*/
struct IndefiniteSets_Rule {
	// Основание гипотезы.
	struct IndefiniteSets_Terma * first;
	// Гипотеза.
	struct IndefiniteSets_Terma * second;
};

// Массив правил нечётких множеств.
struct IndefiniteSets_RuleArray {
	// Количество нечётких множеств.
	size_t count;
	// Указатель на первое правило между двумя нечёткими множествами.
	struct IndefiniteSets_Rule * rule;
};

// Характеристики некоторого одного нечётного множества
struct IndefiniteSets_Characteristic {
	// Название нечёткого множества.
	struct IndefiniteSets_CharArray name;
	// Термы нечёткого множества.
	struct IndefiniteSets_TermaArray terms;
};

// Массив нечётких множеств
struct IndefiniteSets_CharacteristicArray {
	// Количество нечётких множеств.
	size_t count;
	// Указатель на первую характеристику нечётного множества.
	struct IndefiniteSets_Characteristic * characteristic;
};

// Состояние системы нечётких множеств.
struct IndefiniteSets_State
{
	// Массив нечётких множеств.
	struct IndefiniteSets_CharacteristicArray characteristics;
	// Правила нечётких множеств.
	struct IndefiniteSets_RuleArray rules;
};
