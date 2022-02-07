using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.IO;

namespace Practice
{
    static class Delegates
    {
        public static double time = 0;

        public delegate void Timer(string message);
        public static event Timer ntf;

        static string private_text = "";

        public static string Text
        {
            get
            {
                return private_text;
            }
            set
            {
                ntf(value);
            }
        }

        public static void Log(string message)
        {
            StreamWriter stream = new StreamWriter("Elistratov.txt", true);
            stream.WriteLine($"{TimeSpan.FromMilliseconds(time)} | {message}");
            stream.Close();
        }

    }

    class Program
    {
        static string connectionstring = @"Data Source=server-sql1\students;Database=Elistratov;Integrated Security=True;";

        static void starttime()
        {
            while (true)
            {
                Delegates.time = Delegates.time + 10;
                Thread.Sleep(10);
            }

        }

        static void ntfy()
        {
            Delegates.ntf += Delegates.Log;
        }

        static void Main(string[] args)
        {
            Thread thread_t1 = new Thread(new ThreadStart(ntfy));
            thread_t1.Start();

            Thread thread_t2 = new Thread(new ThreadStart(starttime));
            thread_t2.Start();

            SqlConnection base_sql = new SqlConnection(connectionstring);

            while (true)
            {
            bs:
                Console.WriteLine("Меню:");
                Console.WriteLine("1) Посмотреть");
                Console.WriteLine("2) Добавить");
                Console.WriteLine("3) Изменить");
                Console.WriteLine("4) Удалить");

                string stroka = Console.ReadLine();
                int chislo;

                try
                {
                    chislo = Convert.ToInt32(stroka);
                }
                catch
                {
                    Console.WriteLine("не число");
                    goto bs;
                }

                Console.Clear();

                switch (chislo)
                {
                    case 1:
                        {
                            base_sql.Open();

                            SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM CONTRAGENTS", base_sql);

                            base_sql.Close();

                            DataSet data = new DataSet();

                            dataAdapter.Fill(data);
                            for (int i = 0; i < data.Tables[0].Rows.Count; i++)
                            {
                                for (int j = 0; j < data.Tables[0].Columns.Count; j++)
                                {
                                    Console.Write(data.Tables[0].Rows[i][j] + " ||| ");
                                }

                                Console.WriteLine("\n=======================================================");
                            }

                            Delegates.Text = "Просмотр таблицы";

                            break;
                        }
                    case 2:
                        {
                            Console.WriteLine("Введите имя.");
                            string name = Console.ReadLine();
                            Console.WriteLine("Введите фамилию.");
                            string familia = Console.ReadLine();
                            Console.WriteLine("Введите отчество.");
                            string patronic = Console.ReadLine();
                            Console.WriteLine("Введите телефон.");
                            string phone = Console.ReadLine();

                            base_sql.Open();
                            SqlCommand command = new SqlCommand($"INSERT INTO CONTRAGENTS (Имя, Фамилия, Отчество, Телефон) VALUES ('{name}', '{familia}', '{patronic}', '{phone}');", base_sql);
                            command.ExecuteNonQuery();
                            base_sql.Close();


                            Delegates.Text = "Добавление в таблицу: " + name + " " + familia + " " + patronic + " " + phone;
                            break;
                        }
                    case 3:
                        {
                            base_sql.Open();
                            string Id = Console.ReadLine();
                            int privateID;

                            try
                            {
                                privateID = Convert.ToInt32(Id);
                            }
                            catch
                            {
                                base_sql.Close();
                                Console.WriteLine("не число");
                                break;
                            }

                            string arg = "";

                            Console.WriteLine("Введите имя.");
                            string Name = Console.ReadLine();
                            if (Name != "")
                            {
                                arg = arg + $"Имя = '{Name}',";
                            }
                            Console.WriteLine(arg);

                            Console.WriteLine("Введите фамилию.");
                            string Fam = Console.ReadLine();

                            if (Fam != "")
                            {
                                arg = arg + $" Фамилия = '{Fam}',";
                            }
                            Console.WriteLine(arg);

                            Console.WriteLine("Введите отчество.");
                            string Otc = Console.ReadLine();

                            if (Otc != "")
                            {
                                arg = arg + $" Отчество = '{Otc}',";
                            }
                            Console.WriteLine(arg);

                            Console.WriteLine("Введите телефон.");
                            string Phone = Console.ReadLine();

                            if (Phone != "")
                            {
                                arg = arg + $" Телефон = '{Phone}',";
                            }

                            arg = arg.Remove(arg.Length - 1);

                            string cmd = $"UPDATE CONTRAGENTS SET {arg} WHERE id = {privateID};";

                            SqlCommand command = new SqlCommand(cmd, base_sql);
                            command.ExecuteNonQuery();
                            base_sql.Close();

                            Delegates.Text = "Добавление в таблицу: " + Name + " " + Fam + " " + Otc + " " + Phone;

                            break;
                        }
                    case 4:
                        {
                            string Id = Console.ReadLine();

                            int privateID;
                            try
                            {
                                privateID = Convert.ToInt32(Id);
                            }
                            catch
                            {
                                Console.WriteLine("не число");
                                break;
                            }

                            base_sql.Open();
                            SqlCommand command = new SqlCommand(string.Format($"DELETE FROM CONTRAGENTS WHERE id = '{1}';", privateID), base_sql);
                            command.ExecuteNonQuery();
                            base_sql.Close();

                            Delegates.Text = $"удаление строки, id: {privateID}";
                            break;
                        }
                }
            }
        }
    }
}
