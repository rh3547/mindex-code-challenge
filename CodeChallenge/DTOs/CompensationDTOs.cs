using System;
using CodeChallenge.Models;

namespace CodeChallenge.DTOs
{
    /*
        I decided to create basic add DTOs for Compensation for the following reasons:
        - We really shouldn't be passing in an entire Employee object when creating a Compensation. An EmployeeId should suffice.
        - We don't need to pass a CompensationId when creating a Compensation because it is a generated field.

        A proper project would contain more DTOs for all of the system models, but I kept it simple for this challenge.
    */
    public class CompensationAddDTO
    {
        public String EmployeeId { get; set; }
        public float Salary { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}