using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class VideoGamesController : ApiController
    {
        private VideoGame[] vgs = new VideoGame[]
        {
            new VideoGame { Id = 1, System = Platform.SegaGenesis, Title = "Sonic 3" },
            new VideoGame { Id = 2, System = Platform.SegaGenesis, Title = "Dynamite Headdy" },
            new VideoGame { Id = 666, System = Platform.Pc, Title = "Doom II" },
        };

        public IEnumerable<VideoGame> GetAllVideoGames()
        {
            return vgs;
        }

        // Can't define this method!
        // Multiple signatures with 0 params are in this controller: GetAllVideoGames and GetGenesisVideoGames.
        // Error given is "Multiple actions were found that match the request"

        //public IEnumerable<VideoGame> GetGenesisVideoGames()
        //{
        //    return vgs.Where(p => p.System == Platform.SegaGenesis);
        //}

        public IHttpActionResult GetVideoGame(int id)
        {
            var vg = vgs.FirstOrDefault(p => p.Id == id);
            if (vg == null) { return NotFound(); }
            return Ok(vg);
        }
    }
}
