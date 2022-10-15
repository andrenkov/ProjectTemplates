using WebCactusAPI.Models;

namespace WebCactusAPI.Services
{
    public class CactusService
    {
        static List<WebCactus> Cacti { get; }
        static int nextId = 3;

        /// <summary>
        /// Create in-memory list/db on Create()
        /// </summary>
        static CactusService()
        {
            Cacti = new List<WebCactus> 
            {
                new WebCactus{ Id = 1, SpeciesName = "Astrophytum asterias", IsAvailable = true},
                new WebCactus{ Id = 2, SpeciesName = "Thelocactus bicolor", IsAvailable = false}
            };
        }
        public static List<WebCactus> GetAll() => Cacti;

        public static WebCactus? Get(int id) => Cacti.FirstOrDefault(c => c.Id == id);

        public static void Add(WebCactus cactus)
        {
            cactus.Id = nextId++;
            Cacti.Add(cactus);
        }

        public static void Delete(int id)
        {
            WebCactus? cactus = Get(id);
            if (cactus is null)
                return;

            Cacti.Remove(cactus);
        }

        public static void Update(WebCactus cactus)
        {
            int indx = Cacti.FindIndex(c => c.Id == cactus.Id);
            if (indx == -1)
                return;

            Cacti[indx] = cactus;
        }
    }
}
