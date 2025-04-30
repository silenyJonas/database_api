using database_api.Documents;
using database_api.Windows;
using System.Drawing;

namespace database_api.Components
{
    public class RowController
    {
        private string TempRow;
        private string Title;
        private bool Login;
        private int ValidateLogin = 0;
        private bool InpName = true;
        private string TempName = "";
        private string TempPassword = "";
        private ResponseList errList { get; } = new ResponseList();
        private CommandProccesor cmp { get; } = new CommandProccesor();

        public RowController(string _title, bool login)
        {
            Login = login;
            Title = _title;
            TempRow = Login ? $"{Title}name: " : Title;
            RowDataSingleton.GetInstance().RowData[RowDataSingleton.GetInstance().RowCounter] = new Row("Welcome", true, ConsoleColor.Green);
            RowDataSingleton.GetInstance().RowCounter++;            
        }

        public void AddChar(ConsoleKeyInfo info)
        {
            if (info.Key == ConsoleKey.Enter)
            {
                if (Login)
                {
                    AddRowToList(new Package(null, false));
                    LoginInp();
                }
                else
                {

                    string tempData = TempRow;                    
                    AddRowToList(new Package(null, false));
                    Package package = cmp.Execute(tempData, Title); //systemValue - true=systemove hlasky; false=uzivatelsky input                    
                    TempRow = package.Text;
                    AddRowToList(new Package(null, package.systemValue, package.Color, package.Data));
                }
            }
            else if (info.Key == ConsoleKey.Backspace)
            {
                if (Login)
                {
                    string temp = InpName ? TempName : TempPassword;
                    if (temp.Length > 0)
                    {
                        temp = temp.Substring(0, temp.Length - 1);
                        TempRow = TempRow.Substring(0, TempRow.Length - 1);
                        if (InpName)
                            TempName = temp;
                        else
                            TempPassword = temp;
                    }
                }
                else if (TempRow.Length > Title.Length)
                {
                    TempRow = TempRow.Substring(0, TempRow.Length - 1);
                }
                Console.Clear();

            }
            else
            {
                if (Login)
                {
                    if (InpName)
                    {
                        TempName += info.KeyChar;
                    }
                    else
                    {
                        TempPassword += info.KeyChar;
                    }
                }
                TempRow += info.KeyChar;
            }
        }

        private void AddRowToList(Package package)
        {
            int repeatingCounter;
            if(package.Data.Length > 0)
            {
                //je vic radku

                if (RowDataSingleton.GetInstance().RowData.Length == RowDataSingleton.GetInstance().RowCounter)
                {
                    RowDataSingleton.GetInstance().RowData = ExpandArray(RowDataSingleton.GetInstance().RowData, RowDataSingleton.GetInstance().RowCounter);
                }
                RowDataSingleton.GetInstance().RowData[RowDataSingleton.GetInstance().RowCounter] = new Row(TempRow, package.systemValue, package.Color);
                if (Login)
                    TempRow = $"{Title}name: ";
                else
                    TempRow = Title;
                RowDataSingleton.GetInstance().RowCounter++;

                repeatingCounter = package.Data.Length;

               

                for (int i = 0; i < repeatingCounter; i++)
                {
                    if (RowDataSingleton.GetInstance().RowData.Length == RowDataSingleton.GetInstance().RowCounter)
                    {
                        RowDataSingleton.GetInstance().RowData = ExpandArray(RowDataSingleton.GetInstance().RowData, RowDataSingleton.GetInstance().RowCounter);
                    }
                    RowDataSingleton.GetInstance().RowData[RowDataSingleton.GetInstance().RowCounter] = new Row(TempRow + package.Data[i], package.systemValue, ConsoleColor.White);
                    if (Login)
                        TempRow = $"{Title}name: ";
                    else
                        TempRow = Title;
                    RowDataSingleton.GetInstance().RowCounter++;
                }
            }
            else
            {
                //neni vic radku
                repeatingCounter = 1;

                if (RowDataSingleton.GetInstance().RowData.Length == RowDataSingleton.GetInstance().RowCounter)
                {
                    RowDataSingleton.GetInstance().RowData = ExpandArray(RowDataSingleton.GetInstance().RowData, RowDataSingleton.GetInstance().RowCounter);
                }
                RowDataSingleton.GetInstance().RowData[RowDataSingleton.GetInstance().RowCounter] = new Row(TempRow, package.systemValue, package.Color);
                if (Login)
                    TempRow = $"{Title}name: ";
                else
                    TempRow = Title;
                RowDataSingleton.GetInstance().RowCounter++;
            }              

           
        }

        public Row[] GetRows()
        {
            return RowDataSingleton.GetInstance().RowData;
        }
        public string GetLastRow()
        {
            return TempRow;
        }
        private Row[] ExpandArray(Row[] rows, int count)
        {
            
            Row[] newArray = new Row[rows.Length * 2];
            for (int i = 0; i < count; i++)
            {
                newArray[i] = rows[i];
            }
            return newArray;
            

        }
        private bool Validate()
        {
            string[] dataArray;
            using (StreamReader sr = new StreamReader(@"..\..\..\Documents\users.txt"))
            {
                var data = new List<string>();

                while (!sr.EndOfStream)
                {
                    data.Add(sr.ReadLine());
                }
                dataArray = data.ToArray();
            }
            foreach (string item in dataArray)
            {
                if (TempName == item.Split(';')[0])
                {
                    if (TempPassword == item.Split(';')[1])
                    {
                        return true;
                    }
                }
            }
            TempName = "";
            TempPassword = "";
            return false;
        }
        private void LoginInp()
        {
            InpName = !InpName;
            TempRow = InpName ? $"{Title}name: " : $"{Title}password: ";
            ValidateLogin++;

            if (ValidateLogin == 2)
            {
                if (Validate())
                {
                    Login = false;
                    Application.Window = new ListWindow();
                }
                else
                {
                    TempRow = errList.IncorrectLogin;
                    AddRowToList(new Package(null, true));
                    ValidateLogin = 0;
                }
            }
        }
    }
}
