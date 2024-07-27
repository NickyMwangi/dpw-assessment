﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public partial class AspNetRole : BaseEntity
    {
        public AspNetRole()
        {            
            //AspNetRoleClaims = new HashSet<AspNetRoleClaim>();WSS
            AspNetUserRoles = new HashSet<AspNetUserRole>();
        }

        [StringLength(256)]
        public string Name { get; set; }
        [StringLength(256)]
        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }

        
        //[InverseProperty(nameof(AspNetRoleClaim.Role))]
        //public virtual ICollection<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        [InverseProperty(nameof(AspNetUserRole.Role))]
        public virtual ICollection<AspNetUserRole> AspNetUserRoles { get; set; }
    }
}