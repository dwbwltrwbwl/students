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
    
    public partial class schedules
    {
        public int id_schedule { get; set; }
        public int id_group { get; set; }
        public int id_subject { get; set; }
        public int id_day { get; set; }
        public System.TimeSpan start_time { get; set; }
        public System.TimeSpan end_time { get; set; }
    
        public virtual days_of_week days_of_week { get; set; }
        public virtual groups groups { get; set; }
        public virtual subjects subjects { get; set; }
    }
}
