//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace students.ApplicationData
{
    using System;
    using System.Collections.Generic;
    
    public partial class courses
    {
        public courses()
        {
            this.enrollments = new HashSet<enrollments>();
            this.feedback = new HashSet<feedback>();
            this.schedules = new HashSet<schedules>();
            this.subjects = new HashSet<subjects>();
        }
    
        public int id_course { get; set; }
        public string course_name { get; set; }
        public string course_description { get; set; }
        public int id_semester { get; set; }
    
        public virtual semesters semesters { get; set; }
        public virtual ICollection<enrollments> enrollments { get; set; }
        public virtual ICollection<feedback> feedback { get; set; }
        public virtual ICollection<schedules> schedules { get; set; }
        public virtual ICollection<subjects> subjects { get; set; }
    }
}
