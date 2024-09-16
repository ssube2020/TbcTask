using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.RegularExpressions;
using Core.Models.Entities;
using Microsoft.Extensions.Localization;
using TbcTaskApi.Dtos.RequestDtos;

namespace TbcTaskApi.Middlewares;

public class ValidateRequestFilter(IStringLocalizerFactory localizerFactory) : ActionFilterAttribute
{
    private readonly IStringLocalizer localizer = localizerFactory.Create("ValidationResponseMessages.ResponseMessages", "TbcTaskApi");
    
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is CreateUserDto user)
            {
                if (!ValidateUserDto(user, context))
                    return;
            }

            if (argument is EditUserDto editDto)
            {
                if (!ValidateEditUserDto(editDto, context))
                    return;
            }
        }

        base.OnActionExecuting(context);
    }

    private bool ValidateUserDto(CreateUserDto user, ActionExecutingContext context)
    {
        bool isValid = ValidateNameAndSurname(user.Name, out var nameError);
        
        if (!ValidateNameAndSurname(user.Surname, out var surnameError))
            isValid = false;
        
        if (!ValidateGender(user.Gender, out var genderError))
            isValid = false;
        
        if (!ValidatePersonalNumber(user.PersonalNumber, out var personalNumberError))
            isValid = false;

        if (!ValidateAge(user.BirthDate, out var ageError))
            isValid = false;

        if (!ValidatePhones(user.PhoneNumbers, out var phoneError))
            isValid = false;
        
        if (!isValid)
        {
            string errorMessage = $"{nameError},{surnameError},{genderError},{personalNumberError},{ageError},{phoneError}".Trim(',');
            AddErrorToContext(context, localizer["ValidationGeneralError"], errorMessage);
        }

        return isValid;
    }
    
    private bool ValidateEditUserDto(EditUserDto editDto, ActionExecutingContext context)
    {
        bool isValid = ValidateNameAndSurname(editDto.Name, out var nameError);
        
        if (!ValidateNameAndSurname(editDto.Surname, out var surnameError))
            isValid = false;
        
        if (!ValidateGender(editDto.Gender, out var genderError))
            isValid = false;
        
        if (!ValidatePersonalNumber(editDto.PersonalNumber, out var personalNumberError))
            isValid = false;
        
        if (!ValidateAge(editDto.BirthDate, out var ageError))
            isValid = false;

        if (!ValidatePhones(editDto.PhoneNumbers, out var phoneError))
            isValid = false;

        if (!isValid)
        {
            string errorMessage = $"{nameError},{surnameError},{genderError},{personalNumberError},{ageError},{phoneError}".Trim(',');
            AddErrorToContext(context, localizer["ValidationGeneralError"], errorMessage);
        }

        return isValid;
    }
    
    private bool ValidateNameAndSurname(string name, out string errorMessage)
    {
        errorMessage = string.Empty;
        
        if (name.Length < 2 || name.Length > 50)
        {
            errorMessage = localizer["NameLengthError"];
            return false;
        }
        
        var englishRegex = new Regex(@"^[a-zA-Z]+$");
        var georgianRegex = new Regex(@"^[\u10A0-\u10FF]+$");

        if (!englishRegex.IsMatch(name) && !georgianRegex.IsMatch(name))
        {
            errorMessage = localizer["NameLetterValidationErrorMessage"];
            return false;
        }

        return true;
    }
    
    private bool ValidateGender(GenderEnum gender, out string errorMessage)
    {
        errorMessage = string.Empty;

        if (gender != GenderEnum.Female && gender != GenderEnum.Male)
        {
            errorMessage = localizer["GenderValidationError"];
            return false;
        }

        return true;
    }
    
    private bool ValidatePersonalNumber(string input, out string errorMessage)
    {
        errorMessage = string.Empty;
        
        if (!Regex.IsMatch(input, @"^[0-9]+$") || input.Length != 11)
        {
            errorMessage = localizer["PersonalNumberValidationError"];
            return false;
        }
        
        return true;
    }
    
    private bool ValidateAge(DateTime birthDate, out string errorMessage)
    {
        errorMessage = string.Empty;
        DateTime today = DateTime.Today;
        
        int age = today.Year - birthDate.Year;
        
        if (age < 18)
        {
            errorMessage = localizer["AgeValidationError"];
            return false;
        }
        
        return true;
    }

    private bool ValidatePhones(List<PhoneRequestDto>? phones, out string errorMessage)
    {
        errorMessage = string.Empty;
        if (phones != null)
        {
            foreach (var phone in phones)
            {
                if (phone.PhoneNumber.Length < 4 || phone.PhoneNumber.Length > 50 || !Regex.IsMatch(phone.PhoneNumber, @"^[0-9]+$"))
                {
                    errorMessage = localizer["PhoneValidationError"];
                    return false;
                }
            }
        }
        
        return true;
    }

    private static void AddErrorToContext(ActionExecutingContext context, string error, string errorMessage)
    {
        context.Result = new BadRequestObjectResult(new 
        {
            Error = error,
            Message = errorMessage
        });
    }
}
