﻿using Gifter.Models;

namespace Gifter.Repositories
{
    public interface IPostRepository
    {
        List<Post> GetAll();
        Post GetById(int id);
        void Add(Post post);
        void Update(Post post);
        void Delete(int id);
        List<Post> GetAllWithComments();
        Post GetPostByIdWithComments(int id);
        public List<Post> Search(string criterion, bool sortDescending);
        public List<Post> HottestSince(string criterion, bool sortDescending);
    }
}
