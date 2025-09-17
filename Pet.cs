using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment11._3_ClientApp
{
    internal class Pet
    {
        public int PetId { get; set; }
        public string PetType { get; set; }
        public string PetBreed { get; set; }
        public string PetName { get; set; }
        public DateOnly PetBirthDate { get; set; }
        public string PetColor { get; set; }
        public string PetGender { get; set; }
    }
}
