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
    
    public partial class students
    {
        public students()
        {
            this.attendance = new HashSet<attendance>();
        }
        public string FullName => $"{last_name} {first_name} {middle_name}";
        public int id_student { get; set; }
        public string last_name { get; set; }
        public string first_name { get; set; }
        public string middle_name { get; set; }
        public int id_gender { get; set; }
        public int id_group { get; set; }
        public System.DateTime date_birth { get; set; }
    
        public virtual ICollection<attendance> attendance { get; set; }
        public virtual genders genders { get; set; }
        public virtual groups groups { get; set; }
    }
}
