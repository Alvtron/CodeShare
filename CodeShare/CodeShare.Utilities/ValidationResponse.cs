namespace CodeShare.Utilities
{
    public enum ValidationResponse
    {
        Empty,
        Valid,
        Invalid,
        AlreadyExist,
        DoNotExist,
        UserCreated,
        TooShort,
        TooLong,
        ContainsIllegalCharacters,
        Unavailable,
        NoSymbol,
        NoNumber,
        NoLowerCase,
        NoUpperCase,
    };
}
