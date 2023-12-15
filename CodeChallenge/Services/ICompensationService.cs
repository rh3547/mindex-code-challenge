using CodeChallenge.DTOs;
using CodeChallenge.Models;
using System;

namespace CodeChallenge.Services
{
    public interface ICompensationService
    {
        Compensation Create(CompensationAddDTO compensationAddDTO);
        Compensation GetByEmployeeId(String employeeId);
        Compensation GetById(String id);
    }
}
