import validatePassword from '../../src/utils/validatePassword.js'

describe('validatePassword', () => {
    test('should return null when password is valid', () => {
        const password = 'Password123.'

        expect(validatePassword(password)).toBeNull()
    })

    test('should return error message when password does not have at least 6 character', () => {
        const password = 'test'

        expect(validatePassword(password)).toBe('Password must be at least 6 characters.')
    })

    test('should return error message when password does not have non alphanumeric character', () => {
        const password = 'aaaaaa'

        expect(validatePassword(password)).toBe('Passwords must have at least one non alphanumeric character.')
    })

    test('should return error message when password does not have at least one digit', () => {
        const password = 'aaaaa.'

        expect(validatePassword(password)).toBe('Password must have at least one digit (\'0\'-\'9\').')
    })

    test('should return error message when password does not have one lowercase', () => {
        const password = 'AAAA9.'

        expect(validatePassword(password)).toBe('Password must have at least one lowercase (\'a\'-\'z\').')
    })

    test('should return error message when password does not have one uppercase', () => {
        const password = 'aaaa9.'

        expect(validatePassword(password)).toBe('Password must have at least one uppercase (A-Z).')
    })
})