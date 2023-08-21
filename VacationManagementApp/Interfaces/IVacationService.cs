﻿using VacationManagementApp.Models;

namespace VacationManagementApp.Interfaces
{
    public interface IVacationService
    {
        IEnumerable<Vacation> GetVacations(string? currentUserId);

        void AddVacationToDb(Vacation vacation);
        IEnumerable<Vacation> GetYoursEmployeeVacation(string email);
        Vacation GetVacation(int? id);
        string EditVacation(Vacation editedVacation);
        
    }
}
