using Interface.DTO;
using Interface.Helper;
using Interface.IRepository;
using Interface.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Implementation.Repository
{
    public class ConfigurationsManagementImp : IConfigurationsManagement
    {
        private readonly RoyalFoodContext _context;
        private readonly ILogger<ConfigurationsManagementImp> _logger;
        private readonly Helper helper;

        public ConfigurationsManagementImp(RoyalFoodContext context,ILogger<ConfigurationsManagementImp> logger, Helper _helper) 
        { 
            _context = context;
            _logger = logger;
            this.helper = _helper;
        }

        #region Role

        public async Task<List<Role>> AllRoles()
        {
            try
            {
                var RolesAll = await _context.Roles.ToListAsync();
                _logger.LogInformation("We Get All Roles");

                return RolesAll;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return null;

            }

        }
        public async Task<string> AddRole(RoleDTO roleDTO)
        {
            try
            {
                if (await _context.Roles.AnyAsync(x => x.RoleName == roleDTO.name))
                {
                    _logger.LogInformation("Done");

                    return $"Name Role is Already Exsist{roleDTO.name}";


                }
                else
                {
                    Role role = new()
                    {
                        RoleName = roleDTO.name,
                        Permissions = roleDTO.permission

                    };
                    await _context.AddAsync(role);
                    return "Created Role";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");

                return null;

            }
        }

        public async Task<string> EditeRole(int id, RoleDTO roleDTO)
        {
            try
            {
                var editeRole = await _context.Roles.SingleOrDefaultAsync(x => x.RoleId == id);
                if (editeRole != null)
                {
                    editeRole.RoleName = roleDTO.name;
                    editeRole.Permissions = roleDTO.permission;
                    _context.Update(editeRole);
                    _logger.LogInformation($"Updated on Role id: {id}");
                    return "Updated Successfully";
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message);
                return ex.Message;

            }
        }
        public async Task<List<Role>> SortRole(int? id, string? Name, string? permission)
        {
            var SortingRole = await _context.Roles.ToListAsync();
            try
            {
                if (SortingRole == null)
                {
                    return null;
                }
                else
                {
                    if (id != null)
                    {
                        SortingRole = await _context.Roles.Where(x => x.RoleId == id).ToListAsync();
                    }
                    if (Name != null)
                    {
                        SortingRole = await _context.Roles.Where(x => x.RoleName.Contains(Name)).ToListAsync();

                    }
                    if (permission != null)
                    {
                        SortingRole = await _context.Roles.Where(x => x.Permissions.Contains(permission)).ToListAsync();

                    }
                    _logger.LogInformation("Filterring Roles");

                    return SortingRole;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return null;
            }


        }

        #endregion


        #region UserAndAuthantication
        public async Task<List<User>> AllUsers()
        {
            try
            {

                var Allusers = await _context.Users.ToListAsync();
                _logger.LogInformation("Get All Users");
                return Allusers;


            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get Exception{ex.Message}");

                return null;

            }

        }

        public async Task<string> AddUser(RegisterDTO registerDTO)
        {
            try
            {
                if (await _context.Logins.AnyAsync(x => x.Email == registerDTO.email))
                {
                    _logger.LogInformation($" Email is Alredy Registered : {registerDTO.email}");
                    return "Email is Already Registered";
                }
                else
                {
                    User user = new()
                    {
                        FirstName = registerDTO.Fname,
                        LastName = registerDTO.Lname,
                        Phone = registerDTO.phone,
                        Age = registerDTO.age,
                        Address = registerDTO.Addrss,
                        Gender = registerDTO.gender,
                        RoleId = registerDTO.roleid,
                        ProfileImage = registerDTO.profileimage
                    };

                    if (user.RoleId == 0 || user.RoleId is null)
                    {
                        user.RoleId = 7;
                    }

                    await _context.AddAsync(user);
                    await _context.SaveChangesAsync();

                    Login login = new()
                    {
                        Email = registerDTO.email,
                        Password = helper.GenerateSHA384String(registerDTO.password),
                        IsActive = true,
                        UserId = user.UserId,
                        LoginDate = DateTime.UtcNow

                    };
                    await _context.AddAsync(login);
                    Customer customer = new()
                    {
                        UserName = registerDTO.username,
                        UserId = user.UserId

                    };
                    await _context.AddAsync(customer);

                    _logger.LogInformation("Added All register Requirments");
                    return "Created New User";

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ex.Message;

            }
        }
        public async Task<string> Signin(LoginDTO loginDTO)
        {
            var loginuser = await _context.Logins.FirstOrDefaultAsync(x => x.Email == loginDTO.email && x.Password == helper.GenerateSHA384String(loginDTO.password));
            if (loginuser == null)
            {
                return null;
            }
            else
            {
                if (await _context.Logins.AnyAsync(x => x.Email == loginDTO.email && x.Password == helper.GenerateSHA384String(loginDTO.password)))
                {
                    Login login = new()
                    {
                        Email = loginDTO.email,
                        Password = helper.GenerateSHA384String(loginDTO.password),
                        IsActive = true,
                        LoginDate = DateTime.UtcNow.Date

                    };


                    User user = _context.Users.Include(x => x.Logins).Where(x => x.Logins.Any(login => login.Email == loginDTO.email)).FirstOrDefault();
                    if (user is null) return "User Not Found";
                    Role role = _context.Roles.Where(role => role.RoleId == user.RoleId).FirstOrDefault();
                    if (role is null) return "Role Not Found";
                    string roleName = role.RoleName;
                    string token = helper.GenerateJwtToken(loginDTO.email, roleName, loginuser.UserId ?? 0, true);
                    _context.Update(login);
                    _logger.LogInformation("Return Token" + token);
                    return token;
                }
                else
                {
                    _logger.LogError("Null Value");
                    return null;

                }

            }
        }

        public async Task<string> UpdatePassword(ResetPasswordDTO resetPasswordDTO)
        {
            try
            {
                var respass = await _context.Logins.FirstOrDefaultAsync(x => x.Password == helper.GenerateSHA384String(resetPasswordDTO.Currentpassword) && x.Email == resetPasswordDTO.email);
                if (respass is null) return null;

                if (await _context.Logins.AnyAsync(x => x.Password == helper.GenerateSHA384String(resetPasswordDTO.Currentpassword) == true))
                {
                    if (helper.GenerateSHA384String(resetPasswordDTO.NewPassword) == helper.GenerateSHA384String(resetPasswordDTO.ConfirmPassword))
                    {
                        Login login = new Login();
                        login.Password = helper.GenerateSHA384String(resetPasswordDTO.NewPassword);
                        _context.Update(login);
                        return "Successfuly Reset Password";
                    }
                    else
                    {
                        return "Incorrect Password";
                    }


                }
                else
                {
                    return "Incorrect Password or Email";
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return ex.Message;

            }

        }
        public async Task<string> PutNewPaaword(ForgetPasswordDTO forgetPasswordDTO)
        {
            try
            {
                if (await _context.Logins.AnyAsync(x => x.Email == forgetPasswordDTO.email) && await _context.Logins.AnyAsync(x => x.Email != null))
                {
                    if (helper.GenerateSHA384String(forgetPasswordDTO.NewPassword) == helper.GenerateSHA384String(forgetPasswordDTO.NewPassword))
                    {
                        Login login = new Login();
                        login.Password = helper.GenerateSHA384String(forgetPasswordDTO.NewPassword);
                        _context.Update(login);
                        _logger.LogInformation(" Successfully Update the Password");
                        return " Successfully Update the Password";
                    }
                    else
                    {
                        _logger.LogInformation("Error Email or Null");
                        return "Error Email or Null";
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message} ");
                return ex.Message;

            }

        }

        public async Task<List<User>> SortUsers(int? roleid, string? fname, string? lastname, string? address)
        {
            try
            {
                var SortUsers = await _context.Users.ToListAsync();
                if (fname != null)
                {
                    SortUsers = SortUsers.Where(x => x.FirstName.Contains(fname)).ToList();
                    return SortUsers;
                }
                if (lastname != null)
                {
                    SortUsers = SortUsers.Where(x => x.LastName.Contains(lastname)).ToList();
                    return SortUsers;
                }
                if (address != null)
                {
                    SortUsers = SortUsers.Where(x => x.Address.Contains(address)).ToList();
                    return SortUsers;
                }
                if (roleid != null)
                {
                    SortUsers = SortUsers.Where(x => x.RoleId == roleid).ToList();
                    return SortUsers;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //Logout Not Completetd

        public async Task<string> Logount(string token)
        {
            try
            {


                LogoutResponse response = new LogoutResponse();
                if (helper.ValidateJWTtoken(token, out response))
                {
                    if (response.loginid != null)
                    {
                        var logout = await _context.Logins.Where(x => x.LoginId == response.loginid).SingleOrDefaultAsync();
                        if (logout != null)
                        {
                            if (logout.IsActive == true)
                            {
                                logout.IsActive = false;
                                _context.Update(logout);
                                _logger.LogInformation("Logout Susseccefully");
                                return "Successfull Logout";

                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogDebug("error");
                return ex.Message;
            }
        }

        #endregion


        #region Ingredient
        public async Task<string> CreateIngredient(IngredientDTO ingredientDTO)
        {
            try
            {
                if (ingredientDTO != null)
                {

                    Ingredient ingredient = new()
                    {
                        Name = ingredientDTO.ingName,
                        NameAr = ingredientDTO.ingNameAr,
                        Describtion = ingredientDTO.ingDescription,
                        DescribtionAr = ingredientDTO.ingDescriptionAr,
                        Unit = ingredientDTO.unit,
                        IsActive = ingredientDTO.isactive,
                        ImageId = ingredientDTO.imageId

                    };
                    await _context.AddAsync(ingredient);
                    _logger.LogInformation("Added Ingredient");
                    return "Added Ingredient Successfully";
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ex.Message;

            }
        }
        public async Task<string> EditeIngredient(int id, IngredientDTO ingredientDTO)
        {
            try
            {
                var EditeIngred = await _context.Ingredients.FirstOrDefaultAsync(x => x.IngredientId == id);
                if (EditeIngred != null)
                {
                    EditeIngred.Name = ingredientDTO.ingName;
                    EditeIngred.NameAr = ingredientDTO.ingNameAr;
                    EditeIngred.Describtion = ingredientDTO.ingDescription;
                    EditeIngred.DescribtionAr = ingredientDTO.ingDescriptionAr;
                    EditeIngred.IsActive = ingredientDTO.isactive;
                    EditeIngred.ImageId = ingredientDTO.imageId;

                    _context.Update(EditeIngred);
                    _logger.LogInformation("Updating Ingredient");
                    return $"Updating Ingredient id: {id}";

                }
                _logger.LogInformation($"Null Id: {id}");
                return $"Null Id: {id}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ex.Message;
            }

        }

        public async Task<List<Ingredient>> ViewIngredients()
        {
            try
            {
                var AllIngredients = await _context.Ingredients.ToListAsync();
                if (AllIngredients != null) return AllIngredients;
                _logger.LogInformation($"ViewIngredients{AllIngredients}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }

        }
        public async Task<List<Ingredient>> SortIngredient(int? id, string ingName, string? ingNameAr)
        {
            try
            {
                var SortIngred = await _context.Ingredients.ToListAsync();
                if (id != null) { SortIngred = await _context.Ingredients.Where(x => x.IngredientId == id).ToListAsync(); _logger.LogInformation($"Sorting by Ingredients Name: {SortIngred}"); return SortIngred; }
                if (ingName != null) { SortIngred = await _context.Ingredients.Where(x => x.Name.Contains(ingName)).ToListAsync(); _logger.LogInformation($"Sorting by Ingredients Name: {SortIngred}"); return SortIngred; }
                if (ingNameAr != null) { SortIngred = await _context.Ingredients.Where(x => x.Name.Contains(ingNameAr)).ToListAsync(); _logger.LogInformation($"Sorting by Ingredients Name: {SortIngred}"); return SortIngred; }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error: {ex.Message}");
                return null;

            }
        }
        #endregion


        #region Category
        public async Task<string> AddCategory(CategoryDTO categoryDTO)
        {
            try
            {
                if (categoryDTO != null)
                {
                    Category category = new()
                    {
                        CategoryName = categoryDTO.categoryName,
                        CategoryNameAr = categoryDTO.categoryNameAr,
                        Description = categoryDTO.description,
                        DescriptionAr = categoryDTO.descriptionAr,
                        IsActive = categoryDTO.isactive,
                        ImageId = categoryDTO.imageId
                    };
                    await _context.AddAsync(category);
                    _logger.LogInformation($"Successfully Add Category {category}");
                    return "Successfully Add Category";
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error");
                return ex.Message;

            }
        }

        public async Task<string> EditeCategory(int id, CategoryDTO categoryDTO)
        {
            try
            {
                var Editecat = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == id);
                if (Editecat != null)
                {
                    Editecat.CategoryName = categoryDTO.categoryName;
                    Editecat.CategoryNameAr = categoryDTO.categoryNameAr;
                    Editecat.Description = categoryDTO.description; 
                    Editecat.DescriptionAr = categoryDTO.descriptionAr;
                    Editecat.IsActive = categoryDTO.isactive;
                    Editecat.ImageId = categoryDTO.imageId;
                    _context.Update(Editecat);
                    _logger.LogInformation($"Updated successFully{Editecat}");
                    return "Updated successFully";
                }
                return null;

            }
            catch (Exception ex)
            {
                _logger.LogError("Error");
                return ex.Message;

            }
        }


        public async Task<List<Category>> AllCategories()
        {
            try
            {
                var AllCat = await _context.Categories.ToListAsync();
                if (AllCat is null) return null;
                _logger.LogInformation("Get All Categories");
                return AllCat;

            }
            catch (Exception ex)
            {

                _logger.LogError("Error");
                return null;

            }
        }

        public async Task<List<Category>> SortCategory(int? id, string? Name, string? NameAr)
        {
            try
            {
                var SortCat = await _context.Categories.ToListAsync();
                if (id != null)
                {
                    SortCat = await _context.Categories.Where(x => x.CategoryId == id).ToListAsync();
                    return SortCat;

                }
                if (Name != null)
                {
                    SortCat = await _context.Categories.Where(x => x.CategoryName.Contains(Name)).ToListAsync();
                    return SortCat;

                }
                if (NameAr != null)
                {
                    SortCat = await _context.Categories.Where(x => x.CategoryNameAr.Contains(NameAr)).ToListAsync();
                    return SortCat;

                }
                return null;


            }
            catch (Exception ex)
            {
                _logger.LogError("Error");
                return null;
            }
        }
        #endregion

        #region Image
        public async Task<string> CreateImagePath(ImageDTO imageDTO)
        {
            try
            {
                if (imageDTO is null) return string.Empty;
                Image image = new()
                {
                    Path = imageDTO.path,
                    IsDefault = imageDTO.isDefault
                };
                await _context.Images.AddAsync(image);
                _logger.LogInformation("We Added Image Path");
                return "Added Image Path Successfully";

            }
            catch (Exception ex)
            {
                _logger.LogError("Error");
                return string.Empty;
            }
        }
        public async Task<string> EditeImage(int id, ImageDTO imageDTO)
        {
            try
            {
                var Editeimg = await _context.Images.FirstOrDefaultAsync(x => x.Path == imageDTO.path);
                if (Editeimg is null) return string.Empty;
                Editeimg.Path = imageDTO.path;
                Editeimg.IsDefault = imageDTO.isDefault;
                _context.Update(Editeimg);
                _logger.LogInformation("Updated Successfully");
                return "Updated Successfully";

            }
            catch (Exception ex)
            {
                _logger.LogError("Error");
                return string.Empty;
            }
        }
        public async Task<List<Image>> AllImages()
        {
            try
            {
                var Allimg = await _context.Images.ToListAsync();
                if (Allimg is null) return null;
                _logger.LogInformation("Fetch List");
                return Allimg;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error");
                return null;

            }
        }

        public async Task<List<Image>> SortImages(int? id, string? Path)
        {
            try
            {
                var Sortimg = await _context.Images.ToListAsync();
                if (id != null)
                {
                    Sortimg = await _context.Images.Where(x => x.ImageId == id).ToListAsync();
                    return Sortimg;
                }
                if (Path != null)
                {
                    Sortimg = await _context.Images.Where(x => x.Path.Contains(Path)).ToListAsync();
                    return Sortimg;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error");
                return null;

            }

        }
        #endregion

        #region ItemImages
        public async Task<string> CreateItemImage(ItemImageDTO itemimgDTO)
        {
            try
            {
                ImageItem imgitem = new()
                {
                    ItemId = itemimgDTO.itemid,
                    Path = itemimgDTO.imagepath,
                    IsDefault = itemimgDTO.isdefult
                };
                await _context.AddAsync(imgitem);
                _logger.LogInformation("Successfully added image for item");
                return "Successfully added image for item";
            }
            catch (Exception ex)
            {
                _logger.LogError("Error");
                return string.Empty;

            }
        }

        public async Task<string> EditeItemImage(int id, ItemImageDTO itemimgDTO)
        {
            try
            {
                var itemimg = await _context.ImageItems.SingleOrDefaultAsync(x => x.ImageItemId == id);
                if (itemimg != null)
                {
                    itemimg.ItemId = itemimgDTO.itemid;
                    itemimg.Path = itemimgDTO.imagepath;
                    itemimg.IsDefault = itemimgDTO.isdefult;
                    _context.Update(itemimg);
                    _logger.LogInformation("Successfully updated itemimage");
                    return "Successfully updated itemimage";


                }
                return "incorrect id or empty itemimage";
            }
            catch (Exception ex)
            {
                _logger?.LogError("Error");
                return string.Empty;
            }
        }
        #endregion

        #region MealImages
        public async Task<string> CreateMealImage(MealImageDTOcs mealimageDTO)
        {
            try
            {
                ImageMeal imagemeal = new()
                {
                    MealId = mealimageDTO.mealid,
                    Path = mealimageDTO.imagepath,
                    IsDefualt = mealimageDTO.isdefult
                    
                };
                await _context.AddAsync(imagemeal);
                _logger.LogInformation("Added ImageMeal Successfully");
                return "Added ImageMeal Successfully";
            }
            catch (Exception ex)
            {
                _logger?.LogError("Error");
                return string.Empty;
            }
        }

        public async Task<string> EditeMealImage(int id, MealImageDTOcs mealimageDTO)
        {
            try
            {
                var mealimg = await _context.ImageMeals.SingleOrDefaultAsync(x => x.ImageMealId == id);
                if (mealimg != null)
                {
                    mealimg.MealId = mealimageDTO.mealid;
                    mealimg.Path = mealimageDTO.imagepath;
                    mealimg.IsDefualt = mealimageDTO.isdefult;
                    _context.Update(mealimg);
                    _logger.LogInformation("Successfully updated MealImage");
                    return "Successfully updated MealImage";


                }
                return "incorrect id or empty MealImage";
            }
            catch (Exception ex)
            {
                _logger?.LogError("Error");
                return string.Empty;
            }
        }
        #endregion



    }
}
