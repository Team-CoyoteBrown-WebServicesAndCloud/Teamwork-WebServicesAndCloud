namespace SocialNetwork.Data.Data
{
    using System;
    using System.Collections.Generic;
    using Interfaces;
    using Models;
    using Repositories;

    public abstract class SocialNetworkData : ISocialNetworkData
    {
        private ISocialNetworkContext context;
        private IDictionary<Type, object> repositories;

        public SocialNetworkData(ISocialNetworkContext context)
        {
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }
        public ISocialNetworkContext Context
        {
            get
            {
                return this.context;
            }
        }

        public IRepository<ApplicationUser> Users
        {
            get { return this.GetRepository<ApplicationUser>(); }
        }

        public IRepository<Post> Posts
        {
            get { return this.GetRepository<Post>(); }
        }

        public IRepository<Comment> Comments
        {
            get { return this.GetRepository<Comment>(); }
        }

        public IRepository<FriendRequest> FriendRequests
        {
            get { return this.GetRepository<FriendRequest>(); }
        }

        public IRepository<PostLike> PostLikes
        {
            get { return this.GetRepository<PostLike>(); }
        }

        public IRepository<CommentLike> CommentLikes
        {
            get { return this.GetRepository<CommentLike>(); }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }
        private IRepository<T> GetRepository<T>() where T : class
        {
            var typeOfModel = typeof(T);

            if (!this.repositories.ContainsKey(typeOfModel))
            {
                var type = typeof(GenericRepository<T>);
                this.repositories.Add(typeOfModel, Activator.CreateInstance(type, this.context));
            }

            return this.repositories[typeOfModel] as IRepository<T>;
        }
    }
}
