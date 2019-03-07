
// Структура чтобы указать максимальное и минимальное значение.
struct IndefiniteSets_Range {
	// Минимальное значение.
	double min;
	// Максимальное значение.
	double max;
};

/*
Структура представляет из себя массив символов.
*/
struct IndefiniteSets_CharArray {
	// Количество символов.
	size_t size;
	// Указатель на начало строки.
	char * array;
};

// Функция принадлежности (membership function)
struct IndefiniteSets_MF {
	// Название функции принадлежности.
	struct IndefiniteSets_CharArray name;
	/*
	Указатель на функцию принадлежности.
	double x - Основной вход функции.
	double * output - Указатель на результат функции.
	void * context - Указатель на контекст к функции.
	Возвращает: код ошибки функции.
	*/
	int (*mf)(double x, double * output, void * context);
	/*
	Контекст функции. Будет отправлен последним аргументом функции.
	*/
	void * context;
};

// Тип: массив функций принадлежности (membership function)
struct IndefiniteSets_MFArray {
	// Количество функций принадлежности в массиве.
	size_t size;
	// Указатель на начало массива функций принадлежности.
	struct IndefiniteSets_MF * array;
};

// Массив выходов.
struct IndefiniteSets_OutputArray {
	// Количество выходов в массиве.
	size_t size;
	// Указатель на первый выход массива.
	struct IndefiniteSets_Output * array;
};

// Структура описывает одно можетсво входных данных.
struct IndefiniteSets_Input {
	// Минимум и максимум допустимый входной параметр
	struct IndefiniteSets_Range range;
	// Название входного параметра
	struct IndefiniteSets_CharArray name;
	// Массив функций принадлежности (membership function)
	struct IndefiniteSets_MFArray MFs;
};

struct IndefiniteSets_Output {
	// Минимум и максимум допустимый входной параметр
	struct IndefiniteSets_Range range;
	// Название входного параметра
	struct IndefiniteSets_CharArray name;
	// Массив функций принадлежности (membership function)
	struct IndefiniteSets_MFArray MFs;
};

struct IndefiniteSets_InputArray {
	// Количество элементов в массиве.
	size_t size;
	// Указатель на начало массива.
	struct IndefiniteSets_Input * array;
};

// Символизирует состояние системы в целом.
struct IndefiniteSets_System {
	// Входные множества в систему.
	struct IndefiniteSets_InputArray inputs;
	// Выходные множества из системы.
	struct IndefiniteSets_OutputArray outputs;
	// Правила перехода из входных в выходные.
	struct IndefiniteSets_RuleArray rules;
	// Название данной системы.
	struct IndefiniteSets_CharArray name;
};