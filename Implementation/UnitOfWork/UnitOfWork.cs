﻿using Implementation.Repository;
using Interface.Helper;
using Interface.IRepository;
using Interface.IUnitOfWork;
using Interface.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Implementation.UnitOfWork
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly RoyalFoodContext _context;
        private readonly ILogger<ConfigurationsManagementImp> _conflogger;
        private readonly ILogger<MenuManagementImp> _menlogger;
        private readonly ILogger<OrderManagementImp> _ordlogger;
        private readonly ILogger<CustomersManagementImp> _cuslogger;
        private readonly ILogger<SalesDashboardImp> _salelogger;
    
        public UnitOfWork(RoyalFoodContext context, ILoggerFactory loggerFactory, Helper helper) 
        {
            _context = context;
            _conflogger = loggerFactory.CreateLogger<ConfigurationsManagementImp>();
            _menlogger = loggerFactory.CreateLogger<MenuManagementImp>();
            _ordlogger = loggerFactory.CreateLogger<OrderManagementImp>();
            _cuslogger = loggerFactory.CreateLogger<CustomersManagementImp>();
            _salelogger = loggerFactory.CreateLogger<SalesDashboardImp>();

            Configurationsmanagement = new ConfigurationsManagementImp(context, _conflogger, helper);
            Menumanagement = new MenuManagementImp(context, _menlogger);
            Ordermanagement = new OrderManagementImp(context, _ordlogger);
            Customersmanagement = new CustomersManagementImp(context, _cuslogger);
            Salesdashboard = new SalesDashboardImp(context, _salelogger);

        }

        public IConfigurationsManagement Configurationsmanagement { get; set; }
        public IMenuManagement Menumanagement { get; set; }
        public IOrderManagement Ordermanagement { get; set; }
        public ICustomersManagement Customersmanagement { get; set; }
        public ISalesDashboard Salesdashboard { get; set; }


        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose() 
        {
            _context.Dispose();
        
        }

    }
}
