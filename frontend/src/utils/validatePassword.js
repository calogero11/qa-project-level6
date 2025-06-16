function validatePassword(password) {
    const atLeast6Charaters = /^.{6,}$/;
    const atLeast1NonAlphanumericCharater = /[^a-zA-Z0-9]/;
    const atLeast1Digit = /^(?=.*\d).+$/;
    const atLeast1LowercaseCharater = /^(?=.*[a-z]).+$/;
    const atLeast1UppercaseCharater = /^(?=.*[A-Z]).+$/;
    const atLeast1UniqueCharater = /^(?=.*[A-Z]).+$/;

    if (!atLeast6Charaters.test(password)) {
        return 'Password must be at least 6 characters.';
    }
    else if (!atLeast1NonAlphanumericCharater.test(password)) {
        return 'Passwords must have at least one non alphanumeric character.';
    }
    else if (!atLeast1Digit.test(password)) {
        return 'Password must have at least one digit (\'0\'-\'9\').';
    }
    else if (!atLeast1LowercaseCharater.test(password)) {
        return 'Password must have at least one lowercase (\'a\'-\'z\').';
    }
    else if (!atLeast1UppercaseCharater.test(password)) {
        return 'Password must have at least one uppercase (A-Z).';
    }
    else if (!atLeast1UniqueCharater.test(password)) {
        return 'Password must use at least 1 different character.';
    }
    
    return null;
}

export default validatePassword