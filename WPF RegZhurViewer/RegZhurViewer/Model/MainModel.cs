using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegZhurViewer
{
	/// <summary>
	/// Класс главной модели данных
	/// </summary>
    class MainModel
    {
        /// <summary>
        /// метод преобразует время в секундах в нормальную дату
        /// </summary>
        /// <param name="unixTimeStamp">Параметр времени в секундах</param>
        /// <returns>Возвращает дату в нормализованном виде</returns>
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            unixTimeStamp = unixTimeStamp / 10000; //убираем милисекунды
            DateTime dtDateTime = (new DateTime(1, 1, 1, 0, 0, 0, 0)).AddSeconds(unixTimeStamp); //добавляем количество секунд к началу мироздания
            dtDateTime = Convert.ToDateTime(dtDateTime, CultureInfo.CurrentCulture);
            return dtDateTime;
        }
    }
}
