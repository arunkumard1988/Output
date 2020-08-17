using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace levenshtein.Model
{
    public class LevenshteinModel
    {
        [Required]
        public string FirstParam { get; set; }

        [Required]
        public string SecondParam { get; set; }
        public string DifferenceCount { get; set; }
    }
}
