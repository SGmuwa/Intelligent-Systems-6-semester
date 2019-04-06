﻿using System;
using System.Collections.Generic;

namespace lab1
{
    /// <summary>
    /// Класс представления характеристики.
    /// </summary>
    public class Characteristic
    {
        /// <summary>
        /// Список терм, которые описывают характеристику.
        /// </summary>
        protected List<Terma> Terms
            = new List<Terma>();
        /// <summary>
        /// Создание новой характеристики.
        /// </summary>
        /// <param name="name">Имя характериситики.</param>
        /// <param name="min">Минимальное допустимое значение характериситики.</param>
        /// <param name="max">Максимальное допустимое значение характериситики.</param>
        public Characteristic(string name, float min, float max)
        {
            if (name == null)
                throw new ArgumentNullException();
            if (float.IsNaN(min) || float.IsInfinity(min)
                || float.IsNaN(max) || float.IsInfinity(max)
                || name.Length == 0)
                throw new ArgumentException();
            Name = name;
            Min = min;
            Max = max;
        }

        /// <summary>
        /// Получение имени характериситики.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Получение минимального задования характериситики.
        /// </summary>
        public float Min { get; }
        /// <summary>
        /// Получение максимального задования характериситики.
        /// </summary>
        public float Max { get; }

        /// <summary>
        /// Добавление новой термы в характериситику.
        /// </summary>
        /// <param name="nameTerma">Имя термы.</param>
        /// <param name="ranges">Писать числа по парам: первое число - процент от 0 до 1.
        /// Второе: эквивалент характеристики.</param>
        public void Add(string nameTerma, params double[] ranges)
        {
            if (ranges == null || nameTerma == null)
                throw new ArgumentNullException();
            if (ranges.Length % 2 != 0 || nameTerma.Length == 0)
                throw new ArgumentException();
            if (Terms.Find((Terma t) => nameTerma.Equals(t.Name)) != null)
                throw new ArgumentException("Имя уже занято");
            Terma toAdd = new Terma(nameTerma, this);
            double percent;
            double valueCharacteristic;
            for(int i = 0; i < ranges.Length; i++)
            {
                if (0.0 <= ranges[i] && ranges[i] <= 1.0)
                    percent = ranges[i];
                else
                    throw new ArgumentException();
                if (Min <= ranges[++i] && ranges[i] <= Max)
                    valueCharacteristic = ranges[i];
                else
                    throw new ArgumentException();
                toAdd.Add(percent, valueCharacteristic);
            }
            if (!Terms.Contains(toAdd))
                Terms.Add(toAdd);
            else
                throw new ArgumentException();
        }

        /// <summary>
        /// Добавление новой термы на основе предыдущей
        /// </summary>
        /// <param name="logical">Логическая операция над термой.</param>
        /// <param name="nameFrom">Имя предыдущей термы.</param>
        /// <param name="nameNew">Имя новой термы.</param>
        public void Add(LogicalFunctions logical, string nameFrom, string nameNew)
        {
            if (nameNew == null || nameFrom == null)
                throw new ArgumentNullException("Имена не могут быть null.");
            if (nameNew.Length == 0 || nameFrom.Length == 0)
                throw new ArgumentException("Имена должны быть не пустыми.");
            if ((Terms.Find((Terma t) => nameFrom.Equals(t.Name)) == null))
                throw new ArgumentException("Элемент \"" + nameFrom + "\" не найден.");
            Terma toAdd = new Terma(nameNew, this);
            switch(logical)
            {
                case LogicalFunctions.NOT:
                    foreach (PercentAndCharacteristicvalue pacv in this[nameFrom])
                    {
                        toAdd.Add(1.0 - pacv.Percent, pacv.CharacteristicValue);
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
            if (!Terms.Contains(toAdd))
                Terms.Add(toAdd);
            else
                throw new ArgumentException();
        }

        public Terma this[string name]
            => Terms.Find((Terma t) => name.Equals(t.Name));

        public override string ToString()
        {
            return "{ \"Name\":\"" +
                Name +
                "\", \"Min\":\"" +
                Min +
                "\", \"Max\":\"" +
                Max +
                "\", \"Terms:\":" +
                Terms.ToString(", ") +
                "}";
        }
    }
}