function validateEmail(email) {
    const validEmailPattern = /\S+@\S+\.\S+/;

    return !validEmailPattern.test(email)
        ? 'Invalid email address'
        : null;
}

export default validateEmail;