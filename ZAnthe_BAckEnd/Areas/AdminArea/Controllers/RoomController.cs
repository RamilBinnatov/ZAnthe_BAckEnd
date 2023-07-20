//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using System.Reflection.Metadata;
//using ZAnthe_BAckEnd.Data;
//using ZAnthe_BAckEnd.Helpers.Enums;
//using ZAnthe_BAckEnd.Helpers;

//namespace ZAnthe_BAckEnd.Areas.AdminArea.Controllers
//{
//    [Area("AdminArea")]
//    public class RoomController : Controller
//    {
//        #region Readonly
//        private readonly AppDbContext _context;
//        private readonly IWebHostEnvironment _env;
//        #endregion

//        #region Constructor
//        public RoomController(AppDbContext context, IWebHostEnvironment env)
//        {
//            _context = context;
//            _env = env;
//        }
//        #endregion

//        #region Index
//        public async Task<IActionResult> Index(int page = 1, int take = 5)
//        {
//            List<Blog> blogs = await _context.Blogs
//                .Where(m => !m.IsDeleted)
//                .Include(m => m.BlogImage)
//                .Skip((page * take) - take)
//                .Take(take)
//                .ToListAsync();

//            ViewBag.take = take;

//            List<BlogListVM> mapDatas = GetMapDatas(blogs);

//            int count = await GetPageCount(take);

//            Paginate<BlogListVM> result = new Paginate<BlogListVM>(mapDatas, page, count);

//            return View(result);
//        }
//        #endregion

//        #region Create
//        [HttpGet]
//        public async Task<IActionResult> Create()
//        {
//            ViewBag.categories = await GetCategoriesAsync();
//            var data = await GetTagAsync();

//            BlogCreateVM blogCreateVM = new BlogCreateVM()
//            {
//                Tag = data
//            };


//            return View(blogCreateVM);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create(BlogCreateVM blog)
//        {
//            ViewBag.categories = await GetCategoriesAsync();


//            if (!ModelState.IsValid)
//            {
//                return View(blog);
//            }

//            foreach (var photo in blog.Photos)
//            {
//                if (!photo.CheckFileType("image/"))
//                {
//                    ModelState.AddModelError("Photo", "Please choose correct image type");
//                    return View(blog);
//                }


//                if (!photo.CheckFileSize(50000))
//                {
//                    ModelState.AddModelError("Photo", "Please choose correct image size");
//                    return View(blog);
//                }

//            }

//            List<BlogImage> images = new List<BlogImage>();

//            foreach (var photo in blog.Photos)
//            {
//                string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;

//                string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/blog", fileName);

//                await Helper.SaveFile(path, photo);


//                BlogImage image = new BlogImage
//                {
//                    Image = fileName,
//                };

//                images.Add(image);
//            }

//            images.FirstOrDefault().IsMain = true;


//            Blog newBlog = new Blog
//            {
//                Title = blog.Title,
//                Content = blog.Content,
//                CreateDate = DateTime.Now,
//                BlogCategoryId = blog.CategoryId,
//                BlogImage = images,
//                Creator = User.Identity.Name

//            };

//            await _context.Blogs.AddAsync(newBlog);

//            await _context.SaveChangesAsync();


//            foreach (var item in blog.Tag.Where(m => m.IsSelected))
//            {
//                BlogTag blogTag = new BlogTag
//                {
//                    BlogId = newBlog.Id,
//                    TagId = item.Id,
//                };
//                await _context.BlogTags.AddAsync(blogTag);
//            }

//            _context.BlogImages.UpdateRange(images);
//            _context.Blogs.Update(newBlog);
//            await _context.SaveChangesAsync();

//            return RedirectToAction(nameof(Index));
//        }
//        #endregion

//        #region Delete
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Delete(int id)
//        {
//            Blog blog = await _context.Blogs
//                .Where(m => !m.IsDeleted && m.Id == id)
//                .Include(m => m.BlogImage)
//                .FirstOrDefaultAsync();

//            if (blog == null) return NotFound();

//            foreach (var item in blog.BlogImage)
//            {
//                string path = Helper.GetFilePath(_env.WebRootPath, "img", item.Image);
//                Helper.DeleteFile(path);
//                item.IsDeleted = true;
//            }

//            blog.IsDeleted = true;

//            await _context.SaveChangesAsync();

//            return RedirectToAction(nameof(Index));

//        }

//        #endregion

//        #region Detail
//        [HttpGet]
//        public async Task<IActionResult> Detail(int? id)
//        {
//            if (id == null)
//            {
//                return BadRequest();
//            }

//            Blog blog = await _context.Blogs
//                .Where(m => !m.IsDeleted && m.Id == id)
//                .Include(m => m.BlogImage)
//                .Include(m => m.BlogCategory)
//                .FirstOrDefaultAsync();

