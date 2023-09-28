using Interface.DTO;
using Interface.IRepository;
using Interface.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Implementation.Repository
{
    public class MenuManagementImp : IMenuManagement
    {
        private readonly RoyalFoodContext _context;
        private readonly ILogger<MenuManagementImp> _logger;
        public MenuManagementImp(RoyalFoodContext context, ILogger<MenuManagementImp> logger)
        {
            _context = context;
            _logger = logger;

        }

        #region Item
        public async Task<string> CreateItem(ItemDTO itemDTO)
        {
            try
            {
                var ingredient = itemDTO.MyIngredients.Select(x => new IngredientItem { IngredientId = x.ingredId, Quantity = x.qnt }).ToList();
                var itemimge = itemDTO.itemimgcuDTO.Select(x=> new ImageItem { Path =x.imagepaht, IsDefault = x.isdefault}).ToList();

                Item item = new()
                {
                    ItemName = itemDTO?.itemName,
                    ItemNameAr =itemDTO?.itemNameAr,
                    ItemDescribtion = itemDTO?.decreptionItem,
                    ItemDescriptionAr = itemDTO?.decreptionItemAr,
                    Availability = itemDTO?.availability,
                    Price = itemDTO?.price,
                    LastModificationDate = itemDTO?.lastmodification,
                    LastModifiedUserId = itemDTO?.lastusermodefied,
                    CategoryId = itemDTO?.categoryId,
                    IngredientItems = ingredient,
                    ImageItems = itemimge
                };
                await _context.AddAsync(item);
                _logger.LogTrace("Addedd Successfully");
                return "Addedd Successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Exception");
                return ex.Message;

            }

        }
        public async Task<string> EditeItem(int id, ItemDTO itemDTO)
        {
            try
            {
                var item = await _context.Items.SingleOrDefaultAsync(x => x.ItemId == id);
                if (item == null) { _logger.LogWarning("Empty Item"); return string.Empty; }
                item.ItemName = itemDTO?.itemName;
                item.ItemNameAr = itemDTO?.itemNameAr;
                item.ItemDescribtion = itemDTO?.decreptionItem;
                item.ItemDescriptionAr = itemDTO?.decreptionItemAr;
                item.Price = itemDTO?.price;
                item.Availability = itemDTO?.availability;
                item.CategoryId = itemDTO?.categoryId;
                item.LastModificationDate = itemDTO?.lastmodification;
                item.LastModifiedUserId = itemDTO?.lastusermodefied;
                var ingrdnt =await _context.IngredientItems.Where(x=>x.ItemId == id).FirstOrDefaultAsync();
                if (ingrdnt != null)
                {
                    var ingredient = itemDTO?.MyIngredients?.Select(x => new IngredientItem { IngredientId = x.ingredId, Quantity = x.qnt }).ToList();
                    item.IngredientItems = ingredient;
                    _context.UpdateRange(ingredient);
                    await _context.SaveChangesAsync();

                }
                var imgitm = await _context.ImageItems.Where(x => x.ItemId == id).FirstOrDefaultAsync();
                if (imgitm != null) 
                {
                    var itemimge = itemDTO.itemimgcuDTO.Select(x => new ImageItem { Path = x.imagepaht, IsDefault = x.isdefault }).ToList();
                    item.ImageItems = itemimge;
                    _context.UpdateRange(itemimge);
                    await _context.SaveChangesAsync();
                }
                _context.Update(item);
                _logger.LogInformation($"Updated Item id: {id}");
                return $"Updated Item id: {id}";
            }
            catch (Exception ex)
            {
                _logger.LogWarning("error");
                return ex.Message;

            }
        }

        public async Task<List<ItemGetDTO>> AllItems()
        {
            try
            {
                var items = await _context.Items.ToListAsync();
                if (items == null) { return null; }
                var ingrditem = await _context.IngredientItems.ToListAsync();
                var ingredient = await _context.Ingredients.ToListAsync();
                var result = from ing in ingredient
                             join ingit in ingrditem on ing.IngredientId equals ingit.IngredientId
                             join it in items on ingit.ItemId equals it.ItemId
                             select new ItemIngredGetDTO
                             {
                                 ingredientId = ing.IngredientId.ToString(),
                                 ingredientName = ing.Name,
                                 ingredientNameAr = ing.NameAr,
                                 unit = ing.Unit,
                                 qnty = ingit.Quantity.ToString()
                             };
                var imageitems = await _context.ImageItems.ToListAsync();
                var result1 = from img in imageitems
                              join it in items on img.ItemId equals it.ItemId
                              where img.ItemId == it.ItemId
                              select new ItemImageGetDTO 
                              {
                                  imgitmid = img.ImageItemId.ToString(),
                                  path = img.Path.ToString(),
                                  isdefault = img.IsDefault.ToString()
                              };
             
                List < ItemGetDTO > itemgetDTOs = new List<ItemGetDTO>();
                foreach ( var x in items ) 
                {
                    itemgetDTOs.Add(new ItemGetDTO
                    {
                        itemId = x.ItemId.ToString(),
                        itemName = x.ItemName,
                        itemNameAr = x.ItemNameAr,
                        decreptionItem = x.ItemDescribtion,
                        decreptionItemAr = x.ItemDescriptionAr,
                        category = _context.Categories.Where(y => y.CategoryId == x.CategoryId).First().CategoryName,
                        availability = x.Availability.ToString(),
                        price = x.Price.ToString(),
                        lastmodification = x.LastModificationDate.ToString(),
                        lastusermodefied = x.LastModifiedUserId.ToString(),
                        myingitget= result.ToList(),
                        myimagesitem = result1.ToList()

                    }) ;

                }
                _logger.LogInformation("List of Items");
                return itemgetDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogWarning("error");
                return null;
            }

        }

        public async Task<List<ItemGetDTO>> SortItems(int? id, string? itemname, string? itemnamear, string? description, string? descriptionAr, float? price)
        {
            try
            {
                if(id!= null)
                {
                    var items = await _context.Items.Where(x => x.ItemId == id).ToListAsync();
                    if (items == null) { return null; }
                    var ingrditem = await _context.IngredientItems.Where(x=>x.ItemId == id).ToListAsync();
                    var ingredient = await _context.Ingredients.Where(x => x.IngredientId == ingrditem.First().IngredientId).ToListAsync();
                    var result = from ing in ingredient
                                 join ingit in ingrditem on ing.IngredientId equals ingit.IngredientId
                                 join it in items on ingit.ItemId equals it.ItemId
                                 select new ItemIngredGetDTO
                                 {
                                     ingredientId = ing.IngredientId.ToString(),
                                     ingredientName = ing.Name,
                                     ingredientNameAr = ing.NameAr,
                                     unit = ing.Unit,
                                     qnty = ingit.Quantity.ToString()
                                 };
                    var imageitems = await _context.ImageItems.Where(x=>x.ItemId == id).ToListAsync();
                    var result1 = from img in imageitems
                                  join it in items on img.ItemId equals it.ItemId
                                  where img.ItemId == it.ItemId
                                  select new ItemImageGetDTO
                                  {
                                      imgitmid = img.ImageItemId.ToString(),
                                      path = img.Path.ToString(),
                                      isdefault = img.IsDefault.ToString()
                                  };

                    List<ItemGetDTO> itemgetDTOs = new List<ItemGetDTO>();
                    foreach (var x in items)
                    {
                        itemgetDTOs.Add(new ItemGetDTO
                        {
                            itemId = x.ItemId.ToString(),
                            itemName = x.ItemName,
                            itemNameAr = x.ItemNameAr,
                            decreptionItem = x.ItemDescribtion,
                            decreptionItemAr = x.ItemDescriptionAr,
                            category = _context.Categories.Where(y => y.CategoryId == x.CategoryId).First().CategoryName,
                            availability = x.Availability.ToString(),
                            price = x.Price.ToString(),
                            lastmodification = x.LastModificationDate.ToString(),
                            lastusermodefied = x.LastModifiedUserId.ToString(),
                            myingitget = result.ToList(),
                            myimagesitem = result1.ToList()

                        });

                    }
                    _logger.LogInformation("List of Items");
                    return itemgetDTOs;
                }
                if(itemname != null)
                {
                    var items = await _context.Items.Where(x => x.ItemName.Contains(itemname)).ToListAsync();
                    if (items == null) { return null; }
                    var ingrditem = await _context.IngredientItems.Where(x => x.ItemId == items.First().ItemId).ToListAsync();
                    var ingredient = await _context.Ingredients.Where(x=>x.IngredientId== ingrditem.First().IngredientId).ToListAsync();
                    var result = from ing in ingredient
                                 join ingit in ingrditem on ing.IngredientId equals ingit.IngredientId
                                 join it in items on ingit.ItemId equals it.ItemId
                                 select new ItemIngredGetDTO
                                 {
                                     ingredientId = ing.IngredientId.ToString(),
                                     ingredientName = ing.Name,
                                     ingredientNameAr = ing.NameAr,
                                     unit = ing.Unit,
                                     qnty = ingit.Quantity.ToString()
                                 };
                    var imageitems = await _context.ImageItems.Where(x => x.ItemId == items.First().ItemId).ToListAsync();
                    var result1 = from img in imageitems
                                  join it in items on img.ItemId equals it.ItemId
                                  where img.ItemId == it.ItemId
                                  select new ItemImageGetDTO
                                  {
                                      imgitmid = img.ImageItemId.ToString(),
                                      path = img.Path.ToString(),
                                      isdefault = img.IsDefault.ToString()
                                  };
                    List<ItemGetDTO> itemgetDTOs = new List<ItemGetDTO>();
                    foreach (var x in items)
                    {
                        itemgetDTOs.Add(new ItemGetDTO
                        {
                            itemId = x.ItemId.ToString(),
                            itemName = x.ItemName,
                            itemNameAr = x.ItemNameAr,
                            decreptionItem = x.ItemDescribtion,
                            decreptionItemAr = x.ItemDescriptionAr,
                            category = _context.Categories.Where(y => y.CategoryId == x.CategoryId).First().CategoryName,
                            availability = x.Availability.ToString(),
                            price = x.Price.ToString(),
                            lastmodification = x.LastModificationDate.ToString(),
                            lastusermodefied = x.LastModifiedUserId.ToString(),
                            myingitget = result.ToList(),
                            myimagesitem = result1.ToList()

                        });

                    }
                    _logger.LogInformation("List of Items");
                    return itemgetDTOs;
                }

                if (itemnamear != null)
                {
                    var items = await _context.Items.Where(x => x.ItemNameAr.Contains(itemnamear)).ToListAsync();
                    if (items == null) { return null; }
                    var ingrditem = await _context.IngredientItems.Where(x=>x.ItemId == items.First().ItemId).ToListAsync();
                    var ingredient = await _context.Ingredients.Where(x=>x.IngredientId == ingrditem.First().IngredientId).ToListAsync();
                    var result = from ing in ingredient
                                 join ingit in ingrditem on ing.IngredientId equals ingit.IngredientId
                                 join it in items on ingit.ItemId equals it.ItemId
                                 select new ItemIngredGetDTO
                                 {
                                     ingredientId = ing.IngredientId.ToString(),
                                     ingredientName = ing.Name,
                                     ingredientNameAr = ing.NameAr,
                                     unit = ing.Unit,
                                     qnty = ingit.Quantity.ToString()
                                 };
                    var imageitems = await _context.ImageItems.Where(x => x.ItemId == items.First().ItemId).ToListAsync();
                    var result1 = from img in imageitems
                                  join it in items on img.ItemId equals it.ItemId
                                  where img.ItemId == it.ItemId
                                  select new ItemImageGetDTO
                                  {
                                      imgitmid = img.ImageItemId.ToString(),
                                      path = img.Path.ToString(),
                                      isdefault = img.IsDefault.ToString()
                                  };

                    List<ItemGetDTO> itemgetDTOs = new List<ItemGetDTO>();
                    foreach (var x in items)
                    {
                        itemgetDTOs.Add(new ItemGetDTO
                        {
                            itemId = x.ItemId.ToString(),
                            itemName = x.ItemName,
                            itemNameAr = x.ItemNameAr,
                            decreptionItem = x.ItemDescribtion,
                            decreptionItemAr = x.ItemDescriptionAr,
                            category = _context.Categories.Where(y => y.CategoryId == x.CategoryId).First().CategoryName,
                            availability = x.Availability.ToString(),
                            price = x.Price.ToString(),
                            lastmodification = x.LastModificationDate.ToString(),
                            lastusermodefied = x.LastModifiedUserId.ToString(),
                            myingitget = result.ToList(),
                            myimagesitem = result1.ToList()

                        });

                    }
                    _logger.LogInformation("List of Items");
                    return itemgetDTOs;
                }
                if (description != null)
                {
                    var items = await _context.Items.Where(x => x.ItemDescribtion.Contains(description)).ToListAsync();
                    if (items == null) { return null; }
                    var ingrditem = await _context.IngredientItems.Where(x => x.ItemId == items.First().ItemId).ToListAsync();
                    var ingredient = await _context.Ingredients.Where(x => x.IngredientId == ingrditem.First().IngredientId).ToListAsync();
                    var result = from ing in ingredient
                                 join ingit in ingrditem on ing.IngredientId equals ingit.IngredientId
                                 join it in items on ingit.ItemId equals it.ItemId
                                 select new ItemIngredGetDTO
                                 {
                                     ingredientId = ing.IngredientId.ToString(),
                                     ingredientName = ing.Name,
                                     ingredientNameAr = ing.NameAr,
                                     unit = ing.Unit,
                                     qnty = ingit.Quantity.ToString()
                                 };
                    var imageitems = await _context.ImageItems.Where(x => x.ItemId == items.First().ItemId).ToListAsync();
                    var result1 = from img in imageitems
                                  join it in items on img.ItemId equals it.ItemId
                                  where img.ItemId == it.ItemId
                                  select new ItemImageGetDTO
                                  {
                                      imgitmid = img.ImageItemId.ToString(),
                                      path = img.Path.ToString(),
                                      isdefault = img.IsDefault.ToString()
                                  };

                    List<ItemGetDTO> itemgetDTOs = new List<ItemGetDTO>();
                    foreach (var x in items)
                    {
                        itemgetDTOs.Add(new ItemGetDTO
                        {
                            itemId = x.ItemId.ToString(),
                            itemName = x.ItemName,
                            itemNameAr = x.ItemNameAr,
                            decreptionItem = x.ItemDescribtion,
                            decreptionItemAr = x.ItemDescriptionAr,
                            category = _context.Categories.Where(y => y.CategoryId == x.CategoryId).First().CategoryName,
                            availability = x.Availability.ToString(),
                            price = x.Price.ToString(),
                            lastmodification = x.LastModificationDate.ToString(),
                            lastusermodefied = x.LastModifiedUserId.ToString(),
                            myingitget = result.ToList(),
                            myimagesitem = result1.ToList()

                        });

                    }
                    _logger.LogInformation("List of Items");
                    return itemgetDTOs;
                }
                if (descriptionAr != null)
                {
                    var items = await _context.Items.Where(x => x.ItemDescriptionAr.Contains(descriptionAr)).ToListAsync();
                    if (items == null) { return null; }
                    var ingrditem = await _context.IngredientItems.Where(x => x.ItemId == items.First().ItemId).ToListAsync();
                    var ingredient = await _context.Ingredients.Where(x=>x.IngredientId == ingrditem.First().IngredientId).ToListAsync();
                    var result = from ing in ingredient
                                 join ingit in ingrditem on ing.IngredientId equals ingit.IngredientId
                                 join it in items on ingit.ItemId equals it.ItemId
                                 select new ItemIngredGetDTO
                                 {
                                     ingredientId = ing.IngredientId.ToString(),
                                     ingredientName = ing.Name,
                                     ingredientNameAr = ing.NameAr,
                                     unit = ing.Unit,
                                     qnty = ingit.Quantity.ToString()
                                 };
                    var imageitems = await _context.ImageItems.Where(x => x.ItemId == items.First().ItemId).ToListAsync();
                    var result1 = from img in imageitems
                                  join it in items on img.ItemId equals it.ItemId
                                  where img.ItemId == it.ItemId
                                  select new ItemImageGetDTO
                                  {
                                      imgitmid = img.ImageItemId.ToString(),
                                      path = img.Path.ToString(),
                                      isdefault = img.IsDefault.ToString()
                                  };

                    List<ItemGetDTO> itemgetDTOs = new List<ItemGetDTO>();
                    foreach (var x in items)
                    {
                        itemgetDTOs.Add(new ItemGetDTO
                        {
                            itemId = x.ItemId.ToString(),
                            itemName = x.ItemName,
                            itemNameAr = x.ItemNameAr,
                            decreptionItem = x.ItemDescribtion,
                            decreptionItemAr = x.ItemDescriptionAr,
                            category = _context.Categories.Where(y => y.CategoryId == x.CategoryId).First().CategoryName,
                            availability = x.Availability.ToString(),
                            price = x.Price.ToString(),
                            lastmodification = x.LastModificationDate.ToString(),
                            lastusermodefied = x.LastModifiedUserId.ToString(),
                            myingitget = result.ToList(),
                            myimagesitem = result1.ToList()

                        });

                    }
                    _logger.LogInformation("List of Items");
                    return itemgetDTOs;
                }
                if (price != null)
                {
                    var items = await _context.Items.Where(x => x.Price ==price).ToListAsync();
                    if (items == null) { return null; }
                    var ingrditem = await _context.IngredientItems.Where(x=>x.ItemId == items.First().ItemId).ToListAsync();
                    var ingredient = await _context.Ingredients.ToListAsync();
                    var result = from ing in ingredient
                                 join ingit in ingrditem on ing.IngredientId equals ingit.IngredientId
                                 join it in items on ingit.ItemId equals it.ItemId
                                 select new ItemIngredGetDTO
                                 {
                                     ingredientId = ing.IngredientId.ToString(),
                                     ingredientName = ing.Name,
                                     ingredientNameAr = ing.NameAr,
                                     unit = ing.Unit,
                                     qnty = ingit.Quantity.ToString()
                                 };
                    var imageitems = await _context.ImageItems.Where(x => x.ItemId == items.First().ItemId).ToListAsync();
                    var result1 = from img in imageitems
                                  join it in items on img.ItemId equals it.ItemId
                                  where img.ItemId == it.ItemId
                                  select new ItemImageGetDTO
                                  {
                                      imgitmid = img.ImageItemId.ToString(),
                                      path = img.Path.ToString(),
                                      isdefault = img.IsDefault.ToString()
                                  };

                    List<ItemGetDTO> itemgetDTOs = new List<ItemGetDTO>();
                    foreach (var x in items)
                    {
                        itemgetDTOs.Add(new ItemGetDTO
                        {
                            itemId = x.ItemId.ToString(),
                            itemName = x.ItemName,
                            itemNameAr = x.ItemNameAr,
                            decreptionItem = x.ItemDescribtion,
                            decreptionItemAr = x.ItemDescriptionAr,
                            category = _context.Categories.Where(y => y.CategoryId == x.CategoryId).First().CategoryName,
                            availability = x.Availability.ToString(),
                            price = x.Price.ToString(),
                            lastmodification = x.LastModificationDate.ToString(),
                            lastusermodefied = x.LastModifiedUserId.ToString(),
                            myingitget = result.ToList(),
                            myimagesitem = result1.ToList()

                        });

                    }
                    _logger.LogInformation("List of Items");
                    return itemgetDTOs;
                }
                return null;

            }
            catch (Exception ex) 
            {
                _logger.LogDebug("Error");
                return null;

            }
        }
        #endregion


        #region Meal
        public async Task<string> CreateMeal(MealDTO mealDTO)
        {
            try
            {
                var ItemsinMeal = mealDTO.myitemmeal.Select(x => new ItemMeal { ItemId = x.itemid, Quantity = x.qunty }).ToList();
                var ImagesMeal = mealDTO.myimagesmeal.Select(x=> new ImageMeal { Path = x.path, IsDefualt=x.isdefault }).ToList();
                Meal meal = new()
                {
                    MealName = mealDTO.mealname,
                    MealNameAr = mealDTO.mealnameAr,
                    MealDescription = mealDTO.description,
                    MealDescriptionAr = mealDTO.descriptionAr,
                    CategoryId = mealDTO.categoryid,
                    Availability = mealDTO.availability,
                    Price = mealDTO.price,
                    LastModifiedUserId = mealDTO.lastusermodifiy,
                    LastModificationDate = mealDTO.lastmodificationdate,
                    ItemMeals = ItemsinMeal.ToList(),
                    ImageMeals = ImagesMeal.ToList(),
                };
                await _context.AddAsync(meal);
                _logger.LogInformation("Added Meal Succesfully");
                return "Added Meal Succesfully";

            }
            catch (Exception ex)
            {
                _logger.LogError("Error");
                return ex.Message;

            }
        }
        public async Task<string> EditeMeal(int id, MealDTO mealDTO)
        {
            try
            {
                var meals = await _context.Meals.Where(x=>x.MealId == id).SingleOrDefaultAsync();
                if (meals == null) { return "Empty Meal or incorrect id"; }
                meals.MealName = mealDTO.mealname;
                meals.MealNameAr = mealDTO.mealnameAr;
                meals.MealDescription = mealDTO.description;
                meals.MealDescriptionAr =mealDTO.descriptionAr;
                meals.CategoryId = mealDTO.categoryid;
                meals.Availability = mealDTO.availability;
                meals.LastModificationDate = mealDTO.lastmodificationdate;
                meals.LastModifiedUserId = mealDTO.lastusermodifiy;
                meals.Price = mealDTO.price;
                var itemmeal = await _context.ItemMeals.Where(x=>x.ItemMealId == id).SingleOrDefaultAsync();
                if(itemmeal != null)
                {
                    var ItemsinMeal = mealDTO.myitemmeal.Select(x => new ItemMeal { ItemId = x.itemid, Quantity = x.qunty }).ToList();
                    meals.ItemMeals = ItemsinMeal;
                    _context.Update(ItemsinMeal);
                    await _context.SaveChangesAsync();
                }
                var imagemeal = await _context.ImageMeals.Where(x => x.MealId == id).SingleOrDefaultAsync();
                if (imagemeal != null) 
                {
                    var ImagesMeal = mealDTO.myimagesmeal.Select(x => new ImageMeal { Path = x.path, IsDefualt = x.isdefault }).ToList();
                    meals.ImageMeals = ImagesMeal;
                    _context.Update(ImagesMeal);
                    await _context.SaveChangesAsync();
                }
                //Meal meal = new()
                //{
                //    MealName = mealDTO.mealname,
                //    MealNameAr = mealDTO.mealnameAr,
                //    MealDescription = mealDTO.description,
                //    MealDescriptionAr = mealDTO.descriptionAr,
                //    CategoryId = mealDTO.categoryid,
                //    Availability = mealDTO.availability,
                //    Price = mealDTO.price,
                //    LastModifiedUserId = mealDTO.lastusermodifiy,
                //    LastModificationDate = mealDTO.lastmodificationdate,
                //    ItemMeals = ItemsinMeal.ToList(),
                //    ImageMeals = ImagesMeal.ToList(),
                //};
                _context.Update(meals);
                _logger.LogInformation($"Updated Meal Succesfully id {id}");
                return $"Updated Meal Succesfully id {id}";

            }
            catch (Exception ex) 
            {
                _logger.LogError("Error");
                return ex.Message;

            }
        }
        public async Task<List<MealGet>> AllMeals()
        {
            try
            {
                var allmeals = await _context.Meals.ToListAsync();
                if (allmeals is null) { _logger.LogDebug("Error"); return null; }
                var itemmeal = await _context.ItemMeals.ToListAsync();
                var items = await _context.Items.ToListAsync();
                var ingredients = await _context.Ingredients.ToListAsync();
                var ingrditem = await _context.IngredientItems.ToListAsync();
                var result = from ing in ingredients
                             join ingit in ingrditem on ing.IngredientId equals ingit.IngredientId
                             join it in items on ingit.ItemId equals it.ItemId
                             join mlit in itemmeal on it.ItemId equals mlit.ItemId
                             join ml in allmeals on mlit.MealId equals ml.MealId
                             select new MealItemGetDTO
                             {
                                 unit = ing.Unit,
                                 itemname = it.ItemName,
                                 itemmealar = it.ItemNameAr,
                                 qnty = mlit.Quantity.ToString(),
                             };
                var imagemeal = await _context.ImageMeals.ToListAsync();

                var result1 = from iimgml in imagemeal
                              join ml in allmeals on iimgml.MealId equals ml.MealId
                              select new ImagesMealGetDTO
                              {
                                  imageid = iimgml.ImageMealId.ToString(),
                                  imagepath = iimgml.Path,
                                  isdefault = iimgml.IsDefualt.ToString()
                                  
                              };
                List < MealGet > mealget = new List<MealGet>();
                foreach (var m in allmeals)
                {
                    mealget.Add(new MealGet
                    {
                        mealid = m.MealId.ToString(),
                        name = m.MealName,
                        nameAr = m.MealNameAr,
                        description = m.MealDescription,
                        descriptionAr = m.MealDescriptionAr,
                        category = _context.Categories.Where(x => x.CategoryId == m.CategoryId).First().CategoryName,
                        price = m.Price.ToString(),
                        availability = m.Availability.ToString(),
                        lastmodificationdate = m.LastModificationDate.ToString(),
                        lastuseridmodified = m.LastModifiedUserId.ToString(),
                        Mymealitem = result.ToList(),
                        myimagesmeal = result1.ToList()
                    });

                };
              _logger.LogInformation("List of Meals");
                return mealget;
            }
            catch (Exception ex) { _logger.LogError("Error"); return new List<MealGet>(); }

        }
        public async Task<List<MealGet>> SortungMeal(int? id, string? namear, string? nameing, string? descar, string? descing, float? price)
        {
            try
            {
                var meals = await _context.Meals.ToListAsync();
                if (id != null)
                {
                    meals = await _context.Meals.Where(x => x.MealId.Equals(id)).ToListAsync();
                    var itemmeal = await _context.ItemMeals.Where(x => x.MealId == id).ToListAsync();
                    var items = await _context.Items.ToListAsync();
                    var ingredients = await _context.Ingredients.ToListAsync();
                    var ingrditem = await _context.IngredientItems.ToListAsync();
                    var result = from ing in ingredients
                                 join ingit in ingrditem on ing.IngredientId equals ingit.IngredientId
                                 join it in items on ingit.ItemId equals it.ItemId
                                 join mlit in itemmeal on it.ItemId equals mlit.ItemId
                                 join ml in meals on mlit.MealId equals ml.MealId
                                 select new MealItemGetDTO
                                 {
                                     unit = ing.Unit,
                                     itemname = it.ItemName,
                                     itemmealar = it.ItemNameAr,
                                     qnty = mlit.Quantity.ToString(),
                                 };
                    var imagemeal = await _context.ImageMeals.Where(x => x.MealId == id).ToListAsync();

                    var result1 = from iimgml in imagemeal
                                  join ml in meals on iimgml.MealId equals ml.MealId
                                  select new ImagesMealGetDTO
                                  {
                                      imageid = iimgml.ImageMealId.ToString(),
                                      imagepath = iimgml.Path,
                                      isdefault = iimgml.IsDefualt.ToString()

                                  };
                    List<MealGet> mealget = new List<MealGet>();
                    foreach (var m in meals)
                    {
                        mealget.Add(new MealGet
                        {
                            mealid = m.MealId.ToString(),
                            name = m.MealName,
                            nameAr = m.MealNameAr,
                            description = m.MealDescription,
                            descriptionAr = m.MealDescriptionAr,
                            category = _context.Categories.Where(x => x.CategoryId == m.CategoryId).First().CategoryName,
                            price = m.Price.ToString(),
                            availability = m.Availability.ToString(),
                            lastmodificationdate = m.LastModificationDate.ToString(),
                            lastuseridmodified = m.LastModifiedUserId.ToString(),
                            Mymealitem = result.ToList(),
                            myimagesmeal = result1.ToList()
                        });

                    }
                    _logger.LogInformation("Show Results");
                    return mealget;
                }
                if (namear != null)
                {
                    meals = await _context.Meals.Where(x => x.MealNameAr.Contains(namear)).ToListAsync();
                    var itemmeal = await _context.ItemMeals.Where(x => x.MealId == meals.First().MealId).ToListAsync();
                    var items = await _context.Items.Where(x => x.ItemId == itemmeal.First().ItemId).ToListAsync();
                    var ingrditem = await _context.IngredientItems.Where(x => x.ItemId == items.First().ItemId).ToListAsync();
                    var ingredients = await _context.Ingredients.Where(x => x.IngredientId == ingrditem.First().IngredientId).ToListAsync();
                    var result = from ing in ingredients
                                 join ingit in ingrditem on ing.IngredientId equals ingit.IngredientId
                                 join it in items on ingit.ItemId equals it.ItemId
                                 join mlit in itemmeal on it.ItemId equals mlit.ItemId
                                 join ml in meals on mlit.MealId equals ml.MealId
                                 select new MealItemGetDTO
                                 {
                                     unit = ing.Unit,
                                     itemname = it.ItemName,
                                     itemmealar = it.ItemNameAr,
                                     qnty = mlit.Quantity.ToString(),
                                 };
                    var imagemeal = await _context.ImageMeals.Where(x => x.MealId == meals.First().MealId).ToListAsync();

                    var result1 = from iimgml in imagemeal
                                  join ml in meals on iimgml.MealId equals ml.MealId
                                  select new ImagesMealGetDTO
                                  {
                                      imageid = iimgml.ImageMealId.ToString(),
                                      imagepath = iimgml.Path,
                                      isdefault = iimgml.IsDefualt.ToString()

                                  };
                    List<MealGet> mealget = new List<MealGet>();
                    foreach (var m in meals)
                    {
                        mealget.Add(new MealGet
                        {
                            mealid = m.MealId.ToString(),
                            name = m.MealName,
                            nameAr = m.MealNameAr,
                            description = m.MealDescription,
                            descriptionAr = m.MealDescriptionAr,
                            category = _context.Categories.Where(x => x.CategoryId == m.CategoryId).First().CategoryName,
                            price = m.Price.ToString(),
                            availability = m.Availability.ToString(),
                            lastmodificationdate = m.LastModificationDate.ToString(),
                            lastuseridmodified = m.LastModifiedUserId.ToString(),
                            Mymealitem = result.ToList(),
                            myimagesmeal = result1.ToList()
                        });

                    }
                    _logger.LogInformation("Show Results");
                    return mealget;
                }
                if (nameing != null)
                {
                    meals = await _context.Meals.Where(x => x.MealName.Contains(nameing)).ToListAsync();
                    var itemmeal = await _context.ItemMeals.Where(x => x.MealId == meals.First().MealId).ToListAsync();
                    var items = await _context.Items.Where(x => x.ItemId == itemmeal.First().ItemId).ToListAsync();
                    var ingrditem = await _context.IngredientItems.Where(x => x.ItemId == items.First().ItemId).ToListAsync();
                    var ingredients = await _context.Ingredients.Where(x => x.IngredientId == ingrditem.First().IngredientId).ToListAsync();
                    var result = from ing in ingredients
                                 join ingit in ingrditem on ing.IngredientId equals ingit.IngredientId
                                 join it in items on ingit.ItemId equals it.ItemId
                                 join mlit in itemmeal on it.ItemId equals mlit.ItemId
                                 join ml in meals on mlit.MealId equals ml.MealId
                                 select new MealItemGetDTO
                                 {
                                     unit = ing.Unit,
                                     itemname = it.ItemName,
                                     itemmealar = it.ItemNameAr,
                                     qnty = mlit.Quantity.ToString(),
                                 };
                    var imagemeal = await _context.ImageMeals.Where(x => x.MealId == meals.First().MealId).ToListAsync();

                    var result1 = from iimgml in imagemeal
                                  join ml in meals on iimgml.MealId equals ml.MealId
                                  select new ImagesMealGetDTO
                                  {
                                      imageid = iimgml.ImageMealId.ToString(),
                                      imagepath = iimgml.Path,
                                      isdefault = iimgml.IsDefualt.ToString()

                                  };
                    List<MealGet> mealget = new List<MealGet>();
                    foreach (var m in meals)
                    {
                        mealget.Add(new MealGet
                        {
                            mealid = m.MealId.ToString(),
                            name = m.MealName,
                            nameAr = m.MealNameAr,
                            description = m.MealDescription,
                            descriptionAr = m.MealDescriptionAr,
                            category = _context.Categories.Where(x => x.CategoryId == m.CategoryId).First().CategoryName,
                            price = m.Price.ToString(),
                            availability = m.Availability.ToString(),
                            lastmodificationdate = m.LastModificationDate.ToString(),
                            lastuseridmodified = m.LastModifiedUserId.ToString(),
                            Mymealitem = result.ToList(),
                            myimagesmeal = result1.ToList()
                        });

                    }
                    _logger.LogInformation("Show Results");
                    return mealget;
                }
                if (descing != null)
                {
                    meals = await _context.Meals.Where(x => x.MealDescription.Contains(descing)).ToListAsync();
                    var itemmeal = await _context.ItemMeals.Where(x => x.MealId == meals.First().MealId).ToListAsync();
                    var items = await _context.Items.Where(x => x.ItemId == itemmeal.First().ItemId).ToListAsync();
                    var ingrditem = await _context.IngredientItems.Where(x => x.ItemId == items.First().ItemId).ToListAsync();
                    var ingredients = await _context.Ingredients.Where(x => x.IngredientId == ingrditem.First().IngredientId).ToListAsync();
                    var result = from ing in ingredients
                                 join ingit in ingrditem on ing.IngredientId equals ingit.IngredientId
                                 join it in items on ingit.ItemId equals it.ItemId
                                 join mlit in itemmeal on it.ItemId equals mlit.ItemId
                                 join ml in meals on mlit.MealId equals ml.MealId
                                 select new MealItemGetDTO
                                 {
                                     unit = ing.Unit,
                                     itemname = it.ItemName,
                                     itemmealar = it.ItemNameAr,
                                     qnty = mlit.Quantity.ToString(),
                                 };
                    var imagemeal = await _context.ImageMeals.Where(x => x.MealId == meals.First().MealId).ToListAsync();

                    var result1 = from iimgml in imagemeal
                                  join ml in meals on iimgml.MealId equals ml.MealId
                                  select new ImagesMealGetDTO
                                  {
                                      imageid = iimgml.ImageMealId.ToString(),
                                      imagepath = iimgml.Path,
                                      isdefault = iimgml.IsDefualt.ToString()

                                  };
                    List<MealGet> mealget = new List<MealGet>();
                    foreach (var m in meals)
                    {
                        mealget.Add(new MealGet
                        {
                            mealid = m.MealId.ToString(),
                            name = m.MealName,
                            nameAr = m.MealNameAr,
                            description = m.MealDescription,
                            descriptionAr = m.MealDescriptionAr,
                            category = _context.Categories.Where(x => x.CategoryId == m.CategoryId).First().CategoryName,
                            price = m.Price.ToString(),
                            availability = m.Availability.ToString(),
                            lastmodificationdate = m.LastModificationDate.ToString(),
                            lastuseridmodified = m.LastModifiedUserId.ToString(),
                            Mymealitem = result.ToList(),
                            myimagesmeal = result1.ToList()
                        });

                    }
                    _logger.LogInformation("Show Results");
                    return mealget;
                }
                if (descar != null)
                {
                    meals = await _context.Meals.Where(x=>x.MealDescriptionAr.Contains(descar)).ToListAsync();
                    var itemmeal = await _context.ItemMeals.Where(x => x.MealId == meals.First().MealId).ToListAsync();
                    var items = await _context.Items.Where(x => x.ItemId == itemmeal.First().ItemId).ToListAsync();
                    var ingrditem = await _context.IngredientItems.Where(x => x.ItemId == items.First().ItemId).ToListAsync();
                    var ingredients = await _context.Ingredients.Where(x => x.IngredientId == ingrditem.First().IngredientId).ToListAsync();
                    var result = from ing in ingredients
                                 join ingit in ingrditem on ing.IngredientId equals ingit.IngredientId
                                 join it in items on ingit.ItemId equals it.ItemId
                                 join mlit in itemmeal on it.ItemId equals mlit.ItemId
                                 join ml in meals on mlit.MealId equals ml.MealId
                                 select new MealItemGetDTO
                                 {
                                     unit = ing.Unit,
                                     itemname = it.ItemName,
                                     itemmealar = it.ItemNameAr,
                                     qnty = mlit.Quantity.ToString(),
                                 };
                    var imagemeal = await _context.ImageMeals.Where(x => x.MealId == meals.First().MealId).ToListAsync();

                    var result1 = from iimgml in imagemeal
                                  join ml in meals on iimgml.MealId equals ml.MealId
                                  select new ImagesMealGetDTO
                                  {
                                      imageid = iimgml.ImageMealId.ToString(),
                                      imagepath = iimgml.Path,
                                      isdefault = iimgml.IsDefualt.ToString()

                                  };
                    List<MealGet> mealget = new List<MealGet>();
                    foreach (var m in meals)
                    {
                        mealget.Add(new MealGet
                        {
                            mealid = m.MealId.ToString(),
                            name = m.MealName,
                            nameAr = m.MealNameAr,
                            description = m.MealDescription,
                            descriptionAr = m.MealDescriptionAr,
                            category = _context.Categories.Where(x => x.CategoryId == m.CategoryId).First().CategoryName,
                            price = m.Price.ToString(),
                            availability = m.Availability.ToString(),
                            lastmodificationdate = m.LastModificationDate.ToString(),
                            lastuseridmodified = m.LastModifiedUserId.ToString(),
                            Mymealitem = result.ToList(),
                            myimagesmeal = result1.ToList()
                        });

                    }
                    _logger.LogInformation("Show Results");
                    return mealget;
                }
                if (price != null)
                {
                    meals = await _context.Meals.Where(x => x.Price == price).ToListAsync();
                    var itemmeal = await _context.ItemMeals.Where(x => x.MealId == meals.First().MealId).ToListAsync();
                    var items = await _context.Items.Where(x => x.ItemId == itemmeal.First().ItemId).ToListAsync();
                    var ingrditem = await _context.IngredientItems.Where(x => x.ItemId == items.First().ItemId).ToListAsync();
                    var ingredients = await _context.Ingredients.Where(x => x.IngredientId == ingrditem.First().IngredientId).ToListAsync();
                    var result = from ing in ingredients
                                 join ingit in ingrditem on ing.IngredientId equals ingit.IngredientId
                                 join it in items on ingit.ItemId equals it.ItemId
                                 join mlit in itemmeal on it.ItemId equals mlit.ItemId
                                 join ml in meals on mlit.MealId equals ml.MealId
                                 select new MealItemGetDTO
                                 {
                                     unit = ing.Unit,
                                     itemname = it.ItemName,
                                     itemmealar = it.ItemNameAr,
                                     qnty = mlit.Quantity.ToString(),
                                 };
                    var imagemeal = await _context.ImageMeals.Where(x => x.MealId == meals.First().MealId).ToListAsync();

                    var result1 = from iimgml in imagemeal
                                  join ml in meals on iimgml.MealId equals ml.MealId
                                  select new ImagesMealGetDTO
                                  {
                                      imageid = iimgml.ImageMealId.ToString(),
                                      imagepath = iimgml.Path,
                                      isdefault = iimgml.IsDefualt.ToString()

                                  };
                    List<MealGet> mealget = new List<MealGet>();
                    foreach (var m in meals)
                    {
                        mealget.Add(new MealGet
                        {
                            mealid = m.MealId.ToString(),
                            name = m.MealName,
                            nameAr = m.MealNameAr,
                            description = m.MealDescription,
                            descriptionAr = m.MealDescriptionAr,
                            category = _context.Categories.Where(x => x.CategoryId == m.CategoryId).First().CategoryName,
                            price = m.Price.ToString(),
                            availability = m.Availability.ToString(),
                            lastmodificationdate = m.LastModificationDate.ToString(),
                            lastuseridmodified = m.LastModifiedUserId.ToString(),
                            Mymealitem = result.ToList(),
                            myimagesmeal = result1.ToList()
                        });

                    }
                    _logger.LogInformation("Show Results");
                    return mealget;
                }
                else
                {
                    _logger.LogError("No Results");
                    return null;
                }
            }
            catch (Exception ex) { _logger.LogError("Error"); return null; }
        }

        #endregion



    }

}

 