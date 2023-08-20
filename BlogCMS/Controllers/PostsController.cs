
using Microsoft.AspNetCore.Mvc;
using BlogCMS.Data;
using Microsoft.EntityFrameworkCore;
using BlogCMS.Models.DTO;
using BlogCMS.Models.Entities;
using Microsoft.AspNetCore.Authorization;

namespace BlogCMS.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class PostsController : Controller
    {
        private readonly DataContext dbContext;
        public PostsController(DataContext dbContext) {
            this.dbContext = dbContext;
           
        }


        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await dbContext.Posts.ToListAsync();

            return Ok(posts);

        }
        [HttpGet]
        [Route("{Title}")]
        [ActionName("GetPostByTitle")]
        public async Task<IActionResult> GetPostByTitle(string Title)
        {
             var post = await dbContext.Posts.FirstAsync(x => x.Title == Title);



            if (post != null)
            {
                return Ok(post);

            }
            return NotFound();
        }


        [HttpPost]

        public async Task<IActionResult> AddPost(AddPostRequest addPostRequest)
        {

            // convert DTO to Entity

            var post = new Post()
            {
                Title = addPostRequest.Title,
                Content = addPostRequest.Content,
                Author = addPostRequest.Author,
                FeaturedImageUrl = addPostRequest.FeaturedImageUrl,
                PublishDate = addPostRequest.PublishDate,
                UpdateDate = addPostRequest.UpdateDate,
                Summery = addPostRequest.Summery,
                UrlHandler = addPostRequest.UrlHandler,
                Visible = addPostRequest.Visible,
            };

            post.id = Guid.NewGuid();
            await dbContext.Posts.AddAsync(post);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPostByTitle), new {id= post.id} ,post);
        }




        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdatePost([FromRoute] Guid id, UpdatePostRequest updatePostRequest)
        {
            // convert DTO to Entity

         

            var existingPost = await dbContext.Posts.FindAsync(id);

                if(existingPost != null)
            {
                
                existingPost.Title = updatePostRequest.Title;
                existingPost .Content = updatePostRequest.Content;
                 existingPost .Author = updatePostRequest.Author;
                 existingPost.FeaturedImageUrl = updatePostRequest.FeaturedImageUrl;
                existingPost.PublishDate = updatePostRequest.PublishDate;
                existingPost.UpdateDate = updatePostRequest.UpdateDate;
                existingPost.Summery = updatePostRequest.Summery;
                existingPost.UrlHandler = updatePostRequest.UrlHandler;
                existingPost.Visible = updatePostRequest.Visible;

                await dbContext.SaveChangesAsync();
                return Ok(existingPost);
            }
            return NotFound();
        }
        [Authorize]
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult>DeletePost(Guid id)
        {
            var existingPost = await dbContext.Posts.FindAsync(id);

            if(existingPost != null)
            {
                dbContext.Remove(existingPost);
                await dbContext.SaveChangesAsync();
                return Ok(existingPost);
            }
            return NotFound();
        }
    }
}
