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
    
    public partial class roles
    {
        public roles()
        {
            this.teachers = new HashSet<teachers>();
        }
    
        public int id_role { get; set; }
        public string role_name { get; set; }
    
        public virtual ICollection<teachers> teachers { get; set; }
    }
}
