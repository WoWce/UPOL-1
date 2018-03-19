using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5_XML_JSON
{
    public class Student
    {
        public String name { get; set; }
        public String surname { get; set; }
        public String id { get; set; }
    }

    public class StudentList
    {
        public List<Student> studentiPredmetu = new List<Student>();
    }
}
