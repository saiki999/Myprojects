using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UserDataAccess;

namespace MovieRentalAPI.Controllers
{
    public class UsersController : ApiController
    {

        public IEnumerable<User> Get()
        {
            using (MovieRentalsEntities entities = new MovieRentalsEntities())
            {

                return entities.Users.ToList();

            }
        }



        public HttpResponseMessage Get(int id)
        {

            using (MovieRentalsEntities entities = new MovieRentalsEntities())
            {

                var entity = entities.Users.FirstOrDefault(e => e.Id == id);

                if (entity != null)
                {

                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else {


                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User with Id=" + id.ToString() + "not found");

                }
            }




        }

        public HttpResponseMessage Post([FromBody]User user)
        {
            try
            {
                using (MovieRentalsEntities entities = new MovieRentalsEntities())
                {

                    entities.Users.Add(user);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, user);
                    message.Headers.Location = new Uri(Request.RequestUri + user.Id.ToString());

                    return message;

                }
            }

            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (MovieRentalsEntities entities = new MovieRentalsEntities())
                {

                    var entity = entities.Users.FirstOrDefault(u => u.Id == id);

                    if (entity == null)
                    {

                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Entered Id=" + id.ToString() + "Not Found in Databsse");

                    }


                    else
                    {


                        entities.Users.Remove(entity);
                        entities.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, "Id=" + id.ToString() + "Is Removed");

                    }

                }
            }

            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }


        }

        public HttpResponseMessage Put(int id, [FromBody] User user)
        {
            try {
                using (MovieRentalsEntities entities = new MovieRentalsEntities())
                {

                    var entity = entities.Users.FirstOrDefault(u => u.Id == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User with id =" + id.ToString() + "Not Found");


                    }

                    else
                    {

                        entity.FName = user.FName;
                        entity.LName = user.LName;
                        entity.EmailId = user.EmailId;
                        entity.Password = user.Password;
                        entity.ConfirmPassword = user.ConfirmPassword;
                        entity.Type = user.Type;// remove this in UI
                        entity.Gender = user.Gender;
                        entity.Age = user.Age;
                        entity.Phone = user.Phone;
                        entity.Address = user.Address;


                        entities.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }

                }
            }
            catch(Exception ex)
                {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,ex);
            }
            }
    }
    }

