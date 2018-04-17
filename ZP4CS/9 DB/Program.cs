using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _9_DB
{
    class Program
    {

        static void Main(string[] args)
        {
            try
            {
                using(SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename=E:\2.Study\UPOL\semestr 6\ZP4CS(C#)\9 DB\9 DB\students.mdf; Integrated Security=True;Connect Timeout=30";
                    conn.Open();
                    Console.WriteLine("Connection Opened");
                    SqlCommand command = new SqlCommand("SELECT DISTINCT Jmeno, Prijmeni FROM students ORDER BY prijmeni OFFSET 5 ROWS FETCH NEXT 10 ROWS ONLY; ", conn);
                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Console.WriteLine($"{dr[0]}, {dr[1]}");
                        }
                    }                    /*SqlCommand insertCmd = new SqlCommand("INSERT INTO students (OsCislo, Jmeno, Prijmeni, UserName, Rocnik, OborKomb) VALUES(@id, @name, @surname, @usrname, @year, @field); ", conn);
                    insertCmd.Parameters.Add(new SqlParameter("id", "R98765"));
                    insertCmd.Parameters.Add(new SqlParameter("name", "Alois"));
                    insertCmd.Parameters.Add(new SqlParameter("surname", "Fridrich"));
                    insertCmd.Parameters.Add(new SqlParameter("usrname", "fridal00"));
                    insertCmd.Parameters.Add(new SqlParameter("year", 2));
                    insertCmd.Parameters.Add(new SqlParameter("field", "INF"));
                    int affected = insertCmd.ExecuteNonQuery();                    SqlCommand insertCmd2 = new SqlCommand("INSERT INTO students (OsCislo, Jmeno, Prijmeni, UserName, Rocnik, OborKomb) VALUES(@id, @name, @surname, @usrname, @year, @field); ", conn);
                    insertCmd2.Parameters.Add(new SqlParameter("id", "R58410"));
                    insertCmd2.Parameters.Add(new SqlParameter("name", "Ivo"));
                    insertCmd2.Parameters.Add(new SqlParameter("surname", "Zlamal"));
                    insertCmd2.Parameters.Add(new SqlParameter("usrname", "zlaiv00"));
                    insertCmd2.Parameters.Add(new SqlParameter("year", 3));
                    insertCmd2.Parameters.Add(new SqlParameter("field", "INF"));
                    int affected2 = insertCmd2.ExecuteNonQuery();                    SqlCommand updCmd = new SqlCommand("UPDATE students SET UserName=@usrname WHERE OsCislo=@number;", conn);                    updCmd.Parameters.Add(new SqlParameter("usrname", "czerja01"));                    updCmd.Parameters.Add(new SqlParameter("number", "R16814"));                    updCmd.ExecuteNonQuery();

                    SqlCommand delCmd = new SqlCommand("DELETE FROM students WHERE OborKomb=@field;", conn);
                    delCmd.Parameters.Add(new SqlParameter("field", "APLINF"));
                    int affected3 = delCmd.ExecuteNonQuery();
                    Console.WriteLine("Affected: " + affected3);*/                    //TODO Last 3 exercises
                }
            } catch(Exception e)
            {

            }

        }
    }
}
