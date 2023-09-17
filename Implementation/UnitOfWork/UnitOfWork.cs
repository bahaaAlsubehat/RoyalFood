using Implementation.Repository;
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
    
        public UnitOfWork(RoyalFoodContext context, ILoggerFactory loggerFactory) 
        {
            _context = context;
            _conflogger = loggerFactory.CreateLogger<ConfigurationsManagementImp>();
            Configurationsmanagement = new ConfigurationsManagementImp(context, _conflogger);
        }

        public IConfigurationsManagement Configurationsmanagement { get; set; }
        public async Task CompleteAsync()
        {
            _context.SaveChangesAsync();
        }

        public void Dispose() 
        {
            _context.Dispose();
        
        }

    }
}
