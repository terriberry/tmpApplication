using System.Text.RegularExpressions;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.Domain.Common.CodeValidator", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Domain.Common.Validation.Validators;

public static class CodeValidator
{
    public static bool Validate(string code)
    {
        bool valid = false;
        if (code != null)
        {
            code = code.ToUpper();
            var regexValidCharacters = new Regex("^[A-Z0-9_]*$");
            valid = regexValidCharacters.IsMatch(code);
        }
        return valid;
    }

    public static string Message => $"Code may only contain 'A'-'Z','_' characters";
}