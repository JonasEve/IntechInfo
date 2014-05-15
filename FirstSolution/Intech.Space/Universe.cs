using Intech.Business;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Intech.Space
{
    public class Star
    {
        string _name;
        public Galaxy Galaxy { get; private set;  }
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Name cannot be null");

                if (Galaxy.ContainsStar(value))
                    throw new InvalidOperationException("The star name already exist");

                _name = value;
            }
        }

        public Star(Galaxy galaxy, string name)
        {
            if (galaxy == null)
                throw new ArgumentNullException("Galaxy must exist");

            if (name == null)
                throw new ArgumentNullException("Name cannot be null");

            _name = name;
            Galaxy = galaxy;
        }
    }

    public class Galaxy
    {
        string _name;
        List<Star> _stars;
        public Universe Universe { get; private set; }
        public string Name
        {
            get
            {
                return _name;
            }
            private set {}
        }
        public IReadOnlyList<Star> Stars 
        {   get
            {
                return _stars;
            }
            private set{}
        }

        public Galaxy(Universe universe, string name)
        {
            BuildGalaxy(universe, name);
            _stars = new List<Star>();
        }

        public Galaxy(Universe universe, string name, IReadOnlyList<Star> stars)
        {
            BuildGalaxy(universe, name);
            _stars = stars.ToList();
        }

        private void BuildGalaxy(Universe universe, string name)
        {
            if (universe == null)
                throw new ArgumentNullException("Universe must exist");

            if (name == null)
                throw new ArgumentNullException("Name cannot be null");

            _name = name;
            Universe = universe;
        }

        public void AddStar(string starName)
        {
            addStar(starName);
        }

        public void AddStar(Star star)
        {
            addStar(star.Name);
        }

        private void addStar(string starName)
        {
            if (Stars.Where(s => string.Equals(s.Name, starName, StringComparison.InvariantCultureIgnoreCase)).Any())
                throw new InvalidOperationException("The star already exist");

            _stars.Add(new Star(this, starName));
        }

        public bool ContainsStar(string starName)
        {
            return _stars.Where(s => string.Equals(s.Name, starName, StringComparison.InvariantCultureIgnoreCase)).Any();
        }

        public bool RemoveStar(string starName)
        {
            var star = _stars.Where(s => string.Equals(s.Name, starName, StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault();

            if (star == null)
                return false;

            return _stars.Remove(star);
        }

        public void RemoveAllstars()
        {
            _stars.Clear();
        }
    }

    [Serializable]
    public class Universe
    {
        Dictionary<string, Galaxy> _galaxies;

        public Universe()
        {
            _galaxies = new Dictionary<string, Galaxy>(StringComparer.InvariantCultureIgnoreCase);
        }

        public Galaxy this[string key]
        {
            get
            {
                Galaxy value;
                if (!_galaxies.TryGetValue(key, out value))
                {
                    throw new KeyNotFoundException();
                }
                return value;
            }

            private set{}
        }

        public void AddGalaxy(string galaxyName)
        {
            if (_galaxies.ContainsKey(galaxyName))
                throw new InvalidOperationException("The galaxy already exist");

            _galaxies.Add(galaxyName, new Galaxy(this, galaxyName));
        }

        public void AddGalaxy(Galaxy galaxy)
        {
            if (_galaxies.ContainsKey(galaxy.Name))
                throw new InvalidOperationException("The galaxy already exist");

            _galaxies.Add(galaxy.Name, new Galaxy(this, galaxy.Name, galaxy.Stars));
        }

        public bool RemoveGalaxy(string galaxyName)
        {
           return _galaxies.Remove(galaxyName);
        }

        public void RemoveAllGalaxies()
        {
            _galaxies.Clear();
        }

        public bool ContainsGalaxy(string galaxyName)
        {
            return _galaxies.ContainsKey(galaxyName);
        }

        public int GetNumberOfGalaxies()
        {
            return _galaxies.Count;
        }
    }

    public static class GalaxyExtension
    {
        public static void MoveTo(this Galaxy galaxy, Universe universe)
        {
            // add first to detect name clash exception before deletion
            universe.AddGalaxy(galaxy);
            galaxy.Universe.RemoveGalaxy(galaxy.Name);
        }

        public static void Implode(this Galaxy galaxy)
        {
            galaxy.RemoveAllstars();
            galaxy.Universe.RemoveGalaxy(galaxy.Name);
        }

        public static void RenameGalaxy(this Galaxy galaxy, string newGalaxyName)
        {
            galaxy.Universe.AddGalaxy(new Galaxy(galaxy.Universe, newGalaxyName, galaxy.Stars));

            galaxy.Universe.RemoveGalaxy(galaxy.Name);
        }
    }

    public static class StarExtension
    {
        public static void MoveTo(this Star star, Galaxy galaxy)
        {
            // add first to detect name clash exception before deletion
            galaxy.AddStar(star);
            star.Galaxy.RemoveStar(star.Name);
        }

        public static void Implode(this Star star)
        {
            star.Galaxy.RemoveStar(star.Name);
        }

        public static void RenameStar(this Star star, string newStarName)
        {
            star.Name = newStarName;
        }
    }
}
