using System.Collections.Generic;

namespace CCostsProject.json_structure
{
    public class FamilyJson
    {
        public int Id { get; set; }
        public string Creator { get; set; }
        public List<MemberInFamilyJson> Members { get; set; }
    }
}