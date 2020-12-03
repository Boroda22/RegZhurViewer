using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RegZhurViewer
{
    /// <summary>
    /// Структура одной записи в базе данных
    /// </summary>
    class RecordRegZhur
    {
        // дата произошедшего события
        private DateTime data_event;
        // имя пользователя
        private string user_name;
        // имя компьютера
        private string comp_name;
        // имя приложения
        private string appl_name;
        /// <summary>
        /// Имя события
        /// </summary>
        private EventCodes event_name;
        // комментарий
        private string comment;
        // статус транзакции
        private string tranzakt_status;
        // данные
        private string data_val;
        ///<summary>
        /// представление данных
        ///</summary>
        private string data_presentation;
        /// <summary>
        /// Код метаданных
        /// </summary>
        private string metadata_codes;


        /// <summary>
        /// Дата произошедшего события
        /// </summary>
        public DateTime DataEvent
        {
            get { return data_event; }
            set { data_event = value; }
        }
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName
        {
            get { return user_name; }
            set { user_name = value; }
        }
        /// <summary>
        /// Имя компьютера
        /// </summary>
        public string CompName
        {
            get { return comp_name; }
            set { comp_name = value; }
        }
        /// <summary>
        /// Имя приложения
        /// </summary>
        public string ApplName
        {
            get { return appl_name; }
            set { appl_name = value; }
        }
        /// <summary>
        /// Имя события
        /// </summary>
        public EventCodes EventName { get { return event_name; } set { event_name = value; } }       
        /// <summary>
        /// Комментарий
        /// </summary>
        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }
        /// <summary>
        /// Статус транзакции
        /// </summary>
        public string TranzaktStatus
        {
            get { return tranzakt_status; }
            set { tranzakt_status = value; }
        }
        /// <summary>
        /// Данные, которые были изменены
        /// </summary>
        public string DataValue
        {
            get { return data_val; }
            set { data_val = value; }
        }
        /// <summary>
        /// Представление данных
        /// </summary>
        public string DataPresentation
        {
            get { return data_presentation; }
            set { data_presentation = value; }
        }
        /// <summary>
        /// Коды метаданных
        /// </summary>
        public string MetadataCodes
        {
            get { return metadata_codes; }
            set { metadata_codes = value; }
        }        
    }
}