//            List<BlogTag> blogTags = await _context.BlogTags.Where(m => m.BlogId == id).ToListAsync();
//            List<Tag> tags = new List<Tag>();
//            foreach (var tag in blogTags)
//            {
//                Tag dbTag = await _context.Tags.Where(m => m.Id == tag.TagId).FirstOrDefaultAsync();
//                tags.Add(dbTag);
//            }

//            if (blog == null)
//            {
//                return NotFound();
//            }
//            var data = await GetTagAsync();

//            BlogDetailVM blogDetail = new BlogDetailVM
//            {
//                Title = blog.Title,
//                Content = blog.Content,
//                BlogImages = blog.BlogImage,
//                CategoryName = blog.BlogCategory.Name,
//                Tags = tags,
//                CreateDate = blog.CreateDate,
//                Creator = blog.Creator,
//            };




//            return View(blogDetail);
//        }
//        #endregion

//        #region Edit
//        [HttpGet]
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id is null) return BadRequest();

//            ViewBag.categories = await GetCategoriesAsync();

//            Blog dbBlog = await GetByIdAsync((int)id);

//            return View(new BlogEditVM
//            {
//                Title = dbBlog.Title,
//                Content = dbBlog.Content,
//                CategoryId = dbBlog.BlogCategoryId,
//                Images = dbBlog.BlogImage,
//            });
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, BlogEditVM updadetBlog)
//        {
//            ViewBag.categories = await GetCategoriesAsync();

//            if (!ModelState.IsValid) return View(updadetBlog);

//            Blog dbBlog = await GetByIdAsync(id);

//            if (updadetBlog.Photos != null)
//            {

//                foreach (var photo in updadetBlog.Photos)
//                {
//                    if (!photo.CheckFileType("image/"))
//                    {
//                        ModelState.AddModelError("Photo", "Please choose correct image type");
//                        return View(updadetBlog);
//                    }


//                    if (!photo.CheckFileSize(500))
//                    {
//                        ModelState.AddModelError("Photo", "Please choose correct image size");
//                        return View(updadetBlog);
//                    }

//                }

//                foreach (var item in dbBlog.BlogImage)
//                {
//                    string path = Helper.GetFilePath(_env.WebRootPath, "img", item.Image);
//                    Helper.DeleteFile(path);
//                }


//                List<BlogImage> images = new List<BlogImage>();

//                foreach (var photo in updadetBlog.Photos)
//                {

//                    string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;

//                    string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/blog", fileName);

//                    await Helper.SaveFile(path, photo);


//                    BlogImage image = new BlogImage
//                    {
//                        Image = fileName,
//                    };

//                    images.Add(image);

//                }

//                images.FirstOrDefault().IsMain = true;
//            }


//            dbBlog.Title = updadetBlog.Title;
//            dbBlog.Content = updadetBlog.Content;
//            dbBlog.BlogCategoryId = updadetBlog.CategoryId;

//            await _context.SaveChangesAsync();

//            return RedirectToAction(nameof(Index));
//        }

//        #endregion

//        #region Services
//        private async Task<SelectList> GetCategoriesAsync()
//        {
//            IEnumerable<BlogCategory> categories = await _context.BlogCategories.Where(m => !m.IsDeleted).ToListAsync();
//            return new SelectList(categories, "Id", "Name");
//        }
//        private async Task<List<Tag>> GetTagAsync()
//        {
//            List<Tag> tags = await _context.Tags.Where(m => !m.IsDeleted).ToListAsync();
//            return tags;
//        }

//        private async Task<Blog> GetByIdAsync(int id)
//        {
//            return await _context.Blogs
//                .Where(m => !m.IsDeleted && m.Id == id)
//                .Include(m => m.BlogCategory)
//                .Include(m => m.BlogImage)
//                .FirstOrDefaultAsync();
//        }

//        private List<BlogListVM> GetMapDatas(List<Blog> blogs)
//        {
//            List<BlogListVM> blogLists = new List<BlogListVM>();

//            foreach (var blog in blogs)
//            {
//                BlogListVM newBlog = new BlogListVM
//                {
//                    Id = blog.Id,
//                    Title = blog.Title,
//                    MainImage = blog.BlogImage.Where(m => m.IsMain).FirstOrDefault()?.Image,
//                    Creator = blog.Creator
//                };

//                blogLists.Add(newBlog);
//            }

//            return blogLists;
//        }

//        private async Task<int> GetPageCount(int take)
//        {
//            int blogCount = await _context.Blogs.Where(m => !m.IsDeleted).CountAsync();

//            return (int)Math.Ceiling((decimal)blogCount / take);
//        }
//        #endregion
//    }
//}
