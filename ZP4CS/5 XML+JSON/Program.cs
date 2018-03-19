using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace _5_XML_JSON
{
    class Program
    {


        static void readStudentsWriteSecondYear(string xmlPath, string outputPath)
        {
            try
            {
                StudentList students = new StudentList();
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlPath);
                XmlNodeList nodes = doc.GetElementsByTagName("studentPredmetu");
                foreach(XmlNode node in nodes)
                {
                    foreach(XmlNode child in node.ChildNodes)
                    {
                        if(child.Name == "rocnik" && child.InnerText == "2")
                        {
                            Student student = new Student()
                            {
                                name = node["jmeno"].InnerText,
                                surname = node["prijmeni"].InnerText,
                                id = node["osCislo"].InnerText
                            };
                            students.studentiPredmetu.Add(student);
                        }
                    }
                }
                TextWriter tw = new StreamWriter(outputPath);
                var json = new JavaScriptSerializer().Serialize(students);
                tw.WriteLine(json);
                tw.Close();
            } catch(Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
            
        }

        static void readTeachersWritePeters(string input, string outputPath)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                XmlElement teachers = doc.CreateElement("teachers");
                doc.AppendChild(teachers);

                string text = File.ReadAllText(input);
                List<Teachers> deserialized = (List<Teachers>)new JavaScriptSerializer().Deserialize(text, typeof(List<Teachers>));
                foreach (var teachersList in deserialized)
                {
                    foreach (var teacher in teachersList.ucitel)
                    {
                        if (teacher.jmeno == "Petr")
                        {
                            XmlElement item = doc.CreateElement("teacher");

                            XmlElement jmeno = doc.CreateElement("jmeno");
                            jmeno.InnerText = teacher.jmeno;
                            item.AppendChild(jmeno);

                            XmlElement prijmeni = doc.CreateElement("prijmeni");
                            prijmeni.InnerText = teacher.prijmeni;
                            item.AppendChild(prijmeni);

                            XmlElement katedra = doc.CreateElement("katedra");
                            katedra.InnerText = teacher.katedra;
                            item.AppendChild(katedra);

                            teachers.AppendChild(item);
                        }
                    }
                }

                XmlTextWriter xmlTextWriter = new XmlTextWriter(outputPath, Encoding.UTF8);
                xmlTextWriter.Formatting = Formatting.Indented;
                doc.WriteContentTo(xmlTextWriter);
                xmlTextWriter.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        static void deserializeSubjectsWriteFirstN(string input, string outputPath, int count)
        {
            try
            {
                FileStream fs = new FileStream(input, FileMode.Open);
                XmlSerializer serializer = new XmlSerializer(typeof(Courses));
                Courses courses = (Courses)serializer.Deserialize(fs);
                Courses outputCourse = new Courses();
                outputCourse.courselist = courses.courselist.GetRange(0, count);

                TextWriter tw = new StreamWriter(outputPath);
                XmlSerializer sr = new XmlSerializer(typeof(Courses));
                sr.Serialize(tw, outputCourse);
                tw.Close();

            } catch(Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        static void Main(string[] args)
        {
            readStudentsWriteSecondYear(@"E:\2.Study\UPOL\semestr 6\ZP4CS(C#)\5 XML_JSON\5 XML_JSON\studentiPredmetu.xml", @"E:\druhaci.json");
            readTeachersWritePeters(@"E:\2.Study\UPOL\semestr 6\ZP4CS(C#)\5 XML_JSON\5 XML_JSON\uciteleKatedry.json", @"E:\peters.xml");
            deserializeSubjectsWriteFirstN(@"E:\2.Study\UPOL\semestr 6\ZP4CS(C#)\5 XML_JSON\5 XML_JSON\predmetyKatedry.xml", @"E:\firstNSubjects.xml", 20);
            Console.WriteLine("DONE!!!!");
            Console.ReadLine();
        }
    }
}
