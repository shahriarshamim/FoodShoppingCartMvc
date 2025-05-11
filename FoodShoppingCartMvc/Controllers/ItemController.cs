using FoodShoppingCartMvc.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static System.Reflection.Metadata.BlobBuilder;

namespace FoodShoppingCartMvc.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class ItemController : Controller
    {
        private readonly IItemRepository _itemRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IFileService _fileService;

        public ItemController(IItemRepository itemRepo, ICategoryRepository categoryRepo, IFileService fileService)
        {
            _itemRepo = itemRepo;
            _categoryRepo = categoryRepo;
            _fileService = fileService;
        }

        public async Task<IActionResult> Index(int page = 0)
        {
            var items = await _itemRepo.GetItems();
            //return View(items);
            const int PageSize = 3; // you can always do something more elegant to set this

            var count = items.Count();

            var data = items.Skip(page * PageSize).Take(PageSize).ToList();

            ViewBag.MaxPage = (count / PageSize) - (count % PageSize == 0 ? 1 : 0);

            ViewBag.Page = page;

            return View(data);
        }

        public async Task<IActionResult> AddItem()
        {
            var categorySelectList = (await _categoryRepo.GetCategory()).Select(category => new SelectListItem
            {
                Text = category.CategoryName,
                Value = category.Id.ToString(),
            });
            ItemDTO itemToAdd = new() { CategoryList = categorySelectList };
            return View(itemToAdd);
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(ItemDTO itemToAdd)
        {
            var categorySelectList = (await _categoryRepo.GetCategory()).Select(category => new SelectListItem
            {
                Text = category.CategoryName,
                Value = category.Id.ToString(),
            });
            itemToAdd.CategoryList = categorySelectList;

            if (!ModelState.IsValid)
                return View(itemToAdd);

            try
            {
                if (itemToAdd.ImageFile != null)
                {
                    if (itemToAdd.ImageFile.Length > 1 * 1024 * 1024)
                    {
                        throw new InvalidOperationException("Image file can not exceed 1 MB");
                    }
                    string[] allowedExtensions = [".jpeg", ".jpg", ".png"];
                    string imageName = await _fileService.SaveFile(itemToAdd.ImageFile, allowedExtensions);
                    itemToAdd.Image = imageName;
                }
                // manual mapping of BookDTO -> Book
                Item item = new()
                {
                    Id = itemToAdd.Id,
                    ItemName = itemToAdd.ItemName,
                    Weight = itemToAdd.Weight,
                    Image = itemToAdd.Image,
                    CategoryId = itemToAdd.CategoryId,
                    Price = itemToAdd.Price
                };
                await _itemRepo.AddItem(item);
                TempData["successMessage"] = "Item is added successfully";
                return RedirectToAction(nameof(AddItem));
            }
            catch (InvalidOperationException ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View(itemToAdd);
            }
            catch (FileNotFoundException ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View(itemToAdd);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Error on saving data";
                return View(itemToAdd);
            }
        }

        public async Task<IActionResult> UpdateItem(int id)
        {
            var item = await _itemRepo.GetItemById(id);
            if (item == null)
            {
                TempData["errorMessage"] = $"Item with the id: {id} does not found";
                return RedirectToAction(nameof(Index));
            }
            var categorySelectList = (await _categoryRepo.GetCategory()).Select(category => new SelectListItem
            {
                Text = category.CategoryName,
                Value = category.Id.ToString(),
                Selected = category.Id == item.CategoryId
            });
            ItemDTO itemToUpdate = new()
            {
                CategoryList = categorySelectList,
                ItemName = item.ItemName,
                Weight = item.Weight,
                CategoryId = item.CategoryId,
                Price = item.Price,
                Image = item.Image
            };
            return View(itemToUpdate);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateItem(ItemDTO itemToUpdate)
        {
            var categorySelectList = (await _categoryRepo.GetCategory()).Select(category => new SelectListItem
            {
                Text = category.CategoryName,
                Value = category.Id.ToString(),
                Selected = category.Id == itemToUpdate.CategoryId
            });
            itemToUpdate.CategoryList = categorySelectList;

            if (!ModelState.IsValid)
                return View(itemToUpdate);

            try
            {
                string oldImage = "";
                if (itemToUpdate.ImageFile != null)
                {
                    if (itemToUpdate.ImageFile.Length > 1 * 1024 * 1024)
                    {
                        throw new InvalidOperationException("Image file can not exceed 1 MB");
                    }
                    string[] allowedExtensions = [".jpeg", ".jpg", ".png"];
                    string imageName = await _fileService.SaveFile(itemToUpdate.ImageFile, allowedExtensions);
                    // hold the old image name. Because we will delete this image after updating the new
                    oldImage = itemToUpdate.Image;
                    itemToUpdate.Image = imageName;
                }
                // manual mapping of BookDTO -> Book
                Item item = new()
                {
                    Id = itemToUpdate.Id,
                    ItemName = itemToUpdate.ItemName,
                    Weight = itemToUpdate.Weight,
                    CategoryId = itemToUpdate.CategoryId,
                    Price = itemToUpdate.Price,
                    Image = itemToUpdate.Image
                };
                await _itemRepo.UpdateItem(item);
                // if image is updated, then delete it from the folder too
                if (!string.IsNullOrWhiteSpace(oldImage))
                {
                    _fileService.DeleteFile(oldImage);
                }
                TempData["successMessage"] = "Item is updated successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View(itemToUpdate);
            }
            catch (FileNotFoundException ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View(itemToUpdate);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Error on saving data";
                return View(itemToUpdate);
            }
        }

        public async Task<IActionResult> DeleteItem(int id)
        {
            try
            {
                var item = await _itemRepo.GetItemById(id);
                if (item == null)
                {
                    TempData["errorMessage"] = $"Item with the id: {id} does not found";
                }
                else
                {
                    await _itemRepo.DeleteItem(item);
                    if (!string.IsNullOrWhiteSpace(item.Image))
                    {
                        _fileService.DeleteFile(item.Image);
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            catch (FileNotFoundException ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Error on deleting the data";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
