using Lemondo.Context;
using Lemondo.DbClasses;
using Lemondo.UnitofWork.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;



namespace Lemondo.UnitofWork.Repository
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        public BookRepository(LibraryContext context, ILogger logger) : base(logger, context)
        {

        }
        
    }
}
