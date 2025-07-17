using BloggingPlatform.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggingPlatform.Data.Repositories
{
    public interface IPostRepository : IRepositoryAsync<Post>
    {
    }
}
