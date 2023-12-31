﻿using Interface.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.IUnitOfWork
{
    public interface IUnitOfWork
    {
        IConfigurationsManagement Configurationsmanagement { get; }
        IMenuManagement Menumanagement { get; }
        IOrderManagement Ordermanagement { get; }
        ICustomersManagement Customersmanagement { get; }
        ISalesDashboard Salesdashboard { get; }
        Task CompleteAsync();
        void Dispose();
    }
}
