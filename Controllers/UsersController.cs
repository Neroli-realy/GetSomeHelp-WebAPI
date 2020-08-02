using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using GetSomeHelp.Models;
using System.Security.Cryptography;
using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using System.Security.Claims;

namespace GetSomeHelp.Controllers{
    [Route("api/[controller]")]
    public class UsersController : Controller{
        private GSHContext dbContext;

        public UsersController(){
            var connectionString = "";
            this.dbContext = GSHContextFactory.Create(connectionString);
        }

        
        [HttpPost("Login")]
        public ActionResult Login([FromBody]User user){
            //check if the request format is good
            if(!this.ModelState.IsValid)
                return BadRequest();
             //check if valid email
            if(!IsValidEmail(user.Email))
                return BadRequest("Bad Email");

            //check if User Exist
            var tuser = this.dbContext.User.SingleOrDefault(u => u.Email.Equals(user.Email));
            if(tuser != null){
                //validate password
                if(tuser.passHash.Equals(ComputeSha256Hash(user.passHash))){
                    Response.Headers.Add("Auth", GenerateToken(tuser.ID, tuser.name));
                    return Ok(tuser);
                }
            }

            return Unauthorized("Wrong Email Or Password");
        }

        private string GenerateToken(int uID, string name){
            //JWT security key (please don't hardcode it here this is just a demo)
            string SecurityKey = "This_Is_Test_Key_Don't_Use_IT_Here_It's_Not_Safe";

            var SymmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey));

            var SigningCredentials = new SigningCredentials(SymmetricKey, SecurityAlgorithms.HmacSha256Signature);
            var claims = new List<Claim>();
            claims.Add(new Claim("uid", uID.ToString()));
            claims.Add(new Claim("Name", name));
            var token = new JwtSecurityToken(
                issuer: "localhost",
                audience: "localhost",    
                expires: DateTime.Now.AddHours(1),
                signingCredentials: SigningCredentials,
                claims: claims
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [HttpPost("Register")]
        public ActionResult Register([FromBody] User user){
            //check if good Request format
            if(!this.ModelState.IsValid){
                return BadRequest();
            }
            
            //check if valid email
            if(!IsValidEmail(user.Email))
                return BadRequest("Bad Email");

            //check if valid Username
            if(!IsValidUsername(user.name))
                return BadRequest("Bad Username");

            //check if duplicate Email
            if(this.dbContext.User.Any(u => u.Email.Equals(user.Email)))
                return Conflict("This Email is Already Used");

            //add user to DataBase
            user.passHash = ComputeSha256Hash(user.passHash);
            this.dbContext.User.Add(user);
            this.dbContext.SaveChanges();
            return Created($"api/Users/{user.ID}",user);
        }

        bool IsValidEmail(string email)
        {
            try {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch {
                return false;
            }
        }

        bool IsValidUsername(string username){
            string AllowedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
            foreach(var letter in username){
                if(!AllowedCharacters.Contains(letter))
                    return false;
            }
            return true;
        }
        private string ComputeSha256Hash(string rawData)  
        {  
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())  
            {  
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));  
  
                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();  
                for (int i = 0; i < bytes.Length; i++)  
                {  
                    builder.Append(bytes[i].ToString("x2"));  
                }  
                return builder.ToString();  
            }  
        }  
    }
}