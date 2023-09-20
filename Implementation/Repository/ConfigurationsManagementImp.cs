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
            
        public ConfigurationsManagementImp(RoyalFoodContext context,ILogger<ConfigurationsManagementImp> logger, Helper helper) 
        { 
            _context = context;
            _logger = logger;
            this.helper = helper;
        }

        #region Role

        public async Task<List<Role>> AllRoles()
        {
            try
            {
                var RolesAll = await _context.Roles.ToListAsync();
                return RolesAll;
                _logger.LogInformation("We Get All Roles");

                return RolesAll;
            }
            catch (Exception ex)
            { 
                return null;
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
                _logger.LogInformation("Done");
            }
            catch (Exception ex) 
            {
                return null;
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
                        UserId = user.UserId


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
            var loginuser = await _context.Logins.SingleOrDefaultAsync(x => x.Email == loginDTO.email && x.Password == helper.GenerateSHA384String(loginDTO.password));
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

                    };
                    User user = _context.Users.Include(x => x.Logins).Where(x => x.Logins.Any(login => login.Email == loginDTO.email)).FirstOrDefault();
                    if (user is null) return "User Not Found";
                    Role role = _context.Roles.Where(role => role.RoleId == user.RoleId).FirstOrDefault();
                    if (role is null) return "Role Not Found";
                    string roleName = role.RoleName;
                    string token = helper.GenerateJwtToken(loginDTO.email, roleName, loginuser.UserId ?? 0, true);
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

        public async Task<string> Logount(LoginDTO loginDTO)
        {
            return null;
        }

        #endregion

        public async Task<string> CreateIngredient(IngredientDTO ingredientDTO)
        {
            try
            {
                if (ingredientDTO != null)
                {
                   
                    Ingredient ingredient = new()
                    {
                        Name = ingredientDTO.ingName,
                        Describtion = ingredientDTO.ingDescription,
                        Unit = ingredientDTO.unit,
                        IsActive =ingredientDTO.isactive,
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
                var EditeIngred = await _context.Ingredients.FirstOrDefaultAsync(x=>x.IngredientId == id);
                if(EditeIngred != null)
                {
                    EditeIngred.Name = ingredientDTO.ingName;
                    EditeIngred.Describtion = ingredientDTO.ingDescription;
                    EditeIngred.IsActive = ingredientDTO.isactive;
                    EditeIngred.ImageId = ingredientDTO.imageId;

                    _context.Update(EditeIngred);
                    _logger.LogInformation("Updating Ingredient");
                    return $"Updating Ingredient id: {id}";
            
                }
                _logger.LogInformation($"Null Id: {id}");
                return $"Null Id: {id}";
            }
            catch(Exception ex)
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
                if(AllIngredients != null) return AllIngredients;
                _logger.LogInformation($"ViewIngredients{AllIngredients}");
                return null;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
            
        }
        public async Task<List<Ingredient>> SortIngredient(int? id, string ingName)
        {
            try
            {
                var SortIngred = await _context.Ingredients.ToListAsync();
                if (id != null) { SortIngred = await _context.Ingredients.Where(x => x.IngredientId == id).ToListAsync(); _logger.LogInformation($"Sorting by Ingredients Name: {SortIngred}"); return SortIngred; }
                if (ingName != null) { SortIngred = await _context.Ingredients.Where(x => x.Name.Contains(ingName)).ToListAsync(); _logger.LogInformation($"Sorting by Ingredients Name: {SortIngred}"); return SortIngred; }
                return null;
            }
            catch(Exception ex)
            {
                _logger.LogInformation($"Error: {ex.Message}");
                return null;

            }
        }

    }
}
