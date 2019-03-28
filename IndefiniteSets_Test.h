#pragma once
#include "IndefiniteSets.h"
#include <stdio.h>

/*
Проверка примера от преподавателя.
*/
void IndefiniteSets_Test_teacher(FILE * out) {
	/*
	Псевдокод.
	Инициализация.
	Вес яблока {
		МАЛЫЙ		1/50 +	 0.5/150 +	0/250 +		0/350
		СРЕДНИЙ		0/50 +	 1/150 +	0.5/250 +	0/350
		БОЛЬШОЙ		0/50 +	 0/150 +	0.5/250 +	1/350
	}
	Потребительская стоимость яблока {
		НИЗКАЯ	1.0/10 +	0.8/20 +	0.1/30 +	0.0/40
		СРЕДНЯЯ	0.1/10 +	1.0/20 +	0.5/30 +	0.0/40
		ВЫСОКАЯ	0.0/10 +	0.4/20 +	0.7/30 +	1.0/40
	}
	Правила {
		"Вес яблока"."МАЛЫЙ" => "Потребительская стоимость яблока"."НИЗКАЯ"
		"Вес яблока"."СРЕДНИЙ" => "Потребительская стоимость яблока"."ВЫСОКАЯ"
		"Вес яблока"."БОЛЬШОЙ" => "Потребительская стоимость яблока"."СРЕДНИЙ"
	}
	Сделать ввод конкретных данных {
		Множество яблок 1 = ....
		...
	}
	Надо постротить таблицу. R1 =
	N1 ->
	1	0.8	0.1	0		M1
	0.5	0.5	0.1	0		||
	0	0	0	0		\/
	0	0	0	0
	Программа должна реализовать генерацию данной таблицы.
	Программа должна уметь строить НЕ к терме.
	Программа должна уметь добавлять терму к существующим.
	*/

	struct IndefiniteSets_State state = IndefiniteSets_stateMalloc();
	struct IndefiniteSets_Characteristic * characteristic =
		IndefiniteSets_characteristicMalloc("Вес яблока", 50, 350);
	IndefiniteSets_addCharacteristic(state, characteristic);
	IndefiniteSets_addTermMalloc(characteristic,
		"Малый", "1,0/50,0 0,5/150,0 0,0/250,0 0,0/350,0");
	IndefiniteSets_addTermMalloc(characteristic,
		"Средний", "0,0/50,0 1,0/150,0 0,5/250,0 0,0/350,0");
	IndefiniteSets_addTermMalloc(characteristic,
		"Большой", "0,0/50,0 0,0/150,0 0,5/250,0 1,0/350,0");

	state.addCharacteristic(state, "Потребительская стоимость яблока", 10, 40);
	state.getCharacteristic(state, "Потребительская стоимость яблока").addTermMalloc(
		"Низкая", "1,0/10,0 0,8/20,0 0,1/30,0 0,0/40,0");
	state.getCharacteristic(state, "Потребительская стоимость яблока").addTermMalloc(
		"Средняя", "0,1/10,0 1,0/20,0 0,5/30 0,0/40,0");
	state.getCharacteristic(state, "Потребительская стоимость яблока").addTermMalloc(
		"Высокая", "0,0/10,0 0,4/20,0 0,7/30,0 1,0/40,0");

	state.addRule();
}

void IndefiniteSets_Test_main(void) {

}