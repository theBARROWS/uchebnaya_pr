//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace uchebnaya_pr.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class demands
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public demands()
        {
            this.deals = new HashSet<deals>();
        }
    
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int TypeId { get; set; }
        public string Address_City { get; set; }
        public string Address_Street { get; set; }
        public string Address_House { get; set; }
        public string Address_Number { get; set; }
        public Nullable<int> MinFloor { get; set; }
        public Nullable<int> MaxFloor { get; set; }
        public Nullable<int> MinRooms { get; set; }
        public Nullable<int> MaxRooms { get; set; }
        public Nullable<int> MinPrice { get; set; }
        public Nullable<int> MaxPrice { get; set; }
        public Nullable<double> MinArea { get; set; }
        public Nullable<double> MaxArea { get; set; }
    
        public virtual clients clients { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<deals> deals { get; set; }
        public virtual type type { get; set; }
    }
}
