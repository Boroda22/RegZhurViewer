using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace RegZhurViewer
{
    class WorkWithDataBase
    {
        /// <summary>
        /// Объект для работы с базой данных
        /// </summary>
        private SQLiteConnection connect;

        /// <summary>
        /// Метод подключения к файлу данных, при удачном подключении - возвращает true
        /// </summary>
        public bool isConnected(string path_to_data_file)
        {
            bool tmp_is_connected = false;

            try
            {
                //подключаемся к файлу базы данных
                connect = new SQLiteConnection("Data Source=" + path_to_data_file + "; Version=3;", true);
                //открывае базу для чтения
                connect.Open();
                tmp_is_connected = true;
            }
            catch(SQLiteException e)
            {
                tmp_is_connected = false;
                MessageBox.Show(e.Message);
            }
            return tmp_is_connected;
        }

        /// <summary>
        /// Асинхронный метод получения всех данных с отбором по виду события
        /// </summary>
        public Task<ObservableCollection<RecordRegZhur>> GetDataByEvent(int filter, CancellationToken token)
        {
            return Task.Run(() =>
            {
                //проверяем, прерывается ли выполнение задачи
                //token.ThrowIfCancellationRequested();
                //временное хранилище данных
                ObservableCollection<RecordRegZhur> tmp_collection_data = new ObservableCollection<RecordRegZhur>();
                if (connect != null & connect.State == System.Data.ConnectionState.Open)
                {
                    //создаем команду чтения данных
                    string sql_command = "SELECT data_event.date AS date_event," +
                        "data_event.connectID," +
                        "data_event.comment AS comment_data," +
                        "data_event.data AS field_data," +
                        "data_event.dataPresentation AS data_presentation," +
                        "data_event.metadataCodes AS metadata_codes," +
                        "user_codes.name AS user_name," +
                        "comp_codes.name AS comp_name," +
                        "app_codes.name AS app_name," +
                        "event_codes.code AS event_code," +
                        "event_codes.name AS event_name " +
                        "FROM EventLog data_event " +
                        "LEFT JOIN UserCodes user_codes ON data_event.userCode = user_codes.code " +
                        "LEFT JOIN ComputerCodes comp_codes ON data_event.computerCode = comp_codes.code " +
                        "LEFT JOIN AppCodes app_codes ON data_event.appCode = app_codes.code " +
                        "LEFT JOIN EventCodes event_codes ON data_event.eventCode = event_codes.code " +
                        "WHERE event_codes.code = " + filter.ToString();
                    SQLiteCommand cmd = new SQLiteCommand(sql_command, connect);

                    //считываем построчно данные и записываем их в коллекцию
                    try
                    {
                        SQLiteDataReader reader = cmd.ExecuteReader();
                        //используем адаптер, для выборки данных из БД в таблицу
                        //SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                        //Создаем объект Dataset
                        //DataSet ds = new DataSet();
                        //Заполняем Dataset
                        //adapter.Fill(ds);

                        while (reader.Read())
                        {
                            RecordRegZhur tmp_data = new RecordRegZhur();
                            double tmp_tickets = 0;
                            tmp_tickets = Double.Parse(reader["date_event"].ToString());
                            tmp_data.DataEvent = MainModel.UnixTimeStampToDateTime(tmp_tickets);
                            tmp_data.UserName = reader["user_name"].ToString();
                            tmp_data.CompName = reader["comp_name"].ToString();
                            tmp_data.ApplName = reader["app_name"].ToString();
                            int id_event = Int32.Parse(reader["event_code"].ToString());
                            string name_event = reader["event_name"].ToString().Replace("\"", "");
                            EventCodes tmp_evnt = new EventCodes();
                            tmp_evnt.CodeEvent = id_event;
                            tmp_evnt.NameEvent = name_event;
                            tmp_data.EventName = tmp_evnt;
                            tmp_data.Comment = reader["comment_data"].ToString();
                            tmp_data.DataValue = reader["field_data"].ToString();
                            tmp_data.DataPresentation = reader["data_presentation"].ToString();
                            tmp_data.MetadataCodes = reader["metadata_codes"].ToString();

                            tmp_collection_data.Add(tmp_data);
                        }
                    }
                    catch (SQLiteException e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
                return tmp_collection_data;
            });
        }
        /// <summary>
        /// Функция ищет данные в БД по подстроке
        /// </summary>
        /// <param name="filter"> подстрока для поиска</param>
        /// <returns></returns>
        public Task<ObservableCollection<RecordRegZhur>> GetDataBySubString(string filter)
        {
            return Task.Run(() =>
            {
                //временное хранилище данных
                ObservableCollection<RecordRegZhur> tmp_collection_data = new ObservableCollection<RecordRegZhur>();
                if (connect != null & connect.State == System.Data.ConnectionState.Open)
                {
                    //создаем команду чтения данных
                    string sql_command = "SELECT data_event.date AS date_event," +
                    "data_event.connectID," +
                    "data_event.comment AS comment_data," +
                    "data_event.data AS field_data," +
                    "data_event.dataPresentation AS data_presentation," +
                    "data_event.metadataCodes AS metadata_codes," +
                    "user_codes.name AS user_name," +
                    "comp_codes.name AS comp_name," +
                    "app_codes.name AS app_name," +
                    "event_codes.code AS event_code," +
                    "event_codes.name AS event_name " +
                    "FROM EventLog data_event " +
                    "LEFT JOIN UserCodes user_codes ON data_event.userCode = user_codes.code " +
                    "LEFT JOIN ComputerCodes comp_codes ON data_event.computerCode = comp_codes.code " +
                    "LEFT JOIN AppCodes app_codes ON data_event.appCode = app_codes.code " +
                    "LEFT JOIN EventCodes event_codes ON data_event.eventCode = event_codes.code " +
                    "WHERE data_event.dataPresentation LIKE " + "\'%" + filter.ToString() + "%\'";
                    SQLiteCommand cmd = new SQLiteCommand(sql_command, connect);

                    //считываем построчно данные и записываем их в коллекцию
                    try
                    {
                        SQLiteDataReader reader = cmd.ExecuteReader();
                        //используем адаптер, для выборки данных из БД в таблицу
                        //SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                        //Создаем объект Dataset
                        //DataSet ds = new DataSet();
                        //Заполняем Dataset
                        //adapter.Fill(ds);

                        while (reader.Read())
                        {
                            RecordRegZhur tmp_data = new RecordRegZhur();
                            double tmp_tickets = 0;
                            tmp_tickets = Double.Parse(reader["date_event"].ToString());
                            tmp_data.DataEvent = MainModel.UnixTimeStampToDateTime(tmp_tickets);
                            tmp_data.UserName = reader["user_name"].ToString();
                            tmp_data.CompName = reader["comp_name"].ToString();
                            tmp_data.ApplName = reader["app_name"].ToString();
                            int id_event = Int32.Parse(reader["event_code"].ToString());
                            string name_event = reader["event_name"].ToString().Replace("\"", "");
                            EventCodes tmp_evnt = new EventCodes();
                            tmp_evnt.CodeEvent = id_event;
                            tmp_evnt.NameEvent = name_event;
                            tmp_data.EventName = tmp_evnt;
                            tmp_data.Comment = reader["comment_data"].ToString();
                            tmp_data.DataValue = reader["field_data"].ToString();
                            tmp_data.DataPresentation = reader["data_presentation"].ToString();
                            tmp_data.MetadataCodes = reader["metadata_codes"].ToString();

                            tmp_collection_data.Add(tmp_data);
                        }
                    }
                    catch (SQLiteException e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
                return tmp_collection_data;
            });
        }

        /// <summary>
        /// Метод получения списка событий
        /// </summary>
        public ListCollectionView GetEventList()
        {
            List<EventCodes> tmp_list_events = new List<EventCodes>();
            if (connect != null & connect.State == System.Data.ConnectionState.Open)
            {
                //создаем команду чтения данных
                string sql_command = "SELECT event_codes.code AS event_code, event_codes.name AS event_name FROM EventCodes event_codes";
                SQLiteCommand cmd = new SQLiteCommand(sql_command, connect);
                //считываем построчно данные и записываем их в коллекцию
                try
                {
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        EventCodes tmp_evnt = new EventCodes();
                        tmp_evnt.CodeEvent = Int32.Parse(reader["event_code"].ToString());
                        //удаляем кавычки
                        tmp_evnt.NameEvent = reader["event_name"].ToString().Replace("\"", "");

                        tmp_list_events.Add(tmp_evnt);
                    }
                }
                catch (SQLiteException e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            //сортируем список
            ListCollectionView tmp_sorted_list = new ListCollectionView(tmp_list_events);
            tmp_sorted_list.SortDescriptions.Add(new SortDescription(nameof(EventCodes.NameEvent), ListSortDirection.Ascending));

            return tmp_sorted_list;
        }

        //КОНСТРУКТОР КЛАССА
        public WorkWithDataBase()
        {

        }

    }
}
