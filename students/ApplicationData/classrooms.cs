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
    
    public partial class classrooms
    {
        public classrooms()
        {
            this.schedules = new HashSet<schedules>();
        }
    
        public int id_classroom { get; set; }
        public Nullable<int> classroom_number { get; set; }
        public Nullable<int> capacity { get; set; }
    
        public virtual ICollection<schedules> schedules { get; set; }
    }
}
