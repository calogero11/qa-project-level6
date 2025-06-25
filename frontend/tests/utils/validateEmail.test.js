import validateEmail from '../../src/utils/validateEmail.js'

describe('validateEmail', () => {
    test('should return null when email is valid', () => {
        const email = 'admin@example.com'
            
        expect(validateEmail(email)).toBeNull()
    })

    test('should return Invalid email address when email does not have @ symbol', () => {
        const email = 'exampleEmail.com'

        expect(validateEmail(email)).toBe('Invalid email address')
    })

    test('should return Invalid email address when email does not have dot', () => {
        const email = 'example@emailcom'

        expect(validateEmail(email)).toBe('Invalid email address')
    })
})