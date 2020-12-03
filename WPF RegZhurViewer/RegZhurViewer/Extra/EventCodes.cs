using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegZhurViewer
{
    /// <summary>
    /// Виды событий
    /// </summary>
    class EventCodes
    {
        private int code_event;
        private string code_name;

        public int CodeEvent
        {
            get { return code_event; }
            set { code_event = value; }
        }
        public string NameEvent
        {
            get { return code_name; }
            set
            {               
                switch (value)
                {
                    //ДАННЫЕ
                    case "_$Data$_.Delete":
                        code_name = "ДАННЫЕ: Удаление";
                        break;
                    case "_$Data$_.DeletePredefinedData":
                        code_name = "ДАННЫЕ: Удаление предопределенных данных";
                        break;
                    case "_$Data$_.DeleteVersions":
                        code_name = "ДАННЫЕ: Удаление версий";
                        break;
                    case "_$Data$_.New":
                        code_name = "ДАННЫЕ: Добавление";
                        break;
                    case "_$Data$_.NewPredefinedData":
                        code_name = "ДАННЫЕ: Добавление предопределенных данных";
                        break;
                    case "_$Data$_.NewVersion":
                        code_name = "ДАННЫЕ: Добавление версии";
                        break;
                    case "_$Data$_.Post":
                        code_name = "ДАННЫЕ: Проведение";
                        break;
                    case "_$Data$_.Unpost":
                        code_name = "ДАННЫЕ: Отмена проведения";
                        break;
                    case "_$Data$_.Update":
                        code_name = "ДАННЫЕ: Изменение";
                        break;
                    case "_$Data$_.UpdatePredefinedData":
                        code_name = "ДАННЫЕ: Изменение предопределенных данных";
                        break;
                    //ДОСТУП
                    case "_$Access$_.Access":
                        code_name = "ДОСТУП: Доступ";
                        break;
                    case "_$Access$_.AccessDenied":
                        code_name = "ДОСТУП: Отказ в доступе";
                        break;
                    //ТРАНЗАКЦИЯ
                    case "_$Transaction$_.Begin":
                        code_name = "ТРАНЗАКЦИЯ: Начало транзакции";
                        break;
                    case "_$Transaction$_.Commit":
                        code_name = "ТРАНЗАКЦИЯ: Фиксация транзакции";
                        break;
                    case "_$Transaction$_.Rollback":
                        code_name = "ТРАНЗАКЦИЯ: Отмена транзакции";
                        break;
                    //ФОНОВОЕ ЗАДАНИЕ
                    case "_$Job$_.Cancel":
                        code_name = "ФОНОВОЕ ЗАДАНИЕ: Отмена";
                        break;
                    case "_$Job$_.Fail":
                        code_name = "ФОНОВОЕ ЗАДАНИЕ: Ошибка выполнения";
                        break;
                    case "_$Job$_.Start":
                        code_name = "ФОНОВОЕ ЗАДАНИЕ: Запуск";
                        break;
                    case "_$Job$_.Succeed":
                        code_name = "ФОНОВОЕ ЗАДАНИЕ: Успешное завершение";
                        break;
                    case "_$Job$_.Terminate":
                        code_name = "ФОНОВОЕ ЗАДАНИЕ: Принудительное завершение";
                        break;
                    //ИНФОРМАЦИОННАЯ БАЗА
                    case "_$InfoBase$_.ConfigUpdate":
                        code_name = "ИНФО БАЗА: Изменение конфигурации";
                        break;
                    case "_$InfoBase$_.DBConfigUpdate":
                        code_name = "ИНФО БАЗА: Изменение конфигурации базы данных";
                        break;
                    //ОШИБКА ВЫПОЛНЕНИЯ
                    case "_$PerformError$_":
                        code_name = "Ошибка выполнения";
                        break;
                    //СЕАНС
                    case "_$Session$_.Authentication":
                        code_name = "СЕАНС: Аутентификация";
                        break;
                    case "_$Session$_.AuthenticationError":
                        code_name = "СЕАНС: Ошибка аутентификации";
                        break;
                    case "_$Session$_.Finish":
                        code_name = "СЕАНС: Завершение";
                        break;
                    case "_$Session$_.Start":
                        code_name = "СЕАНС: Начало";
                        break;
                    //ПОЛЬЗОВАТЕЛИ
                    case "_$User$_.New":
                        code_name = "ПОЛЬЗОВАТЕЛИ: Добавление";
                        break;
                    case "_$User$_.Update":
                        code_name = "ПОЛЬЗОВАТЕЛИ: Изменение";
                        break;
                    default:
                        //выводим то, что есть
                        code_name = value;
                        break;
                }
            }
        }
    }
}
