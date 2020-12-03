using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace RegZhurViewer
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// экземпляр базы данных
        /// </summary>
        private WorkWithDataBase db_work;       
        /// <summary>
        /// путь к файлу с данными
        /// </summary>
        public string PathToFile { get; set; }
        /// <summary>
        /// Список данных
        /// </summary>
        public ObservableCollection<RecordRegZhur> CollectionData { get; set; }
        /// <summary>
        /// Список событий
        /// </summary>
        public ListCollectionView DataEvents { get; set; }
        /// <summary>
        /// Выбранное значение для фильтра по событиям
        /// </summary>
        public EventCodes CurrentValueFilter { get; set; }
        /// <summary>
        /// Подстрока поиска
        /// </summary>
        public string SearchString { get; set; }
        /// <summary>
        /// Процесс поиска запущен
        /// </summary>
        public bool IsCalculating { get; set; }
        /// <summary>
        /// Текст кнопки фильтра по событиям
        /// </summary>
        public string CaptionBtnSearchEvent { get; set; }
        /// <summary>
        /// Свойство содержит текст кнопки фильтра по представлению
        /// </summary>
        public string CaptionBtnSearchPresent { get; set; }
        /// <summary>
        /// Токен отмены задачи отбора по событию
        /// </summary>
        private CancellationTokenSource cts_event = new CancellationTokenSource();
        /// <summary>
        /// Флаг запущенной задачи отбора по событию
        /// </summary>
        private bool TaskSearchEventIsRuning { get; set; }
        /// <summary>
        /// Флаг запущенной задачи отбора по части строки
        /// </summary>
        private bool TaskSearchPresentsRunning { get; set; }
        /// <summary>
        /// Количество найденных записей
        /// </summary>
        public int CountResultRecord { get; set; }


        //ОБРАБОТЧИКИ КОМАНД
        //КОМАНДА ОТКРЫТИЯ КАТАЛОГА С ФАЙЛОМ
        public ICommand CommandOpenDataFile
        {
            get { return new DelegateCommand(OpenFileDir); }
        }
        //КОМАНДА ФИЛЬТРАЦИИ ДАННЫХ ПО СОБЫТИЮ
        public ICommand CommandGetData
        {
            get { return new DelegateCommand(GetDataAsync); }
        }
        //КОМАНДА ФИЛЬТРАЦИИ ДАННЫХ ПО ПОДСТРОКЕ
        public ICommand CommandSearchSubString
        {
            get { return new DelegateCommand(SearchSubStringAsync); }
        }


        //РЕАЛИЗАЦИЯ КОМАНД
        /// <summary>
        /// Процедура открытия каталога с файлом
        /// </summary>
        private void OpenFileDir()
        {
            OpenFileDialog myDialog = new OpenFileDialog();
            myDialog.Filter = "Файлы журнала регистраций(*.lgd)|*.lgd;" + "|Все файлы (*.*)|*.*";
            myDialog.CheckFileExists = true;
            myDialog.Multiselect = false;
            if (myDialog.ShowDialog() == true)
            {
                PathToFile = myDialog.FileName;
                OnPropertyChanged("PathToFile");
                //загружаем список событий
                if (db_work.isConnected(PathToFile))
                {
                    DataEvents = db_work.GetEventList();
                    OnPropertyChanged("DataEvents");
                }
            }
        }
        /// <summary>
        /// Процедура отбора данных по событию
        /// </summary>      
        private async void GetDataAsync()
        {           
            if (PathToFile != null && db_work.isConnected(PathToFile))
            {
                if (!TaskSearchEventIsRuning)
                {
                    TaskSearchEventIsRuning = true;
                    //Запускаем признак выполнения длительной операции
                    IsCalculating = true;
                    OnPropertyChanged("IsCalculating");
                    //меняем текст кнопки
                    CaptionBtnSearchEvent = "Отменить";
                    OnPropertyChanged("CaptionBtnSearchEvent");
                    //запускаем асинхронную функцию
                    CollectionData = await db_work.GetDataByEvent(CurrentValueFilter.CodeEvent, cts_event.Token);
                    OnPropertyChanged("CollectionData");
                    //определяем количество найденных записей
                    CountResultRecord = CollectionData.Count();
                    OnPropertyChanged("CountResultRecord");
                    //признак выполнения длительной операции
                    IsCalculating = false;
                    OnPropertyChanged("IsCalculating");
                    //cбрасывам флаг выполнения задачи
                    TaskSearchEventIsRuning = false;
                    //меняем текст кнопки
                    CaptionBtnSearchEvent = "Отфильтровать";
                    OnPropertyChanged("CaptionBtnSearchEvent");
                    MessageBox.Show("Операция завершилась, найдено " + CountResultRecord.ToString() + " записей");
                }
                else
                {
                    //признак выполнения длительной операции
                    IsCalculating = false;
                    OnPropertyChanged("IsCalculating");
                    //меняем текст кнопки
                    CaptionBtnSearchEvent = "Отфильтровать";
                    OnPropertyChanged("CaptionBtnSearchEvent");
                    //cбрасывам флаг выполнения задачи
                    TaskSearchEventIsRuning = false;
                    cts_event.Cancel();
                    MessageBox.Show("Операция прервана пользователем");
                }
            }
            else
            {
                MessageBox.Show("Ошибка пути к файлу с данными");
            }
        }
        /// <summary>
        /// процедура поиска данных по подстроке
        /// </summary>
        private async void SearchSubStringAsync()
        {
            if (PathToFile != null && db_work.isConnected(PathToFile))
            {
                if (!TaskSearchPresentsRunning)
                {
                    TaskSearchPresentsRunning = true;
                    //Запускаем признак выполнения длительной операции
                    IsCalculating = true;
                    OnPropertyChanged("IsCalculating");
                    //меняем текст кнопки
                    CaptionBtnSearchPresent = "Отменить";
                    OnPropertyChanged("CaptionBtnSearchPresent");
                    //запускаем асинхронную функцию
                    CollectionData = await db_work.GetDataBySubString(SearchString);
                    OnPropertyChanged("CollectionData");
                    //определяем количество найденных записей
                    CountResultRecord = CollectionData.Count();
                    OnPropertyChanged("CountResultRecord");
                    //Запускаем признак выполнения длительной операции
                    IsCalculating = false;
                    OnPropertyChanged("IsCalculating");
                    //признак выполнения длительной операции
                    IsCalculating = false;
                    OnPropertyChanged("IsCalculating");
                    //cбрасывам флаг выполнения задачи
                    TaskSearchPresentsRunning = false;
                    //меняем текст кнопки
                    CaptionBtnSearchPresent = "Отфильтровать";
                    OnPropertyChanged("CaptionBtnSearchPresent");
                    MessageBox.Show("Операция завершилась, найдено " + CountResultRecord.ToString() + " записей");
                }
                else
                {
                    //признак выполнения длительной операции
                    IsCalculating = false;
                    OnPropertyChanged("IsCalculating");
                    //меняем текст кнопки
                    CaptionBtnSearchPresent = "Отфильтровать";
                    OnPropertyChanged("CaptionBtnSearchPresent");
                    //cбрасывам флаг выполнения задачи
                    TaskSearchPresentsRunning = false;
                    cts_event.Cancel();
                    MessageBox.Show("Операция прервана пользователем");
                }
            }
            else
            {
                MessageBox.Show("Ошибка пути к файлу с данными");
            }
        }


        //КОНСТРУКТОР МОДЕЛИ
        public MainWindowViewModel()
        {
            //инициализируем модель базы данных
            db_work = new WorkWithDataBase();
            CaptionBtnSearchEvent = "Отфильтровать";
            CaptionBtnSearchPresent = "Отфильтровать";
            TaskSearchEventIsRuning = false;
            TaskSearchPresentsRunning = false;
            CountResultRecord = 0;
        }


        // реализация обработчиков INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
