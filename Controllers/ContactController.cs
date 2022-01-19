using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace CarnegieAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Contact")]
    public class ContactController : ApiController
    {
        // POST api/Contact/Add
        [HttpPost]
        [Route("Add")]
        public async Task<IHttpActionResult> Add(Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrEmpty(contact.Name))
            {
                return BadRequest("Contact Name cannot be empty!");
            }

            try
            {
                using (DBEntities db = new DBEntities())
                {
                    var result = await Task.Run(() => db.Contacts.Add(contact));
                    db.SaveChanges();

                    if (result != null)
                        return Ok(result);
                    else
                        return InternalServerError();
                }
                
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
            
        }

        // POST api/Contact/Edit
        [HttpPost]
        [Route("Edit")]
        public async Task<IHttpActionResult> Edit(Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrEmpty(contact.Name))
            {
                return BadRequest("Contact Name cannot be empty!");
            }

            try
            {
                using (DBEntities db = new DBEntities())
                {
                    var c = await Task.Run(() => db.Contacts.FirstOrDefault(e => e.Id == contact.Id));
                    if (c == null)
                    {
                        return Content(HttpStatusCode.NotFound, "Contact with Id " + contact.Id.ToString() + " not found to update!");
                    }
                    else
                    {
                        c.Name = contact.Name;
                        c.Address = contact.Address;
                        c.Phone = contact.Phone;
                        c.Email = contact.Email;

                        db.SaveChanges();

                        return Ok(c);
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }

        }

        // POST api/Contact/Delete
        [HttpPost]
        [Route("Delete")]
        public async Task<IHttpActionResult> Delete([FromUri] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                using (DBEntities db = new DBEntities())
                {
                    var contact = db.Contacts.FirstOrDefault(e => e.Id == id);
                    if (contact == null)
                    {
                        return BadRequest("Contact with Id " + id.ToString() + " not found to delete!");
                    }

                    var result = await Task.Run(() => db.Contacts.Remove(contact));
                    db.SaveChanges();

                    if (result != null)
                        return Ok();
                    else
                        return BadRequest("Contact with Id " + id.ToString() + " cannot be deleted!");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        // POST api/Contact/DeleteAll
        [HttpPost]
        [Route("DeleteAll")]
        public async Task<IHttpActionResult> DeleteAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                using (DBEntities db = new DBEntities())
                {
                    var result = await Task.Run(() => db.Database.ExecuteSqlCommand("TRUNCATE TABLE [Contact]"));
                    db.SaveChanges();

                    return Ok(result);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        // GET api/Contact/GetContacts
        [HttpGet]
        [Route("GetContacts")]
        public async Task<IHttpActionResult> GetContacts()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                using (DBEntities db = new DBEntities())
                {
                    List<pGetContacts_Result> contactList = await Task.Run(() => db.pGetContacts().ToList());
                    
                    if (contactList != null)
                        return Ok(contactList);
                    else
                        return Content(HttpStatusCode.NotFound, "No data was found");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

    }
}
