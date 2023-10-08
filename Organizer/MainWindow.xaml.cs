using Organizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace Organizer
{
    public partial class MainWindow : Window
    {
        private struct LastDate 
        {
            public string lastDateString;
        }

        public MainWindow()
        {
            InitializeComponent();
            UpdateTaskDays();
        }

        private void FillTasksData() {
            tasksGrid.ItemsSource = DbUtils.db.Tasks.Where(w => w.Status == 1).Select(p => new
            {
                p.Id,
                p.Description,
                p.Day,
                DbUtils.db.Types.FirstOrDefault(f => f.Id == p.Type).Type,
                p.Advanced
            }).OrderByDescending(p => p.Advanced).Select(s => new
            {
                s.Id,
                s.Description,
                s.Day,
                s.Type
            }).ToList();

            tasksPutGrid.ItemsSource = DbUtils.db.Tasks.Where(w => w.Status == 2).Select(p => new
            {
                p.Id,
                p.Description,
                DbUtils.db.Types.FirstOrDefault(f => f.Id == p.Type).Type,
            }).ToList();
        }

        private void UpdateTaskDays()
        {
            string path = Environment.CurrentDirectory + @"\Config\LastDate.json";
            LastDate lastDate = JsonConvert.DeserializeObject<LastDate>(File.ReadAllText(path));
            if (DateTime.Now.Date == Convert.ToDateTime(Convert.ToDateTime(lastDate.lastDateString)))
            {
                FillTasksData();
                return;
            }
            string dayDiffer = "";
            string dateDiffer = (Convert.ToDateTime(DateTime.Now.Date) - Convert.ToDateTime(Convert.ToDateTime(lastDate.lastDateString))).ToString();
            for (int i = 0; dateDiffer[i] != '.'; i++)
            {
                dayDiffer += dateDiffer[i];
            }
            int dayDifferInt = Convert.ToInt32(dayDiffer);
            IEnumerable<Tasks> tasksUpdate = DbUtils.db.Tasks.AsEnumerable().Where(w => w.Status == 1).Select(s =>
            {
                s.Day = s.Day - dayDifferInt;
                return s;
            });
            foreach (Tasks task in tasksUpdate)
            {
                // update entry
                DbUtils.db.Entry(task).State = EntityState.Modified;
            }
            DbUtils.db.SaveChanges();
            lastDate.lastDateString = DateTime.Now.Date.ToString();
            File.WriteAllText(path, JsonConvert.SerializeObject(lastDate, Formatting.Indented));
            FillTasksData();
        }

        private void CreateTaskBut_Click(object sender, RoutedEventArgs e)
        {
            newTask newTask = new newTask();
            newTask.ShowDialog();
            FillTasksData();
        }

        private void UpdateTaskBut_Click(Object sender, RoutedEventArgs e)
        {
            UpdateTaskDays();
        }

        private void PreviewTasksBut_Click(Object sender, RoutedEventArgs e)
        {
            TypesWindow typesWindow = new TypesWindow();
            typesWindow.ShowDialog();
        }

        private void TasksGrid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (tasksGrid.SelectedCells.Count() == 0) return;
            var cellInfo = tasksGrid.SelectedCells[0];
            int content = Convert.ToInt16((cellInfo.Column.GetCellContent(cellInfo.Item) as TextBlock).Text);
            var task = DbUtils.db.Tasks.Where(x => x.Id == content).FirstOrDefault();
            if (task != null)
            {
                ModificationTask modificationTask = new ModificationTask(task);
                modificationTask.ShowDialog();
                FillTasksData();
            }
            else
            {
                MessageBox.Show("error");
                return;
            }
        }

        private void TasksPutGrid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (tasksPutGrid.SelectedCells.Count() == 0) return;
            var cellInfo = tasksPutGrid.SelectedCells[0];
            int content = Convert.ToInt16((cellInfo.Column.GetCellContent(cellInfo.Item) as TextBlock).Text);
            var task = DbUtils.db.Tasks.Where(x => x.Id == content).FirstOrDefault();
            if (task != null)
            {
                task.Status = 1;
                DbUtils.db.SaveChanges();
                FillTasksData();
            }
            else
            {
                MessageBox.Show("error");
                return;
            }
        }

        private void TasksGrid_AutoGeneratedColumns(object sender, EventArgs e)
        {
            foreach (var col in tasksGrid.Columns)
            {
                col.Header = Translator.Translate(col.Header.ToString());
            }   
            foreach (var col in tasksPutGrid.Columns)
            {
                col.Header = Translator.Translate(col.Header.ToString());
            }
        }
    }
}
