using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using System;
using WebCactusAPI.Models;
using WebCactusAPI.Services;
using System.Net.Mime;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebCactusAPI.Controllers
{
    /// <summary>
    /// Queries the service for all cacti and automatically returns data with a Content-Type value of application/json
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CactusController : ControllerBase
    {
        public CactusController() { }

        // GET all action
        [HttpGet]
        public ActionResult<List<WebCactus>> GetAll() => CactusService.GetAll();

        // GET by Id action
        [HttpGet("{id}")]
        public ActionResult<WebCactus> Get(int id)
        { 
            WebCactus? cactus = CactusService.Get(id);
            if (cactus is null)
                return NotFound();

            return Ok(cactus);
        }

        // POST action
        /// <summary>
        /// Returns IActionResult because the ActionResult return type isn't known until runtime. 
        /// The NotFound and NoContent methods return NotFoundResult and NoContentResult types, respectively. 
        /// IActionResult lets the client know if the request succeeded and provides the ID of the newly created cactus
        /// Cactus parameter will be found in the request body
        /// CreatedAtAction	201	The cactus was added to the in-memory cache.
        /// BadRequest is implied	400	The request body's cactus object is invalid.
        /// </summary>
        /// <param name="cactus"></param>
        /// <returns></returns>
        [HttpPost]
        //more optional attributes below
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create(WebCactus cactus)
        {
            // This code will save the cactus and return a result
            CactusService.Add(cactus);

            //CreatedAtAction() creates an object that produces a response.
            //The first parameter in the CreatedAtAction method call represents an action name.
            //The nameof keyword is used to avoid hard-coding the action name. 
            //CreatedAtAction uses the action name to generate a location HTTP response header with a URL to the newly created cactus
            return CreatedAtAction(nameof(Create), new { id = cactus.Id}, cactus);
        }

        // PUT action
        /// <summary>
        /// Returns IActionResult because the ActionResult return type isn't known until runtime. 
        /// The NotFound and NoContent methods return NotFoundResult and NoContentResult types, respectively.
        /// NoContent	204	The cactus was updated in the in-memory cache.
        /// BadRequest	400	The request body's Id value doesn't match the route's id value.
        /// BadRequest is implied	400	The request body's cactus object is invalid.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cactus"></param>
        /// <returns></returns>
        [HttpPut]
        //more optional attributes below
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Update(int id, WebCactus cactus)
        {
            // This code will update the cactus and return a result
            if (id != cactus.Id)
                return BadRequest();

            WebCactus? existingCactus = CactusService.Get(id);
            if (existingCactus is null)
                return NotFound();

            CactusService.Update(cactus);

            return NoContent();//empty 204 response
        }

        // DELETE action
        /// <summary>
        /// Returns IActionResult because the ActionResult return type isn't known until runtime. 
        /// The NotFound and NoContent methods return NotFoundResult and NoContentResult types, respectively.
        /// NoContent	204	The cactus was deleted from the in-memory cache
        /// NotFound	404	A cactus that matches the provided id parameter doesn't exist in the in-memory cache.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // This code will delete the cactus and return a result
            WebCactus? existingCactus = CactusService.Get(id);
            if (existingCactus is null)
                return NotFound();

            CactusService.Delete(id);

            return NoContent();//empty 204 response
        }
 
    }
}
